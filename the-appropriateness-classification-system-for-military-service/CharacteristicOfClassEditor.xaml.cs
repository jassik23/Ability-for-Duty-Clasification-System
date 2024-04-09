using System.Windows;
using Newtonsoft.Json.Linq;

namespace Ability_for_Duty_Clasification_System;

public partial class CharacteristicOfClassEditor : Window
{

    private class DataGridData
    {
        public DataGridData(string characteristicName, string characteristicType, string characteristicValue)
        {
            CharacteristicName = characteristicName;
            CharacteristicType = characteristicType;
            CharacteristicValue = characteristicValue;
        }

        public string CharacteristicName { get; set; }
        public string CharacteristicType { get; set; }
        public string CharacteristicValue { get; set; }
    }
    
    private List<string> GetClassNames()
    {
        List<string> classNames = new List<string>();
        JObject? classes = App.GetDataKnowledge();
        foreach (var className in classes!)
        {
            classNames.Add(className.Key);
        }

        return classNames;
    }
    
    public CharacteristicOfClassEditor(string? selectedClass)
    {
        InitializeComponent();
        
        List<string> classNames = GetClassNames();
        ClassComboBox.ItemsSource = classNames;
        if (selectedClass is not null)
        {
            ClassComboBox.SelectedItem = selectedClass;
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

    private void ClassDefinitionEditor_OnClick(object sender, RoutedEventArgs e)
    {
        ClassDefinitionEditor window = new ClassDefinitionEditor();
        window.Show();
        this.Close();
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        return;
    }

    private void ChangeButton_OnClick(object sender, RoutedEventArgs e)
    {
        ClassDefinitionEditor window = new ClassDefinitionEditor();
        window.Show();
        this.Close();
    }

    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        BaseOfKnowlengeEditor window = new BaseOfKnowlengeEditor();
        window.Show();
        this.Close();
    }

    private void ClassComboBox_OnSelected(object sender, RoutedEventArgs e)
    {
        string selectedState = ClassComboBox.SelectedItem.ToString()!;
        JObject selectedCharacteristics = (JObject)App.GetDataKnowledge()!.GetValue(selectedState)!;
        JObject selectedCharacteristicTypes = App.GetDataTypes()!;
        List<DataGridData> data = new List<DataGridData>();
        foreach (var character in selectedCharacteristics)
        {
            string characterValue;
            if (character.Value is JArray)
            {
                characterValue = "[" + string.Join(", ", character.Value!) + "]";
            }
            else
            {
                characterValue = (string)character.Value!;
            }
            
            data.Add(new DataGridData(character.Key, 
                ((string)selectedCharacteristicTypes.GetValue(character.Key))!, characterValue));
        }

        dataGridData.ItemsSource = data;
    }
}