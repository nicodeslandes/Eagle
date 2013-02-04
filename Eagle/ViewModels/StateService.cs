using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Eagle.ViewModels
{
    [Export(typeof(IStateService))]
    public class StateService : IStateService
    {
        private readonly Subject<IStateCaptureContext> _savingEventSubject = new Subject<IStateCaptureContext>();

        public void MarkAsDirty()
        {
            var captureContext = new StateCaptureContext();
            _savingEventSubject.OnNext(captureContext);

            XElement xmlState = new XElement("State",
                new XAttribute("{http://www.w3.org/2000/xmlns/}xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new XAttribute("{http://www.w3.org/2000/xmlns/}xsd", "http://www.w3.org/2001/XMLSchema"),
                from kvp in captureContext.States
                let key = kvp.Key
                let state = kvp.Value
                select new XElement(key, new XAttribute("stateType", state.GetType().FullName),
                    this.SerializeToXml(state)));

            string fileName = GetConfigFileName();
            using (var xmlStream = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true, NamespaceHandling = NamespaceHandling.OmitDuplicates }))
            {
                xmlState.WriteTo(xmlStream);
            }
        }

        private readonly Dictionary<string, object> _states = new Dictionary<string, object>();

        public async Task InitializeAsync()
        {
            var configFileName = this.GetConfigFileName();
            if (!File.Exists(configFileName))
                return;

            var content = await this.ReadTextAsync(configFileName);
            var stateXml = XDocument.Parse(content).Root;
            foreach (var stateElement in stateXml.Elements())
            {
                try
                {
                    string key = stateElement.Name.LocalName;
                    var serializedState = stateElement.Elements().First();
                    string stateTypeName = (string)stateElement.Attribute("stateType");
                    var stateType = Assembly.GetExecutingAssembly().GetType(stateTypeName);
                    var stateSerializer = new XmlSerializer(stateType);
                    _states[key] = stateSerializer.Deserialize(serializedState.CreateReader());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error while reading saved state: " + ex);
                }
            }
        }

        private async Task<string> ReadTextAsync(string filePath, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                StringBuilder sb = new StringBuilder();

                byte[] buffer = new byte[0x1000];
                int numRead;
                int bufferIndex = 0;
                Decoder decoder = encoding.GetDecoder();
                char[] chars = new char[0x1000];
                bool firstRead = true;
                while ((numRead = await sourceStream.ReadAsync(buffer, bufferIndex, buffer.Length - bufferIndex)) != 0)
                {
                    int bufferReadIndex = 0;
                    if (firstRead)
                    {
                        firstRead = false;
                        // Skip BOM
                        if (numRead >= 3 && buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
                        {
                            bufferReadIndex = 3;
                        }
                    }
                    bool completed;
                    int charsUsed;
                    int bytesUsed;
                    decoder.Convert(buffer, bufferReadIndex, numRead - bufferReadIndex, chars, 0, chars.Length, false, out bytesUsed, out charsUsed, out completed);
                    string text = new string(chars, 0, charsUsed);
                    sb.Append(text);

                    bufferIndex = numRead - bufferReadIndex - bytesUsed;
                    if (bufferIndex > 0)
                    {
                        Array.Copy(buffer, bytesUsed + bufferReadIndex, buffer, 0, bufferIndex);
                    }
                }

                return sb.ToString();
            }
        }

        private XElement SerializeToXml(object state)
        {
            var memStream = new MemoryStream();
            var serializer = new XmlSerializer(state.GetType());
            serializer.Serialize(memStream, state);

            memStream.Position = 0;
            return XElement.Load(memStream);
        }

        public IObservable<IStateCaptureContext> SavingEvent
        {
            get { return _savingEventSubject.AsObservable(); }
        }

        private string GetConfigFileName()
        {
            var userAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var configFolder = Path.Combine(userAppFolder, "Eagle");
            Directory.CreateDirectory(configFolder);
            return Path.Combine(configFolder, "config.xml");
        }


        public bool TryGetState<T>(string key, out T state) where T : class
        {
            object storedState;
            _states.TryGetValue(key, out storedState);
            state = storedState as T;
            return state != null;
        }
    }
}
