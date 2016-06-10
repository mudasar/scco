using System.ComponentModel;

namespace SCCO.WPF.MVC.CS.Utilities.FileCopy
{
    internal class FileCopyViewModel
    {
        private string _currentProjectVersion = "Unavailable";
        private string _destinationFolder = "Not set";
        private string _latestProjectVersion = "Unavailable";
        private string _processLabel = "File Copy";
        private string _progressStatus = "Idle";
        private string _sourceFolder = "Not set";

        public string LatestProjectVersion
        {
            get { return _latestProjectVersion; }
            set
            {
                _latestProjectVersion = value;
                OnPropertyChanged("LatestProjectVersion");
            }
        }

        public string CurrentProjectVersion
        {
            get { return _currentProjectVersion; }
            set
            {
                _currentProjectVersion = value;
                OnPropertyChanged("CurrentProjectVersion");
            }
        }

        public string ProgressStatus
        {
            get { return _progressStatus; }
            set
            {
                _progressStatus = value;
                OnPropertyChanged("ProgressStatus");
            }
        }

        public string DestinationFolder
        {
            get { return _destinationFolder; }
            set
            {
                _destinationFolder = value;
                OnPropertyChanged("DestinationFolder");
            }
        }

        public string SourceFolder
        {
            get { return _sourceFolder; }
            set
            {
                _sourceFolder = value;
                OnPropertyChanged("SourceFolder");
            }
        }

        public string ProcessLabel
        {
            get { return _processLabel; }
            set
            {
                _processLabel = value;
                OnPropertyChanged("ProcessLabel");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal bool CheckStatus()
        {
            ProgressStatus = CurrentProjectVersion == LatestProjectVersion
                                 ? "Your version is up-to-date"
                                 : "Your version is not up-to-date";

            return CurrentProjectVersion == LatestProjectVersion;
        }
    }
}