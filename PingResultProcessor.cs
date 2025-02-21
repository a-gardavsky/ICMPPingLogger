using ICMPPingLogger;
using System.Collections.Concurrent;

public class PingResultProcessor
{
    private readonly ConcurrentQueue<PingData> _resultsQueue;
    private readonly IDataWriter _dataWriter;
    private readonly CancellationToken _token;

    public PingResultProcessor(ConcurrentQueue<PingData> resultsQueue, IDataWriter dataWriter, CancellationToken token, ILogger logger)
    {
        _resultsQueue = resultsQueue;
        _dataWriter = dataWriter;
        _token = token;
    }

    public void ProcessResults()
    {
        try
        {
            while (!_token.IsCancellationRequested || !_resultsQueue.IsEmpty)
            {
                List<PingData> batch = new();

                while (_resultsQueue.TryDequeue(out var result))
                {
                    batch.Add(result);
                }

                if (batch.Count > 0)
                {
                    _dataWriter.Save(batch);
                }

                if (!_token.IsCancellationRequested)
                {
                    Thread.Sleep(500);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Normal finishing
        }
    }
}
