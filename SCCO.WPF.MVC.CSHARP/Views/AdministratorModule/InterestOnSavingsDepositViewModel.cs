using System.ComponentModel;
using System.Linq;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    public class InterestOnSavingsDepositViewModel : INotifyPropertyChanged
    {
        private VoucherCollection _collection;
        private Voucher _selectedItem;
        private decimal _interestRate;
        private decimal _requiredBalance;
        private decimal _totalInterestEarned;
        private int _quarter;
        private Account _interestExpenseOnSavingsDepositAccount;

        public event PropertyChangedEventHandler PropertyChanged;

        public VoucherCollection Collection
        {
            get { return _collection; }
            set
            {
                if (_collection == value) return;
                _collection = value; OnPropertyChanged("Collection");
                if(_collection == null) return;

                TotalInterestEarned = Collection.Sum(voucher => voucher.Credit);
            }
        }

        public Voucher SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value; OnPropertyChanged("SelectedItem");
            }
        }
        public decimal InterestRate
        {
            get { return _interestRate; }
            set { _interestRate = value; OnPropertyChanged("InterestRate"); }
        }

        public decimal RequiredBalance
        {
            get { return _requiredBalance; }
            set { _requiredBalance = value; OnPropertyChanged("RequiredBalance");}
        }

        public decimal TotalInterestEarned
        {
            get { return _totalInterestEarned; }
            set { _totalInterestEarned = value; OnPropertyChanged("TotalInterestEarned"); }
        }

        public int Quarter
        {
            get { return _quarter; }
            set { _quarter = value; OnPropertyChanged("Quarter"); }
        }

        public Account InterestExpenseOnSavingsDepositAccount
        {
            get { return _interestExpenseOnSavingsDepositAccount; }
            set { _interestExpenseOnSavingsDepositAccount = value; OnPropertyChanged("InterestExpenseOnSavingsDepositAccount"); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
