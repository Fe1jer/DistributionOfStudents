using ChartJSCore.Models;
using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Services;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels.Admissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DistributionOfStudents.Controllers
{
    [Route("Faculties/{facultyName}/{groupId}/[controller]/[action]")]
    public class AdmissionsController : Controller
    {
        private readonly ILogger<AdmissionsController> _logger;
        private readonly IAdmissionsRepository _admissionsRepository;
        private readonly IGroupsOfSpecialitiesRepository _groupsRepository;
        private readonly IRecruitmentPlansRepository _plansRepository;
        private readonly ISubjectsRepository _subjectsRepository;
        private readonly IGroupsOfSpecialitiesStatisticRepository _groupsStatisticRepository;
        private readonly IRecruitmentPlansStatisticRepository _plansStatisticRepository;

        public AdmissionsController(ILogger<AdmissionsController> logger, IAdmissionsRepository admissionsRepository, IGroupsOfSpecialitiesRepository groupsRepository,
            IRecruitmentPlansRepository plansRepository, ISubjectsRepository subjectsRepository,
            IGroupsOfSpecialitiesStatisticRepository groupsStatisticRepository, IRecruitmentPlansStatisticRepository plansStatisticRepository)
        {
            _logger = logger;
            _admissionsRepository = admissionsRepository;
            _groupsRepository = groupsRepository;
            _plansRepository = plansRepository;
            _subjectsRepository = subjectsRepository;
            _groupsStatisticRepository = groupsStatisticRepository;
            _plansStatisticRepository = plansStatisticRepository;
        }

        // GET: Admissions/Details/5
        public async Task<IActionResult> Details(string facultyName, int groupId, int id)
        {
            Admission? admission = await _admissionsRepository.GetByIdAsync(id, new AdmissionsSpecification().IncludeGroup().IncludeSpecialtyPriorities().IncludeStudentScores());

            if (admission == null)
            {
                return NotFound();
            }
            admission.SpecialityPriorities = admission.SpecialityPriorities.OrderBy(i => i.Priority).ToList();

            return View(admission);
        }

        // GET: Admissions/Create
        public async Task<IActionResult> Create(string facultyName, int groupId)
        {
            List<StudentScore> scores = new();
            List<SpecialityPriorityVM> priorities = new();
            GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification(facultyName).IncludeSpecialties().IncludeSubjects());
            if (group == null)
            {
                return NotFound();
            }
            List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
            plans = plans.Where(p => (group.Specialities ?? new()).Contains(p.Speciality)).ToList();
            (group.Subjects ?? new()).ForEach(subject => scores.Add(new StudentScore() { Subject = subject }));
            plans.ForEach(plan => priorities.Add(new SpecialityPriorityVM(plan)));

            CreateChangeAdmissionVM model = new(scores, priorities);

            return View(model);
        }

        // POST: Admissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string facultyName, int groupId, CreateChangeAdmissionVM model)
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
                    SpecialityPriorities = specialityPriorities
                };
                await _admissionsRepository.AddAsync(admission);
                await UpdateStatisticAsync(facultyName, groupId);
                _logger.LogInformation("Заявка студента - {Surname} {Name} {Patronymic} - добавлена", admission.Student.Surname, admission.Student.Name, admission.Student.Patronymic);

                return RedirectToAction("Details", "GroupsOfSpecialties", new { facultyName, id = groupId });
            }
            return View(model);
        }

        // GET: Admissions/Edit/5
        public async Task<IActionResult> Edit(string facultyName, int groupId, int id)
        {
            List<SpecialityPriorityVM> priorities;
            Admission? admission = await _admissionsRepository.GetByIdAsync(id, new AdmissionsSpecification().IncludeGroup().IncludeSpecialtyPriorities().IncludeStudentScores());
            GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification(facultyName).IncludeSpecialties().IncludeSubjects());
            if (admission == null || group == null)
            {
                return NotFound();
            }

            List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
            plans = plans.Where(p => (group.Specialities ?? new()).Contains(p.Speciality)).ToList();
            priorities = GetSpecialityPrioritiesVM(plans, admission);

            CreateChangeAdmissionVM model = new(id, admission, priorities);

            return View(model);
        }

        // POST: Admissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string facultyName, int groupId, CreateChangeAdmissionVM model)
        {
            if (ModelState.IsValid)
            {
                Admission? admission = await _admissionsRepository.GetByIdAsync(model.Id ?? 0, new AdmissionsSpecification().IncludeSpecialtyPriorities().IncludeStudentScores());
                if (admission == null)
                {
                    return RedirectToAction("Details", "GroupsOfSpecialties", new { facultyName, id = groupId });
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
                admission.Student.Surname = model.Student.Surname.Trim();
                admission.Student.Name = model.Student.Name.Trim();
                admission.Student.Patronymic = model.Student.Patronymic.Trim();
                admission.Student.GPS = model.Student.GPS;

                await _admissionsRepository.UpdateAsync(admission);
                await UpdateStatisticAsync(facultyName, groupId);

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
                return RedirectToAction("Details", "GroupsOfSpecialties", new { facultyName, id = groupId });
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string facultyName, int groupId, int id)
        {
            Admission? admission = await _admissionsRepository.GetByIdAsync(id, new AdmissionsSpecification());
            if (admission != null)
            {
                await _admissionsRepository.DeleteAsync(id);
                await UpdateStatisticAsync(facultyName, groupId);
                _logger.LogInformation("Заявка студента - {Surname} {Name} {Patronymic} - была удалена", admission.Student.Surname, admission.Student.Name, admission.Student.Patronymic);
            }

            return RedirectToAction("Details", "GroupsOfSpecialties", new { facultyName, id = groupId });
        }

        private async Task<bool> AdmissionExists(int id)
        {
            var admissions = await _admissionsRepository.GetAllAsync();
            return admissions.Any(e => e.Id == id);
        }

        private async Task UpdateStatisticAsync(string facultyName, int groupId)
        {
            GroupOfSpecialties group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeSpecialties().IncludeAdmissions()) ?? new();
            await UpdateGroupStatisticAsync(group);
            await UpdatePlansStatisticAsync(facultyName, group);
        }

        private async Task UpdateGroupStatisticAsync(GroupOfSpecialties group)
        {
            GroupOfSpecialitiesStatistic groupStatistic = await _groupsStatisticRepository.GetByGroupAndDateAsync(group.Id, DateTime.Today)
                ?? new() { Date = DateTime.Today, GroupOfSpecialties = group };
            groupStatistic.CountOfAdmissions = (group.Admissions ?? new()).Count;
            Task task = groupStatistic.Id != 0 ? _groupsStatisticRepository.UpdateAsync(groupStatistic) : _groupsStatisticRepository.AddAsync(groupStatistic);
            await task;
        }

        private async Task UpdatePlansStatisticAsync(string facultyName, GroupOfSpecialties group)
        {
            List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
            DistributionService distributionService = new(plans, group.Admissions);
            plans = distributionService.GetPlansWithEnrolledStudents();
            foreach (var plan in plans)
            {
                RecruitmentPlanStatistic planStatistic = await _plansStatisticRepository.GetByPlanAndDateAsync(plan.Id, DateTime.Today)
                    ?? new() { Date = DateTime.Today, RecruitmentPlan = await _plansRepository.GetByIdAsync(plan.Id) ?? new() };
                planStatistic.Score = plan.PassingScore;
                Task task = planStatistic.Id != 0 ? _plansStatisticRepository.UpdateAsync(planStatistic) : _plansStatisticRepository.AddAsync(planStatistic);
                await task;
            }
        }

        private static List<SpecialityPriorityVM> GetSpecialityPrioritiesVM(List<RecruitmentPlan> plans, Admission admission)
        {
            List<SpecialityPriorityVM> priorities = new();
            foreach (RecruitmentPlan plan in plans)
            {
                SpecialityPriorityVM priority = new(plan);
                foreach (SpecialityPriority admissionPriority in admission.SpecialityPriorities)
                {
                    if (plan.Id == admissionPriority.RecruitmentPlan.Id)
                    {
                        priority.Priority = admissionPriority.Priority;
                    }
                }
                priorities.Add(priority);
            }

            return priorities;
        }
    }
}
