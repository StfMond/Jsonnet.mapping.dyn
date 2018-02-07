using System;
using System.Linq;
using Jsonnet.mapping.dyn.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jsonnet.mapping.dyn
{
    public class Program
    {
        private static void Main(string[] args)
        {
            const string jsonResult = "{'Id':'3087','NameProduct':'010','Validate':'001','ValidationData':{'Id':666,'Type':1}, 'Answers': [{'Id':'01','Value': '03'}, {'Id':'09','Value': '03'}]}";
            var object1 = JObject.Parse(jsonResult);

            const string jsonTemplate = "{'Id':'Id','Name':'NameProduct','Identification': {'Id': 'ValidationData.Id', 'Obj': { 'Val1': 'ValidationData.Id', 'Val2': { 'Name2': 'ValidationData.Type' }}} ,'Answer-Answers':[{ 'Child-Answers': [{ 'ChildName': 'Id', 'ChildRep': 'Value' }]  , 'Val2': { 'Name2': 'ValidationData.Type' } ,'IdAnwers':'Value','IdQuestion':'Id'}]}";
            var object2 = JObject.Parse(jsonTemplate);

            var finalOb = new JObject();
            foreach (var property in object2)
            {
                finalOb.Add(property.Key.Split('-').FirstOrDefault(), MappingJObject.EvaluateType(property, object1));
            }
            Console.Write(JsonConvert.SerializeObject(finalOb));

            Console.ReadLine();
        }
    }
}
