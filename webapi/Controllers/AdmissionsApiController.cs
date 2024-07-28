using BLL.DTO;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Filters.Base;
using webapi.ViewModels.Admissions;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmissionsApiController : BaseController
    {
        private readonly ILogger<AdmissionsApiController> _logger;
        private readonly IAdmissionsService _service;

        public AdmissionsApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<AdmissionsApiController> logger, IAdmissionsService service) : base(accessor, generator)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/AdmissionsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmissionViewModel>>> GetAdmissions() => Mapper.Map<List<AdmissionViewModel>>(await _service.GetAllAsync());

        [HttpGet("GroupAdmissions/{groupId}")]
        public async Task<ActionResult<object>> GetGroupAdmissions(Guid groupId, [FromQuery] DefaultFilter filter)
        {
            var (rows, count) = await _service.GetByGroupAsync(groupId, filter);

            return new
            {
                admissions = Mapper.Map<List<AdmissionViewModel>>(rows),
                count
            };
        }

        // GET: api/AdmissionsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmissionViewModel>> GetAdmission(Guid id)
        {
            AdmissionViewModel model = Mapper.Map<AdmissionViewModel>(await _service.GetAsync(id));

            return model != null ? model : NotFound();
        }

        // PUT: api/AdmissionsApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> PutAdmission(Guid id, AdmissionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var updateDto = Mapper.Map<AdmissionDTO>(model);
                    var dto = await _service.SaveAsync(updateDto);

                    _logger.LogInformation("Заявка абитуриента - {Surname} {Name} {Patronymic} - изменена", dto.Student.Surname, dto.Student.Name, dto.Student.Patronymic);

                    return Ok();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Произошла ошибка при изменении заявки аббитуриента - {Surname} {Name} {Patronymic}", model.Student.Surname, model.Student.Name, model.Student.Patronymic);
            }

            return BadRequest(ModelState);
        }

        // POST: api/AdmissionsApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{groupId}")]
        [Authorize(Roles = "commission")]
        public async Task<ActionResult<AdmissionViewModel>> PostAdmission(Guid groupId, AdmissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = Mapper.Map<AdmissionDTO>(model);
                dto = await _service.SaveAsync(dto);

                _logger.LogInformation("Заявка студента - {Surname} {Name} {Patronymic} - добавлена", dto.Student.Surname, dto.Student.Name, dto.Student.Patronymic);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/AdmissionsApi/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> DeleteAdmission(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Удаление заявки студента");
            }

            return Ok();
        }
    }
}
