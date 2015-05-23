using System.Collections.Generic;
using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models;
//using SCCO.WPF.MVC.CS.Models.Accounts;

namespace SCCO.WPF.MVC.CS.Views.AccountModule
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        //private List<AccountCategory> _accountCategories;

        private AccountCollection _accounts;

        private Account _selectedItem;

        public event PropertyChangedEventHandler PropertyChanged;

        //public List<AccountCategory> AccountCategories
        //{
        //    get { return _accountCategories; }
        //    set
        //    {
        //        if (_accountCategories == value) return;
        //        _accountCategories = value;
        //        OnPropertyChanged("AccountCategories");
        //    }
        //}

        public AccountCollection Collection
        {
            get { return _accounts; }
            set
            {
                if (_accounts == value) return;
                _accounts = value;
                OnPropertyChanged("Accounts");
            }
        }

        public Account SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
