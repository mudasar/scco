using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.UserModule
{
    public class EditUserViewModel : INotifyPropertyChanged
    {
        private User _user;
        private Models.Collections.Collectors _collectors;

        public Models.User User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged("User");}
        }

        public Models.Collections.Collectors Collectors
        {
            get { return _collectors; }
            set { _collectors = value; OnPropertyChanged("Collectors"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
