using System.Collections.ObjectModel;
using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.GeneralLedgerBalanceModule
{
    public class GeneralLedgerBalanceViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<GeneralLedgerBalance> _collection;
        private GeneralLedgerBalance _selectedItem;

        public ObservableCollection<GeneralLedgerBalance> Collection
        {
            get { return _collection; }
            set
            {
                if (_collection == value) return;
                _collection = value; OnPropertyChanged("Collection");
                
            }
        }

        public GeneralLedgerBalance SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value; OnPropertyChanged("SelectedItem");
            }
        }


        public void RefreshCollection()
        {
            var collection  = new ObservableCollection<GeneralLedgerBalance>();
            var query = "SELECT * FROM `glbal` ORDER BY ACC_CODE";
            var dataTable = Database.DatabaseController.ExecuteSelectQuery(query);
            foreach (System.Data.DataRow dataRow in dataTable.Rows)
            {
                var item = new GeneralLedgerBalance();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }

            Collection = collection;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
