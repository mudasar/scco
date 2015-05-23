using System.Collections.ObjectModel;
using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.AccountVerifier;

namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
{
    public class AccountVerifierViewModel : INotifyPropertyChanged
    {
        private AccountDetail _selectedDetail;
        private AccountSummary _selectedAccount;
        private ObservableCollection<AccountSummary> _accountSummaries;
        private ObservableCollection<AccountDetail> _accountDetails;
        private Nfmb _member;

        public Nfmb Member
        {
            get { return _member; }
            set
            {
                if (_member == value) return;
                _member = value; OnPropertyChanged("Member");
            }
        }

        public AccountDetail SelectedDetail
        {
            get { return _selectedDetail; }
            set
            {
                if (_selectedDetail == value) return;
                _selectedDetail = value; OnPropertyChanged("SelectedDetail");
            }
        }

        public AccountSummary SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                if (_selectedAccount == value) return;
                _selectedAccount = value; OnPropertyChanged("SelectedAccount");
            }
        }

        public ObservableCollection<AccountSummary> AccountSummaries
        {
            get { return _accountSummaries; }
            set
            {
                if (_accountSummaries == value) return;
                _accountSummaries = value; OnPropertyChanged("AccountSummaries");
            }
        }

        public ObservableCollection<AccountDetail> AccountDetails
        {
            get { return _accountDetails; }
            set
            {
                if (_accountDetails == value) return;
                _accountDetails = value; OnPropertyChanged("AccountDetails");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
