using System;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views
{
    public partial class TransactionDatePickerWindow
    {
        public TransactionDatePickerWindow()
        {
            InitializeComponent();
            datePicker1.SelectedDate = GlobalSettings.DateOfOpenTransaction;
            btnUpdate.Click += Update;
        }

        public DateTime SelectedDate { get; set; }

        private void Update(object sender, EventArgs e)
        {
            if (datePicker1.SelectedDate != null)
            {
                var selectedDate = (DateTime)datePicker1.SelectedDate;
                if (SelectedDate.ToShortDateString() != selectedDate.ToShortDateString())
                {
                    SelectedDate = selectedDate;
                    GlobalSettings.Update(GlobalKeys.DateOfOpenTransaction.ToKeyword(), SelectedDate);
                    DialogResult = true;
                }
            }
            Close();
        }
    }
}

//var gvTransactionDate = GlobalVariable.GetByKeyword("TransactionDate");
//gvTransactionDate.CurrentValue = transactionDatePicker.SelectedDate.ToShortDateString();
//gvTransactionDate.Update();