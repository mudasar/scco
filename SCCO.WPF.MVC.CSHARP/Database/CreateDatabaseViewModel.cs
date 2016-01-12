using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace SCCO.WPF.MVC.CS.Database
{
    public class CreateDatabaseViewModel : INotifyPropertyChanged

    {
        private List<string> _databases;
        private int _progress;
        private string _sourceDatabase;
        private string _targetDatabase;

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public List<string> Databases
        {
            get { return _databases; }
            set
            {
                _databases = value;
                OnPropertyChanged("Databases");
            }
        }

        public string TargetDatabase
        {
            get { return _targetDatabase; }
            set
            {
                _targetDatabase = value;
                OnPropertyChanged("TargetDatabase");
            }
        }

        public string SourceDatabase
        {
            get { return _sourceDatabase; }
            set
            {
                _sourceDatabase = value;
                if (!string.IsNullOrEmpty(value))
                {
                    TargetDatabase = GenerateTargetDatabase(value);
                }
                OnPropertyChanged("SourceDatabase");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void Initialize()
        {
            Databases = new List<string>();
            if (DatabaseController.IsServerConnected())
            {
                DataTable dataTable = DatabaseController.ShowDatabases();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    Databases.Add(dataRow[0].ToString());
                }
            }
        }

        private string GenerateTargetDatabase(string value)
        {
            string[] arr = value.Split('_');
            try
            {
                if (arr.Length == 3)
                {
                    arr[1] = string.Format("{0}", Convert.ToInt32(arr[1]) + 1);
                }
            }
            catch (Exception)
            {
                return value;
            }

            return string.Join("_", arr);
        }
    }
}