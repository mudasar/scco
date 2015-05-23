using System;
using System.Windows;

namespace SCCO.WPF.MVC.CS.Views
{
    public interface IDataEntry
    {
        void Create(object sender, RoutedEventArgs routedEventArgs);
        void Read(object sender, RoutedEventArgs routedEventArgs);
        void Update(object sender, RoutedEventArgs routedEventArgs);
        void Delete(object sender, RoutedEventArgs routedEventArgs);
    }
}
