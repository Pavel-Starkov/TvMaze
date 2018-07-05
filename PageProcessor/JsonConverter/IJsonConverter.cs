namespace PageProcessor.JsonConverter
{
    public interface IJsonConverter
    {
        T DeserializeObject<T>(string json);
        string SerializeObject(object value);
    }
}
