using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

class MainClass
{
    static void Main()
    {
        WebClient client = new WebClient();
        string jsonString = client.DownloadString("https://coderbyte.com/api/challenges/json/json-cleaning");


        JObject jsonObject = JObject.Parse(jsonString);
        CleanJsonObject(jsonObject);

        string modifiedJsonString = jsonObject.ToString();
        Console.WriteLine(modifiedJsonString);
    }

    static void CleanJsonObject(JObject jsonObject)
    {
        List<JProperty> properties = jsonObject.Properties().ToList();

        foreach (JProperty property in properties)
        {
            JToken value = property.Value;

            if (value.Type == JTokenType.Null || (value.Type == JTokenType.String && string.IsNullOrWhiteSpace(value.ToString())))
            {
                property.Remove();
            }
            else if (value.Type == JTokenType.Array)
            {
                JArray array = (JArray)value;
                for (int i = array.Count - 1; i >= 0; i--)
                {
                    if (array[i].Type == JTokenType.Null || (array[i].Type == JTokenType.String && string.IsNullOrWhiteSpace(array[i].ToString())))
                    {
                        array.RemoveAt(i);
                    }
                }

                if (array.Count == 0)
                {
                    property.Remove();
                }
            }
            else if (value.Type == JTokenType.Object)
            {
                CleanJsonObject((JObject)value);

                if (!value.HasValues)
                {
                    property.Remove();
                }
            }
        }
    }
}
