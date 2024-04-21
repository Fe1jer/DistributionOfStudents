using BLL.Services.Interfaces;
using ChartJSCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticApiController : BaseController
    {
        private readonly ILogger<StatisticApiController> _logger;
        private readonly IStatisticService _service;

        public StatisticApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<StatisticApiController> logger, IStatisticService service) : base(accessor, generator)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("PlansStatisticChart")]
        public async Task<ActionResult<string>> GetPlansStatistic(string facultyUrl, Guid groupId)
        {
            Chart chart = await _service.GetPlansStatisticAsync(facultyUrl, groupId);

            return new ActionResult<string>(chart.SerializeBody());
        }

        [HttpGet("GroupStatisticChart")]
        public async Task<ActionResult<string>> GetGroupStatistic(Guid groupId)
        {
            Chart chart = await _service.GetGroupStatisticAsync(groupId);

            return new ActionResult<string>(chart.SerializeBody());
        }

        [HttpPut("{facultyUrl}/{groupId}")]
        public async Task<ActionResult> PostGroupStatistic(string facultyUrl, Guid groupId)
        {
            await _service.SaveAsync(facultyUrl, groupId);

            return Ok();
        }
    }
}
