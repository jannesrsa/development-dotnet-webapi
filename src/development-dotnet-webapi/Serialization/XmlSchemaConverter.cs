using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace DevelopmentDotnetWebApi.Serialization
{
    public class XmlSchemaConverter : JsonConverter<XmlSchema>
    {
        public override XmlSchema Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var xml = reader.GetString();
            return DeserializeXmlSchema(xml);
        }

        public override void Write(Utf8JsonWriter writer, XmlSchema value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, SerializeXmlSchema(value), options);
        }

        internal static string SerializeXmlSchema(XmlSchema value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var sbSchema = new StringBuilder();

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true
            };

            using (var writer = XmlWriter.Create(sbSchema, settings))
            {
                value.Write(writer);
            }

            return sbSchema.ToString();
        }

        internal XmlSchema DeserializeXmlSchema(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }

            var schema = XmlSchema.Read(new MemoryStream(Encoding.ASCII.GetBytes(xml)), null);
            return schema;
        }
    }
}