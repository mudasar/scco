using System;

namespace SCCO.WPF.MVC.CS.Views
{
    public partial class UserTransactionDateWindow
    {
        private DateTime _selectedTransactionDate;
        public DateTime SelectedTransactionDate
        {
            get { return _selectedTransactionDate; }
            set { _selectedTransactionDate = value; }
        }

        public UserTransactionDateWindow()
        {
            InitializeComponent();
            _selectedTransactionDate = Controllers.MainController.UserTransactionDate;
            datePicker1.SelectedDate = _selectedTransactionDate;
        }

        private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(datePicker1.SelectedDate != null)
            {
                _selectedTransactionDate = (DateTime)datePicker1.SelectedDate;
                DialogResult = true;
            }
            Close();
        }
    }
}
