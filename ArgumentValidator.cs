using System.Net;

namespace ICMPPingLogger
{
    public class ArgumentValidator
    {
        private readonly ILogger _logger;

        public ArgumentValidator(ILogger logger)
        {
            _logger = logger;
        }

        public bool Validate(int duration, string[] ipAddresses)
        {
            if (duration <= 0)
            {
                _logger.WriteUsageInfo($"Invalid duration: {duration}");
                return false;
            }

            foreach (var ip in ipAddresses)
            {
                if (!IsValidIpAddress(ip))
                {
                    _logger.WriteUsageInfo($"Invalid IP address: {ip}");
                    return false;
                }
            }
            return true;
        }

        private static bool IsValidIpAddress(string ip)
        {
            return IPAddress.TryParse(ip, out _);
        }
    }
}
