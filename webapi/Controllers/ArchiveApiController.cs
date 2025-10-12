using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using webapi.ViewModels.Archive;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchiveApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<ArchiveApiController> logger, IArchiveService service) : BaseController(accessor, generator)
    {
        private readonly ILogger<ArchiveApiController> _logger = logger;
        private readonly IArchiveService _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<int>>> GetArchive() => await _service.GetYears();

        [HttpGet("GetArchive/{year}")]
        public async Task<ActionResult<IEnumerable<string>>> GetArchive(int year) => await _service.GetForms(year);

        [HttpGet("GetArchive/{year}/{form}")]
        public async Task<ActionResult<IEnumerable<ArchiveFacultyViewModel>>> GetArchve(int year, string form) => Mapper.Map<List<ArchiveFacultyViewModel>>(await _service.GetFacultiesArchive(year, form));
    }
}
