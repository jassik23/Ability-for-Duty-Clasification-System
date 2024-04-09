using System.Windows;

namespace Ability_for_Duty_Clasification_System;

public partial class ClassDefinitionEditor : Window
{
    public ClassDefinitionEditor()
    {
        InitializeComponent();
    }

    private void MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}