using System.ComponentModel;

namespace SCCO.WPF.MVC.CS.Views.DepartmentModule
{
    public class DepartmentViewModel : INotifyPropertyChanged
    {
        private Models.DepartmentCollection _collection;
        private Models.Department _selectedItem;

        public Models.DepartmentCollection Collection
        {
            get { return _collection; }
            set
            {
                if (_collection == value) return;
                _collection = value; OnPropertyChanged("Collection");
            }
        }

        public Models.Department SelectedItem
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
