using BLL.DTO.Specialities;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.ViewModels.Specialities;

namespace webapi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialitiesApiController : BaseController
    {
        private readonly ILogger<SpecialitiesApiController> _logger;
        private readonly ISpecialitiesService _service;

        public SpecialitiesApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<SpecialitiesApiController> logger, ISpecialitiesService service) : base(accessor, generator)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("FacultySpecialities/{facultyUrl}")]
        public async Task<ActionResult<IEnumerable<SpecialityViewModel>>> GetFacultySpecialities(string facultyUrl)
            => Mapper.Map<List<SpecialityViewModel>>(await _service.GetByFacultyAsync(facultyUrl));

        [HttpGet("FacultyDisabledSpecialities/{facultyUrl}")]
        public async Task<ActionResult<IEnumerable<SpecialityViewModel>>> GetFacultyDisabledSpecialities(string facultyUrl)
            => Mapper.Map<List<SpecialityViewModel>>(await _service.GetByFacultyAsync(facultyUrl, true));

        [HttpGet("GroupSpecialities/{groupId}")]
        public async Task<ActionResult<IEnumerable<SpecialityViewModel>>> GetGroupSpecialities(Guid groupId)
            => Mapper.Map<List<SpecialityViewModel>>(await _service.GetByGroupAsync(groupId));

        [HttpGet("{id}")]
        public async Task<ActionResult<SpecialityViewModel>> GetSpeciality(Guid id)
        {
            SpecialityViewModel model = Mapper.Map<SpecialityViewModel>(await _service.GetAsync(id));

            return model != null ? model : NotFound();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> PutSpeciality(SpecialityViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = Mapper.Map<SpecialityDTO>(model);
                    dto = await _service.SaveAsync(dto);

                    _logger.LogInformation("Изменена специальность - {SpecialityName}", dto.FullName);

                    return Ok();
                }
            }
            catch
            {
                _logger.LogError("Произошла ошибка при изменении специальности - {SpecialityName}", model.FullName);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("{facultyId}")]
        [Authorize(Roles = "commission")]
        public async Task<ActionResult<SpecialityViewModel>> PostSpeciality(Guid facultyId, SpecialityViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.FacultyId = facultyId;
                var dto = Mapper.Map<SpecialityDTO>(model);
                dto = await _service.SaveAsync(dto);

                _logger.LogInformation("Создана специальность - {SpecialityName}", dto.FullName);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> DeleteSpeciality(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Удаление специальности");
            }
            return Ok();
        }
    }
}
