using System;
using System.Globalization;
using System.Windows.Data;

namespace SCCO.WPF.MVC.CS.Resources
{
    [ValueConversion(typeof (Decimal), typeof (String))]
    internal class DecimalPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            if (string.IsNullOrEmpty(value.ToString()))
                return string.Empty;

            var amount = (Decimal) value;
            if (amount == 0m)
                return string.Empty;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  CultureInfo culture)
        {
            if (targetType != typeof (decimal) || value == null)
                return value;

            string str = value.ToString();

            if (String.IsNullOrWhiteSpace(str))
                return 0M;

            str = str.TrimEnd(culture.NumberFormat.PercentSymbol.ToCharArray());

            decimal result;
            if (decimal.TryParse(str, out result))
            {
                result /= 100;
            }

            return result;
        }
    }

    [ValueConversion(typeof (Decimal), typeof (String))]
    internal class PersistedDecimalPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            if (value == null)
                return 0m;

            if (string.IsNullOrEmpty(value.ToString()))
                return 0m;

            var amount = (Decimal) value;
            if (amount == 0m)
                return 0m;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  CultureInfo culture)
        {
            if (targetType != typeof (decimal) || value == null)
                return 0m;

            string str = value.ToString();

            if (String.IsNullOrWhiteSpace(str))
                return 0m;

            str = str.TrimEnd(culture.NumberFormat.PercentSymbol.ToCharArray());

            decimal result;
            if (decimal.TryParse(str, out result))
            {
                result /= 100;
            }

            return result;
        }
    }
}