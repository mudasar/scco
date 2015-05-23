using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SCCO.WPF.MVC.CS.Resources
{
    [ValueConversion(typeof (string), typeof (bool))]
    public class StringToBooleanConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = value as string;
            if (string.IsNullOrEmpty(source)) return false;

            var firstChar = source.Substring(0, 1);

            switch (firstChar.ToLower())
            {
                case "y":
                    return true;
                case "t":
                    return true;
                case "n":
                    return false;
                case "f":
                    return false;
                case "1":
                    return true;
                case "0":
                    return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "F";
            if (!System.Convert.ToBoolean(value)) return "F";
            return 'T';
        }

        #endregion
    }
}
