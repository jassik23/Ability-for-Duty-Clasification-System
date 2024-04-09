using System.Windows;
using Newtonsoft.Json.Linq;


namespace Ability_for_Duty_Clasification_System;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void EditBase_OnClick(object sender, RoutedEventArgs e)
    {
        BaseOfKnowlengeEditor window = new BaseOfKnowlengeEditor();
        window.Show();
        this.Close();
    }

    private void ClassDefinition_OnClick(object sender, RoutedEventArgs e)
    {
        if (IsConsistency())
        {
            ClassDefinition window = new ClassDefinition();
            window.Show();
            this.Close();
        }
        else
        {
            MessageBox.Show("Проверка целостности базы знаний не прошла. Заполните недостающие элементы.", 
                "Проверка не прошла", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }

    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private bool IsConsistency()
    {
        foreach (var dataClass in App.GetDataKnowledge()!)
        {
            foreach (var dataClassValue in ((JObject)dataClass.Value!)!)
            {
                if (dataClassValue.Value is not JArray)
                {
                    if ((string)dataClassValue.Value! == "")
                    {
                        return false;
                    }
                }
            }
            
        }
        return true;
    }
    
}