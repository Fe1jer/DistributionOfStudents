using AutoMapper;
using BLL.DTO;
using BLL.DTO.GroupsOfSpecialities;
using BLL.DTO.RecruitmentPlans;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Postgres.Repositories.Interfaces.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapi.ViewModels.Faculties;
using webapi.ViewModels.GroupsOfSpecialities;
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
        public async Task<ActionResult<RecruitmentPlanViewModel>> GetRecruitmentPlan(Guid id)
        {
            RecruitmentPlanViewModel model = Mapper.Map<RecruitmentPlanViewModel>(await _service.GetAsync(id));

            return model != null ? model : NotFound();
        }

        [HttpGet("{facultyName}/{year}")]
        public async Task<ActionResult<IEnumerable<SpecialityPlansViewModel>>> GetFacultyPlans(string facultyName, int year)
            => Mapper.Map<List<SpecialityPlansViewModel>>(await _service.GetByFacultyAsync(facultyName, year));

        [HttpGet("{facultyName}/lastYear")]
        public async Task<ActionResult<DetailsFacultyPlansViewModel>> GetFacultyLastYearRecruitmentPlans(string facultyName)
            => Mapper.Map<DetailsFacultyPlansViewModel>(await _service.GetLastByFacultyAsync(facultyName));

        [HttpGet("{facultyName}/{groupId}/GroupPlans")]
        public async Task<ActionResult<IEnumerable<RecruitmentPlanViewModel>>> GetGroupPlans([FromServices] IAdmissionsService admissionsService,
            [FromServices] IGroupsOfSpecialitiesService groupsService, string facultyName, Guid groupId)
        {
            GroupOfSpecialitiesDTO? group = await groupsService.GetAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            List<RecruitmentPlanDTO> plans = await _service.GetByGroupAsync(groupId);

            if (!group.IsCompleted)
            {
                List<AdmissionDTO> admissions = await admissionsService.GetByGroupAsync(groupId);
                IDistributionService distributionService = new DistributionService(plans, admissions);
                plans = distributionService.GetPlansWithEnrolledStudents();
            }

            return Mapper.Map<List<RecruitmentPlanViewModel>>(plans);
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

        [HttpPut("{facultyName}/{year}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> PutFacultyRecruitmentPlans(string facultyName, int year, IEnumerable<SpecialityPlansViewModel> plansForSpecialities)
        {
            if (ModelState.IsValid)
            {
                var updateDto = Mapper.Map<List<SpecialityPlansDTO>>(plansForSpecialities);
                await _service.SaveAsync(updateDto, facultyName, year);

                _logger.LogInformation("План приёма на - {FacultyName} - за {Year} обновлён", facultyName, year);

                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost("{facultyName}/{year}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> PostFacultyRecruitmentPlans(string facultyName, int year, IEnumerable<SpecialityPlansViewModel> plansForSpecialities)
        {
            if (ModelState.IsValid)
            {
                var updateDto = Mapper.Map<List<SpecialityPlansDTO>>(plansForSpecialities);
                await _service.SaveAsync(updateDto, facultyName, year);

                _logger.LogInformation("План приёма на - {FacultyName} - за {Year} создан", facultyName, year);

                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{facultyName}/{year}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> DeleteRecruitmentPlan(string facultyName, int year)
        {
            try
            {
                await _service.DeleteAsync(facultyName, year);
                _logger.LogInformation("План приёма на - {FacultyName} - за {Year} год удалён", facultyName, year);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Удаление плана набора");
            }
            return Ok();
        }
    }
}
