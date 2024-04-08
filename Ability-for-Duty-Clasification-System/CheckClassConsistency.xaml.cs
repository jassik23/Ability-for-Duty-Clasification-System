using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace Ability_for_Duty_Clasification_System;

public partial class CheckClassConsistency : Window
{

    public class DataGridClassItem
    {
        public DataGridClassItem(string checkClass, string resultOfCheck)
        {
            CheckClass = checkClass;
            ResultOfCheck = resultOfCheck;
        }

        public string CheckClass { get; set; }
        public string ResultOfCheck { get; set; }
        
    }

    public class DataGridCharacterItem
    {
        public DataGridCharacterItem(string character, string resultOfCheck, string breakedClasses)
        {
            Character = character;
            ResultOfCheck = resultOfCheck;
            BreakedClasses = breakedClasses;
        }

        public string Character { get; set; }
        public string ResultOfCheck { get; set; }
        public string BreakedClasses { get; set; }
    }
    
    private KeyValuePair<List<DataGridCharacterItem>, List<DataGridClassItem>> GetConsistencyForClass()
    {
        List<DataGridCharacterItem> resultOfConsistencyForCharacter = new List<DataGridCharacterItem>();
        List<DataGridClassItem> resultOfConsistencyForClass = new List<DataGridClassItem>();
        Dictionary<string, string> resultOfConsistencyForClassDict = new Dictionary<string, string>();
        foreach (var dataClass in App.GetDataKnowledge()!)
        {
            if (dataClass.Key == "Все значения")
            {
                continue;
            }

            bool isRight = true;
            foreach (var dataClassValue in ((JObject)dataClass.Value!)!)
            {
                if (dataClassValue.Value is not JArray)
                {
                    if ((string)dataClassValue.Value! == "")
                    {
                        isRight = false;
                        if (resultOfConsistencyForClassDict.ContainsKey(dataClassValue.Key))
                        {
                            resultOfConsistencyForClassDict[dataClassValue.Key] =
                                resultOfConsistencyForClassDict[dataClassValue.Key] + "; " + dataClass.Key;
                        }
                        else
                        {
                            resultOfConsistencyForClassDict[dataClassValue.Key] =
                                dataClass.Key;
                        }
                    }
                }
            }
            if (isRight)
            {
                resultOfConsistencyForClass.Add(new DataGridClassItem(dataClass.Key, "Проверка пройдена"));
            }
            else
            {
                resultOfConsistencyForClass.Add(new DataGridClassItem(dataClass.Key, "Отсутствует значение"));
            }
        }

        foreach (var character in App.GetDataTypes()!)
        {
            if (resultOfConsistencyForClassDict.TryGetValue(character.Key, out var value))
            {
                resultOfConsistencyForCharacter.Add(new DataGridCharacterItem(character.Key, "Отсутствует значение",
                    value));
            }
            else
            {
                resultOfConsistencyForCharacter.Add(new DataGridCharacterItem(character.Key, "Проверка пройдена", ""));
            }
        }

        return new KeyValuePair<List<DataGridCharacterItem>, List<DataGridClassItem>>(resultOfConsistencyForCharacter, resultOfConsistencyForClass);
    }
    
    public CheckClassConsistency()
    {
        InitializeComponent();
        KeyValuePair<List<DataGridCharacterItem>, List<DataGridClassItem>> results = GetConsistencyForClass();
        dataGridClass.ItemsSource = results.Value;
        dataGridCharacter.ItemsSource = results.Key;
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
        BaseOfKnowlengeEditor window = new BaseOfKnowlengeEditor();
        window.Show();
        this.Close();
    }
}