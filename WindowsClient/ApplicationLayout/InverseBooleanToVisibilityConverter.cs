﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WindowsClient.ApplicationLayout
{
    internal class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            =>(bool)value == true ? Visibility.Collapsed : Visibility.Visible;


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception();
        }
    }

}
