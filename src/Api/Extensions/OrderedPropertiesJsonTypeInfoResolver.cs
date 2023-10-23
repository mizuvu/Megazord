using System.Text.Json.Serialization.Metadata;
using System.Text.Json;

namespace Zord.Api.Extensions;

public class OrderedPropertiesJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var order = 0;
        JsonTypeInfo typeInfo = base.GetTypeInfo(type, options);
        if (typeInfo.Kind == JsonTypeInfoKind.Object)
        {
            foreach (JsonPropertyInfo property in typeInfo.Properties.OrderBy(a => a.Name))
            {
                property.Order = order++;
            }
        }
        return typeInfo;
    }
}
