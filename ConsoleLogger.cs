namespace ICMPPingLogger
{
    public class ConsoleLogger : ILogger
    {
        public void WriteInfo(string message) => Console.WriteLine(message);

        public void WriteUsageInfo(string message)
        {
            if (!string.IsNullOrEmpty(message))
                Console.WriteLine(message);
            Console.WriteLine("Usage: program <duration in seconds> <IP1> <IP2> ...");
        }
    }
}
