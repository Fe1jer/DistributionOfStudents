namespace webapi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using webapi.Data.Interfaces.Repositories;
using webapi.Data.Interfaces.Services;
using webapi.Data.Models;
using webapi.ViewModels;
using webapi.ViewModels.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersApiController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    public UsersApiController(IUserService userService, IUserRepository userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateAsync(LoginVM model)
    {
        var response = await _userService.Authenticate(model);

        if (response == null)
            return BadRequest(new { message = "Неверные данные пользователя" });

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAllAsync() => Ok(await _userRepository.GetAllAsync());

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        User? user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutUser(int id, [FromForm] ChangeUserVM model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            User? _user = await _userRepository.GetByIdAsync(id);
            if (_user == null)
                return BadRequest();

            try
            {
                _user.Surname = model.Surname;
                _user.Name = model.Name;
                _user.Patronymic = model.Patronymic;

                if (model.FileImg != null)
                {
                    string path = "\\img\\Users\\" + _user.UserName + "_" + model.FileImg.FileName;
                    if (_user.Img != "\\img\\Users\\bntu.jpg")
                    {
                        IFileService.DeleteFile(_user.Img);
                    }
                    _user.Img = IFileService.UploadFile(model.FileImg, path);
                }

                await _userRepository.UpdateAsync(_user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UsersExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        return BadRequest(ModelState);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<User>> PostSubject([FromBody] CreateUserVM model)
    {
        if (await _userRepository.GetAllAsync() == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Users' is null.");
        }
        if (ModelState.IsValid)
        {
            var response = await _userService.Registration(model);

            if (response == null)
                return BadRequest(new { message = "Имя пользователя занято" });

            return Ok();
        }

        return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        if (await _userRepository.GetAllAsync() == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Users' is null.");
        }
        User? user = await _userRepository.GetByIdAsync(id);
        if (user != null)
        {
            await _userRepository.DeleteAsync(id);
        }

        return Ok();
    }

    private async Task<bool> UsersExists(int id)
    {
        return (await _userRepository.GetAllAsync()).Any(e => e.Id == id);
    }
}
