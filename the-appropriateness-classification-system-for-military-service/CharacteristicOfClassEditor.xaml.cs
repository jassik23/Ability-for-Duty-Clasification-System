using System.Windows;
using Newtonsoft.Json.Linq;

namespace Ability_for_Duty_Clasification_System;

public partial class CharacteristicOfClassEditor : Window
{

    public class DataGridData
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
        ClassDefinitionTemplateAdding window = new ClassDefinitionTemplateAdding();
        window.Show();
        this.Close();
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedItem = GetSelectedItem();
        if (selectedItem == null)
        {
            return;
        }

        var onResetAnswer = MessageBox.Show("Вы точно хотите отчистить значения?", "OK?",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (onResetAnswer == MessageBoxResult.No)
        {
            return;
        }
        App.GetDataKnowledge()!.GetValue(ClassComboBox.Text)!.Value<JObject>()!.GetValue(
            selectedItem.CharacteristicName)!.Replace(new JValue(""));
        UpdateDataGrid();
    }

    private void ChangeButton_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedItem = GetSelectedItem();
        if (selectedItem == null)
        {
            return;
        }
        ClassDefinitionEditor window = new ClassDefinitionEditor(selectedItem, ClassComboBox.Text);
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
        UpdateDataGrid();
    }
    private CharacteristicOfClassEditor.DataGridData? GetSelectedItem()
    {
        if (CharacteristicDataGrid.SelectedItem == null)
        {
            CheckValueFunctions.CreateErrorMessage("Вы не выбрали элемент");
            return null;
        }

        return (CharacteristicOfClassEditor.DataGridData)CharacteristicDataGrid.SelectedItem;
    }

    private void UpdateDataGrid()
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

        CharacteristicDataGrid.ItemsSource = data;
    }
    
}