using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views
{
    /// <summary>
    /// Interaction logic for ExplanationWindow.xaml
    /// </summary>
    public partial class ExplanationWindow
    {
        private readonly TransactionHeader _transactionHeader;
        private bool _canModify;

        public ExplanationWindow()
        {
            InitializeComponent();
        }

        public ExplanationWindow(TransactionHeader transactionHeader)
        {
            InitializeComponent();
            _transactionHeader = transactionHeader;
            //txtExplanation.Text = _transactionHeader.Explanation;
            txtExplanation.DataContext = _transactionHeader;
        }

        public bool CanModify
        {
            get { return _canModify; }
            set
            {
                _canModify = value;
                txtExplanation.IsReadOnly = !_canModify;
                UpdateButton.IsEnabled = _canModify;
            }
        }

        public string CurrentValue
        {
            get { return txtExplanation.Text; }
            set { txtExplanation.Text = value; }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            if (CanModify)
            {
                _transactionHeader.Explanation = txtExplanation.Text;
                _transactionHeader.Update();
            }else
            {
                MessageWindow.ShowAlertMessage("Record is locked for editing.");
            }
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        private void CancelButtonOnClick(object sender, System.Windows.RoutedEventArgs e)
        {
        	Close();
        }

    }
}