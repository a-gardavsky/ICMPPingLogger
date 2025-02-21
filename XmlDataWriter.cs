using System.Xml;

namespace ICMPPingLogger
{
    public class XmlDataWriter : IDataWriter, IDisposable
    {
        private readonly string _outputFile;
        private XmlWriter? _writer;
        private FileStream? _fileStream;

        public XmlDataWriter(string outputFile)
        {
            _outputFile = outputFile;
            InitializeXmlFile();
        }

        private void InitializeXmlFile()
        {
            _fileStream = new FileStream(_outputFile, FileMode.Create, FileAccess.Write, FileShare.None);
            _writer = XmlWriter.Create(_fileStream, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = false });

            _writer.WriteStartDocument();
            _writer.WriteStartElement("PingResults");
            _writer.Flush();
        }

        public void Save(List<PingData> batch)
        {
            if (_writer == null)
                throw new InvalidOperationException("XML Writer is not initialized.");

            foreach (PingData pingData in batch)
            {
                _writer.WriteStartElement("Ping");
                _writer.WriteAttributeString("IP", pingData.Ip);
                _writer.WriteAttributeString("Time", pingData.Time.ToString());
                _writer.WriteAttributeString("Success", pingData.Success.ToString());
                _writer.WriteEndElement();
            }

            _writer.Flush();
        }

        public void Dispose()
        {
            if (_writer != null)
            {
                _writer.WriteEndElement(); // Close <PingResults>
                _writer.WriteEndDocument();
                _writer.Flush();
                _writer.Close();
                _writer.Dispose();
                _writer = null;
            }

            _fileStream?.Close();
            _fileStream?.Dispose();
            _fileStream = null;
        }
    }
}
