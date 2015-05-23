using System.Windows;
using System.Windows.Input;

namespace SCCO.WPF.MVC.CS.Views {
    public class BaseWindow : Window{
        public void DragWindow(object sender, MouseButtonEventArgs e)  {
            DragMove();
        }
        public void CloseWindow(object sender, RoutedEventArgs e) {
            Close();
        }

        //internal void EntryFieldOnEnterKeyDown(object sender, KeyEventArgs e)
        //{
        //    MainController.MoveFocusToNextControlOnEnter(sender, e);
        //}

        //protected override void OnDeactivated(EventArgs e)
        //{
        //    Opacity = .8f;
        //}

        //protected override void OnActivated(EventArgs e)
        //{
        //    Opacity = 1f;
        //}
    }

}
