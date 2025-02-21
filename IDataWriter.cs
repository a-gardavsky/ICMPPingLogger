namespace ICMPPingLogger
{
    public interface IDataWriter : IDisposable
    {
        void Save(List<PingData> batch);
    }
}
