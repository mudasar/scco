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
        private int _progressValue;
        public int ProgressValue
        {
            get { return _progressValue; }
            set { _progressValue = value; OnPropertyChanged("ProgressValue"); }
        }

        public Result Process2()
        {
            var transactionDate = MainController.LoggedUser.TransactionDate;
            var newYear = transactionDate.Year;
            var oldYear = newYear - 1;

            var branch = Settings.Default.BranchName;
            var environment = Settings.Default.DatabaseEnvironment;

            var oldDatabase = string.Format("{0}_{1}_{2}", branch, oldYear, environment);
            var newDatabase = string.Format("{0}_{1}_{2}", branch, newYear, environment);

            // execute sp for updating "end_balances" table for previous year
            const string storedProcedure = "sp_update_end_balances";
            var parameters = new List<SqlParameter> { new SqlParameter("ti_transaction_year", oldYear) };
            //DatabaseController.ExecuteStoredProcedure(storedProcedure, parameters.ToArray());

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

                // update slbal of current year
                var truncateTable = string.Format("TRUNCATE TABLE {0}.slbal", newDatabase);
                DatabaseController.ExecuteNonQuery(truncateTable);

                // INSERT INTO newDatabase.table1 (Column1, Column2) 
                // SELECT column1, column2 FROM oldDatabase.table1;
                var sqlStatement = new StringBuilder();
                sqlStatement.AppendFormat("INSERT INTO {0}.{1} ", newDatabase, "slbal");
                sqlStatement.AppendLine();
                sqlStatement.AppendFormat("SELECT * FROM {0}.{1};", oldDatabase, "end_balances");

                DatabaseController.ExecuteSelectQuery(sqlStatement);
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
        public void Process()
        {
            var transactionDate = MainController.LoggedUser.TransactionDate;
            var newYear = transactionDate.Year;
            var oldYear = newYear - 1;

            var branch = Settings.Default.BranchName;
            var environment = Settings.Default.DatabaseEnvironment;

            var oldDatabase = string.Format("{0}_{1}_{2}", branch, oldYear, environment);
            var newDatabase = string.Format("{0}_{1}_{2}", branch, newYear, environment);

            var sqlStatement = new StringBuilder();
            sqlStatement.AppendFormat("SELECT * FROM {0}.{1};", oldDatabase, "nfmb");
            var dataTable = DatabaseController.ExecuteSelectQuery(sqlStatement);
            var totalCount = dataTable.Rows.Count;
            var counter = 0;
            foreach (System.Data.DataRow dataRow in dataTable.Rows)
            {
                counter++;
                var jea = (counter/totalCount)*100;
                ProgressValue = jea;
                System.Threading.Thread.Sleep(300);
            }
        }
    }
}
