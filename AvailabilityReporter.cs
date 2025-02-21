namespace ICMPPingLogger
{
    public class AvailabilityReporter
    {
        private readonly IDataReader _dataReader;
        private readonly ILogger _logger;

        public AvailabilityReporter(IDataReader dataReader, ILogger logger)
        {
            _dataReader = dataReader;
            _logger = logger;
        }

        public void GenerateReport()
        {
            _logger.WriteInfo("Availability Report:");
            var availabilityResults = _dataReader.CalculateAvailability();

            foreach (var kvp in availabilityResults)
            {
                _logger.WriteInfo($"IP: {kvp.Key} - Availability: {kvp.Value:F2}%");
            }
        }
    }
}
