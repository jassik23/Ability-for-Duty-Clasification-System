using System.Windows;

namespace Ability_for_Duty_Clasification_System;

public partial class ClassDefinition : Window
{
    public ClassDefinition()
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

    private void ClassificateWindow_OnClick(object sender, RoutedEventArgs e)
    {
        ResultOfClassificateWindow window = new ResultOfClassificateWindow();
        window.Show();
        this.Close();
    }

    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        MainWindow window = new MainWindow();
        window.Show();
        this.Close();
    }
}