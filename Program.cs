using Microsoft.Extensions.DependencyInjection;

namespace ICMPPingLogger
{
    class Program
    {

        static async Task Main(string[] args)
        {
            if (args.Length < 2 || !int.TryParse(args[0], out int duration))
            {
                var logger = new ConsoleLogger();
                logger.WriteUsageInfo("Invalid arguments.");
                return;
            }

            string outputFile = "ping_results.xml";
            string[] ipAddresses = args[1..];

            // Dependency Injection container
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ILogger, ConsoleLogger>()  // Logging
                .AddSingleton<ArgumentValidator>()       // Input arguments validation 
                .AddSingleton<IDataReader>(_ => new XmlDataReader(outputFile)) // Read of XML results
                .AddSingleton<IDataWriter>(_ => new XmlDataWriter(outputFile)) // Write results to XML
                .AddSingleton<PingAsync>()              // Ping management
                .AddSingleton<AvailabilityReporter>()   // Availability counting
                .BuildServiceProvider();

            var loggerService = serviceProvider.GetRequiredService<ILogger>();
            var validator = serviceProvider.GetRequiredService<ArgumentValidator>();

            if (!validator.Validate(duration, ipAddresses))
                return;

            try
            {
                using (var dataWriter = serviceProvider.GetRequiredService<IDataWriter>())
                using (var pingAsync = serviceProvider.GetRequiredService<PingAsync>())
                {
                    loggerService.WriteInfo($"Start app {DateTime.Now}");
                    await pingAsync.Execute(duration, ipAddresses);
                    loggerService.WriteInfo($"Finished {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                loggerService.WriteInfo($"Error occurred: {ex.Message}");
            }
            finally
            {
                loggerService.WriteInfo($"Test completed. Results saved to {outputFile}");

                var reporter = serviceProvider.GetRequiredService<AvailabilityReporter>();
                reporter.GenerateReport();
            }
        }

    }
}