using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Magicodes.Mvc.TrustIp
{
    public static class IPHelper
    {
        public static List<byte[]> TrustIplist;

        public static bool AllowAnyIp = false;

        private static ILogger _logger;

        /// <summary>
        /// 根据配置文件初始化
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="isReload">是否重新加载</param>
        public static void InitByConfiguration(IConfiguration configuration, ILogger logger, bool isReload = false)
        {
            _logger = logger;
            if (!isReload && TrustIplist != null)
            {
                return;
            }
            
            if (configuration.GetSection("TrustIpList") == null)
            {
                AllowAnyIp = true;
                _logger.LogWarning(
                    $"AllowAnyIp: true");
                return;
            }
            //从配置中获取授信IP列表（仅第一次）
            var ipAddresses = configuration.GetSection("TrustIpList").Get<List<string>>()
                .Select(IPAddress.Parse).ToList();

            //检查是否配置了任意IP(0.0.0.0)
            if (ipAddresses.Any(p => p.Equals(IPAddress.Any)))
            {
                AllowAnyIp = true;
                _logger.LogWarning(
                    $"AllowAnyIp: true");
                return;
            }
            //获取字节列表
            TrustIplist = ipAddresses.Select(p => p.GetAddressBytes()).ToList<byte[]>();
        }

        /// <summary>
        /// 是否为信任IP
        /// </summary>
        /// <param name="iPAddress"></param>
        /// <returns></returns>
        public static bool IsTrustIP(IPAddress iPAddress)
        {
            if (AllowAnyIp)
            {
                return true;
            }
            var bytes = iPAddress.GetAddressBytes();
            if (!TrustIplist.Any(p => p.SequenceEqual(bytes)))
            {
                _logger.LogInformation(
                    $"服务器拒绝了该IP的请求: {iPAddress}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否为信任IP
        /// </summary>
        /// <param name="iPAddress"></param>
        /// <returns></returns>
        public static bool IsTrustIP(string iPAddress) => IsTrustIP(IPAddress.Parse(iPAddress));
    }
}
