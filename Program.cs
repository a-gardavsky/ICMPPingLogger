namespace ICMPPingLogger
{
    class Program
    {
        static string _outputFile = "ping_results.xml";

        static async Task Main(string[] args)
        {
            if (args.Length < 2 || !int.TryParse(args[0], out int duration))
            {
                GlobalHelper.WriteValidUseInfo("");
                return;
            }

            IDataReader _dataReader = new XmlDataReader(_outputFile);
            string[] ipAddresses = args[1..];
            ICMPPingLoggerHelper icmpPingLoggerHelper = new ICMPPingLoggerHelper(_dataReader);
            if (!icmpPingLoggerHelper.ArgsAreValid(duration, ipAddresses))
                return;

            using (IDataWriter _dataWriter = new XmlDataWriter(_outputFile))
            {
                PingAsync pa = new PingAsync(_dataWriter);
                Console.WriteLine($"Start app {DateTime.Now}");
                await pa.Execute(duration, ipAddresses);
                Console.WriteLine($"Finished {DateTime.Now}");
            }

            Console.WriteLine($"Test completed. Results saved to {_outputFile}");
            icmpPingLoggerHelper.CalculateAvailability();
        }

    }
}