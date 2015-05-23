using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.CollectorModule
{
    public partial class CollectorListDetailView : IListDetailView
    {
        private CollectorCollection _lookup;
        private CollectorViewModel _viewModel;

        public CollectorListDetailView()
        {
            InitializeComponent();

            RefreshDisplay();

            txtSearch.TextChanged += (sender, args) => Search();

            btnAdd.Click += (sender, args) => Add();
            btnEdit.Click += (sender, args) => Edit();
            btnDelete.Click += (sender, args) => Delete();
        }

        #region Implementation of IListDetailView

        public void Add()
        {
            var addView = new AddCollectorView();
            if (addView.ShowDialog() == true)
            {
                _lookup.Add(addView.NewItem);
                _viewModel.Collection.Add(addView.NewItem);
            }
        }

        public void Edit()
        {
            if (_viewModel.SelectedItem == null) return;
            var editView = new EditCollectorView(_viewModel.SelectedItem.ID);
            if (editView.ShowDialog() == true)
            {
                _viewModel.SelectedItem.Find(_viewModel.SelectedItem.ID);
            }
        }

        public void Delete()
        {
            if (_viewModel.SelectedItem == null) return;
            if (MessageWindow.ConfirmDeleteRecord() == MessageBoxResult.Yes)
            {
                _viewModel.SelectedItem.Destroy();
                _lookup.Remove(_lookup.FirstOrDefault(item => item.ID == _viewModel.SelectedItem.ID));
                _viewModel.Collection.Remove(_viewModel.SelectedItem);
            }
        }

        public void Search()
        {
            if (_lookup == null) return;
            if (!_lookup.Any()) return;

            var searchItem = txtSearch.Text;
            if (searchItem.Trim().Length == 0)
            {
                RefreshDisplay();
            }
            else
            {
                var filteredItem = from item in _lookup
                                   where item.CollectorName.ToLower().Contains(searchItem.ToLower())
                                   select item;

                var viewModel = new CollectorViewModel {Collection = new CollectorCollection()};
                foreach (var item in filteredItem)
                {
                    viewModel.Collection.Add(item);
                }
                _viewModel = viewModel;
                DataContext = _viewModel;
            }
        }

        public void RefreshDisplay()
        {
            _lookup = Collector.CollectAll();
            _viewModel = new CollectorViewModel {Collection = Collector.CollectAll()};
            DataContext = _viewModel;
        }

        #endregion
    }
}
