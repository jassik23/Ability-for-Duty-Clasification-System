using System.Windows;

namespace Ability_for_Duty_Clasification_System;

public partial class BaseOfKnowlengeEditor : Window
{
    public BaseOfKnowlengeEditor()
    {
        InitializeComponent();
    }
    private void ClassButton_OnClick(object sender, RoutedEventArgs e)
    {
        ClassEditor newWindow = new ClassEditor();
        newWindow.Show();
        this.Close();
    }

    private void EditMarks_OnClick(object sender, RoutedEventArgs e)
    {
        CharacteristicOfClassEditor newWindow = new CharacteristicOfClassEditor(null);
        newWindow.Show();
        this.Close();
    }

    private void CheckBaseOfKnowlenge_OnClick(object sender, RoutedEventArgs e)
    {
        CheckClassConsistency newWindow = new CheckClassConsistency();
        newWindow.Show();
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

    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        MainWindow window = new MainWindow();
        window.Show();
        this.Close();
    }
}