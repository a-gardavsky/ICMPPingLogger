using System.Net;

namespace ICMPPingLogger
{
    public static class GlobalHelper
    {
        public static void WriteValidUseInfo(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
                Console.WriteLine(msg);
            Console.WriteLine("Usage: program <duration in seconds> <IP1> <IP2> ...");
        }

        public static bool IsValidIpAddress(string ip)
        {
            return IPAddress.TryParse(ip, out _);
        }
    }
}
