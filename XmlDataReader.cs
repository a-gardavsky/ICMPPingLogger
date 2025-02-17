using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ICMPPingLogger
{

    public class XmlDataReader : IDataReader
    {
        private readonly string _inputFile;

        public XmlDataReader(string inputFile)
        {
            _inputFile = inputFile;
        }

        public Dictionary<string, double> CalculateAvailability()
        {
            Dictionary<string, (int successCount, int totalCount)> stats = new();

            using XmlReader reader = XmlReader.Create(_inputFile);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Ping")
                {
                    string ip = reader.GetAttribute("IP") ?? "UNKNOWN";
                    bool success = bool.TryParse(reader.GetAttribute("Success"), out bool parsedSuccess) && parsedSuccess;

                    if (!stats.ContainsKey(ip))
                    {
                        stats[ip] = (0, 0);
                    }

                    var (successCount, totalCount) = stats[ip];
                    stats[ip] = (successCount + (success ? 1 : 0), totalCount + 1);
                }
            }

            return stats.ToDictionary(kvp => kvp.Key, kvp => (double)kvp.Value.successCount / kvp.Value.totalCount * 100);
        }
    }
}
