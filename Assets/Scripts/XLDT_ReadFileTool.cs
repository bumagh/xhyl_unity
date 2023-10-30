using JsonFx.Json;

public class XLDT_ReadFileTool
{
	public static T JsonToClass<T>(string json) where T : class
	{
		return JsonReader.Deserialize<T>(json);
	}

	public static string JsonByObject(object value)
	{
		return JsonWriter.Serialize(value);
	}
}
