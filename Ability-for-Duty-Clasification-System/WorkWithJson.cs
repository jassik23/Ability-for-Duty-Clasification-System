using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ability_for_Duty_Clasification_System;

public abstract class WorkWithJson
{
    public static JObject GetClassesFromJson(string jsonFileName)
    {
        string jsonString = File.ReadAllText(jsonFileName);
        JObject jObject = JObject.Parse(jsonString);
        return jObject;
    }

    public static void SetClassesToJson(JObject jObject, string jsonFileName)
    {
        File.WriteAllText(@jsonFileName, jObject.ToString());
    }
}