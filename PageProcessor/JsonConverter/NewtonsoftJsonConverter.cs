using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PageProcessor.JsonConverter
{
    public class NewtonsoftJsonConverter : IJsonConverter
    {
        private readonly IsoDateTimeConverter _dateTimeConverter;
        public NewtonsoftJsonConverter(string dateTimeFormat)
        {
            _dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateTimeFormat };
        }

        public T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, _dateTimeConverter);
        }
    }
}
