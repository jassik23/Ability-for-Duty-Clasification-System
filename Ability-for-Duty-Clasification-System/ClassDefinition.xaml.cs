using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json.Linq;

namespace Ability_for_Duty_Clasification_System;

public partial class ClassDefinition : Window
{
    public class Data
    {
        public Data(string name, string type, string selectionType, List<JToken> value)
        {
            Name = name;
            Type = type;
            SelectionType = selectionType;
            Value = value;
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SelectionType { get; set; }
        public List<JToken> Value { get; set; }
    }
    
    public ClassDefinition()
    {
        InitializeComponent();
        List<Data> data = new List<Data>();
        var allValues = App.GetDataKnowledge()?["Все значения"];
        foreach (var variable in App.GetDataTypes()!)
        {
            string selectorType;
            switch (variable.Value?.ToString())
            {
                case "Качественный":
                    selectorType = "ComboBox";
                    break;
                case "Интервальный":
                    selectorType = "TextBox";
                    break;
                case "Логический":
                    selectorType = "CheckBox";
                    break;
                default:
                    selectorType = "CheckBox";
                    break;
            }
            
            data.Add(new Data(variable.Key, variable.Value?.ToString() ?? string.Empty, selectorType, 
                allValues[variable.Key].ToList()));
        }
        DataGrid.ItemsSource = data;
    }
    private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        TextBox? textBox = sender as TextBox;
        if (!(Char.IsDigit(e.Text, 0) || char.IsWhiteSpace(e.Text, 0) || 
              ((e.Text == ",") && (!textBox.Text.Contains(",") && textBox.Text.Length != 0)) ))
        {
            e.Handled = true;
        }
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

    private void ClassificateWindow_OnClick(object sender, RoutedEventArgs e)
    {
        var data = CollectData();
        if (data == null)
        {
            return;
        }

        var resultOfChecking = CheckDigitFields(data);
        if (!resultOfChecking)
        {
            return;
        }
        ResultOfClassificateWindow window = new ResultOfClassificateWindow(data);
        window.Show();
        this.Close();
    }

    private bool CheckDigitFields(JObject data)
    {
        var dataTypes = App.GetDataTypes();
        var allDataKnowledge = App.GetDataKnowledge();
        foreach (var typeField in dataTypes)
        {
            if (typeField.Value!.Value<string>() == "Интервальный")
            {
                string type = allDataKnowledge?.GetValue("Все значения").Value<JObject>().GetValue(typeField.Key)!.Value<string>();
                int startIndex = int.Parse(type.Substring(type.IndexOf('[') + 1, type.IndexOf("..", StringComparison.Ordinal) - type.IndexOf('[') - 1));
                int endIndex = int.Parse(type.Substring(type.IndexOf("..", StringComparison.Ordinal) + 2, type.IndexOf(']') - type.IndexOf("..", StringComparison.Ordinal) - 2));
                char letter = type[0];
                if (letter == 'I')
                {
                    int number;
                    bool isNumeric = int.TryParse(data.GetValue(typeField.Key)?.ToString(), out number);
                    if (!isNumeric)
                    {
                        PrintMessageErrorBox($"Ваше число в поле {typeField.Key} не является типом int");
                        return false;
                    }
                    if ((startIndex > number) || (number > endIndex))
                    {
                        PrintMessageErrorBox($"Ваше число {number.ToString()} в поле {typeField.Key} не принадлежит промежутку {type}");
                        return false;
                    }
                }
                else if (letter == 'R')
                {
                    float number;
                    bool isNumeric = float.TryParse(data.GetValue(typeField.Key)?.ToString(), out number);
                    if (!isNumeric)
                    {
                        PrintMessageErrorBox($"Ваше число в поле {typeField.Key} не является типом Float");
                        return false;
                    }
                    if ((startIndex > number) || (number > endIndex))
                    {
                        PrintMessageErrorBox($"Ваше число {number.ToString()} в поле {typeField.Key} не принадлежит промежутку {type}");
                        return false;
                    }
                }
                
            }
        }

        return true;
    }
    
    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        MainWindow window = new MainWindow();
        window.Show();
        this.Close();
    }
    
    private JObject CollectData()
    {
        JObject selectedValues = new JObject();

        foreach (var item in DataGrid.Items)
        {
            var row = item as Data;
            if (row != null)
            {
                if (DataGrid.Columns[2].GetCellContent(item) is ContentPresenter cellContent)
                {
                    var selectedValue = GetControlValue(cellContent);
                    if (selectedValue == "")
                    {
                        PrintMessageErrorBox("Вы не ввели параметр. Введите параметр");
                        return null;
                    }
                    selectedValues.Add(row.Name, selectedValue);
                }
            }
        }

        return selectedValues;
    }
    
    private string GetControlValue(FrameworkElement? cellContent)
    {
        if (cellContent == null)
            return null!;
        var child = VisualTreeHelper.GetChild(cellContent, 0);
        if (child is TextBox textBox)
        {
            return textBox.Text;
        }
        else if (child is CheckBox checkBox)
        {
            return checkBox.IsChecked == true ? "Да" : "Нет";
        }
        else if (child is ComboBox comboBox)
        {
            return (comboBox.SelectedItem == null ? "" : ((JValue)comboBox.SelectedItem).Value<string>()) ?? "";
        }

        return null!;
    }

    private void PrintMessageErrorBox(string message)
    {
        MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}