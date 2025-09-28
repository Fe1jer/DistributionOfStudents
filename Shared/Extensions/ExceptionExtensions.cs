using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using NLog;
using Shared.Extensions;

namespace Shared.Extensions
{
    public static class ExceptionExtensions
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        public static void UseExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                ExceptionHandler = context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>();

                    if (ex != null)
                    {
                        Logger.Error(ex);
                    }

                    return Task.CompletedTask;
                }
            });
        }
    }
}
