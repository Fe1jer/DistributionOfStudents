using BLL.DTO;

namespace webapi.Providers
{
    public class RequestContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public RequestContextDTO GetRequestContext()
        {
            var context = _httpContextAccessor.HttpContext;
            return new RequestContextDTO
            {
                DeviceInfo = context?.Request.Headers.UserAgent.ToString(),
                IpAddress = context?.Connection.RemoteIpAddress?.ToString(),
                Language = context?.Request.Headers.AcceptLanguage.ToString()
            };
        }
    }
}