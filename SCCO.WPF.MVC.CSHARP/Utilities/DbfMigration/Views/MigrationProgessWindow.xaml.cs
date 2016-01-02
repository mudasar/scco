using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Views;

namespace SCCO.WPF.MVC.CS.Utilities.DbfMigration.Views
{
    public partial class MigrationProgessWindow
    {
        private readonly Queue<FileInfo> _tablesForMigration;

        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private int _overallProgressCounter;

        public MigrationProgessWindow()
        {
            InitializeComponent();
            InitializeWorker();
        }

        public MigrationProgessWindow(Queue<FileInfo> tablesForMigration)
            : this()
        {
            _tablesForMigration = tablesForMigration;
            OverallProgressBar.Maximum = tablesForMigration.Count;

            _worker.RunWorkerAsync();
        }

        private void InitializeWorker()
        {
            _worker.DoWork += WorkerOnDoWork;
            _worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
            _worker.ProgressChanged += WorkerOnProgressChanged;
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            CurrentActionProgressBar.Maximum = 100;
        }

        private void WorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            CurrentActionProgressBar.Value = progressChangedEventArgs.ProgressPercentage;
            OverallProgressBar.Value = _overallProgressCounter;
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
                MessageWindow.ShowNotifyMessage("Processing complete.");
            }
            Close();
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            _overallProgressCounter = 0;
            int totalErrors = 0;
            while (_tablesForMigration.Count > 0)
            {
                FileInfo tableInfo = _tablesForMigration.Dequeue();
                string dbFile = tableInfo.FullName;

                #region -- migrate table to mysql ---

                try
                {
                    DataTable table = MigrationHelper.OpenFile(dbFile);
                    if (table == null)
                    {
                        throw new Exception(string.Format("Unable to open {0}.", dbFile));
                    }
                    int totalRecords = table.Rows.Count;
                    int counter = 0;
                    foreach (DataRow dataRow in table.Rows)
                    {
                        var paramList = new List<SqlParameter>();
                        var fieldNames = new List<string>();
                        var fieldParams = new List<string>();

                        foreach (DataColumn column in table.Columns)
                        {
                            object defaultFieldValue = MigrationHelper.GetDefaultFieldValue(dataRow[column],
                                                                                            column.DataType);
                            if (defaultFieldValue == null)
                                continue;

                            string fieldParam = "?" + column.ColumnName;
                            paramList.Add(new SqlParameter(fieldParam, defaultFieldValue));
                            fieldNames.Add("`" + column.ColumnName + "`");
                            fieldParams.Add(fieldParam);
                        }

                        string tableName = Path.GetFileNameWithoutExtension(dbFile);
                        var sqlBuilder = new StringBuilder();
                        sqlBuilder.AppendLine(string.Format("INSERT INTO `{0}`", tableName));
                        sqlBuilder.AppendLine(string.Format("({0})", string.Join(", ", fieldNames)));
                        sqlBuilder.AppendLine(string.Format("VALUES ({0})", string.Join(", ", fieldParams)));
                        DatabaseController.ExecuteInsertQuery(sqlBuilder.ToString(), paramList.ToArray());
                        counter++;
                        var percent = (int) (counter*(100m/totalRecords));
                        _worker.ReportProgress(percent);
                    }
                    _overallProgressCounter++;
                }
                catch (Exception exception)
                {
                    totalErrors++;
                    Logger.ExceptionLogger(this, exception);
                    MessageWindow.ShowAlertMessage(exception.Message);
                    _tablesForMigration.Enqueue(tableInfo);
                    if (totalErrors > 10)
                    {
                        _worker.CancelAsync();
                    }
                }

                #endregion -- end migrate table to mysql --
            }

            UpdateJournalVoucherDocumentNumber();
        }

        private void UpdateJournalVoucherDocumentNumber()
        {
            const string sql = "UPDATE `jv` SET DOC_NUM = DOC_NUM + (1000 * MONTH(DOC_DATE)) WHERE DOC_NUM < 1000";
            DatabaseController.ExecuteNonQuery(sql);
        }
    }
}