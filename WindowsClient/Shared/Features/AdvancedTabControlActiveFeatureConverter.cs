using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ActiproSoftware.Windows.Controls.Docking;

namespace WindowsClient.Shared.Features
{
    internal class AdvancedTabControlActiveFeatureConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var tabItems = values[1] as IEnumerable;
            var activeTab = values[0];

            foreach (var tabItem in tabItems)
            {
                var tab = (AdvancedTabItem)tabItem;
                if (tab.Tag == null) continue;
                if (tab.Tag.ToString() == activeTab.ToString())
                {
                    return tabItem;
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { (value as FrameworkElement)?.Tag };
        }
    }
}
