using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Views.TimeDepositModule;

namespace SCCO.WPF.MVC.CS.Views
{
    /// <summary>
    /// Interaction logic for ForwardedBalanceWindow.xaml
    /// </summary>
    public partial class ForwardedBalanceListWindow
    {

        private List<Models.ForwardedBalanceOld> _forwardedBalances;
        private DateTime _userDate;
        public ForwardedBalanceListWindow()
        {
            InitializeComponent();
            _userDate = Controllers.MainController.UserTransactionDate;
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            DataContext = _forwardedBalances = Models.ForwardedBalanceOld.GetListByYear(_userDate.Year);
            var result = from forwardedBalance in _forwardedBalances
                         where
                             (forwardedBalance.MemberCode + " " + forwardedBalance.MemberName + " " +
                              forwardedBalance.AccountCode).ToUpper().Contains(SearchTextBox.Text.Trim().ToUpper())
                         select forwardedBalance;

            LedgerGrid.ItemsSource = result;
        }

        private void TimeDepositDetailsButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (LedgerGrid.SelectedItems.Count <= 0) return;
            var selectedItem = (Models.ForwardedBalanceOld) LedgerGrid.SelectedItem;
            if (selectedItem.AccountCode != Controllers.AdministratorSettings.TimeDepositCode) return;
            if(selectedItem.TimeDepositDetailId <= 0)
            {
                if(MessageWindow.ShowConfirmMessage("There is no Time Deposit detail in selected record. Do you want to assign one?") == MessageBoxResult.Yes)
                {

                }
            }
            //var timeDepositDetailModel = new Models.TimeDepositDetails();
            //timeDepositDetailModel.Find(selectedItem.TimeDepositDetailId);
            //var timeDepositDetailsWindow = new TimeDepositEditWindow(timeDepositDetailModel);
            //timeDepositDetailsWindow.ShowDialog();
        }

        private void SearchButtonOnClick(object sender, RoutedEventArgs e)
        {
            RefreshDisplay();
        }

        private void AddButtonOnClick(object sender, RoutedEventArgs e)
        {
            var newItem = new Models.ForwardedBalanceOld {ForwardedYear = _userDate.Year, DocumentDate = _userDate};

            var forwardedBalanceEditWindow = new ForwardedBalanceEditWindow(newItem);
            forwardedBalanceEditWindow.ShowDialog();
            if (forwardedBalanceEditWindow.ActionResult.Success)
                RefreshDisplay();
        }

        private void DeleteButtonOnClick(object sender, RoutedEventArgs e)
        {
            var selectedRecord = (Models.ForwardedBalanceOld) LedgerGrid.SelectedItem;

            if (selectedRecord == null) return;

            if (MessageWindow.ShowConfirmMessage("Do you really want to delete selected record?") !=
                MessageBoxResult.Yes) return;

            var result = selectedRecord.Destroy();
            if (result.Success)
            {
                RefreshDisplay();
                MessageWindow.ShowNotifyMessage("Delete successful!");
            }
            else
                MessageWindow.ShowAlertMessage(result.Message+"!");
        }

        private void EditButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (LedgerGrid.SelectedItems.Count == 0) return;

            var selectedRecord = (Models.ForwardedBalanceOld)LedgerGrid.SelectedItem;
            if (selectedRecord == null) return;

            var forwardedBalanceEditWindow = new ForwardedBalanceEditWindow(Controllers.ModelController.DeepClone(selectedRecord));
            forwardedBalanceEditWindow.ShowDialog();
            if (forwardedBalanceEditWindow.ActionResult.Success)
            {
                RefreshDisplay();
            }
            
        }

    }
}
