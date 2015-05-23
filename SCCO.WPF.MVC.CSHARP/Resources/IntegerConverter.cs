using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SCCO.WPF.MVC.CS.Resources
{
    [ValueConversion(typeof(int), typeof(String))]
    public class IntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var amount = (int)value;
            if (amount == 0m)
                return string.Empty;

            var result = amount.ToString(CultureInfo.InvariantCulture);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value as string;
            if (strValue == string.Empty)
                return 0m;

            int resultAmount;
            if (int.TryParse(strValue, out resultAmount))
            {
                return resultAmount;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
