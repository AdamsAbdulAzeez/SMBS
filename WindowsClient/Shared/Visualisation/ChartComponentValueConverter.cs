using System;
using System.Globalization;
using System.Windows.Data;
using WindowsClient.Shared.Visualisation.CartesianChart;

namespace WindowsClient.Shared.Visualisation
{
    internal class CartesianChartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ICartesianChartControl chartControlContract) return null;

            return chartControlContract as CartesianChartControl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
