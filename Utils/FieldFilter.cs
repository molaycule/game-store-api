using GameStore.Api.Interfaces;

namespace GameStore.Api.Utils;

public static class FieldFilter
{
    public static Dictionary<string, object?> Filter<T>(this T dto, string fields) where T : IDto
    {
        var fieldNames = fields?.Split(',');

        if (fieldNames is null || fieldNames.Length == 0) return ObjectToDictionary.Convert(dto);

        var filteredDto = new Dictionary<string, object?>();

        foreach (var fieldName in fieldNames)
        {
            var capitalizedFieldName = char.ToUpper(fieldName[0]) + fieldName[1..];
            // Use reflection to get the property value from the original DTO
            var property = typeof(T).GetProperty(capitalizedFieldName);
            filteredDto.Add(fieldName, property?.GetValue(dto));
        }

        return filteredDto;
    }

    public static IEnumerable<Dictionary<string, object?>> FilterList<T>(this IEnumerable<T> dtos, string fields) where T : IDto
    {
        var result = new List<Dictionary<string, object?>>();

        foreach (var dto in dtos) result.Add(dto.Filter(fields));

        return result;
    }
}