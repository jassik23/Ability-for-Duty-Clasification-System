using System.Windows;
using System.Windows.Controls;

namespace Ability_for_Duty_Clasification_System;

public class DynamicDataTemplateSelector: DataTemplateSelector
{
    public override DataTemplate
        SelectTemplate(object item, DependencyObject container)
    {
        FrameworkElement element = container as FrameworkElement;

        if (element != null && item != null && item is ClassDefinition.Data)
        {
            ClassDefinition.Data? model = item as ClassDefinition.Data;

            return (DataTemplate)element.FindResource(model.SelectionType + "Template");
        }

        return null;
    }
}