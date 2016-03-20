using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;

namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    public partial class TimeDepositDetailsView
    {
        private TimeDepositDetails _timeDepositDetails;
        private readonly int _voucherId;
        private readonly VoucherTypes _voucherType;

        public TimeDepositDetailsView()
        {
            InitializeComponent();
            btnEdit.Visibility = MainController.LoggedUser.IsTimeDepositManager
                                     ? Visibility.Visible
                                     : Visibility.Collapsed;
            btnEdit.Click += (sender, args) => ShowTimeDepositEditView();

        }

        public TimeDepositDetailsView(TimeDepositDetails timeDepositDetails) : this()
        {
            _timeDepositDetails = timeDepositDetails;
            DataContext = _timeDepositDetails;
        }

        public TimeDepositDetailsView(VoucherTypes voucherType, int voucherId)
            : this()
        {
            _timeDepositDetails = TimeDepositDetails.FindByVoucher(voucherType, voucherId);
            _voucherType = voucherType;
            _voucherId = voucherId;
            DataContext = _timeDepositDetails;
        }

        private void ShowTimeDepositEditView()
        {
            if (_voucherId == 0) return;

            var view = new TimeDepositEditView(_timeDepositDetails);
            if (view.ShowDialog() == true)
            {
                _timeDepositDetails.Update(_voucherType, _voucherId);
                Voucher.Touch(_voucherType, _voucherId, MainController.LoggedUser.ID);
            }
            _timeDepositDetails = TimeDepositDetails.FindByVoucher(_voucherType, _voucherId);
            DataContext = _timeDepositDetails;
        }
    }
}