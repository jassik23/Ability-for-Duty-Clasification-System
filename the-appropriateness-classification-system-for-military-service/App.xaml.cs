using System.Configuration;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace Ability_for_Duty_Clasification_System;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static JObject? DataTypes { get; set; }
    private static JObject? DataKnowledge { get; set; }
    private static JObject? DataTemplateKnowledge { get; set; }
    private readonly string _dataKnowledgeName = "dataKnowledge.json";
    private readonly string _dataTemplateKnowledgeName = "dataTemplateKnowledge.json";
    private readonly string _dataTypesName = "dataTypes.json";

    public static JObject? GetDataTypes()
    {
        return DataTypes;
    }
    public static JObject? GetDataKnowledge()
    {
        return DataKnowledge;
    }

    public static JObject? GetDataTemplateKnowledge()
    {
        return DataTemplateKnowledge;
    }
    public static void SetDataTypes(JObject? newDataTypes)
    {
        DataTypes = newDataTypes;
    }
    public static void SetDataKnowledge(JObject? newDataKnowledge)
    {
        DataKnowledge = newDataKnowledge;
    }
    
    private void App_OnExit(object sender, ExitEventArgs e)
    {
        WorkWithJson.SetClassesToJson(DataKnowledge, _dataKnowledgeName);
        WorkWithJson.SetClassesToJson(DataTypes, _dataTypesName);
        WorkWithJson.SetClassesToJson(DataTemplateKnowledge, _dataTemplateKnowledgeName);
        System.Console.WriteLine("Exit");
    }

    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        DataKnowledge = WorkWithJson.GetClassesFromJson(_dataKnowledgeName);
        DataTypes = WorkWithJson.GetClassesFromJson(_dataTypesName);
        DataTemplateKnowledge = WorkWithJson.GetClassesFromJson(_dataTemplateKnowledgeName);
    }
}