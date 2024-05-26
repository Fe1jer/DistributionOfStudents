using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.RecruitmentPlans;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.ViewModels.RecruitmentPlans;

namespace webapi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruitmentPlansApiController : BaseController
    {
        private readonly ILogger<RecruitmentPlansApiController> _logger;
        private readonly IRecruitmentPlansService _service;

        public RecruitmentPlansApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<RecruitmentPlansApiController> logger, IRecruitmentPlansService service) : base(accessor, generator)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetailsFacultyPlansViewModel>>> GetFacultiesPlans()
            => Mapper.Map<List<DetailsFacultyPlansViewModel>>(await _service.GetLastFacultiesPlansAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<RecruitmentPlanItemViewModel>> GetRecruitmentPlan(Guid id)
        {
            RecruitmentPlanItemViewModel model = Mapper.Map<RecruitmentPlanItemViewModel>(await _service.GetAsync(id));

            return model != null ? model : NotFound();
        }

        [HttpGet("{facultyUrl}/{year}")]
        public async Task<ActionResult<IEnumerable<SpecialityPlansViewModel>>> GetFacultyPlans(string facultyUrl, int year)
            => Mapper.Map<List<SpecialityPlansViewModel>>(await _service.GetByFacultyAsync(facultyUrl, year));

        [HttpGet("{facultyUrl}/lastYear")]
        public async Task<ActionResult<DetailsFacultyPlansViewModel>> GetFacultyLastYearRecruitmentPlans(string facultyUrl)
            => Mapper.Map<DetailsFacultyPlansViewModel>(await _service.GetLastByFacultyAsync(facultyUrl));

        [HttpGet("{facultyUrl}/{groupId}/GroupPlans")]
        public async Task<ActionResult<IEnumerable<RecruitmentPlanItemViewModel>>> GetGroupPlans([FromServices] IGroupsOfSpecialitiesService groupsService,
            [FromServices] IDistributionService distributionService, string facultyUrl, Guid groupId)
        {
            GroupOfSpecialitiesDTO? group = await groupsService.GetAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            List<RecruitmentPlanDTO> plans = await _service.GetByGroupAsync(groupId);

            if (!group.IsCompleted)
            {
                var distribution = await distributionService.GetAsync(facultyUrl, groupId);
                plans = distribution.Keys.ToList();
            }

            return Mapper.Map<List<RecruitmentPlanItemViewModel>>(plans);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> PutRecruitmentPlan(Guid id, RecruitmentPlanViewModel model)
        {
            if (ModelState.IsValid)
            {
                RecruitmentPlanDTO plan = await _service.GetAsync(id);
                if (plan == null)
                {
                    return NotFound();
                }
                plan.Count = model.Count;
                plan.Target = model.Target;

                await _service.SaveAsync(plan);

                _logger.LogInformation("План приёма на - {SpecialityName} - обновлён", plan.Speciality.FullName);

                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{facultyUrl}/{year}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> PutFacultyRecruitmentPlans(string facultyUrl, int year, IEnumerable<SpecialityPlansViewModel> plansForSpecialities)
        {
            if (ModelState.IsValid)
            {
                var updateDto = Mapper.Map<List<SpecialityPlansDTO>>(plansForSpecialities);
                await _service.SaveAsync(updateDto, facultyUrl, year);

                _logger.LogInformation("План приёма на - {facultyUrl} - за {Year} обновлён", facultyUrl, year);

                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost("{facultyUrl}/{year}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> PostFacultyRecruitmentPlans(string facultyUrl, int year, IEnumerable<SpecialityPlansViewModel> plansForSpecialities)
        {
            if (ModelState.IsValid)
            {
                var updateDto = Mapper.Map<List<SpecialityPlansDTO>>(plansForSpecialities);
                await _service.SaveAsync(updateDto, facultyUrl, year);

                _logger.LogInformation("План приёма на - {facultyUrl} - за {Year} создан", facultyUrl, year);

                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{facultyUrl}/{year}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> DeleteRecruitmentPlan(string facultyUrl, int year)
        {
            try
            {
                await _service.DeleteAsync(facultyUrl, year);
                _logger.LogInformation("План приёма на - {facultyUrl} - за {Year} год удалён", facultyUrl, year);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Удаление плана набора");
            }
            return Ok();
        }
    }
}
