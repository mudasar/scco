using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SCCO.WPF.MVC.CS.Resources
{
    [ValueConversion(typeof(Decimal), typeof(String))]
    class DecimalPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                      System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            if (string.IsNullOrEmpty(value.ToString()))
                return string.Empty;

            var amount = (Decimal)value;
            if (amount == 0m)
                return string.Empty;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(decimal) || value == null)
                return value;

            string str = value.ToString();

            if (String.IsNullOrWhiteSpace(str))
                return 0M;

            str = str.TrimEnd(culture.NumberFormat.PercentSymbol.ToCharArray());

            decimal result = 0M;
            if (decimal.TryParse(str, out result))
            {
                result /= 100;
            }

            return result;
        }
    }
}
