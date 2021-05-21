using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;

namespace DevelopmentDotnetWebApi.Serialization
{
    public class DBNullConverter : JsonConverter<DBNull>
    {
        public override DBNull Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Log.Error("DBNullConverter.Read should not be called");

            return DBNull.Value;
        }

        public override void Write(Utf8JsonWriter writer, DBNull value, JsonSerializerOptions options)
        {
            writer.WriteNullValue();
        }
    }
}