using BLL.DTO.Faculties;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.ViewModels.Faculties;

namespace webapi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultiesApiController : BaseController
    {
        private readonly ILogger<FacultiesApiController> _logger;
        private readonly IFacultiesService _service;

        public FacultiesApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<FacultiesApiController> logger, IFacultiesService service) : base(accessor, generator)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetailsFacultyViewModel>>> GetFaculties() =>
            Mapper.Map<List<DetailsFacultyViewModel>>(await _service.GetAllAsync());

        [HttpGet("{shortName}")]
        public async Task<ActionResult<DetailsFacultyViewModel>> GetFaculty(string shortName)
        {
            DetailsFacultyViewModel model = Mapper.Map<DetailsFacultyViewModel>(await _service.GetAsync(shortName));

            return model != null ? model : NotFound();
        }

        [HttpPut("{shortName}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> PutFaculty(string shortName, [FromForm] FacultyViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _service.CheckUrlIsUniqueAsync(model.ShortName, shortName))
                    {
                        var dto = Mapper.Map<FacultyDTO>(model);
                        dto = await _service.SaveAsync(dto);

                        _logger.LogInformation("Изменён факультет - {FacultyPreName} на {FacultyPastName}", dto.FullName, dto.FullName);

                        return Ok();
                    }
                    ModelState.AddModelError("modelErrors", "Такой факультет уже существует");
                }
                catch (IOException)
                {
                    ModelState.AddModelError("modelErrors", "В данный момент невозможно изменить изображение");
                    ModelState.AddModelError("modelErrors", "Повторите попытку позже");
                    _logger.LogError("Произошла ошибка при изменении факультета - {FacultyName}", model.FullName);
                }
                catch
                {
                    _logger.LogError("Произошла ошибка при изменении факультета - {FacultyName}", model.FullName);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> PostFaculty([FromForm] FacultyViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = Mapper.Map<FacultyDTO>(model);
                    dto = await _service.SaveAsync(dto);

                    _logger.LogInformation("Добавлен факультет - {FacultyPreName}", dto.FullName);

                    return Ok();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Добавление факультета");

            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{shortName}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> DeleteFaculty(string shortName)
        {
            try
            {
                await _service.DeleteAsync(shortName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Удаление факультета");
            }
            return Ok();
        }
    }
}
