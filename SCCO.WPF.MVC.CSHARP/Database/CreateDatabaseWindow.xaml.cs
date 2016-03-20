using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using SCCO.WPF.MVC.CS.Views;

namespace SCCO.WPF.MVC.CS.Database
{
    public partial class CreateDatabaseWindow
    {
        private readonly CreateDatabaseViewModel _viewModel;
        private readonly BackgroundWorker _worker = new BackgroundWorker();

        public CreateDatabaseWindow()
        {
            InitializeComponent();
            InitializeWorker();

            _viewModel = new CreateDatabaseViewModel();
            _viewModel.Initialize();
            DataContext = _viewModel;
            _viewModel.Databases.ForEach(dbName => DatabasesComboBox.Items.Add(dbName));

            CreateButton.Click += (sender, args) => CreateDatabase();
        }

        private void CreateDatabase()
        {
            if (!CheckRequirements()) return;

            if (!ActionConfirmed()) return;
           
            if (!CreateTargetDatabase()) return;

            ProgressIndicator.Visibility = Visibility.Visible;

            if (!CopyRoutine()) return;

            _worker.RunWorkerAsync();

        }

        private bool CreateTargetDatabase()
        {
            try
            {
                DatabaseController.CreateDatabase(_viewModel.TargetDatabase);
                return true;
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
            return false;
        }

        private bool CopyRoutine()
        {
            try
            {
                const string csvFileName = "proc.csv";
                string csvFullName = Path.Combine(DatabaseController.ProgramDataDirectory(), csvFileName);
                if (File.Exists(csvFullName))
                {
                    File.Delete(csvFullName);
                }
                var conn = DatabaseController.GenericConnection(null);
                var paramCsvFile = new SqlParameter("@csv_file", csvFullName);
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("SELECT * FROM mysql.proc WHERE db = @source_db");
                queryBuilder.AppendLine("INTO");
                queryBuilder.AppendLine("OUTFILE @csv_file");
                queryBuilder.AppendLine("FIELDS TERMINATED BY ';' LINES TERMINATED BY '\r\n';");
                var params1 = new SqlParameter[2];
                params1[0] = new SqlParameter("@source_db", _viewModel.SourceDatabase);
                params1[1] = paramCsvFile;
                DatabaseController.ExecuteSelectQuery(conn, queryBuilder.ToString(), params1);

                queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("LOAD DATA INFILE @csv_file");
                queryBuilder.AppendLine("INTO TABLE mysql.proc FIELDS");
                queryBuilder.AppendLine("TERMINATED BY ';' LINES TERMINATED BY '\r\n' SET db = @target_db;");
                var params2 = new SqlParameter[2];
                params2[0] = new SqlParameter("@target_db", _viewModel.TargetDatabase);
                params2[1] = paramCsvFile;
                DatabaseController.ExecuteSelectQuery(conn, queryBuilder.ToString(), params2);

                if (File.Exists(csvFullName))
                {
                    File.Delete(csvFullName);
                }
                return true;
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
            return false;
        }

        private void CopyDatabase()
        {
            try
            {
                var connection = DatabaseController.GenericConnection(null);
                
                var paramSourceDB = new SqlParameter("@source_db", _viewModel.SourceDatabase);
                var queryTableNames = new StringBuilder();
                queryTableNames.AppendLine("SELECT table_name FROM information_schema.tables");
                queryTableNames.AppendLine("WHERE table_schema = @source_db");
                DataTable tableNames = DatabaseController.ExecuteSelectQuery(connection,
                                                                             queryTableNames.ToString(),
                                                                             new[] {paramSourceDB});

                int processed = 0;
                int total = tableNames.Rows.Count;
                foreach (DataRow dataRow in tableNames.Rows)
                {
                    string tableName = dataRow["table_name"].ToString();
                    var paramTableName = new SqlParameter("@table_name", tableName);

                    // check if base table
                    // SELECT table_type FROM information_schema.tables where table_schema = '$old_news' and table_name = '$table'"
                    var queryTableType = new StringBuilder();
                    queryTableType.AppendLine("SELECT table_type FROM information_schema.tables");
                    queryTableType.AppendLine("WHERE table_schema = @source_db AND table_name = @table_name");
                    var tableTypeParams = new List<SqlParameter> {paramSourceDB, paramTableName};

                    DataTable tableTypes = DatabaseController.ExecuteSelectQuery(connection,
                                                                                 queryTableType.ToString(),
                                                                                 tableTypeParams.ToArray());
                    foreach (DataRow type in tableTypes.Rows)
                    {
                        string tableType = type["table_type"].ToString();
                        if (tableType == "BASE TABLE")
                        {
                            // CREATE TABLE targetDB.@tableName LIKE sourceDB.@tableName;
                            // INSERT INTO targetDB.@tableName SELECT * FROM sourceDB.@tableName;
                            var executeCreateTable = new StringBuilder();
                            executeCreateTable.AppendLine(string.Format("CREATE TABLE `{0}`.`{2}` LIKE `{1}`.`{2}`",
                                                                        _viewModel.TargetDatabase,
                                                                        _viewModel.SourceDatabase,
                                                                        tableName));

                            DatabaseController.ExecuteNonQuery(connection, executeCreateTable.ToString());

                            // do not copy vouchers data from previous year
                            switch (tableName)
                            {
                                case "or":
                                    break;
                                case "jv":
                                    break;
                                case "cv":
                                    break;
                                case "voucher_logs": // voucher logs should be empty
                                    break;
                                default:
                                    var executeCopyData = new StringBuilder();
                                    executeCopyData.AppendLine(string.Format("INSERT INTO `{0}`.`{1}`",
                                                                             _viewModel.TargetDatabase, tableName));

                                    executeCopyData.AppendLine(string.Format("SELECT * FROM `{0}`.`{1}`",
                                                                             _viewModel.SourceDatabase,
                                                                             tableName));

                                    DatabaseController.ExecuteInsertQuery(connection, executeCopyData.ToString());
                                    break;
                            }
                        }
                    }
                    processed++;
                    _viewModel.Progress = Convert.ToInt32((processed*100m)/total);
                    _worker.ReportProgress(_viewModel.Progress);
                }
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
                _worker.CancelAsync();
            }
        }

        private bool CheckRequirements()
        {
            string message;
            if (string.IsNullOrWhiteSpace(_viewModel.SourceDatabase))
            {
                message = "Please select a source database.";
                MessageWindow.ShowAlertMessage(message);
                return false;
            }

            if (string.IsNullOrWhiteSpace(_viewModel.TargetDatabase))
            {
                message = "Please enter output database name.";
                MessageWindow.ShowAlertMessage(message);
                return false;
            }
            return true;
        }

        private bool ActionConfirmed()
        {
            string message;
            if (DatabaseController.IsDatabaseExist(_viewModel.TargetDatabase))
            {
                message =
                    string.Format(
                        "Database '{0}' already exists! All the information in this database will be overwritten?",
                        _viewModel.TargetDatabase);

                if (MessageWindow.ShowWarningMessage(message) != MessageBoxResult.OK)
                {
                    return false;
                }
            }
            message = string.Format("Creating database may take a long time. Do you want to proceed?");
            if (MessageWindow.ShowConfirmMessage(message) != MessageBoxResult.Yes)
            {
                return false;
            }

            return true;
        }

        private void InitializeWorker()
        {
            _worker.DoWork += WorkerOnDoWork;
            _worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
            _worker.ProgressChanged += WorkerOnProgressChanged;
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            ProgressIndicator.Maximum = 100;
        }

        private void WorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            ProgressIndicator.Value = progressChangedEventArgs.ProgressPercentage;
        }

        private void WorkerOnRunWorkerCompleted(object s, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageWindow.ShowAlertMessage(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                MessageWindow.ShowAlertMessage("Processing was cancelled.");
            }
            else
            {
                ProgressIndicator.Visibility = Visibility.Collapsed;
                MessageWindow.ShowNotifyMessage("Processing complete.");
            }
            Close();
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            CopyDatabase();
        }
    }
}