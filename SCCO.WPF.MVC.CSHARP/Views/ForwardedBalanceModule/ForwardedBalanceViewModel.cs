using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.ForwardedBalanceModule
{
    public class ForwardedBalanceViewModel : INotifyPropertyChanged
    {
        private ForwardedBalanceCollection _collection;
        private ForwardedBalance _selectedItem;

        public ForwardedBalanceCollection Collection
        {
            get { return _collection; }
            set
            {
                if (_collection == value) return;
                _collection = value; OnPropertyChanged("Collection");
                
            }
        }

        public ForwardedBalance SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value; OnPropertyChanged("SelectedItem");
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
