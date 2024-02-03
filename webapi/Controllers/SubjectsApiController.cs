using BLL.DTO.Subjects;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.ViewModels.General;

namespace webapi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsApiController : BaseController
    {
        private readonly ILogger<SubjectsApiController> _logger;
        private readonly ISubjectsService _service;

        public SubjectsApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<SubjectsApiController> logger, ISubjectsService service) : base(accessor, generator)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/ApiSubjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectViewModel>>> GetSubjects() => Mapper.Map<List<SubjectViewModel>>(await _service.GetAllAsync());

        [HttpGet("GroupSubjects/{groupId}")]
        public async Task<ActionResult<IEnumerable<SubjectViewModel>>> GetGroupSubjects(Guid groupId) => Mapper.Map<List<SubjectViewModel>>(await _service.GetByGroupAsync(groupId));

        // GET: api/ApiSubjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectViewModel>> GetSubject(Guid id) => Mapper.Map<SubjectViewModel>(await _service.GetAsync(id));

        // PUT: api/ApiSubjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> PutSubject(SubjectViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dto = Mapper.Map<SubjectDTO>(model);
                    dto = await _service.SaveAsync(dto);

                    _logger.LogInformation("Изменен предмет - {Subject}", dto.Name);

                    return Ok();
                }
            }
            catch
            {
                _logger.LogError("Произошла ошибка при изменении предмета - {Subject}", model.Name);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Authorize(Roles = "commission")]
        public async Task<ActionResult<SubjectViewModel>> PostSubject([FromBody] SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = Mapper.Map<SubjectDTO>(model);
                dto = await _service.SaveAsync(dto);

                _logger.LogInformation("Создан предмет - {Subject}", dto.Name);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/ApiSubjects/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "commission")]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Удаление предмета");
            }
            return Ok();
        }
    }
}
