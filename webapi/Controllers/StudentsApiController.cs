using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Filters.Base;
using webapi.ViewModels.Students;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<StudentsApiController> logger, IAdmissionsService service) : BaseController(accessor, generator)
    {
        private readonly ILogger<StudentsApiController> _logger = logger;
        private readonly IAdmissionsService _service = service;

        [HttpGet]
        public async Task<ActionResult<object>> GetStudents([FromQuery] DefaultFilter filter)
        {
            var (rows, count) = await _service.GetByLastYearAsync(filter);

            return new
            {
                admissions = Mapper.Map<List<StudentItemViewModel>>(rows),
                count
            };
        }
    }
}