using BLL.DTO.GroupsOfSpecialities;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.ViewModels.GroupsOfSpecialities;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsOfSpecialitiesApiController : BaseController
    {
        private readonly ILogger<GroupsOfSpecialitiesApiController> _logger;
        private readonly IGroupsOfSpecialitiesService _service;

        public GroupsOfSpecialitiesApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<GroupsOfSpecialitiesApiController> logger, IGroupsOfSpecialitiesService service) : base(accessor, generator)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("FacultyGroups/{facultyUrl}/{year}")]
        public async Task<ActionResult<IEnumerable<GroupOfSpecialitiesViewModel>>> GetFacultyGroups(string facultyUrl, int year)
            => Mapper.Map<List<GroupOfSpecialitiesViewModel>>(await _service.GetByFacultyAsync(facultyUrl, year));

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupOfSpecialitiesViewModel>> GetGroup(Guid id)
        {
            GroupOfSpecialitiesViewModel model = Mapper.Map<GroupOfSpecialitiesViewModel>(await _service.GetAsync(id));

            return model != null ? model : NotFound();
        }

        [HttpPost("{facultyName}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> PostGroup(string facultyName, UpdateGroupOfSpecialitiesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updateDto = Mapper.Map<UpdateGroupOfSpecialitiesDTO>(model);
                var dto = await _service.SaveAsync(updateDto);

                _logger.LogInformation("Создана группа - {GroupName} - {Year} года на факультете - {FacultyName}", dto.Name, dto.FormOfEducation.Year, facultyName);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{facultyName}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> PutGroup(string facultyName, UpdateGroupOfSpecialitiesViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var updateDto = Mapper.Map<UpdateGroupOfSpecialitiesDTO>(model);
                    var dto = await _service.SaveAsync(updateDto);

                    _logger.LogInformation("Изменена группа - {GroupName} - {Year} года на факультете {FacultyName}", dto.Name, dto.FormOfEducation.Year, facultyName);

                    return Ok();
                }
            }
            catch
            {
                _logger.LogError("Произошла ошибка при изменении группы - {GroupName}", model.Name);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{facultyName}/{id}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> DeleteGroup(Guid id)
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
