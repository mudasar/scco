using System.Windows;

namespace SCCO.WPF.MVC.CS.UserControls
{
    /// <summary>
    /// Interaction logic for TextBoxSearchControl.xaml
    /// </summary>
    public partial class TextBoxSearchControl
    {
        public delegate void ClickHandler(object sender, RoutedEventArgs e);

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (TextBoxSearchControl));

        private bool _isReadOnly;
        private string _customTooltip = "Search";

        public TextBoxSearchControl()
        {
            InitializeComponent();
        }

        public string Text
        {
            get
            {
                var text = (string) GetValue(TextProperty);
                return text;
            }
            set { SetValue(TextProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                _isReadOnly = value;
                TextBoxSearchItem.IsReadOnly = _isReadOnly;
            }
        }

        public string CustomTooltip
        {
            get { return _customTooltip; }
            set { _customTooltip = value;
                ButtonSearch.ToolTip = value;
            }
        }

        public event ClickHandler Click;

        public void OnClick(object sender, RoutedEventArgs e)
        {
            var handler = Click;
            if (handler != null) handler(this, e);
        }

        private void SearchButtonOnClick(object sender, RoutedEventArgs e)
        {
            OnClick(sender, e);
        }
    }
}