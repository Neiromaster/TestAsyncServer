using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Model
{
    public static class Converters
    {
        public static string ObjectToXml<T>(T obj)
        {
            var formatter = new XmlSerializer(typeof(T));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Auto,
                Indent = false
            };
            var sb = new StringBuilder();
            using (var xw = XmlWriter.Create(sb, xmlWriterSettings))
            {
                formatter.Serialize(xw, obj, ns);
            }
            Debug.WriteLine(sb);
            return sb.ToString();
        }

        public static T XmlToObject<T>(string xml)
        {
            var result = default(T);
            var formatter = new XmlSerializer(typeof(T));
            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    result = (T)formatter.Deserialize(reader);
                    Debug.WriteLine(result);
                }
            }
            catch (InvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }
            return result;
        }
    }
}