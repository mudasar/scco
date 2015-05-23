//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace SCCO.WPF.MVC.CS.Views {
//    /// <summary>
//    /// Interaction logic for ClassificationMaintenanceWindow.xaml
//    /// </summary>
//    public partial class ClassificationMaintenanceWindow : Window {
//        public ClassificationMaintenanceWindow() {
//            InitializeComponent();
//        }
//    }
//}
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views {
    /// <summary>
    /// Interaction logic for ClassificationMaintenanceWindow.xaml
    /// </summary>
    public partial class ClassificationMaintenanceWindow {
        public ClassificationMaintenanceWindow() {
            InitializeComponent();
            _currentClassification = new Classification();
            DataContext = _currentClassification;
        }

        private Classification _currentClassification = new Classification();


        private void Create(object sender, RoutedEventArgs e) {
            DataContext = _currentClassification = new Classification();
        }

        private void Read(object sender, RoutedEventArgs e) {
            List<Classification> classifications = Classification.GetList();
            List<SearchItem> searchItems =
                classifications.Select(classification => new SearchItem(classification.ClassificationId, classification.Description)).ToList();

            var searchWindow = new SearchWindow(searchItems);
            searchWindow.ShowDialog();
            if (searchWindow.DialogResult == true) {
                DataContext = _currentClassification = new Classification(searchWindow.SelectedItem.ItemId);
            }
        }

        private void Update(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(_currentClassification.Description)) {
                MessageWindow.ShowAlertMessage("Classification Name must not be empty!");
                return;
            }
            if (_currentClassification.ClassificationId == 0) {
                _currentClassification.Create();
                return;
            }
            _currentClassification.Update();
        }

        private void Delete(object sender, RoutedEventArgs e) {
            if (
                MessageWindow.ShowConfirmMessage(
                    "You are about to delete current Classification information. Do you want to proceed?") ==
                MessageBoxResult.Yes) {
                _currentClassification.Destroy();
                MessageWindow.ShowNotifyMessage("Classification information deleted!");
                DataContext = _currentClassification = new Classification(0);
            }
        }
    }
}
