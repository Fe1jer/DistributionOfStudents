using BLL.DTO.User;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Providers;
using webapi.ViewModels;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserTokensApiController(IHttpContextAccessor accessor, LinkGenerator generator, IUserTokenService userService, RequestContextProvider contextProvider) : BaseController(accessor, generator)
    {
        private readonly IUserTokenService _service = userService;
        private readonly RequestContextProvider _contextProvider = contextProvider;

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> AuthenticateAsync(LoginViewModel request)
        {
            var dto = Mapper.Map<LoginDTO>(request);
            var context = _contextProvider.GetRequestContext();
            var response = await _service.AuthenticateAsync(dto, context);

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
        public async Task<IActionResult> SignOutAsync(string refreshToken)
        {
            var result = await _service.SignOutAsync(refreshToken);

            return Ok(result);
        }
    }
}
