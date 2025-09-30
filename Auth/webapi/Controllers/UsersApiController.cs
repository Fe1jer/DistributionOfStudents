namespace webapi.Controllers;

using BLL.DTO.User;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using webapi.ViewModels;
using webapi.ViewModels.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersApiController : BaseController
{
    private readonly ILogger<UsersApiController> _logger;
    private readonly IUserService _service;

    public UsersApiController(IHttpContextAccessor accessor, LinkGenerator generator, ILogger<UsersApiController> logger, IUserService userService) : base(accessor, generator)
    {
        _logger = logger;
        _service = userService;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAllAsync() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserViewModel>> GetUser(Guid id)
    {
        UserViewModel model = Mapper.Map<UserViewModel>(await _service.GetAsync(id));

        return model != null ? model : NotFound();
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutUser(Guid id, [FromForm] UserViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var dto = Mapper.Map<UserDTO>(model);
                dto = await _service.SaveAsync(dto);

                _logger.LogInformation("Изменена специальность - {User}", dto?.UserName);

                return Ok();
            }
        }
        catch
        {
            _logger.LogError("Произошла ошибка при изменении специальности - {User}", model.UserName);
        }

        return BadRequest(ModelState);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<UserViewModel>> PostUser([FromForm] UserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var dto = Mapper.Map<RegisterDTO>(model);
            var response = await _service.Registration(dto);

            if (response == null)
                return BadRequest(new { message = "Имя пользователя занято" });

            return Ok();
        }

        return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteSubject(Guid id)
    {
        try
        {
            await _service.DeleteAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Удаление пользователя");
        }
        return Ok();
    }
}
