using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.ProductImageModule
{
    public class ProductImageViewModel : INotifyPropertyChanged
    {
        private ProductImageCollection _collection;
        private ProductImage _selectedItem;

        public ProductImageCollection Collection
        {
            get { return _collection; }
            set
            {
                if (_collection == value) return;
                _collection = value; OnPropertyChanged("Collection");
            }
        }

        public ProductImage SelectedItem
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

