using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Views;

namespace SCCO.WPF.MVC.CS.Utilities.DbfMigration.Views
{
	public partial class MigrationProgessWindow
	{
		public MigrationProgessWindow()
		{
			InitializeComponent();
			InitializeWorker();
			// Insert code required on object creation below this point.
		}

        public MigrationProgessWindow(List<string> dataFolderList, List<string> tablesToMigrate):this()
        {
            _dataFolderList = dataFolderList;
            _tablesToMigrate = tablesToMigrate;
            OverallProgressBar.Maximum = _dataFolderList.Count*_tablesToMigrate.Count;

            _worker.RunWorkerAsync();
        }

	    private readonly List<string> _dataFolderList;
	    private readonly List<string> _tablesToMigrate;
        private readonly BackgroundWorker _worker = new BackgroundWorker();
	    private int _overallProgressCounter;

        private void InitializeWorker()
        {
            _worker.DoWork += WorkerOnDoWork;
            _worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
            _worker.ProgressChanged += WorkerOnProgressChanged;
            _worker.WorkerReportsProgress = true;
            CurrentActionProgressBar.Maximum = 100;
        }

        private void WorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            CurrentActionProgressBar.Value = progressChangedEventArgs.ProgressPercentage;
            OverallProgressBar.Value = _overallProgressCounter;
        }

        private void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            MessageWindow.ShowNotifyMessage("Process Complete!");
            Close();
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var dataFolderList = _dataFolderList;
            var tables = _tablesToMigrate;
            _overallProgressCounter = 0;
            foreach (var dataFolder in dataFolderList)
            {
                foreach (var tablePathName in tables)
                {
                    var dbFile = Path.Combine(dataFolder, Path.ChangeExtension(tablePathName, ".dbf"));
                    try
                    {
                        System.Data.DataTable table = Interface.OpenFile(dbFile);
                        var totalRecords = table.Rows.Count;
                        int counter = 0;
                        foreach (System.Data.DataRow dataRow in table.Rows)
                        {
                            var paramList = new List<SqlParameter>();
                            var fieldNames = new List<string>();
                            var fieldParams = new List<string>();

                            foreach (System.Data.DataColumn column in table.Columns)
                            {
                                var defaultFieldValue = Interface.GetDefaultFieldValue(dataRow[column], column.DataType);
                                if (defaultFieldValue == null)
                                    continue;

                                var fieldParam = "?" + column.ColumnName;
                                paramList.Add(new SqlParameter(fieldParam, defaultFieldValue));
                                fieldNames.Add("`" + column.ColumnName + "`");
                                fieldParams.Add(fieldParam);
                            }

                            var tableName = Path.GetFileNameWithoutExtension(dbFile);
                            var sqlBuilder = new StringBuilder();
                            sqlBuilder.AppendLine(string.Format("INSERT INTO `{0}`", tableName));
                            sqlBuilder.AppendLine(string.Format("({0})", string.Join(", ", fieldNames)));
                            sqlBuilder.AppendLine(string.Format("VALUES ({0})", string.Join(", ", fieldParams)));
                            DatabaseController.ExecuteInsertQuery(sqlBuilder.ToString(), paramList.ToArray());
                            counter++;
                            var percent = (int)(counter * (100m / totalRecords));
                            _worker.ReportProgress(percent);
                        }
                        _overallProgressCounter++;
                    }
                    catch (Exception ex)
                    {
                        Logger.ExceptionLogger(this, ex);
                        MessageWindow.ShowAlertMessage(ex.Message);
                    }
                }
            }
            // Update JV Docnum (+1000) to make it unique
            UpdateJournalVoucherDocumentNumber();
        }

        private void UpdateJournalVoucherDocumentNumber()
        {
            const string sql = "UPDATE `jv` SET DOC_NUM = DOC_NUM + (1000 * MONTH(DOC_DATE)) WHERE DOC_NUM < 1000";
            DatabaseController.ExecuteNonQuery(sql);
        }
	}
}