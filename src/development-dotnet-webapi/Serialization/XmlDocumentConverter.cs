using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

namespace DevelopmentDotnetWebApi.Serialization
{
    public class XmlDocumentConverter : JsonConverter<XmlDocument>
    {
        public override XmlDocument Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var xml = JsonSerializer.Deserialize<string>(ref reader, options);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            return xmlDocument;
        }

        public override void Write(Utf8JsonWriter writer, XmlDocument value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.OuterXml, options);
        }
    }
}