using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Magicodes.Mvc.TrustIp
{
    public static class Extensions
    {
        public static IApplicationBuilder UseTrustIP(this IApplicationBuilder app, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var logger = loggerFactory.CreateLogger<TrustIPMiddleware>();
            IPHelper.InitByConfiguration(configuration, logger);
            return app.UseMiddleware<TrustIPMiddleware>(logger);
        }

    }
}
