using JsonFx.Json;
using UnityEngine;

public class ReadFileTool : MonoBehaviour
{
	public static T JsonToClass<T>(string json) where T : class
	{
		return JsonReader.Deserialize<T>(json);
	}

	public static T AddressToClass<T>(string txtAddress) where T : class
	{
		TextAsset textAsset = Resources.Load(txtAddress) as TextAsset;
		return JsonToClass<T>(textAsset.text);
	}

	public static T[] JsonToClasses<T>(string json) where T : class
	{
		return JsonReader.Deserialize<T[]>(json);
	}

	public static T[] AddressToClasses<T>(string txtAddress) where T : class
	{
		TextAsset textAsset = Resources.Load(txtAddress) as TextAsset;
		return JsonToClasses<T>(textAsset.text);
	}
}
