using System;
using System.Text;

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
                var transactionDate = (DateTime)datePicker1.SelectedDate;
                var database = Database.DatabaseController.GetDatabaseByYear(transactionDate.Year);
                if (!Database.DatabaseController.IsDatabaseExist(database))
                {
                    var messageBuilder = new StringBuilder();
                    messageBuilder.AppendLine("A database for this transaction date does not exist or not yet created.");
                    messageBuilder.AppendLine("Please consult your system administrator.");
                    MessageWindow.ShowAlertMessage(messageBuilder.ToString());
                    return;
                }
                _selectedTransactionDate = transactionDate;
                DialogResult = true;
            }
            Close();
        }
    }
}
