using System.Windows;
using Newtonsoft.Json.Linq;

namespace Ability_for_Duty_Clasification_System;

public partial class TemplateCharacteristicOfClassEditor : Window
{
    
    public TemplateCharacteristicOfClassEditor()
    {
        InitializeComponent();
        SetTemplateData();
    }

    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        BaseOfKnowlengeEditor window = new BaseOfKnowlengeEditor();
        window.Show();
        this.Close();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        MainWindow window = new MainWindow();
        window.Show();
        this.Close();
    }

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        ClassDefinitionTemplateAdding window = new ClassDefinitionTemplateAdding();
        window.Show();
        this.Close();
    }

    private void SetTemplateData()
    {
        JObject? characteristicTypes = App.GetDataTypes();
        List<CharacteristicOfClassEditor.DataGridData> templateData = new List<CharacteristicOfClassEditor.DataGridData>();
        foreach (var character in App.GetDataTemplateKnowledge()!)
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
            
            templateData.Add(new CharacteristicOfClassEditor.DataGridData(character.Key, 
                ((string)characteristicTypes!.GetValue(character.Key))!, characterValue));
        }

        CharacteristicDataGrid.ItemsSource = templateData;
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedItem = GetSelectedItem();
        if (selectedItem == null)
        {
            return;
        }

        var answer = MessageBox.Show("Вы точно хотите удалить признак? " +
                        "Если вы удалите признак, то этот признак удалится во всех классах", "",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (answer == MessageBoxResult.Yes)
        {
            var characteristicName =
                selectedItem.CharacteristicName;
            App.GetDataTemplateKnowledge()!.Remove(characteristicName);
            App.GetDataTypes()!.Remove(characteristicName);
            foreach (var classCharacteristics in App.GetDataKnowledge()!)
            {
                classCharacteristics.Value!.Value<JObject>()!.Remove(characteristicName);
            }
        }
        SetTemplateData();
        return;
    }
    
    private void CreateErrorMessage(string description)
    {
        MessageBox.Show(description, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void ChangeButton_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedItem = GetSelectedItem();
        if (selectedItem == null)
        {
            return;
        }

        if (selectedItem.CharacteristicType == "Логический")
        {
            CheckValueFunctions.CreateErrorMessage("Логический признак у всех значений изменить нельзя, " +
                                                   "так как логический тип всегда 'Да' или 'Нет'");
            return; 
        }
        ClassDefinitionEditor window = new ClassDefinitionEditor(selectedItem, "");
        window.Show();
        this.Close();
    }

    private CharacteristicOfClassEditor.DataGridData? GetSelectedItem()
    {
        if (CharacteristicDataGrid.SelectedItem == null)
        {
            CreateErrorMessage("Вы не выбрали элемент для удаления");
            return null;
        }

        return (CharacteristicOfClassEditor.DataGridData)CharacteristicDataGrid.SelectedItem;
    }
}