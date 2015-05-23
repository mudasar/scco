using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SCCO.WPF.MVC.CS.Resources
{
    [ValueConversion(typeof(string), typeof(string))]
    public class AccountNatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var nature = value.ToString();
            switch (nature.ToLower())
            {
                case "d":
                    return "Debit";
                case "c":
                    return "Credit";
            }
            
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value as string;
            if (strValue == string.Empty)
                return "";

            var nature = value.ToString();
            switch (nature.ToLower())
            {
                case "debit":
                    return "D";
                case "credit":
                    return "C";
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
