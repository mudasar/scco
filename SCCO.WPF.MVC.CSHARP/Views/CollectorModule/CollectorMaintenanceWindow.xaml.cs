﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.CollectorModule
{
    /// <summary>
    /// Interaction logic for CollectorMaintenanceWindow.xaml
    /// </summary>
    public partial class CollectorMaintenanceWindow
    {
        public CollectorMaintenanceWindow()
        {
            InitializeComponent();
            _currentCollector = new Models.Collector();
            DataContext = _currentCollector;
        }

        private Models.Collector _currentCollector = new Models.Collector();


        private void Create(object sender, RoutedEventArgs e)
        {
            DataContext = _currentCollector = new Models.Collector();
        }

        private void Read(object sender, RoutedEventArgs e)
        {
            List<Collector> collectors = Models.Collector.GetList();
            List<SearchItem> searchItems =
                collectors.Select(collector => new SearchItem(collector.CollectorId, collector.CollectorName)).ToList();

            var searchWindow = new SearchWindow(searchItems);
            searchWindow.ShowDialog();
            if (searchWindow.DialogResult == true)
            {
                _currentCollector.Find(searchWindow.SelectedItem.ItemId);
                DataContext = _currentCollector;
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentCollector.CollectorName))
            {
                MessageWindow.ShowAlertMessage("Collector Name must not be empty!");
                return;
            }
            if (_currentCollector.CollectorId == 0)
            {
                _currentCollector.Create();
                return;
            }
            _currentCollector.Update();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (
                MessageWindow.ShowConfirmMessage(
                    "You are about to delete current Collector information. Do you want to proceed?") ==
                MessageBoxResult.Yes)
            {
                _currentCollector.Destroy();
                MessageWindow.ShowNotifyMessage("Collector information deleted!");
                DataContext = _currentCollector = new Models.Collector();
            }
        }
    }
}
