using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        ClassDefinition window = new ClassDefinition();
        window.Show();
        this.Close();
    }

    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}