using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WindowsClient.Shared.Visualisation
{
    public class ConverterUtils
    {
        public static Brush StringToBrush(string color)
        {
            return new BrushConverter().ConvertFromString(color) as SolidColorBrush;
        }

        public static Visibility GetVisibility(string visibility)
        {
            if (visibility == "Visible")
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
    }
}
