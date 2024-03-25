using System.Windows;
using System.Windows.Controls;

namespace Ability_for_Duty_Clasification_System;

public partial class ClassEditor : Window
{
    public ClassEditor()
    {
        InitializeComponent();
        TextBlock temp = new TextBlock();
        temp.FontSize = 16;
        temp.Text = "Категория А";
        
        CategoriesListBox.Items.Add(temp);
    }
    private void EditMarks_OnClick(object sender, RoutedEventArgs e)
    {
        CharacteristicOfClassEditor newWindow = new CharacteristicOfClassEditor();
        newWindow.Show();
        this.Close();
    }

    private void AddClass_OnClick(object sender, RoutedEventArgs e)
    {
        ClassAdding window = new ClassAdding();
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
}