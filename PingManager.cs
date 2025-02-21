using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace ICMPPingLogger
{
    public class PingManager
    {
        private readonly ConcurrentQueue<PingData> _resultsQueue;
        private readonly CancellationToken _token;
        private readonly ILogger _logger;
        private const int _period = 100;   // 100 ms mezi pingi
        private const int _timeout = 300;  // Timeout 300 ms

        public PingManager(ConcurrentQueue<PingData> resultsQueue, CancellationToken token, ILogger logger)
        {
            _resultsQueue = resultsQueue;
            _token = token;
            _logger = logger;
        }

        public async Task RunPingHost(string ip, int duration)
        {
            using Ping pingSender = new();
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed.TotalSeconds < duration && !_token.IsCancellationRequested)
            {
                Stopwatch sw = Stopwatch.StartNew();
                bool success = false;

                try
                {
                    PingReply reply = await pingSender.SendPingAsync(ip, _timeout);
                    success = reply.Status == IPStatus.Success;
                }
                catch (PingException ex)
                {
                    _logger.WriteInfo($"[PingManager] Error pinging {ip}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.WriteInfo($"[PingManager] Unexpected error for {ip}: {ex}");
                }

                _resultsQueue.Enqueue(new PingData { Ip = ip, Time = sw.ElapsedMilliseconds, Success = success });

                // Počkáme na další ping s ohledem na dobu zpracování předchozího
                int remainingTime = _period - (int)sw.ElapsedMilliseconds;
                if (remainingTime > 0)
                {
                    try
                    {
                        await Task.Delay(remainingTime, _token);
                    }
                    catch (TaskCanceledException)
                    {
                        _logger.WriteInfo($"[PingManager] Task canceled for {ip}");
                        break;
                    }
                }
            }
        }
    }
}
