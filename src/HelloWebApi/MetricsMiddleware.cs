using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace HelloWebApi
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MetricsMiddleware
    {
        private readonly RequestDelegate _next;
        ILogger _log;

        public MetricsMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger(nameof(MetricsMiddleware));
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Items["Correlation"] = Guid.NewGuid();
            //throw new Exception("fsdfsd!");
            Stopwatch sw = Stopwatch.StartNew();

            //using (_log.BeginScope<Guid>(Guid.NewGuid()))
            using (_log.BeginScope("fdfdfdsfsdfsdfsdfs {0}", "amir"))
            {
                try
                {
                    await _next(httpContext);
                    _log.LogInformation($"Handled {httpContext.Request.Path} in {sw.ElapsedMilliseconds} MS");
                }
                catch (Exception e)
                {

                    _log.LogCritical($"Exception!! {httpContext.Response.StatusCode} {e}");
                }
            }




        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MetricsMiddlewareExtensions
    {
        public static IApplicationBuilder UseMetricsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MetricsMiddleware>();
        }
    }
}
