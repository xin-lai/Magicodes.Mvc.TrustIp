using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Magicodes.Mvc.TrustIp
{
    /// <summary>
    /// 中间件
    /// </summary>
    public class TrustIPMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TrustIPMiddleware> _logger;

        public TrustIPMiddleware(RequestDelegate next, ILogger<TrustIPMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;
            _logger.LogInformation(
                $"IP: {remoteIp}");

            if (IPHelper.IsTrustIP(remoteIp))
            {
                await _next.Invoke(context);
                return;
            }
            _logger.LogWarning(
                $"服务器拒绝了该IP的请求: {remoteIp}");
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }
    }
}
