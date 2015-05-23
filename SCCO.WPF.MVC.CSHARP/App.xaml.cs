using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SCCO.WPF.MVC.CS {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Select the text in a TextBox when it receives focus.
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(SelectivelyIgnoreMouseButton));
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent,
                new RoutedEventHandler(SelectAllText));
            EventManager.RegisterClassHandler(typeof(TextBox), Control.MouseDoubleClickEvent,
                new RoutedEventHandler(SelectAllText));

            // move to next control on enter
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.KeyDownEvent, new KeyEventHandler(TextBox_KeyDown));
            EventManager.RegisterClassHandler(typeof(CheckBox), UIElement.KeyDownEvent, new KeyEventHandler(CheckBox_KeyDown));
            base.OnStartup(e);
        }

        void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focused, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }


        // move to next control on enter
        void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & ((TextBox) sender).AcceptsReturn == false) MoveToNextUIElement(e);
        }

        void CheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            MoveToNextUIElement(e);
            //Sucessfully moved on and marked key as handled.
            //Toggle check box since the key was handled and
            //the checkbox will never receive it.
            if (e.Handled)
            {
                CheckBox cb = (CheckBox)sender;
                cb.IsChecked = !cb.IsChecked;
            }

        }

        void MoveToNextUIElement(KeyEventArgs e)
        {
            // Creating a FocusNavigationDirection object and setting it to a
            // local field that contains the direction selected.
            const FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;

            // MoveFocus takes a TraveralReqest as its argument.
            TraversalRequest request = new TraversalRequest(focusDirection);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                if (elementWithFocus.MoveFocus(request)) e.Handled = true;
            }
        }


        public App()
        {
            Dispatcher.UnhandledException += (sender, args) =>
                                                 {
                                                     var msgBuilder = new StringBuilder();
                                                     msgBuilder.AppendLine("An unhandled exception occured. Application will terminate.");
                                                     msgBuilder.AppendLine();
                                                     msgBuilder.AppendLine(args.Exception.Message);
                                                     Views.MessageWindow.ShowAlertMessage(msgBuilder.ToString());
                                                     Utilities.Logger.ExceptionLogger(typeof(App),args.Exception);
                                                     args.Handled = true;
                                                     Environment.Exit(0);
                                                 };
        }
        
    }
}
