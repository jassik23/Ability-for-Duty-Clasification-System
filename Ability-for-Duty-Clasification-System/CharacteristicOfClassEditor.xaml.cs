using System.Windows;

namespace Ability_for_Duty_Clasification_System;

public partial class CharacteristicOfClassEditor : Window
{
    public CharacteristicOfClassEditor()
    {
        InitializeComponent();
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
}