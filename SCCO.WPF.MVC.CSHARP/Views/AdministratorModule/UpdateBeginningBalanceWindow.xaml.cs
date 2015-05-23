using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    /// <summary>
    /// Update subsidiary ledger beginning balance
    /// </summary>
    public partial class UpdateBeginningBalanceWindow
    {
        private readonly UpdateBeginningBalanceViewModel _viewModel;
        public UpdateBeginningBalanceWindow()
        {
            InitializeComponent();
            _viewModel = new UpdateBeginningBalanceViewModel();
            DataContext = _viewModel;
            ProcessButton.Click += (s, e) =>
                {

                    //try
                    //{
                    //    _viewModel.Process();
                    //    MessageWindow.ShowNotifyMessage("Updating beginning balance complete and successful!");
                    //    DialogResult = true;
                    //    Close();
                    //}
                    //catch (Exception exception)
                    //{
                    //    MessageWindow.ShowAlertMessage(exception.Message);
                    //}
                    _startProcessing();

                };
        }


        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            var backgroundWorker = sender as BackgroundWorker;
            if (backgroundWorker == null) return;

            _viewModel.Process2();
        }
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void _startProcessing()
        {
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
