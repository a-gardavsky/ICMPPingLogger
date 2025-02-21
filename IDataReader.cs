namespace ICMPPingLogger
{
    public interface IDataReader
    {
        public Dictionary<string, double> CalculateAvailability();
    }
}
