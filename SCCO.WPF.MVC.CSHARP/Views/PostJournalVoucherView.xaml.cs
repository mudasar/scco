using System;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views
{
    public partial class PostJournalVoucherView
    {
        private readonly JournalVoucher _viewModel;
        public JournalVoucher JournalVoucher { get { return _viewModel; } }
        public PostJournalVoucherView(DateTime postingDate)
        {
            InitializeComponent();

            _viewModel = new JournalVoucher();
            _viewModel.VoucherNo = Voucher.LastDocumentNo(VoucherTypes.JV) + 1;
            _viewModel.VoucherDate = postingDate;

            DataContext = _viewModel;

            btnPost.Click += btnPost_Click;
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            var collection = JournalVoucher.FindByDocumentNumber(_viewModel.VoucherNo);
            if (collection.Count > 0)
            {
                MessageWindow.ShowAlertMessage("JV No. already in use.");
                return;
            }
            DialogResult = true;
            Close();
        }
    }
}