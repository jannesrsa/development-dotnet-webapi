using System;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DevelopmentDotnetWebApi.Serialization
{
    public class DataTableJsonConverter : JsonConverter<DataTable>
    {
        public static void WriteDataTable(Utf8JsonWriter jsonWriter, DataTable source, JsonSerializerOptions options)
        {
            // Column types
            jsonWriter.WriteStartObject();

            jsonWriter.WriteStartObject("Columns");
            foreach (DataColumn col in source.Columns)
            {
                var key = col.ColumnName.Trim();
                jsonWriter.WriteString(key, col.DataType.FullName);
            }
            jsonWriter.WriteEndObject();

            // Row values
            jsonWriter.WriteStartArray("Rows");
            foreach (DataRow dr in source.Rows)
            {
                jsonWriter.WriteStartObject();
                foreach (DataColumn col in source.Columns)
                {
                    var key = col.ColumnName.Trim();

                    jsonWriter.WritePropertyName(key);
                    JsonSerializer.Serialize(jsonWriter, dr[col], dr[col].GetType(), options);
                }
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();

            jsonWriter.WriteEndObject();
        }

        public override DataTable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var rootElement = jsonDoc.RootElement;
            var dataTable = rootElement.JsonElementToDataTable();
            return dataTable;
        }

        public override void Write(Utf8JsonWriter writer, DataTable value, JsonSerializerOptions options)
        {
            WriteDataTable(writer, value, options);
        }
    }
}