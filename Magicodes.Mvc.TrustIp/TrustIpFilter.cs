using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Magicodes.Mvc.TrustIp
{
    /// <summary>
    /// 信任IP筛选器
    /// </summary>
    public class TrustIPFilter : ActionFilterAttribute
    {
        private readonly ILogger<TrustIPFilter> _logger;
        public static bool AllowAnyIp = false;

        public TrustIPFilter
            (ILogger<TrustIPFilter> logger, IConfiguration configuration)
        {
            _logger = logger;
            
            IPHelper.InitByConfiguration(configuration, logger);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation(
                $"IP: {context.HttpContext.Connection.RemoteIpAddress}");

            if (IPHelper.IsTrustIP(context.HttpContext.Connection.RemoteIpAddress))
            {
                base.OnActionExecuting(context);
                return;
            }
            _logger.LogWarning(
                $"服务器拒绝了该IP的请求: {context.HttpContext.Connection.RemoteIpAddress}");

            context.Result = new StatusCodeResult(403);
        }
    }
}
