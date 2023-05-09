using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data.Interfaces.Repositories;
using webapi.Data.Models;
using webapi.Data.Specifications;
using webapi.ViewModels.Admissions;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmissionsApiController : ControllerBase
    {
        private readonly ILogger<AdmissionsApiController> _logger;
        private readonly IAdmissionsRepository _admissionsRepository;
        private readonly IGroupsOfSpecialitiesRepository _groupsRepository;
        private readonly IRecruitmentPlansRepository _plansRepository;
        private readonly ISubjectsRepository _subjectsRepository;

        public AdmissionsApiController(ILogger<AdmissionsApiController> logger, IAdmissionsRepository admissionsRepository, IGroupsOfSpecialitiesRepository groupsRepository,
            IRecruitmentPlansRepository plansRepository, ISubjectsRepository subjectsRepository)
        {
            _logger = logger;
            _admissionsRepository = admissionsRepository;
            _groupsRepository = groupsRepository;
            _plansRepository = plansRepository;
            _subjectsRepository = subjectsRepository;
        }

        // GET: api/AdmissionsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admission>>> GetAdmissions()
        {
            return await _admissionsRepository.GetAllAsync();
        }

        [HttpGet("GroupAdmissions/{groupId}")]
        public async Task<ActionResult<object>> GetGroupAdmissions(int groupId, string? searchStudents, int page, int pageLimit)
        {
            GroupOfSpecialties group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeRecruitmentPlans().IncludeAdmissions()) ?? new();

            if (group == null)
            {
                return NotFound();
            }

            List<Admission> admissions = SearchAdmissions(searchStudents, group.Admissions)
                .OrderBy(i => i.Student.Surname).ThenBy(i => i.Student.Name).ThenBy(i => i.Student.Patronymic).ToList();

            foreach (Admission admission in admissions)
            {
                admission.GroupOfSpecialties = new();
                admission.Student.Admissions = null;
                foreach (SpecialityPriority priority in admission.SpecialityPriorities)
                {
                    priority.RecruitmentPlan.EnrolledStudents = null;
                    priority.RecruitmentPlan.Speciality = new();
                }
            }

            return new
            {
                admissions = admissions.Skip((page - 1) * pageLimit).Take(pageLimit).ToList(),
                countOfSearchStudents = admissions.Count
            };
        }

        // GET: api/AdmissionsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admission>> GetAdmission(int id)
        {
            Admission? admission = await _admissionsRepository.GetByIdAsync(id, new AdmissionsSpecification().IncludeSpecialtyPriorities().IncludeStudentScores());

            if (admission == null)
            {
                return NotFound();
            }
            admission.GroupOfSpecialties = new();
            admission.Student.Admissions = null;
            foreach (SpecialityPriority priority in admission.SpecialityPriorities)
            {
                priority.RecruitmentPlan.EnrolledStudents = null;
                priority.RecruitmentPlan.Speciality = new();
            }
            admission.SpecialityPriorities = admission.SpecialityPriorities.OrderBy(i => i.Priority).ToList();

            return admission;
        }

        // PUT: api/AdmissionsApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmission(int id, CreateChangeAdmissionVM model)
        {
            if (ModelState.IsValid)
            {
                Admission? admission = await _admissionsRepository.GetByIdAsync(id, new AdmissionsSpecification().IncludeSpecialtyPriorities().IncludeStudentScores());
                if (admission == null)
                {
                    return Ok();
                }

                List<SpecialityPriority> specialityPriorities = new();
                List<Subject> subjects = new();

                foreach (StudentScore studentScore in admission.StudentScores)
                {
                    studentScore.Score = model.StudentScores.First(i => i.Id == studentScore.Id).Score;
                }
                foreach (SpecialityPriorityVM specialityPriority in model.SpecialitiesPriority.Where(p => p.Priority > 0))
                {
                    SpecialityPriority priority = admission.SpecialityPriorities.FirstOrDefault(i => i.RecruitmentPlan.Id == specialityPriority.PlanId) ??
                        new() { RecruitmentPlan = await _plansRepository.GetByIdAsync(specialityPriority.PlanId) ?? new() };
                    priority.Priority = specialityPriority.Priority;
                    specialityPriorities.Add(priority);
                }

                admission.SpecialityPriorities = specialityPriorities;
                admission.PassportID = model.PassportID;
                admission.IsTargeted = model.IsTargeted;
                admission.IsWithoutEntranceExams = model.IsWithoutEntranceExams;
                admission.IsOutOfCompetition = model.IsOutOfCompetition;
                admission.PassportSeries = model.PassportSeries;
                admission.PassportNumber = model.PassportNumber;
                admission.Email = model.Email;
                admission.Student.Surname = model.Student.Surname.Trim();
                admission.Student.Name = model.Student.Name.Trim();
                admission.Student.Patronymic = model.Student.Patronymic.Trim();
                admission.Student.GPS = model.Student.GPS;

                await _admissionsRepository.UpdateAsync(admission);

                _logger.LogInformation("Заявка абитуриента - {Surname} {Name} {Patronymic} - изменена", admission.Student.Surname, admission.Student.Name, admission.Student.Patronymic);
                try
                {
                    await _admissionsRepository.UpdateAsync(admission);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AdmissionExists(admission.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok();
            }

            return BadRequest(ModelState);
        }

        // POST: api/AdmissionsApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{groupId}")]
        public async Task<ActionResult<Admission>> PostAdmission(int groupId, CreateChangeAdmissionVM model)
        {
            if (ModelState.IsValid)
            {
                List<SpecialityPriority> specialityPriorities = new();
                List<Subject> subjects = new();

                foreach (StudentScore studentScore in model.StudentScores)
                {
                    Subject? subject = await _subjectsRepository.GetByIdAsync(studentScore.Subject.Id);
                    if (subject != null)
                    {
                        studentScore.Subject = subject;
                    }
                }
                foreach (SpecialityPriorityVM specialityPriority in model.SpecialitiesPriority.Where(p => p.Priority > 0))
                {
                    RecruitmentPlan? plan = await _plansRepository.GetByIdAsync(specialityPriority.PlanId);
                    if (plan != null)
                    {
                        SpecialityPriority priority = new() { Priority = specialityPriority.Priority, RecruitmentPlan = plan };
                        specialityPriorities.Add(priority);
                    }
                }

                GroupOfSpecialties group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeAdmissions()) ?? new();
                Admission admission = new()
                {
                    GroupOfSpecialties = await _groupsRepository.GetByIdAsync(groupId) ?? new(),
                    StudentScores = model.StudentScores,
                    Student = new() { Name = model.Student.Name.Trim(), Surname = model.Student.Surname.Trim(), Patronymic = model.Student.Patronymic.Trim(), GPS = model.Student.GPS },
                    DateOfApplication = model.DateOfApplication,
                    SpecialityPriorities = specialityPriorities,
                    PassportID = model.PassportID,
                    PassportSeries = model.PassportSeries,
                    PassportNumber = model.PassportNumber,
                    Email = model.Email,
                    IsTargeted = model.IsTargeted,
                    IsWithoutEntranceExams = model.IsWithoutEntranceExams,
                    IsOutOfCompetition = model.IsOutOfCompetition,
                };
                await _admissionsRepository.AddAsync(admission);
                _logger.LogInformation("Заявка студента - {Surname} {Name} {Patronymic} - добавлена", admission.Student.Surname, admission.Student.Name, admission.Student.Patronymic);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/AdmissionsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmission(int id)
        {
            Admission? admission = await _admissionsRepository.GetByIdAsync(id, new AdmissionsSpecification());
            if (admission != null)
            {
                await _admissionsRepository.DeleteAsync(id);
                _logger.LogInformation("Заявка студента - {Surname} {Name} {Patronymic} - была удалена", admission.Student.Surname, admission.Student.Name, admission.Student.Patronymic);
            }

            return Ok();
        }

        private async Task<bool> AdmissionExists(int id)
        {
            var admissions = await _admissionsRepository.GetAllAsync();
            return admissions.Any(e => e.Id == id);
        }

        private static List<Admission> SearchAdmissions(string? searchStudents, List<Admission>? admissions)
        {
            admissions ??= new();

            if (searchStudents != null)
            {
                List<string> searchWords = searchStudents.Split(" ").ToList();
                foreach (string word in searchWords)
                {
                    admissions = admissions.Where(i => i.Student.Name.ToLower().Contains(word.ToLower())).ToList()
                        .Union(admissions.Where(i => i.Student.Surname.ToLower().Contains(word.ToLower()))).Distinct()
                        .Union(admissions.Where(i => i.Student.Patronymic.ToLower().Contains(word.ToLower()))).Distinct()
                        .ToList();
                }
            }

            return admissions;
        }
    }
}
