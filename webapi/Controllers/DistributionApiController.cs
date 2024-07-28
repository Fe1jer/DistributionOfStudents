using BLL.DTO.RecruitmentPlans;
using BLL.Extensions;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapi.ViewModels.Distribution;
using webapi.ViewModels.RecruitmentPlans;

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
            List<DistributedPlanViewModel> result;
            var distributedPlans = await _service.GetAsync(facultyUrl, groupId);

            if (!distributedPlans.AreControversialStudents())
            {
                result = Mapper.Map<List<DistributedPlanViewModel>>(distributedPlans.Keys);
                return new { plans = result.OrderBy(s => int.TryParse(s.Speciality.Code, out int n) ? n : int.MaxValue), areControversialStudents = false };
            }

            result = Mapper.Map<List<DistributedPlanViewModel>>(GetDistributedPlans(distributedPlans.Keys.ToList()));
            return new { plans = result, areControversialStudents = true };
        }

        [HttpPost("{facultyUrl}/{groupId}/CreateDistribution")]
        [Authorize(Roles = "commission")]
        public async Task<ActionResult<object>> CreateDistribution(string facultyUrl, Guid groupId, List<PlanForDistributionViewModel> models)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<DistributedPlanViewModel> result;
                    var plans = Mapper.Map<List<PlanForDistributionDTO>>(models);
                    var distributedPlans = await _service.CreateAsync(facultyUrl, groupId, plans);

                    if (!distributedPlans.AreControversialStudents())
                    {
                        result = Mapper.Map<List<DistributedPlanViewModel>>(distributedPlans.Keys);
                        return new { plans = result.OrderBy(s => int.TryParse(s.Speciality.Code, out int n) ? n : int.MaxValue), areControversialStudents = false };
                    }

                    //TODO: Отбросить специальности после спорной
                    result = Mapper.Map<List<DistributedPlanViewModel>>(GetDistributedPlans(distributedPlans.Keys.ToList()));
                    return new { plans = result, areControversialStudents = true };
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
            catch(Exception ex)
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

        private static List<RecruitmentPlanDTO> GetDistributedPlans(List<RecruitmentPlanDTO> plans)
        {
            List<RecruitmentPlanDTO> distributedPlans = new();

            foreach (RecruitmentPlanDTO plan in plans.OrderByDescending(i => i.PassingScore))
            {
                distributedPlans.Add(plan);
                if (plan.Count < plan.EnrolledStudents.Count) break;
            }

            return distributedPlans;
        }
    }
}
