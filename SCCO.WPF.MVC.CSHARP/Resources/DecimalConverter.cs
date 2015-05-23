using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SCCO.WPF.MVC.CS.Resources
{
    [ValueConversion(typeof(Decimal), typeof(String))]
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            if(string.IsNullOrEmpty(value.ToString()))
                return string.Empty;

            var amount = (Decimal)value;
            if (amount == 0m)
                return string.Empty;

            var result = amount.ToString("N2");
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value as string;
            if (strValue == string.Empty)
                return 0m;

            Decimal resultAmount;
            if (Decimal.TryParse(strValue, out resultAmount))
            {
                return resultAmount;
            }
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(Decimal), typeof(String))]
    public class DecimalConverterDefaultZero : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0.00";

            if (string.IsNullOrEmpty(value.ToString()))
                return "0.00";

            var amount = (Decimal)value;
            if (amount == 0m)
                return "0.00";

            var result = amount.ToString("N2");
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value as string;
            if (strValue == string.Empty)
                return 0m;

            Decimal resultAmount;
            if (Decimal.TryParse(strValue, out resultAmount))
            {
                return resultAmount;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
