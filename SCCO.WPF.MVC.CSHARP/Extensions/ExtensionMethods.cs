using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCCO.WPF.MVC.CS.Extensions
{
    public static class ExtensionMethods
    {
        public static bool IsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }
    }
}
