namespace HotRS.Tools.Core.Utilities.JSON;

public class ObfuscatingConverter : JsonConverter<string>
{
    public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException(Properties.Resources.NOUNOBFUSCATE);
    }

    public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
    {
        //Replace the value with static text
        value = Properties.Resources.OBFUSCATED;
        JToken t = JToken.FromObject(value);

        if (t.Type != JTokenType.Object)
        {
            t.WriteTo(writer);
        }
        else
        {
            JObject o = (JObject)t;
            IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();
            o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));
            o.WriteTo(writer);
        }
    }
}
