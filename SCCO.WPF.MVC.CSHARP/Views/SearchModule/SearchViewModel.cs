using System.Collections.ObjectModel;
using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.SearchModule
{
    public class SearchItemViewModel : INotifyPropertyChanged
    {
        private SearchItem _selectedItem;
        private SearchItems _searchItems;

        public SearchItem SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }

        public SearchItems SearchItems
        {
            get { return _searchItems; }
            set { _searchItems = value; OnPropertyChanged("SearchItems"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SearchItems : ObservableCollection<SearchItem>
    {}
}
