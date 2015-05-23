using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    public class LoanProductViewModel : INotifyPropertyChanged
    {
        private LoanProductCollection _collection;
        private LoanProduct _selectedItem;

        public LoanProductCollection Collection
        {
            get { return _collection; }
            set
            {
                if (_collection == value) return;
                _collection = value; OnPropertyChanged("Collection");
            }
        }

        public LoanProduct SelectedItem
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
