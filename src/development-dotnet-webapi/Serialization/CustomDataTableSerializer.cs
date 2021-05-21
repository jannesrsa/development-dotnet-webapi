using System;
using System.Data;
using System.Text.Json;

namespace DevelopmentDotnetWebApi.Serialization
{
    public static class CustomDataTableSerializer
    {
        public static DataTable JsonElementToDataTable(this JsonElement dataRoot)
        {
            var dataTable = new DataTable();

            foreach (var col in dataRoot.GetProperty("Columns").EnumerateObject())
            {
                var colValue = col.Value;
                dataTable.Columns.Add(new DataColumn(col.Name, Type.GetType(colValue.GetString())));
            }

            foreach (var element in dataRoot.GetProperty("Rows").EnumerateArray())
            {
                var row = dataTable.NewRow();
                foreach (var col in element.EnumerateObject())
                {
                    var column = dataTable.Columns[col.Name];
                    row[column] = col.Value.JsonElementToTypedValue(column.DataType);
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private static object JsonElementToTypedValue(this JsonElement jsonElement, Type type)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.Object:      // 1  (these need special handling)?
                case JsonValueKind.Array:       // 2
                case JsonValueKind.String:      // 3
                    if (type == typeof(Guid))
                    {
                        if (jsonElement.TryGetGuid(out Guid guidValue))
                        {
                            return guidValue;
                        }
                        else
                        {
                            return DBNull.Value;
                        }
                    }
                    else
                    {
                        if (jsonElement.TryGetDateTime(out DateTime datetime))
                        {
                            // If an offset was provided, use DateTimeOffset.
                            if (datetime.Kind == DateTimeKind.Local)
                            {
                                if (jsonElement.TryGetDateTimeOffset(out DateTimeOffset datetimeOffset))
                                {
                                    return datetimeOffset;
                                }
                            }
                            return datetime;
                        }
                        return jsonElement.ToString();
                    }
                case JsonValueKind.Number:      // 4
                    if (jsonElement.TryGetInt64(out long longValue))
                    {
                        return longValue;
                    }
                    else
                    {
                        return jsonElement.GetDouble();
                    }
                case JsonValueKind.True:        // 5
                case JsonValueKind.False:       // 6
                    return jsonElement.GetBoolean();

                case JsonValueKind.Undefined:   // 0
                case JsonValueKind.Null:        // 7
                    return DBNull.Value;

                default:
                    return jsonElement.ToString();
            }
        }

        private static Type ValueKindToType(this JsonValueKind valueKind, string value)
        {
            switch (valueKind)
            {
                case JsonValueKind.String:      // 3
                    return typeof(string);

                case JsonValueKind.Number:      // 4
                    if (long.TryParse(value, out var intValue))
                    {
                        return typeof(long);
                    }
                    else
                    {
                        return typeof(double);
                    }
                case JsonValueKind.True:        // 5
                case JsonValueKind.False:       // 6
                    return typeof(bool);

                case JsonValueKind.Undefined:   // 0
                    return null;

                case JsonValueKind.Object:      // 1
                    return typeof(object);

                case JsonValueKind.Array:       // 2
                    return typeof(Array);

                case JsonValueKind.Null:        // 7
                    return null;

                default:
                    return typeof(object);
            }
        }
    }
}