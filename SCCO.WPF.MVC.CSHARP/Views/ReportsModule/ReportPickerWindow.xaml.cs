using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Views.ReportItemModule;

namespace SCCO.WPF.MVC.CS.Views.ReportsModule
{
    public partial class ReportPickerWindow 
    {
        private ReportItemCollection _lookup;
        private ReportItemViewModel _viewModel;

        private readonly string _category;
        public ReportPickerWindow(string category)
        {
            InitializeComponent();

            _category = category;
            RefreshDisplay();

            txtSearch.TextChanged += (sender, args) => Search();
            btnShowReport.Click += (sender, args) => ShowReport();
        }
      
        private void ShowReport()
        {
           var result = _viewModel.SelectedItem.LoadReport();
            if (!result.Success)
                MessageWindow.ShowAlertMessage(result.Message);
        }

        private void Search()
        {
            if (_lookup == null) return;
            if (!_lookup.Any()) return;

            var searchItem = txtSearch.Text;
            if (searchItem.Trim().Length == 0)
            {
                _viewModel.Collection = _lookup;
                DataContext = _viewModel;
            }
            else
            {
                var filteredItem = from item in _lookup
                                   where item.Title.ToLower().Contains(searchItem.ToLower())
                                   select item;

                var viewModel = new ReportItemViewModel { Collection = new ReportItemCollection() };
                foreach (var item in filteredItem)
                {
                    viewModel.Collection.Add(item);
                }
                _viewModel = viewModel;
                DataContext = _viewModel;
            }
        }

        private void RefreshDisplay()
        {
            _lookup = ReportItem.GetListByCategory(_category);
            _viewModel = new ReportItemViewModel { Collection = ReportItem.GetListByCategory(_category) };
            if(_category == "SCHEDULES")
            {
                var sqlParameters = new SqlParameter[1];
                sqlParameters[0] = new SqlParameter("as_of", MainController.LoggedUser.TransactionDate);
                foreach (var item in _lookup)
                {
                    item.StoredProcedureParameters = sqlParameters;
                }
                foreach (var item in _viewModel.Collection)
                {
                    item.StoredProcedureParameters = sqlParameters;
                }

            }
            DataContext = _viewModel;
        }

    }
}
