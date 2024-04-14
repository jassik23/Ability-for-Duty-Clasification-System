using System.Windows;
using System.Windows.Controls;

namespace Ability_for_Duty_Clasification_System;

public partial class ClassEditor : Window
{
    public ClassEditor()
    {
        InitializeComponent();
        SetClassNames();
    }
    private void EditClassName_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedItem = CheckSelectedItem("Выберите класс для изменения названия");
        if (selectedItem is not null)
        {
            ClassAddingAndRename window = new ClassAddingAndRename(true, selectedItem.ToString());
            window.Show();
            this.Close();
        }
    }

    private void AddClass_OnClick(object sender, RoutedEventArgs e)
    {
        ClassAddingAndRename window = new ClassAddingAndRename(false, "");
        window.Show();
        this.Close();
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

    private void DeleteClassButton_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedItem = CheckSelectedItem("Выберите класс");
        if (selectedItem is not null)
        {
            var answer = MessageBox.Show($"Вы точно хотите удалить класс {selectedItem.ToString()}?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer is MessageBoxResult.Yes)
            {
                App.GetDataKnowledge()!.Remove(selectedItem.ToString()!);
                SetClassNames();
            }
        }
    }

    private void SetClassNames()
    {
        List<string> classesName = new List<string>();
        foreach (var classes in App.GetDataKnowledge()!)
        {
            classesName.Add(classes.Key);
        }
        
        CategoriesListBox.ItemsSource = classesName;
    }
    private object? CheckSelectedItem(string description)
    {
        var selectedItem = CategoriesListBox.SelectedItem;
        if (selectedItem is null)
        {
            MessageBox.Show(description, "Выбор", MessageBoxButton.OK, MessageBoxImage.Information);
            return null;
        }
        return selectedItem;
    }

    private void ClassDescriptionButton_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedItem = CheckSelectedItem("Выберите класс");
        if (selectedItem is not null)
        {
            CharacteristicOfClassEditor window = new CharacteristicOfClassEditor(selectedItem.ToString());
            window.Show();
            this.Close();
        }
    }

    private void Back_OnClick(object sender, RoutedEventArgs e)
    {
        BaseOfKnowlengeEditor window = new BaseOfKnowlengeEditor();
        window.Show();
        this.Close();
    }
}