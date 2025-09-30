using BLL.DTO.User;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.ViewModels;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserTokensApiController : BaseController
    {
        private readonly IUserTokenService _service;

        public UserTokensApiController(IHttpContextAccessor accessor, LinkGenerator generator, IUserTokenService userService) : base(accessor, generator)
        {
            _service = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> AuthenticateAsync(LoginViewModel request)
        {
            var dto = Mapper.Map<LoginDTO>(request);
            var response = await _service.AuthenticateAsync(dto);

            if (response == null)
                return BadRequest(new { message = "Неверные данные пользователя" });

            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshAsync([FromBody] string refreshToken)
        {
            var tokens = await _service.RefreshAsync(refreshToken); 
            
            return Ok(tokens);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("sign-out")]
        public async Task<IActionResult> SignOut(string refreshToken)
        {
            var result = await _service.SignOutAsync(refreshToken);

            return Ok(result);
        }
    }
}
