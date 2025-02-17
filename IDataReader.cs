using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMPPingLogger
{
    public interface IDataReader
    {
        public Dictionary<string, double> CalculateAvailability();
    }
}
