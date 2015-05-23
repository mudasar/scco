using System.Windows;
using System.Windows.Controls;

namespace SCCO.WPF.MVC.CS.UserControls
{
    /// <summary>
    /// Interaction logic for TextBoxSearchControl.xaml
    /// </summary>
    public partial class TextBoxSearchControl : UserControl
    {

        public TextBoxSearchControl()
        {
            InitializeComponent();
        }

        public delegate void ClickHandler(object sender, RoutedEventArgs e);

        public event ClickHandler Click;

        public void OnClick(object sender, RoutedEventArgs e)
        {
            ClickHandler handler = Click;
            if (handler != null) handler(this, e);
        }

        private void SearchButtonOnClick(object sender, RoutedEventArgs e)
        {
            OnClick(sender, e);
        }

        public string Text
        {
            get { var text = (string) GetValue(TextProperty);
                return text;
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value;
                TextBoxSearchItem.IsReadOnly = _isReadOnly;
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof (string),
                                                                                             typeof (
                                                                                                 TextBoxSearchControl));

        private bool _isReadOnly;
    }

}
