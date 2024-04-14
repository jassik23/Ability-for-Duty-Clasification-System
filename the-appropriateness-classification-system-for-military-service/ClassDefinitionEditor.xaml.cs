using System.Globalization;
using System.Net.Mime;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace Ability_for_Duty_Clasification_System;

public partial class ClassDefinitionEditor : Window
{
    public class Characteristic
    {
        public Characteristic(string? characteristicValue, bool isUsed)
        {
            CharacteristicValue = characteristicValue;
            IsUsed = isUsed;
        }

        public string? CharacteristicValue { get; set; }
        public bool IsUsed { get; set; }
    }
    
    private readonly string _selectedClass;
    private CharacteristicOfClassEditor.DataGridData _selectedData;
    public ClassDefinitionEditor(CharacteristicOfClassEditor.DataGridData selectedData, string selectedClass)
    {
        InitializeComponent();
        _selectedData = selectedData;
        SelectedCharacteristicTextBlock.Text = selectedData.CharacteristicName;
        TypeOfSelectedCharacteristicTextBlock.Text = selectedData.CharacteristicType;
        _selectedClass = selectedClass;
        if (_selectedClass == "")
        {
            TemplateInputBox.Visibility = Visibility.Visible;
            TemplateInputBox.Text = selectedData.CharacteristicType == "Качественный" ? 
                selectedData.CharacteristicValue.Replace("[", "").Replace("]", "").Replace(",", ";"):
                selectedData.CharacteristicValue;
        }
        else if (selectedData.CharacteristicType == "Интервальный")
        {
            InputWrapPanel.Visibility = Visibility.Visible;
            CharacteristicValueTextBoxFrom.Text = CheckValueFunctions.GetStartValue(selectedData.CharacteristicValue).ToString(CultureInfo.CurrentCulture);
            CharacteristicValueTextBoxTo.Text = CheckValueFunctions.GetEndValue(selectedData.CharacteristicValue).ToString(CultureInfo.CurrentCulture);
        }
        else // Для изменения признаков класса типа логика и качество
        {
            List<Characteristic> elements = new List<Characteristic>();
            ValuesDataGrid.Visibility = Visibility.Visible;
            foreach (var value in (JArray)(App.GetDataTemplateKnowledge()!.GetValue(selectedData.CharacteristicName)!))
            {
                bool isUsed = _selectedData.CharacteristicValue.Contains(value.Value<string>()!);
                elements.Add(new Characteristic(value.Value<string>(), isUsed));
            }

            ValuesDataGrid.ItemsSource = elements;
        }
    }

    private void MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void SaveCharacteristicButton_OnClick(object sender, RoutedEventArgs e)
    {
        
        if (_selectedClass == "")
        {
            if (!SaveTemplateCharacteristic()) return;
            var window = new TemplateCharacteristicOfClassEditor();
            window.Show();
            this.Close();
        }
        else
        {
            if (!SaveClassCharacteristic()) return;
            var window = new CharacteristicOfClassEditor(_selectedClass);
            window.Show();
            this.Close();
        }
    }

    private bool SaveClassCharacteristic()
    {
        switch (_selectedData.CharacteristicType)
        {
            case "Интервальный":
                var intervalValue = App.GetDataTemplateKnowledge()!.GetValue(_selectedData.CharacteristicName)!.Value<string>()!;
                if (!CheckInputValues()) return false;
                App.GetDataKnowledge()!.GetValue(_selectedClass)!.
                    Value<JObject>()!.GetValue(_selectedData.CharacteristicName)!.Replace(
                        new JValue(
                            $"{intervalValue[0]}[{CharacteristicValueTextBoxFrom.Text}..{CharacteristicValueTextBoxTo.Text}]"
                            ));
                break;
            case "Логический": case "Качественный":
                JArray values = new JArray();
                foreach (var character in ValuesDataGrid.Items)
                {
                    var ch = (Characteristic)character;
                    if (ch.IsUsed)
                    {
                        values.Add(ch.CharacteristicValue);
                    }
                }
                App.GetDataKnowledge()!.GetValue(_selectedClass)!.Value<JObject>()!.GetValue(_selectedData.CharacteristicName)!.Replace(values);
                break;
        }

        return true;
    }

    private bool CheckInputValues()
    {
        var intervalValue = App.GetDataTemplateKnowledge()!.GetValue(_selectedData.CharacteristicName)!.Value<string>()!;
        var startIsNumeric = float.TryParse(CharacteristicValueTextBoxFrom.Text, out var startValue);
        var endIsNumeric = float.TryParse(CharacteristicValueTextBoxTo.Text, out var endValue);
        var startTemplateValue = CheckValueFunctions.GetStartValue(intervalValue);
        var endTemplateValue = CheckValueFunctions.GetEndValue(intervalValue);
        if (!startIsNumeric || !endIsNumeric)
        {
            CheckValueFunctions.CreateErrorMessage("Вы ввели не число");
            return false;
        }

        if (startValue >= endValue)
        {
            CheckValueFunctions.CreateErrorMessage("Значение 'От' не может быть больше значения 'До'");
            return false;
        }
        if ((startValue < startTemplateValue) || (endTemplateValue < endValue))
        {
            CheckValueFunctions.CreateErrorMessage("Значения не соответствуют определенным " +
                                                   "минимальным и/или максимальным значениям");
            return false;
        }

        if (intervalValue[0] == 'I' && (startValue != (int)startValue || endValue != (int)endValue))
        {
            CheckValueFunctions.CreateErrorMessage("В данном признаке значения могут быть только целыми числами");
            return false;
        }

        return true;
    }
    
    private bool SaveTemplateCharacteristic()
    {
        switch (_selectedData.CharacteristicType)
        {
            case "Качественный":
                if (!(CheckValueFunctions.CheckQualitativeElement(TemplateInputBox.Text)))
                {
                    return false;
                }

                var characteristicValue = TemplateInputBox.Text;
                foreach (var characteristicOfClass in App.GetDataKnowledge()!)
                {
                    foreach (var character in (JArray)(characteristicOfClass.Value!.Value<JObject>()!.GetValue(_selectedData.CharacteristicName)!))
                    {
                        if (!characteristicValue.Contains(character.Value<string>()!))
                        {
                            MessageBox.Show($"Вы не можете изменить все значения, так как в значениях " +
                                            $"прихнаков классов есть значение, которого нет здесь. Значение: {character.Value<string>()}", 
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }
                }
                App.GetDataTemplateKnowledge()!.GetValue(_selectedData.CharacteristicName)!.Replace(
                    JArray.FromObject(characteristicValue.Split("; ")));
                break;
            case "Интервальный":
                if (!(CheckValueFunctions.CheckQualitativeElement(TemplateInputBox.Text)))
                {
                    return false;
                }
                break;
        }

        return true;
    }

    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_selectedClass == "")
        {
            TemplateCharacteristicOfClassEditor window = new TemplateCharacteristicOfClassEditor();
            window.Show();
        }
        else
        {
            CharacteristicOfClassEditor window = new CharacteristicOfClassEditor(_selectedClass);
            window.Show();
        }
        this.Close();
    }
}