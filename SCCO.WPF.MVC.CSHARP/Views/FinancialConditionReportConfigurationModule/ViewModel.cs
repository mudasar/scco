using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.FinancialConditionReportConfigurationModule
{
    internal class ViewModel : ViewModelBase
    {
        private FinancialConditionReportConfigurationCollection _collection;
        private FinancialConditionReportConfiguration _selectedItem;

        public FinancialConditionReportConfigurationCollection Collection
        {
            get { return _collection; }
            set
            {
                if (_collection == value) return;
                _collection = value; OnPropertyChanged("Collection");
            }
        }

        public FinancialConditionReportConfiguration SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value; OnPropertyChanged("SelectedItem");
            }
        }

        public void InitializeData()
        {
            Collection = FinancialConditionReportConfiguration.CollectAll();
        }
    }
}
