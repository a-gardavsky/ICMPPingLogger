using System.Collections.Concurrent;

namespace ICMPPingLogger
{
    public class PingAsync : IDisposable
    {
        private readonly PingManager _pingManager;
        private readonly PingResultProcessor _pingResultProcessor;
        private readonly CancellationTokenSource _cts = new();

        public PingAsync(IDataWriter dataWriter, ILogger logger)
        {
            ConcurrentQueue<PingData> resultsQueue = new();
            _pingManager = new PingManager(resultsQueue, _cts.Token, logger);
            _pingResultProcessor = new PingResultProcessor(resultsQueue, dataWriter, _cts.Token, logger);
        }

        public async Task Execute(int duration, string[] ipAddresses)
        {
            List<Task> pingTasks = ipAddresses
                .Select(ip => Task.Run(() => _pingManager.RunPingHost(ip, duration)))
                .ToList();

            Task writerTask = Task.Run(() => _pingResultProcessor.ProcessResults());

            await Task.Delay(duration * 1000);
            _cts.Cancel(); // Finish running operations

            await Task.WhenAll(pingTasks);
            await writerTask;
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}
