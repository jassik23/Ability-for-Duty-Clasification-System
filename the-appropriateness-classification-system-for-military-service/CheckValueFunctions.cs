using System.Text.RegularExpressions;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace Ability_for_Duty_Clasification_System;

public static class CheckValueFunctions
{
    public static bool CheckQualitativeElement(string characteristicQualitativeValue)
    {
        Regex regexQual = new Regex(@"^(\w+\s)*\w+(; (\w+\s)*\w+)*$");
        if (!(regexQual.IsMatch(characteristicQualitativeValue)))
        {
            CheckValueFunctions.CreateErrorMessage("Введенные данные не соответствуют формату ввода интервальных значений");
            return false;
        }

        return true;
    }

    public static float GetStartValue(string value)
    {
        return float.Parse(value.Substring(value.IndexOf('[') + 1, 
            value.IndexOf("..", StringComparison.Ordinal) - value.IndexOf('[') - 1));
    }

    public static float GetEndValue(string value)
    {
        return float.Parse(value.Substring
        (value.IndexOf("..", StringComparison.Ordinal) + 2, value.IndexOf(']') - value.IndexOf
            ("..", StringComparison.Ordinal) - 2));
    }
    
    public static bool CheckIntervalElement(string characteristicIntervalValue)
    {
        Regex regexInter = new Regex(@"^(I\[(-\d+|\d+)\.\.(-\d+|\d+)\])|(R\[(-\d+,\d+|\d+,\d+|-\d+|\d+)\.\.(-\d+,\d+|\d+,\d+|-\d+|\d+)\])$");
        if (!(regexInter.IsMatch(characteristicIntervalValue)))
        {
            CreateErrorMessage("Введенные данные не соответствуют формату ввода интервальных значений");
            return false;
        }
        float startIndex = GetStartValue(characteristicIntervalValue);
        float endIndex = GetEndValue(characteristicIntervalValue);
        if (endIndex <= startIndex)
        {
            CreateErrorMessage("Значения от должно быть меньше значения до");
            return false;
        }

        return true;
    }
    public static void CreateErrorMessage(string description)
    {
        MessageBox.Show(description, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}