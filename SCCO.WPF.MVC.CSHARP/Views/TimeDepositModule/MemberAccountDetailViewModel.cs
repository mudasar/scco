using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.AccountVerifier;

namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    public class MemberAccountDetailViewModel : INotifyPropertyChanged
    {
        private Nfmb _member;
        private AccountDetail _accountDetail;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Nfmb Member
        {
            get { return _member; }
            set
            {
                _member = value;
                OnPropertyChanged("Member");
            }
        }

        public AccountDetail AccountDetail
        {
            get { return _accountDetail; }
            set
            {
                _accountDetail = value;
                OnPropertyChanged("AccountDetail");
            }
        }
    }
}
