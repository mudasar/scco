using System;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    public partial class LoanPostingWindow
    {

        public LoanPostingWindow()
        {
            InitializeComponent();
        }

        public LoanPostingWindow(Models.Loan.LoanPostingDetails loanPostingDetails) : this()
        {
            _loanPostingDetails = loanPostingDetails;
            DataContext = _loanPostingDetails;
            DocumentTypeBox.SelectionChanged += (s, e) => UpdateVoucherNumber();
            DocumentTypeBox.Items.Clear();
            DocumentTypeBox.Items.Add("CV");
            DocumentTypeBox.Items.Add("JV");
        }

        private void UpdateVoucherNumber()
        {
            var documentType = (string)DocumentTypeBox.SelectedItem;
            switch (documentType)
            {
                case "JV":
                    _loanPostingDetails.VoucherNumber = Voucher.LastDocumentNo(VoucherTypes.JV) + 1;
                    break;
                case "CV":
                    _loanPostingDetails.VoucherNumber = Voucher.LastDocumentNo(VoucherTypes.CV) + 1;
                    break;
            }
        }

        private void AcceptButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (_loanPostingDetails.VoucherNumber == 0)
            {
                MessageWindow.ShowAlertMessage("Invalid Document Number!");
                return;
            }

            if (_loanPostingDetails.ReleaseNumber == 0)
            {
                MessageWindow.ShowAlertMessage("Invalid Release Number!");
                return;
            }

            if (_loanPostingDetails.ReleaseDate == new DateTime())
            {
                MessageWindow.ShowAlertMessage("Invalid Release Date!");
                return;
            }
            DialogResult = true;
            Close();
        }

        private readonly Models.Loan.LoanPostingDetails _loanPostingDetails;
    }
}
