using System.Text.Json;
using System.Text.Json.Serialization;

namespace DevelopmentDotnetWebApi.Serialization
{
    public static class SerializationOptions
    {
        public static JsonSerializerOptions JsonSerializerOptions = Configure(new JsonSerializerOptions());

        public static JsonSerializerOptions Configure(JsonSerializerOptions options)
        {
            options.PropertyNameCaseInsensitive = true;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.Converters.Add(new ObjectToInferredTypesConverter());
            options.Converters.Add(new DBNullConverter());
            options.Converters.Add(new DataTableJsonConverter());
            options.Converters.Add(new TimeSpanConverter());
            options.Converters.Add(new JsonNonStringKeyDictionaryConverterFactory());
            options.Converters.Add(new XmlDocumentConverter());
            options.Converters.Add(new XmlSchemaConverter());

            return options;
        }
    }
}