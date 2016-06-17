using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TechReady.Portal.Helpers
{
    public class NoZoneDateTimeConverter : DateTimeConverterBase
    {
        //read and write date time type values without time zone as all date time values are in IST.
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value != null)
                return (DateTime)reader.Value;
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss"));
        }
    }
}