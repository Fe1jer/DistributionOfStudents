using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels.Admissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistributionOfStudents.Controllers
{
    [Route("Faculties/{facultyName}/{groupId}/[controller]/[action]")]
    public class AdmissionsController : Controller
    {
        private readonly ILogger<AdmissionsController> _logger;
        private readonly IAdmissionsRepository _admissionsRepository;
        private readonly IGroupsOfSpecialitiesRepository _groupsRepository;
        private readonly IRecruitmentPlansRepository _planRepository;
        private readonly ISubjectsRepository _subjectsRepository;

        public AdmissionsController(ILogger<AdmissionsController> logger, IAdmissionsRepository admissionsRepository, IGroupsOfSpecialitiesRepository groupsRepository,
            IRecruitmentPlansRepository planRepository, ISubjectsRepository subjectsRepository)
        {
            _logger = logger;
            _admissionsRepository = admissionsRepository;
            _groupsRepository = groupsRepository;
            _planRepository = planRepository;
            _subjectsRepository = subjectsRepository;
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
            List<RecruitmentPlan> plans = await _planRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
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
                    RecruitmentPlan? plan = await _planRepository.GetByIdAsync(specialityPriority.PlanId);
                    if (plan != null)
                    {
                        SpecialityPriority priority = new() { Priority = specialityPriority.Priority, RecruitmentPlan = plan };
                        specialityPriorities.Add(priority);
                    }
                }

                Admission admission = new()
                {
                    GroupOfSpecialties = await _groupsRepository.GetByIdAsync(groupId) ?? new(),
                    StudentScores = model.StudentScores,
                    Student = new() { Name = model.Student.Name, Surname = model.Student.Surname, Patronymic = model.Student.Patronymic, GPS = model.Student.GPS },
                    DateOfApplication = model.DateOfApplication,
                    SpecialityPriorities = specialityPriorities
                };

                await _admissionsRepository.AddAsync(admission);

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

            List<RecruitmentPlan> plans = await _planRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
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
                        new() { RecruitmentPlan = await _planRepository.GetByIdAsync(specialityPriority.PlanId) ?? new() };
                    priority.Priority = specialityPriority.Priority;
                    specialityPriorities.Add(priority);
                }

                admission.SpecialityPriorities = specialityPriorities;
                admission.Student.Surname = model.Student.Surname;
                admission.Student.Name = model.Student.Name;
                admission.Student.Patronymic = model.Student.Patronymic;
                admission.Student.GPS = model.Student.GPS;

                await _admissionsRepository.UpdateAsync(admission);

                _logger.LogInformation("Заявка студента - {Surname} {Name} {Patronymic} - изменена", admission.Student.Surname, admission.Student.Name, admission.Student.Patronymic);
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
                _logger.LogInformation("Заявка студента - {Surname} {Name} {Patronymic} - была удалена", admission.Student.Surname, admission.Student.Name, admission.Student.Patronymic);
            }

            return RedirectToAction("Details", "GroupsOfSpecialties", new { facultyName, id = groupId });
        }

        private async Task<bool> AdmissionExists(int id)
        {
            var admissions = await _admissionsRepository.GetAllAsync();
            return admissions.Any(e => e.Id == id);
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
