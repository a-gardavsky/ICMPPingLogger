namespace ICMPPingLogger
{
    public class PingData
    {
        public string Ip {  get; set; } = string.Empty;
        public long Time { get; set; }
        public bool Success {  get; set; }
    }
}
