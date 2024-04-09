using System.Windows;
using Newtonsoft.Json.Linq;

namespace Ability_for_Duty_Clasification_System;

public partial class ClassAddingAndRename : Window
{
    private readonly bool _isEditor = false;
    private readonly string _classEditingName = "";
    public ClassAddingAndRename(bool isEditor, string classEditingName)
    {
        InitializeComponent();
        if (isEditor)
        {
            AddClassButton.Content = "Изменить класс";
            ClassNameTextBox.Text = classEditingName;
        }
        this._classEditingName = classEditingName;
        this._isEditor = isEditor;
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

    private void AddClassButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (ClassNameTextBox.Text == "")
        {
            MessageBox.Show("Вы не ввели имя класса. Пожалуйста, введите имя класса", "Класс не введён", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (App.GetDataKnowledge()!.TryGetValue(ClassNameTextBox.Text, out JToken _))
        {
            MessageBox.Show("Такой класс уже существует. Вы не можете добавить такой же класс",
                "Низя", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (_isEditor)
        {
            App.GetDataKnowledge()!.Add(ClassNameTextBox.Text, App.GetDataKnowledge()!.GetValue(_classEditingName));
            App.GetDataKnowledge()!.Remove(_classEditingName);
        }
        else
        {
            JObject newClass = new JObject();
            foreach (var templateClass in App.GetDataTemplateKnowledge()!)
            {
                newClass.Add(templateClass.Key, "");
            }
            App.GetDataKnowledge()!.Add(ClassNameTextBox.Text, newClass);
        }
        ClassEditor window = new ClassEditor();
        window.Show();
        this.Close();

    }
}