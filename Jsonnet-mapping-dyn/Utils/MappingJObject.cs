using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Jsonnet.mapping.dyn.Utils
{
    public static class MappingJObject
{
    private static JArray ObjectToJArray(KeyValuePair<string, JToken> property, JObject source)
    {
        var item = new JArray();
        var key = property.Key.Split('-').LastOrDefault();
        foreach (var token in source.SelectToken(key))
        {
            var arTemplate = property.Value.Children().FirstOrDefault()?.Children<JProperty>().ToArray();
            var tokenProp = token.ToObject<Dictionary<string, object>>();

            var obA = new JObject();
            if (arTemplate != null)
            {
                foreach (var propTem in arTemplate)
                {
                    IDictionary<string, JToken> objectProp = new JObject(propTem);
                    var val = EvaluateType(objectProp.First(), source, tokenProp);
                    obA.Add(new JProperty(propTem.Name.Split('-').FirstOrDefault(), val));
                }
            }
            item.Add(obA);
        }
        return item;
    }

    private static JObject ToObject(JObject property, JObject source)
    {
        var obA = new JObject();

        foreach (var item in property.Children<JProperty>())
        {
            IDictionary<string, JToken> objectProp = new JObject(item);
            var val = EvaluateType(objectProp.First(), source);
            obA.Add(new JProperty(item.Name, val));
        }
        return obA;
    }

    public static dynamic EvaluateType(KeyValuePair<string, JToken> itemProp, JObject source, Dictionary<string, object> ar = null)
    {
        switch (itemProp.Value.Type)
        {
            case JTokenType.Array:
                return ObjectToJArray(itemProp, source);
            case JTokenType.Object:
                return ToObject((JObject)itemProp.Value, source);
            case JTokenType.String:
                return ar == null
                    ? source.SelectToken(itemProp.Value.ToString())
                    : ar.FirstOrDefault(x => x.Key == itemProp.Value.ToString()).Value;
        }
        return null;
    }
}
}
