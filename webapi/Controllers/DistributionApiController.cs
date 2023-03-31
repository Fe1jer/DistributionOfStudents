using Microsoft.AspNetCore.Mvc;
using webapi.Data.Interfaces.Repositories;
using webapi.Data.Models;
using webapi.Data.Services;
using webapi.Data.Specifications;
using webapi.ViewModels.Distribution;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributionApiController : ControllerBase
    {
        private readonly ILogger<DistributionApiController> _logger;
        private readonly IGroupsOfSpecialitiesRepository _groupsRepository;
        private readonly IRecruitmentPlansRepository _plansRepository;
        private readonly IStudentsRepository _studentRepository;

        public DistributionApiController(ILogger<DistributionApiController> logger, IGroupsOfSpecialitiesRepository groupsRepository,
            IRecruitmentPlansRepository plansRepository, IStudentsRepository studentRepository)
        {
            _logger = logger;
            _groupsRepository = groupsRepository;
            _plansRepository = plansRepository;
            _studentRepository = studentRepository;
        }

        [HttpGet("{facultyName}/{groupId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetDistribution(string facultyName, int groupId)
        {
            List<RecruitmentPlan> plans;
            GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeAdmissions().IncludeSpecialties());

            if (group == null)
            {
                return NotFound();
            }
            plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
            DistributionService distributionService = new(plans, group.Admissions);
            List<RecruitmentPlan> plansWithEnrolledStudents = distributionService.GetPlansWithEnrolledStudents();
            if (!distributionService.AreControversialStudents())
            {
                return GetConfirmDistributedPlans(plansWithEnrolledStudents);
            }

            return GetDistributedPlans(plansWithEnrolledStudents, group.Admissions ?? new());
        }

        [HttpPost("{facultyName}/{groupId}/CreateDistribution")]
        public async Task<ActionResult<IEnumerable<object>>> CreateDistribution(string facultyName, int groupId, List<PlanForDistributionVM> models)
        {
            if (ModelState.IsValid)
            {
                GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification(facultyName).IncludeAdmissions().IncludeSpecialties());
                if (group == null)
                {
                    return NotFound();
                }
                try
                {
                    DistributionService distributionService = new(await GetPlansFromModelAsync(models, facultyName, group), group.Admissions);
                    List<RecruitmentPlan> plansWithEnrolledStudents = distributionService.GetPlansWithEnrolledStudents();
                    if (!distributionService.AreControversialStudents())
                    {
                        return GetConfirmDistributedPlans(plansWithEnrolledStudents);
                    }

                    return GetDistributedPlans(plansWithEnrolledStudents, group.Admissions ?? new());
                }
                catch
                {
                    return BadRequest(ModelState);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost("{facultyName}/{groupId}/ConfirmDistribution")]
        public async Task<IActionResult> ConfirmDistribution(string facultyName, int groupId, List<ConfirmDistributedPlanVM> models)
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
                    return BadRequest(ModelState);
                }
                plans = await GetPlansFromModelAsync(models, facultyName, group);
                DistributionService distributionService = new(plans, group.Admissions);
                foreach (var plan in distributionService.GetPlansWithPassingScores())
                {
                    await _plansRepository.UpdateAsync(plan);
                }
                group.IsCompleted = true;
                await _groupsRepository.UpdateAsync(group);
                _logger.LogInformation("Студенты в группе {GroupName} на факультете {FacultyShortName} были зачислены", group.Name, facultyName);

                return Ok();
            }
            catch
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{facultyName}/{groupId}")]
        public async Task<IActionResult> DeleteDistribution(string facultyName, int groupId)
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

            return Ok();
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
                    distributedStudents.Add(new(admissions.First(i => i.Student.Id == student.Student.Id), plan));
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

        private async Task<List<RecruitmentPlan>> GetPlansFromModelAsync(IEnumerable<PlanForDistributionVM> models, string facultyName, GroupOfSpecialties group)
        {
            List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
            foreach (PlanForDistributionVM distributedPlan in models)
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

        private async Task<List<RecruitmentPlan>> GetPlansFromModelAsync(List<ConfirmDistributedPlanVM> models, string facultyName, GroupOfSpecialties group)
        {
            List<RecruitmentPlan> plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
            foreach (ConfirmDistributedPlanVM distributedPlan in models)
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
    }
}
