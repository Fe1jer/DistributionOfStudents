using webapi.Data.Models;
using webapi.Data.Services;
using webapi.Data.Specifications;
using webapi.ViewModels.Faculties;
using Microsoft.AspNetCore.Mvc;
using webapi.Data.Interfaces.Repositories;
using webapi.Data.Interfaces.Services;

namespace webapi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultiesApiController : ControllerBase
    {
        private readonly ILogger<FacultiesApiController> _logger;
        private readonly IFacultiesRepository _facultiesRepository;

        public FacultiesApiController(ILogger<FacultiesApiController> logger, IFacultiesRepository facultyRepository)
        {
            _logger = logger;
            _facultiesRepository = facultyRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Faculty>>> GetFaculties() => (await _facultiesRepository.GetAllAsync());

        [HttpGet("{shortName}")]
        public async Task<ActionResult<Faculty>> GetFaculty(string shortName)
        {
            Faculty? faculty = await _facultiesRepository.GetByShortNameAsync(shortName);

            if (faculty == null)
            {
                return NotFound();
            }

            return faculty;
        }

        [HttpPut("{shortName}")]
        public async Task<IActionResult> PutFaculty(string shortName, [FromForm] CreateChangeFacultyVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Faculty? sameFaculty = await _facultiesRepository.GetByShortNameAsync(model.Faculty.ShortName);
                    if (sameFaculty == null || shortName.ToLower() == model.Faculty.ShortName.ToLower())
                    {
                        Faculty? faculty = await _facultiesRepository.GetByShortNameAsync(shortName);
                        if (faculty == null)
                        {
                            return NotFound();
                        }
                        faculty.FullName = model.Faculty.FullName;
                        faculty.ShortName = model.Faculty.ShortName;
                        string path = "\\img\\Faculties\\";
                        path += model.Img != null ? faculty.ShortName.Replace(" ", "_") + "\\" : "Default.jpg";

                        if (model.Img != null && faculty.Img != "\\img\\Faculties\\Default.jpg")
                        {
                            string[] deletePath = faculty.Img.Split('.');
                            IFileService.DeleteFile(faculty.Img);
                            faculty.Img = deletePath[0] + "_300x170." + deletePath[1];
                            IFileService.DeleteFile(faculty.Img);
                            faculty.Img = IFileService.UploadFile(model.Img, path + model.Img.FileName);
                            IFileService.ResizeAndCrop(faculty.Img, 300, 170);
                        }
                        else if (model.Img != null)
                        {
                            faculty.Img = IFileService.UploadFile(model.Img, path + model.Img.FileName);
                            IFileService.ResizeAndCrop(faculty.Img, 300, 170);
                        }

                        await _facultiesRepository.UpdateAsync(faculty);
                        _logger.LogInformation("Изменён факультет - {FacultyName}", faculty.FullName);

                        return Ok();
                    }
                    else
                    {
                        ModelState.AddModelError("modelErrors", "Такой факультет уже существует");
                    }
                }
            }
            catch (IOException)
            {
                ModelState.AddModelError("modelErrors", "В данный момент невозможно изменить изображение");
                ModelState.AddModelError("modelErrors", "Повторите попытку позже");
                _logger.LogError("Произошла ошибка при изменении факультета - {FacultyName}", shortName);
            }
            catch
            {
                _logger.LogError("Произошла ошибка при изменении факультета - {FacultyName}", shortName);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> PostFaculty([FromForm] CreateChangeFacultyVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Faculty? sameFaculty = await _facultiesRepository.GetByShortNameAsync(model.Faculty.ShortName, new FacultiesSpecification());
                    if (sameFaculty == null)
                    {
                        Faculty faculty = new() { Img = "\\img\\Faculties\\Default.jpg" };
                        string path = "\\img\\Faculties\\";
                        path += model.Img != null ? model.Faculty.ShortName.Replace(" ", "_") + "\\" : "Default.jpg";

                        faculty = new()
                        {
                            FullName = model.Faculty.FullName,
                            ShortName = model.Faculty.ShortName,
                            Img = model.Img != null ? IFileService.UploadFile(model.Img, path + model.Img.FileName) : path
                        };

                        Task addFaculty = _facultiesRepository.AddAsync(faculty);
                        if (model.Img != null)
                        {
                            IFileService.ResizeAndCrop(faculty.Img, 300, 170);
                        }

                        await addFaculty;
                        _logger.LogInformation("Создан факультет - {FacultyName}", faculty.FullName);

                        return Ok();
                    }
                    else
                    {
                        ModelState.AddModelError("modelErrors", "Такой факультет уже существует");
                    }
                }
            }
            catch
            {
                _logger.LogError("Произошла ошибка при создании факультета");

            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{shortName}")]
        public async Task<IActionResult> DeleteFaculty(string shortName)
        {
            Faculty? faculty = await _facultiesRepository.GetByShortNameAsync(shortName);
            if (faculty != null)
            {
                await _facultiesRepository.DeleteAsync(faculty.Id);
                _logger.LogInformation("Факультет - {FacultyName} - был удалён", faculty.FullName);
            }

            return Ok();
        }
    }
}
