using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Shared.Helpers
{
    public class LinkGeneratorHelper
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly LinkGenerator _generator;

        public static LinkGeneratorHelper Current;

        public LinkGeneratorHelper(IHttpContextAccessor accessor, LinkGenerator generator)
        {
            Current = this;
            _accessor = accessor;
            _generator = generator;
        }

        public string? GetUriByAction(string? action = null, string? controller = null, object? routeValues = null, string? scheme = null)
        {
            return _generator.GetUriByAction(_accessor.HttpContext, action, controller, routeValues, scheme);
        }
    }
}
