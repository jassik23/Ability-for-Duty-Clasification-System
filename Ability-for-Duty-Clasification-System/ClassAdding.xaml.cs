using System.Windows;

namespace Ability_for_Duty_Clasification_System;

public partial class ClassAdding : Window
{
    public ClassAdding()
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
}