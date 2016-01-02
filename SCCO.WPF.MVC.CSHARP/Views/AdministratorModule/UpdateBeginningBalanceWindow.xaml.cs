using System;
using System.ComponentModel;
using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    /// <summary>
    ///     Update subsidiary ledger beginning balance
    /// </summary>
    public partial class UpdateBeginningBalanceWindow
    {
        private readonly UpdateBeginningBalanceViewModel _viewModel;

        public UpdateBeginningBalanceWindow()
        {
            InitializeComponent();
            DateTime loginDate = MainController.LoggedUser.TransactionDate;
            _viewModel = new UpdateBeginningBalanceViewModel
                {
                    CutoffDate = Convert.ToDateTime(string.Format("12/31/{0}", loginDate.Year - 1))
                };
            DataContext = _viewModel;
            ProcessButton.Click += (s, e) => StartProcessing();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            if (backgroundWorker == null) return;

            _viewModel.PerformUpdate();
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void StartProcessing()
        {
            var database = Database.DatabaseController.GetDatabaseByYear(MainController.LoggedUser.TransactionDate.Year);
            if (!Database.DatabaseController.IsDatabaseExist(database))
            {
                MessageWindow.ShowAlertMessage(string.Format("Database '{0}' does not exist!", database));
                return;
            }
        
            int validCutoffYear = MainController.LoggedUser.TransactionDate.Year - 1;
            if (_viewModel.CutoffDate.Year != validCutoffYear)
            {
                string notValidCutoffYear = string.Format("Cutoff Year is not valid. Must be year {0}.", validCutoffYear);
                MessageWindow.ShowAlertMessage(notValidCutoffYear);
                return;
            }

            ProcessButton.IsEnabled = false;
            CloseButton.IsEnabled = false;
            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.IsIndeterminate = true;

            var worker = new BackgroundWorker {WorkerReportsProgress = true};
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += (sender, args) =>
                {
                    ProcessButton.IsEnabled = true;
                    CloseButton.IsEnabled = true;
                    ProgressBar.Visibility = Visibility.Collapsed;
                    MessageWindow.ShowNotifyMessage("Process Complete!");
                };
            worker.RunWorkerAsync();
        }
    }
}