using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.CollectorModule
{
    public class CollectorViewModel : INotifyPropertyChanged
    {
        private CollectorCollection _collectors;
        private Collector _selectedItem;

        public CollectorCollection Collection
        {
            get { return _collectors; }
            set
            {
                if (_collectors == value) return;
                _collectors = value; OnPropertyChanged("Collectors");
            }
        }

        public Collector SelectedItem
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
