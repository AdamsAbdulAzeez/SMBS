using System;
using System.Globalization;
using System.Windows.Data;

namespace WindowsClient.ApplicationLayout
{
    public class ZeroToEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val)
            {
                if (val == 0)
                {
                    return null;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? value : 0;
        }
    }
}
