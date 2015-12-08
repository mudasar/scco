using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Properties;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    internal class UpdateBeginningBalanceViewModel : ViewModelBase
    {
        private DateTime _cutoffDate;
        private bool _isUpdateGeneralLedger;
        private bool _isUpdateSubsidiaryLedger;
        private int _progressValue;

        public int ProgressValue
        {
            get { return _progressValue; }
            set
            {
                _progressValue = value;
                OnPropertyChanged("ProgressValue");
            }
        }

        public DateTime CutoffDate
        {
            get { return _cutoffDate; }
            set
            {
                _cutoffDate = value;
                OnPropertyChanged("CutoffDate");
            }
        }

        public bool IsUpdateGeneralLedger
        {
            get { return _isUpdateGeneralLedger; }
            set
            {
                _isUpdateGeneralLedger = value;
                OnPropertyChanged("IsUpdateGeneralLedger");
            }
        }

        public bool IsUpdateSubsidiaryLedger
        {
            get { return _isUpdateSubsidiaryLedger; }
            set
            {
                _isUpdateSubsidiaryLedger = value;
                OnPropertyChanged("IsUpdateSubsidiaryLedger");
            }
        }


        public Result PerformUpdate()
        {
            DateTime transactionDate = MainController.LoggedUser.TransactionDate;
            int newYear = transactionDate.Year;
            int oldYear = CutoffDate.Year;

            string branch = Settings.Default.BranchName;
            string environment = Settings.Default.DatabaseEnvironment;

            string oldDatabase = string.Format("{0}_{1}_{2}", branch, oldYear, environment);
            string newDatabase = string.Format("{0}_{1}_{2}", branch, newYear, environment);

            // execute sp for updating "end_balances" table for previous year
            const string storedProcedure = "sp_update_end_balances";
            var parameters = new List<SqlParameter> {new SqlParameter("td_cutoff_date", CutoffDate)};

            var connectionBuilder = new MySqlConnectionStringBuilder
                {
                    Server = Settings.Default.DatabaseServer,
                    Port = Settings.Default.DatabasePort,
                    Database = oldDatabase,
                    UserID = Settings.Default.DatabaseUser,
                    Password = Password.Decrypt(Settings.Default.DatabasePassword),
                    ConnectionTimeout = 60,
                    DefaultCommandTimeout = 60,
                    AllowZeroDateTime = false,
                    ConvertZeroDateTime = true,
                    RespectBinaryFlags = false
                };

            var sqlConnection = new MySqlConnection(connectionBuilder.ToString());
            var sqlCommand = new MySqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = storedProcedure,
                    Connection = sqlConnection,
                    CommandTimeout = 0,
                };
            foreach (SqlParameter parameter in parameters)
            {
                sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            var dataAdapter = new MySqlDataAdapter(sqlCommand);
            var dataTable = new DataTable();
            try
            {
                dataAdapter.Fill(dataTable);

                if (IsUpdateSubsidiaryLedger)
                {
                    // update slbal of current year
                    string truncateTable = string.Format("TRUNCATE TABLE {0}.slbal", newDatabase);
                    DatabaseController.ExecuteNonQuery(truncateTable);

                    // INSERT INTO newDatabase.table1 (Column1, Column2) 
                    // SELECT column1, column2 FROM oldDatabase.table1;
                    var sqlStatement = new StringBuilder();
                    sqlStatement.AppendFormat("INSERT INTO {0}.{1} ", newDatabase, "slbal");
                    sqlStatement.AppendLine();
                    sqlStatement.AppendFormat("SELECT * FROM {0}.{1};", oldDatabase, "end_balances");

                    DatabaseController.ExecuteSelectQuery(sqlStatement);
                }
                if (IsUpdateGeneralLedger)
                {
                    // present_database VARCHAR(40), previous_database VARCHAR(40), cutoff_date DATE
                    var sqlParameters = new List<SqlParameter>();
                    sqlParameters.Add(new SqlParameter("present_database", newDatabase));
                    sqlParameters.Add(new SqlParameter("previous_database", oldDatabase));
                    sqlParameters.Add(new SqlParameter("cutoff_date", CutoffDate));
                    DatabaseController.ExecuteStoredProcedure("sp_update_general_ledger_balances", sqlParameters);
                }
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return new Result(true, "Success");
        }
    }
}