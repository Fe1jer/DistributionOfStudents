using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories;
using DistributionOfStudents.Data.Services;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.ViewModels.Distribution;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DistributionOfStudents.Controllers
{
    [Route("Faculties/{facultyName}/{groupId}/[controller]/[action]")]
    public class DistributionController : Controller
    {
        private readonly ILogger<DistributionController> _logger;
        private readonly IGroupsOfSpecialitiesRepository _groupsRepository;
        private readonly IRecruitmentPlansRepository _plansRepository;
        private readonly IStudentsRepository _studentRepository;

        public DistributionController(ILogger<DistributionController> logger, IGroupsOfSpecialitiesRepository groupsRepository,
            IRecruitmentPlansRepository plansRepository, IStudentsRepository studentRepository)
        {
            _logger = logger;
            _groupsRepository = groupsRepository;
            _plansRepository = plansRepository;
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Create(string facultyName, int groupId)
        {
            CreateDistributionVM model;
            List<RecruitmentPlan> plans;
            GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification(facultyName).IncludeAdmissions().IncludeSpecialties());

            if (group == null)
            {
                return NotFound();
            }
            plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
            DistributionService distributionService = new(plans, group.Admissions);
            List<RecruitmentPlan> plansWithEnrolledStudents = distributionService.GetPlansWithEnrolledStudents();
            if (!distributionService.AreControversialStudents())
            {
                return RedirectToAction("ConfirmDistribution", new { facultyName, groupId, jsonPlans = CreateJsonOfPlansWithEnrolledStudents(plansWithEnrolledStudents) });
            }

            model = new()
            {
                GroupName = group.Name,
                Plans = GetDistributedPlans(plansWithEnrolledStudents, group.Admissions ?? new())
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string facultyName, int groupId, CreateDistributionVM model)
        {
            if (ModelState.IsValid)
            {
                List<RecruitmentPlan> plans;
                GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification(facultyName).IncludeAdmissions().IncludeSpecialties());
                if (group == null)
                {
                    return NotFound();
                }
                try
                {
                    plans = await GetPlansFromModel(model, facultyName, group);
                    DistributionService distributionService = new(plans, group.Admissions);
                    List<RecruitmentPlan> plansWithEnrolledStudents = distributionService.GetPlansWithEnrolledStudents();
                    if (!distributionService.AreControversialStudents())
                    {
                        return RedirectToAction("ConfirmDistribution", new { facultyName, groupId, jsonPlans = CreateJsonOfPlansWithEnrolledStudents(plansWithEnrolledStudents) });
                    }
                    model.Plans = GetDistributedPlans(plansWithEnrolledStudents, group.Admissions ?? new());

                    return View(model);
                }
                catch
                {
                    return View(model);
                }
            }
            foreach (var plan in model.Plans)
            {
                foreach (IsDistributedStudentVM DistributedStudent in plan.DistributedStudents)
                {
                    DistributedStudent.IsDistributed = !(plan.Count < plan.DistributedStudents.Count && DistributedStudent.Score == plan.PassingScore);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDistribution(string facultyName, int groupId, string jsonPlans)
        {
            ConfirmDistributionVM model;
            List<RecruitmentPlan> plans = new();
            Dictionary<int, List<int>> plansWithEnrolledStudents = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(jsonPlans) ?? new();
            GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }
            foreach (KeyValuePair<int, List<int>> keyValuePair in plansWithEnrolledStudents)
            {
                RecruitmentPlan? plan = await _plansRepository.GetByIdAsync(keyValuePair.Key, new RecruitmentPlansSpecification().IncludeSpecialty());
                if (plan != null)
                {
                    plan.EnrolledStudents = new();
                    foreach (int studentId in keyValuePair.Value)
                    {
                        Student? student = await _studentRepository.GetByIdAsync(studentId);
                        if (student != null)
                        {
                            plan.EnrolledStudents.Add(new EnrolledStudent() { Student = student });
                        }
                    }
                    plans.Add(plan);
                }
            }

            model = new()
            {
                GroupName = group.Name,
                Plans = GetConfirmDistributedPlans(plans)
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDistribution(string facultyName, int groupId, ConfirmDistributionVM model)
        {
            try
            {
                GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification(facultyName).IncludeAdmissions().IncludeSpecialties());
                if (group == null)
                {
                    return NotFound();
                }
                List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().IncludeEnrolledStudents().WhereFaculty(facultyName).WhereGroup(group));
                if (plans.Select(i => i.EnrolledStudents).Any(i => i != null && i.Count > 0))
                {
                    ModelState.AddModelError(string.Empty, "Невозможно распределить студентов, так как уже существуют зачисленные студенты на этих специальностях");
                    return View(model);
                }
                plans = await GetPlansFromModel(model, facultyName, group);
                DistributionService distributionService = new(plans, group.Admissions);
                plans = distributionService.GetPlansWithPassingScores();
                group.IsCompleted = true;
                foreach (var plan in plans)
                {
                    await _plansRepository.UpdateAsync(plan);
                }
                await _groupsRepository.UpdateAsync(group);
                _logger.LogInformation("Студенты в группе {GroupName} на факультете {FacultyShortName} были зачислены", group.Name, facultyName);

                return RedirectToAction("Details", "GroupsOfSpecialties", new { facultyName, id = groupId });
            }
            catch
            {
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(string facultyName, int groupId)
        {
            GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification(facultyName).IncludeAdmissions().IncludeSpecialties());
            if (group == null)
            {
                return NotFound();
            }
            List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().IncludeEnrolledStudents().WhereFaculty(facultyName).WhereGroup(group));

            group.IsCompleted = false;
            await _groupsRepository.UpdateAsync(group);
            foreach (RecruitmentPlan plan in plans)
            {
                plan.EnrolledStudents = null;
                await _plansRepository.UpdateAsync(plan);
            }
            _logger.LogInformation("Зачисленные студенты в группе {GroupName} на факультете {FacultyShortName} были удалены", group.Name, facultyName);

            return RedirectToAction("Details", "GroupsOfSpecialties", new { facultyName, id = groupId });
        }

        private async Task<List<RecruitmentPlan>> GetPlansFromModel(CreateDistributionVM model, string facultyName, GroupOfSpecialties group)
        {
            List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
            foreach (PlanForDistributionVM distributedPlan in model.Plans)
            {
                RecruitmentPlan? plan = await _plansRepository.GetByIdAsync(distributedPlan.PlanId, new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
                if (plan != null)
                {
                    plan.PassingScore = distributedPlan.PassingScore;
                    plan.EnrolledStudents = new();
                    foreach (IsDistributedStudentVM distributedStudent in distributedPlan.DistributedStudents.Where(i => i.IsDistributed))
                    {
                        Student? student = await _studentRepository.GetByIdAsync(distributedStudent.Student.Id);
                        if (student != null)
                        {
                            plan.EnrolledStudents.Add(new EnrolledStudent() { Student = student });
                        }
                    }
                    plans = plans.Select(i => i.Id != plan.Id ? i : plan).ToList();
                }
            }

            return plans;
        }

        private async Task<List<RecruitmentPlan>> GetPlansFromModel(ConfirmDistributionVM model, string facultyName, GroupOfSpecialties group)
        {
            List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
            foreach (ConfirmDistributedPlanVM distributedPlan in model.Plans)
            {
                RecruitmentPlan plan = plans.First(i => i.Id == distributedPlan.PlanId);
                plan.PassingScore = distributedPlan.PassingScore;
                plan.EnrolledStudents = new();
                foreach (ConfirmDistributedStudentVM distributedStudent in distributedPlan.DistributedStudents)
                {
                    Student? student = await _studentRepository.GetByIdAsync(distributedStudent.StudentId);
                    if (student != null)
                    {
                        plan.EnrolledStudents.Add(new EnrolledStudent() { Student = student });
                    }
                }
            }

            return plans;
        }

        private static string CreateJsonOfPlansWithEnrolledStudents(List<RecruitmentPlan> plansWithEnrolledStudents)
        {
            Dictionary<int, List<int>> readyPlans = new();
            plansWithEnrolledStudents.ForEach(plan => readyPlans.Add(plan.Id, new List<int>((plan.EnrolledStudents ?? new()).Select(i => i.Student.Id))));
            return JsonConvert.SerializeObject(readyPlans);
        }

        private static List<PlanForDistributionVM> GetDistributedPlans(List<RecruitmentPlan> plans, List<Admission> admissions)
        {
            List<PlanForDistributionVM> distributedPlans = new();
            bool isControversialPlan = false;

            foreach (RecruitmentPlan plan in plans.OrderByDescending(i => i.PassingScore))
            {
                PlanForDistributionVM distributedPlan = new(plan);
                List<IsDistributedStudentVM> distributedStudents = new();
                plan.EnrolledStudents ??= new();
                foreach (EnrolledStudent student in plan.EnrolledStudents)
                {
                    Admission studentAdmission = admissions.First(i => i.Student.Id == student.Student.Id);
                    distributedStudents.Add(new(studentAdmission, plan));
                    isControversialPlan = plan.Count < plan.EnrolledStudents.Count;
                }
                distributedPlan.DistributedStudents = distributedStudents.OrderByDescending(i => i.Score).ToList();
                distributedPlans.Add(distributedPlan);
                if (isControversialPlan) break;
            }

            return distributedPlans;
        }

        private static List<ConfirmDistributedPlanVM> GetConfirmDistributedPlans(List<RecruitmentPlan> plans)
        {
            List<ConfirmDistributedPlanVM> distributedPlans = new();

            foreach (RecruitmentPlan plan in plans.OrderBy(f => int.Parse(string.Join("", f.Speciality.Code.Where(c => char.IsDigit(c))))))
            {
                distributedPlans.Add(new(plan));
            }

            return distributedPlans;
        }
    }
}
