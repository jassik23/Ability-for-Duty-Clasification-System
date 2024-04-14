using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace Ability_for_Duty_Clasification_System;

public partial class ClassDefinitionTemplateAdding : Window
{
    public ClassDefinitionTemplateAdding()
    {
        InitializeComponent();
        CharacteristicTypeComboBox.ItemsSource = new string[] { "Качественный", "Интервальный", "Логический" };
    }
    private void MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        string characteristicName = CharacteristicNameTextBox.Text;
        if (characteristicName == "" || CharacteristicTypeComboBox.SelectedItem == null)
        {
            CheckValueFunctions.CreateErrorMessage("Введите параметры для добавления");
            return;
        }

        string characteristicType = CharacteristicTypeComboBox.SelectedItem.ToString()!;
        switch (characteristicType)
        {
            case "Качественный":
                if (!(CheckValueFunctions.CheckQualitativeElement(CharacteristicValueTextBox.Text)))
                {
                    return;
                }
                var characteristicValue = JArray.FromObject(CharacteristicValueTextBox.Text.Split("; ")); 
                if (!AddCharacteristicOfClass(characteristicName, characteristicType, characteristicValue)) return;
                break;
            case "Интервальный":
                string? characteristicIntervalValue = CharacteristicValueTextBox.Text;
                if (!(CheckValueFunctions.CheckIntervalElement(characteristicIntervalValue)))
                {
                    return;
                }
                if (!AddCharacteristicOfClass(characteristicName, characteristicType, (JValue)CharacteristicValueTextBox.Text)) return;
                break;
            case "Логический":
                if (!AddCharacteristicOfClass(CharacteristicNameTextBox.Text,
                    CharacteristicTypeComboBox.SelectedItem.ToString()!, new JArray() { "Да", "Нет" })) return;
                break;
        }

        TemplateCharacteristicOfClassEditor window = new TemplateCharacteristicOfClassEditor();
        window.Show();
        this.Close();
    }

    
    
    private bool AddCharacteristicOfClass(string characteristicName, string characteristicType,
        object characteristicValue)
    {
        if (!App.GetDataTypes()!.TryAdd(characteristicName, characteristicType))
        {
            CheckValueFunctions.CreateErrorMessage("Такой признак уже есть");
            return false;
        }
        App.GetDataTemplateKnowledge()!.Add(characteristicName, (JToken)characteristicValue);
        foreach (var classes in App.GetDataKnowledge()!)
        {
            classes.Value!.Value<JObject>()!.Add(characteristicName, "");
        }

        return true;
    }
    
    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        TemplateCharacteristicOfClassEditor window = new TemplateCharacteristicOfClassEditor();
        window.Show();
        this.Close();
    }

    private void CharacteristicTypeComboBox_OnSelected(object sender, RoutedEventArgs e)
    {
        string selectedState = CharacteristicTypeComboBox.SelectedItem.ToString()!;
        if (selectedState == "Логический")
        {
            CharacteristicValueTextBox.Visibility = Visibility.Hidden;
            CharacteristicValueTextBlock.Visibility = Visibility.Hidden;
            return;
        }

        CharacteristicValueTextBlock.Visibility = Visibility.Visible;
        CharacteristicValueTextBox.Visibility = Visibility.Visible;
    }

    private void TypeHintItem_OnClick(object sender, RoutedEventArgs e)
    {
        string hint = string.Join("\n",
            $"Качественный признак вводится следующим способом:",
            $"признак1; признак2; ... признакn",
            $"То есть ваши признаки должны быть введены в одну строчку через символ '; '",
            $"Интервальный признак вводится следующим образом:",
            $"(I|R)[число1..число2]",
            $"То есть в начале вы вводите тип интервала(I - целые числа, R - рациональные числа), после чего эти числа",
            $"Пример: I[1..5] R[1,31..3,16]"
        );
        MessageBox.Show(hint, "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    
    
}