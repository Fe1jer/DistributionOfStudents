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

        [HttpGet("{facultyName}/{groupId}/Competition")]
        public async Task<ActionResult<float>> GetCompetition(string facultyName, int groupId)
        {
            List<RecruitmentPlan> plans;
            GroupOfSpecialties? group = await _groupsRepository.GetByIdAsync(groupId, new GroupsOfSpecialitiesSpecification().IncludeAdmissions().IncludeSpecialties());

            if (group == null)
            {
                return NotFound();
            }
            plans = await _plansRepository.GetAllAsync(new RecruitmentPlansSpecification().WhereFaculty(facultyName).WhereGroup(group));
            DistributionService distributionService = new(plans, group.Admissions);

            return (float)Math.Round(distributionService.Competition, 2);
        }

        [HttpGet("{facultyName}/{groupId}")]
        public async Task<ActionResult<object>> GetDistribution(string facultyName, int groupId)
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
                foreach (RecruitmentPlan plan in plansWithEnrolledStudents)
                {
                    plan.Speciality.GroupsOfSpecialties = null;
                    plan.Speciality.RecruitmentPlans = null;
                    foreach (EnrolledStudent enrolledStudent in plan.EnrolledStudents ?? new())
                    {
                        foreach (Admission admission in enrolledStudent.Student.Admissions ?? new())
                        {
                            admission.Student = new();
                            admission.GroupOfSpecialties = new();
                            admission.SpecialityPriorities = new();
                        }
                    }
                }
                return new { plans = plansWithEnrolledStudents.OrderBy(f => int.Parse(string.Join("", f.Speciality.Code.Where(c => char.IsDigit(c))))).ToList(), areControversialStudents = false };
            }

            return new { plans = GetDistributedPlans(plansWithEnrolledStudents, group.Admissions ?? new()), areControversialStudents = true };
        }

        [HttpPost("{facultyName}/{groupId}/CreateDistribution")]
        public async Task<ActionResult<object>> CreateDistribution(string facultyName, int groupId, List<PlanForDistributionVM> models)
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
                        foreach (RecruitmentPlan plan in plansWithEnrolledStudents)
                        {
                            plan.Speciality.GroupsOfSpecialties = null;
                            plan.Speciality.RecruitmentPlans = null;
                            foreach (EnrolledStudent enrolledStudent in plan.EnrolledStudents ?? new())
                            {
                                foreach (Admission admission in enrolledStudent.Student.Admissions ?? new())
                                {
                                    admission.Student = new();
                                    admission.GroupOfSpecialties = new();
                                    admission.SpecialityPriorities = new();
                                }
                            }
                        }
                        return new { plans = plansWithEnrolledStudents.OrderBy(f => int.Parse(string.Join("", f.Speciality.Code.Where(c => char.IsDigit(c))))).ToList(), areControversialStudents = false };
                    }

                    return new { plans = GetDistributedPlans(plansWithEnrolledStudents, group.Admissions ?? new()), areControversialStudents = true };
                }
                catch
                {
                    return BadRequest(ModelState);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost("{facultyName}/{groupId}/ConfirmDistribution")]
        public async Task<IActionResult> ConfirmDistribution(string facultyName, int groupId, List<PlanForDistributionVM> models)
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
                plan.PassingScore = 0;
                plan.EnrolledStudents = null;
                await _plansRepository.UpdateAsync(plan);
            }
            _logger.LogInformation("Зачисленные студенты в группе {GroupName} на факультете {FacultyShortName} были удалены", group.Name, facultyName);

            return Ok();
        }

        private static List<RecruitmentPlan> GetDistributedPlans(List<RecruitmentPlan> plans, List<Admission> admissions)
        {
            List<RecruitmentPlan> distributedPlans = new();
            bool isControversialPlan = false;

            foreach (RecruitmentPlan plan in plans.OrderByDescending(i => i.PassingScore))
            {
                plan.Speciality.GroupsOfSpecialties = null;
                plan.Speciality.RecruitmentPlans = null;
                plan.EnrolledStudents ??= new();
                foreach (EnrolledStudent student in plan.EnrolledStudents)
                {
                    student.Student.Admissions = admissions.Where(i => i.Student.Id == student.Student.Id).ToList();
                    foreach (Admission admission in student.Student.Admissions ?? new())
                    {
                        admission.Student = new();
                        admission.GroupOfSpecialties = new();
                        admission.SpecialityPriorities = new();
                    }
                    isControversialPlan = plan.Count < plan.EnrolledStudents.Count;
                }
                plan.EnrolledStudents = plan.EnrolledStudents.OrderByDescending(i => i.Student.GPS + (i.Student.Admissions != null ? i.Student.Admissions[0].StudentScores.Sum(s => s.Score) : new())).ToList();
                distributedPlans.Add(plan);
                if (isControversialPlan) break;
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
                        Student? student = await _studentRepository.GetByIdAsync(distributedStudent.StudentId);
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
    }
}
