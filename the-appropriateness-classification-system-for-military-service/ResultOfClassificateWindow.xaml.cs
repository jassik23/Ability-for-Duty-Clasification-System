using System.Windows;
using Newtonsoft.Json.Linq;

namespace Ability_for_Duty_Clasification_System;

public partial class ResultOfClassificateWindow : Window
{
    private readonly JObject _selectedValues;
    public ResultOfClassificateWindow(JObject selectedValues)
    {
        this._selectedValues = selectedValues;
        InitializeComponent();
        Tuple<string, string>? results = Classification();
        SuitableCategoriesTextBlock.Text = results?.Item1;
        ExplanationTextBox.Text = results?.Item2;
    }
    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
    private void Menu_OnClick(object sender, RoutedEventArgs e)
    {
        MainWindow newWindow = new MainWindow();
        newWindow.Show();
        this.Close();
    }

    private Tuple<string, string>? Classification()
    {
        var dataTypeJson = App.GetDataTypes();
        var classificationJson = App.GetDataKnowledge();
        string goodResults = "";
        string badResults = "";
        foreach (var category in classificationJson!)
        {
            bool flag = true;
            foreach (var attribute in category.Value!)
            {
                var element = (JProperty)attribute;
                switch (dataTypeJson!.GetValue(element.Name)?.ToString())
                {
                    case "Интервальный":
                        string? type = (string)element.Value!;
                        float startIndex = float.Parse(type.Substring(type.IndexOf('[') + 1, type.IndexOf("..", StringComparison.Ordinal) - type.IndexOf('[') - 1));
                        float endIndex = float.Parse(type.Substring(type.IndexOf("..", StringComparison.Ordinal) + 2, type.IndexOf(']') - type.IndexOf("..", StringComparison.Ordinal) - 2));
                        float value =
                            float.Parse(this._selectedValues.GetValue(element.Name)?.ToString() ?? string.Empty);
                        if ((value < startIndex) || (value > endIndex))
                        {
                            badResults +=
                                $"{category.Key} не подходит, так как {element.Name} \u2209 [{startIndex};{endIndex}]" + "\n";
                            flag = false;
                        }
                        break;
                    case "Качественный": case "Логический":
                        string[]? classValue = element.Value.ToObject<string[]>();
                        string selectedValue = this._selectedValues.GetValue(element.Name)!.ToString();
                        if (classValue != null && !classValue.Contains(selectedValue))
                        {
                            badResults +=
                                $"{category.Key} не подходит, так как {element.Name} \u2209 [{string.Join(", ", classValue)}]" + "\n";
                            flag = false;
                        }
                        break;
                    default:
                        System.Console.WriteLine("3");
                        break;
                }
                if (!flag)
                {
                    break;
                }
                
            }
            if (flag)
            {
                goodResults += category.Key + " ";
            }
            
        }

        return new Tuple<string, string>(goodResults, badResults);
    }
}