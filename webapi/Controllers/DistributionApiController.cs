using BLL.DTO.RecruitmentPlans;
using BLL.Extensions;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapi.ViewModels.Distribution;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributionApiController : BaseController
    {
        private readonly ILogger<DistributionApiController> _logger;
        private readonly IDistributionService _service;

        public DistributionApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<DistributionApiController> logger, IDistributionService service) : base(accessor, generator)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{facultyUrl}/{groupId}/Competition")]
        public async Task<ActionResult<float>> GetCompetition(string facultyUrl, Guid groupId) => await _service.GetCompetitionAsync(facultyUrl, groupId);

        [HttpGet("{facultyUrl}/{groupId}")]
        [Authorize(Roles = "commission")]
        public async Task<ActionResult<object>> GetDistribution(string facultyUrl, Guid groupId)
        {
            var distributedPlans = await _service.GetAsync(facultyUrl, groupId);

            if (!distributedPlans.AreControversialStudents())
            {
                return new { plans = distributedPlans.Keys.OrderBy(f => int.Parse(string.Join("", f.Speciality.Code.Where(c => char.IsDigit(c))))).ToList(), areControversialStudents = false };
            }

            //TODO: вывод в списке заявок аббитуриаентов приоритет специальностей
            //TODO: Смапить во вьюшки, чтобы не было цикличности зависимостей
            return new { plans = distributedPlans.Keys, areControversialStudents = true };
        }

        [HttpPost("{facultyUrl}/{groupId}/CreateDistribution")]
        [Authorize(Roles = "commission")]
        public async Task<ActionResult<object>> CreateDistribution(string facultyUrl, Guid groupId, List<PlanForDistributionViewModel> models)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var plans = Mapper.Map<List<PlanForDistributionDTO>>(models);
                    var distributedPlans = await _service.CreateAsync(facultyUrl, groupId, plans);

                    if (!distributedPlans.AreControversialStudents())
                    {
                        return new { plans = distributedPlans.Keys.OrderBy(f => int.Parse(string.Join("", f.Speciality.Code.Where(c => char.IsDigit(c))))).ToList(), areControversialStudents = false };
                    }

                    //TODO: Отбросить специальности после спорной
                    //TODO: Отсортировать студентов по баллам по убыванию
                    return new { plans = distributedPlans.Keys, areControversialStudents = true };
                }
                catch
                {
                    return BadRequest(ModelState);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost("{facultyUrl}/{groupId}/ConfirmDistribution")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> ConfirmDistribution(string facultyUrl, Guid groupId, bool notify, List<PlanForDistributionViewModel> models)
        {
            try
            {
                if (await _service.ExistsEnrolledStudentsAsync(facultyUrl, groupId))
                {
                    ModelState.AddModelError("modelErrors", "Невозможно распределить студентов, так как уже существуют зачисленные студенты на этих специальностях");
                    return BadRequest(ModelState);
                }
                var plans = Mapper.Map<List<PlanForDistributionDTO>>(models);
                await _service.SaveAsync(facultyUrl, groupId, plans, notify);

                _logger.LogInformation("Студенты в группе {GroupName} на факультете {FacultyShortName} были зачислены", groupId, facultyUrl);

                return Ok();
            }
            catch
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{facultyUrl}/{groupId}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> DeleteDistribution(string facultyUrl, Guid groupId)
        {
            await _service.DeleteAsync(facultyUrl, groupId);
            _logger.LogInformation("Зачисленные студенты в группе {GroupName} на факультете {FacultyShortName} были удалены", groupId, facultyUrl);

            return Ok();
        }

        /*private static List<RecruitmentPlanDTO> GetDistributedPlans(List<RecruitmentPlanDTO> plans, List<AdmissionDTO> admissions)
        {
            List<RecruitmentPlanDTO> distributedPlans = new();
            bool isControversialPlan = false;

            foreach (RecruitmentPlanDTO plan in plans.OrderByDescending(i => i.PassingScore))
            {
                plan.EnrolledStudents ??= new();
                foreach (EnrolledStudentDTO student in plan.EnrolledStudents)
                {
                    student.Student.Admissions = admissions.Where(i => i.Student.Id == student.Student.Id).ToList();

                    isControversialPlan = plan.Count < plan.EnrolledStudents.Count;
                }
                plan.EnrolledStudents = plan.EnrolledStudents.OrderByDescending(i => i.Student.GPA + (i.Student.Admissions != null ? i.Student.Admissions[0].StudentScores.Sum(s => s.Score) : new())).ToList();
                distributedPlans.Add(plan);
                if (isControversialPlan) break;
            }

            return distributedPlans;
        }*/
    }
}
