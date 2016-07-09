using System;
using SCCO.WPF.MVC.CS.Utilities.BackgroundTasks;

namespace SCCO.WPF.MVC.CS.Utilities
{
    public partial class ProgressView
    {
        public ProgressView(BackgroundTaskViewModel vm, string title)
        {
            InitializeComponent();
            vm.TaskStarting += TaskStarted;
            vm.ProgressChanged += ProgressChanged;
            vm.TaskCompleted += TaskCompleted;
            DataContext = vm;
            FormTitle.Content = title;
        }


        void TaskStarted(object sender, EventArgs e)
        {
            Dispatcher.Invoke(new Action(() => OverallProgressLabel.Content = "Processing..."));
        }

        void TaskCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => OverallProgressLabel.Content = "Completed!"));
            Close();
        }

        void ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => OverallProgressBar.Value = e.ProgressPercentage));
        }
    }
}
