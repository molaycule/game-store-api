namespace GameStore.Api.Utils;

public static class ObjectToDictionary
{
	public static Dictionary<string, object?> Convert(object obj)
	{
		var dictionary = new Dictionary<string, object?>();
		var properties = obj.GetType().GetProperties();

		foreach (var property in properties) dictionary.Add(property.Name, property?.GetValue(obj));

		return dictionary;
	}
}
