using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMPPingLogger
{
    public class PingData
    {
        public string Ip {  get; set; } = string.Empty;
        public long Time { get; set; }
        public bool Success {  get; set; }
    }
}
