using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Filters.Base;
using webapi.ViewModels.Admissions;
using webapi.ViewModels.Students;

namespace webapi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsApiController : BaseController
    {
        private readonly ILogger<StudentsApiController> _logger;
        private readonly IAdmissionsService _service;

        public StudentsApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<StudentsApiController> logger, IAdmissionsService service) : base(accessor, generator)
        {
            _logger = logger;
            _service = service;
        }

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