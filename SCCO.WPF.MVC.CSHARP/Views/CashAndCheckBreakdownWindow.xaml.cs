using System.Windows;

namespace SCCO.WPF.MVC.CS.Views
{
	/// <summary>
	/// Interaction logic for CashAndCheckBreakdownWindow.xaml
	/// </summary>
	public partial class CashAndCheckBreakdownWindow
	{
	    private bool _isReadOnly;

	    public CashAndCheckBreakdownWindow()
		{
			InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

        public CashAndCheckBreakdownWindow(Models.CashAndCheckBreakDown cashAndCheckBreakDown)
        {
            InitializeComponent();
            DataContext = cashAndCheckBreakDown;
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value;
                OkButton.IsEnabled = !IsReadOnly;
            }
        }

	    private void OkButtonOnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
	}
}