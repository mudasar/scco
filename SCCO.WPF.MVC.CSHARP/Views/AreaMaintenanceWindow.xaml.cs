using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views {
    /// <summary>
    /// Interaction logic for AreaMaintenanceWindow.xaml
    /// </summary>
    public partial class AreaMaintenanceWindow : IDataEntry {

        public AreaMaintenanceWindow() {
            InitializeComponent();
            _currentArea = new Area();
            DataContext = _currentArea;
        }

        private Area _currentArea = new Area();


        public void Create(object sender, RoutedEventArgs e)
        {
            DataContext = _currentArea = new Area();
        }

        public void Read(object sender, RoutedEventArgs e)
        {
            List<Area> areas = Area.GetList();
            List<SearchItem> searchItems =
                areas.Select(area => new SearchItem(area.AreaId, area.AreaName)).ToList();

            var searchWindow = new SearchWindow(searchItems);
            searchWindow.ShowDialog();
            if (searchWindow.DialogResult == true) {
                DataContext = _currentArea = new Area(searchWindow.SelectedItem.ItemId);
            }
        }

        public void Update(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentArea.AreaName)) {
                MessageWindow.ShowAlertMessage("Area of Operation must not be empty!");
                return;
            }
            if (_currentArea.AreaId == 0) {
                _currentArea.Create();
                return;
            }
            _currentArea.Update();
        }

        public void Delete(object sender, RoutedEventArgs e)
        {
            if (
                MessageWindow.ShowConfirmMessage(
                    "You are about to delete current Area information. Do you want to proceed?") ==
                MessageBoxResult.Yes) {
                _currentArea.Destroy();
                MessageWindow.ShowNotifyMessage("Area information deleted!");
                DataContext = _currentArea = new Area(0);
            }
        }
    }
}
