using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;

namespace SCCO.WPF.MVC.CS.Utilities.BackgroundTasks
{
    public class BackgroundTaskViewModel
    {
        protected readonly BackgroundWorker _backgroundWorker;

        public event EventHandler TaskStarting = (s, e) => { };

        public event ProgressChangedEventHandler ProgressChanged
        {
            add { _backgroundWorker.ProgressChanged += value; }
            remove { _backgroundWorker.ProgressChanged -= value; }
        }

        public event RunWorkerCompletedEventHandler TaskCompleted
        {
            add { _backgroundWorker.RunWorkerCompleted += value; }
            remove { _backgroundWorker.RunWorkerCompleted -= value; }
        }

        private ICommand _executeLongTask;

        public ICommand ExecuteLongTask
        {
            get
            {
                return _executeLongTask ??
                       (_executeLongTask = new RelayCommand(param => _backgroundWorker.RunWorkerAsync()));
            }
        }

        public BackgroundTaskViewModel()
        {
            _backgroundWorker = new BackgroundWorker {WorkerReportsProgress = true};
            _backgroundWorker.DoWork += ExecuteTask;
        }

        public virtual void ExecuteTask(object sender, DoWorkEventArgs e)
        {
            OnTaskStarting();
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                _backgroundWorker.ReportProgress(i + 1);
            }
        }

        protected void OnTaskStarting()
        {
            TaskStarting(this, EventArgs.Empty);
        }
    }

    
}
