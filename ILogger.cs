namespace ICMPPingLogger
{
    public interface ILogger
    {
        void WriteInfo(string message);
        void WriteUsageInfo(string message);
    }
}
