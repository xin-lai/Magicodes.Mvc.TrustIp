using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Magicodes.Mvc.TrustIp
{
    public static class Extensions
    {
        /// <summary>
        /// 启用授信IP
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
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


        public static HashSet<T> ToHashSet<T>(
            this IEnumerable<T> source,
            IEqualityComparer<T> comparer = null)
        {
            return new HashSet<T>(source, comparer);
        }
    }
}
