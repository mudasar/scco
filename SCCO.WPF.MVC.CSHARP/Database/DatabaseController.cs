using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using SCCO.WPF.MVC.CS.Properties;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Database
{
    public class DatabaseController
    {
        private MySqlConnectionStringBuilder _connectionBuilder;
        private MySqlCommand _dbCommand;

        public static MySqlConnection SharedDbConnection
        {
            get;
            set;
        }

        public string DatabaseName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public uint Port
        {
            get;
            set;
        }

        public string Server
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        protected MySqlConnectionStringBuilder ConnectionBuilder
        {
            get
            {
                if ((_connectionBuilder == null))
                {
                    _connectionBuilder = new MySqlConnectionStringBuilder
                        {
                            Server = Server,
                            Port = Port,
                            Database = DatabaseName,
                            UserID = UserName,
                            Password = Password,
                            ConnectionTimeout = 60,
                            DefaultCommandTimeout = 60,
                            AllowZeroDateTime = false,
                            ConvertZeroDateTime = true,
                            RespectBinaryFlags = true
                        };
                }
                return _connectionBuilder;
            }
        }

        protected MySqlCommand DbCommand
        {
            get
            {
                if ((_dbCommand == null))
                {
                    _dbCommand = new MySqlCommand();
                }
                _dbCommand.Connection = new MySqlConnection(ConnectionBuilder.ToString());
                return _dbCommand;
            }
        }

        public static int ExecuteInsertQuery(string sqlInsert, params SqlParameter[] sqlParameters)
        {
            if (sqlInsert == string.Empty) return 0;

            if (SharedDbConnection != null)
            {
                var sqlCommand = new MySqlCommand
                {
                    CommandType = CommandType.Text,
                    CommandText = sqlInsert,
                    Connection = SharedDbConnection
                };

                foreach (SqlParameter parameter in sqlParameters)
                {
                    sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                if (SharedDbConnection.State != ConnectionState.Open)
                {
                    SharedDbConnection.Open();
                }
                int sqlResult = sqlCommand.ExecuteNonQuery();
                if (sqlResult > 0)
                {
                    sqlCommand.CommandText = "SELECT LAST_INSERT_ID()";
                    sqlCommand.Parameters.Clear();
                    sqlResult = Convert.ToInt32(sqlCommand.ExecuteScalar());
                }
                SharedDbConnection.Close();
                return sqlResult;
            }
            if (Validate())
            {
                return ExecuteInsertQuery(sqlInsert, sqlParameters);
            }
            throw new Exception("ExecuteInsertQuery failed.");
        }

        public static int ExecuteNonQuery(string sqlStatement, params SqlParameter[] sqlParameters)
        {
            if (SharedDbConnection != null)
            {
                var sqlCommand = new MySqlCommand
                {
                    CommandType = CommandType.Text,
                    CommandText = sqlStatement,
                    Connection = SharedDbConnection
                };

                foreach (SqlParameter parameter in sqlParameters)
                {
                    sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                if (SharedDbConnection.State != ConnectionState.Open)
                {
                    SharedDbConnection.Open();
                }
                int sqlResult = sqlCommand.ExecuteNonQuery();
                return sqlResult;
            }
            if (Validate())
            {
                return ExecuteInsertQuery(sqlStatement, sqlParameters);
            }
            throw new Exception("ExecuteNonQuery failed.");
        }

        public static DataTable ExecuteSelectQuery(string sqlSelect, params SqlParameter[] parameters)
        {
            if (SharedDbConnection == null)
            {
                Validate();
            }

            var sqlCommand = new MySqlCommand
            {
                CommandType = CommandType.Text,
                CommandText = sqlSelect,
                Connection = SharedDbConnection
            };
            foreach (SqlParameter parameter in parameters)
            {
                sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            var dataAdapter = new MySqlDataAdapter(sqlCommand);
            var dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable ExecuteSelectQuery(StringBuilder sqlBuilder)
        {
            return ExecuteSelectQuery(sqlBuilder.ToString());
        }

        public static DataTable ExecuteSelectQuery(StringBuilder sqlBuilder, SqlParameter sqlParam)
        {
            return ExecuteSelectQuery(sqlBuilder.ToString(), sqlParam);
        }

        public static DataTable ExecuteStoredProcedure(string storedProcedure, params SqlParameter[] parameters)
        {
            if (SharedDbConnection == null)
            {
                Validate();
            }

            var sqlCommand = new MySqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = storedProcedure,
                Connection = SharedDbConnection
            };
            foreach (SqlParameter parameter in parameters)
            {
                sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            var dataAdapter = new MySqlDataAdapter(sqlCommand);
            var dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            return dataTable;
        }

        public static string GenerateDeleteStatement(string tableName, SqlParameter whereParameter)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("DELETE FROM ");
            queryBuilder.Append(tableName);

            // where condition
            queryBuilder.Append(" WHERE ");
            var whereCondition = whereParameter.Key.Replace("?", "") + "=" + whereParameter.Key;
            queryBuilder.Append(whereCondition);

            return queryBuilder.ToString();
        }

        public static string GenerateInsertStatement(string tableName, List<SqlParameter> parameters)
        {
            if (!parameters.Any()) return string.Empty;

            #region --- AUTO-GENERATE QUERY BASED ON SQLPARAMETERS ---

            var queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO ");
            queryBuilder.Append("`" + tableName + "` (`");

            // generate fields
            var fields = parameters.Select(parameter => parameter.Key.Replace("?", "")).ToList();
            queryBuilder.Append(string.Join("`,`", fields));
            queryBuilder.Append("`) VALUES (");

            // generate values
            var values = parameters.Select(parameter => parameter.Key).ToList();
            queryBuilder.Append(string.Join(",", values));
            queryBuilder.Append(")");

            #endregion --- AUTO-GENERATE QUERY BASED ON SQLPARAMETERS ---

            return queryBuilder.ToString();
        }

        public static string GenerateSelectStatement(string tableName, SqlParameter whereParameter)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM ");
            queryBuilder.Append(tableName);

            // where condition
            queryBuilder.Append(" WHERE ");
            var whereCondition = whereParameter.Key.Replace("?", "") + "=" + whereParameter.Key;
            queryBuilder.Append(whereCondition);
            queryBuilder.Append(" LIMIT 1");

            return queryBuilder.ToString();
        }

        public static string GenerateSelectStatement(string tableName)
        {
            return string.Format("SELECT * FROM `{0}`", tableName);
        }

        public static string GenerateUpdateStatement(string tableName, List<SqlParameter> parameters, SqlParameter whereParameter)
        {
            #region --- AUTO-GENERATE QUERY BASED ON SQLPARAMETERS ---

            var queryBuilder = new StringBuilder();
            queryBuilder.Append("UPDATE ");
            queryBuilder.Append("`" + tableName + "` ");

            // set values for each field
            queryBuilder.Append("SET ");
            var fields = parameters.Select(parameter => "`" + parameter.Key.Replace("?", "") + "`" + "=" + parameter.Key).ToList();
            queryBuilder.Append(string.Join(",", fields));

            // where condition
            queryBuilder.Append(" WHERE ");
            var whereCondition = whereParameter.Key.Replace("?", "") + "=" + whereParameter.Key;
            queryBuilder.Append(whereCondition);

            #endregion --- AUTO-GENERATE QUERY BASED ON SQLPARAMETERS ---

            return queryBuilder.ToString();
        }

        // static methods
        public static bool Validate()
        {
            var database = string.Format("{0}_{1}_{2}", Settings.Default.BranchName, DateTime.Now.Year,
                             Settings.Default.DatabaseEnvironment);

            bool functionReturnValue;
            try
            {
                var toReturnValue = new DataTable();
                var connectionBuilder = new MySqlConnectionStringBuilder
                    {
                        Server = Settings.Default.DatabaseServer,
                        Port = Settings.Default.DatabasePort,
                        Database = database,
                        UserID = Settings.Default.DatabaseUser,
                        Password = Utilities.Password.Decrypt(Settings.Default.DatabasePassword),
                        ConnectionTimeout = 60,
                        DefaultCommandTimeout = 60,
                        AllowZeroDateTime = false,
                        ConvertZeroDateTime = true,
                        RespectBinaryFlags = false
                    };
                var sqlConnection = new MySqlConnection(connectionBuilder.ToString());
                using (var dbAdapter = new MySqlDataAdapter("EXPLAIN test", sqlConnection))
                {
                    dbAdapter.Fill(toReturnValue);
                    if (toReturnValue.Rows.Count == 0)
                    {
                        dbAdapter.Fill(toReturnValue);
                    }
                }
                functionReturnValue = true;
                SharedDbConnection = sqlConnection;
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(new DatabaseController(), exception);
                functionReturnValue = false;
            }
            return functionReturnValue;
        }

        public static DataTable FindRecord(string tableName, int id)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM");
            sqlBuilder.AppendLine(tableName);
            sqlBuilder.AppendLine("WHERE id = ?id");

            var parameter = new SqlParameter("?id", id);
            return ExecuteSelectQuery(sqlBuilder, parameter);
        }

        internal static int DeleteRecord(string tableName, int id)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("DELETE FROM");
            sqlBuilder.AppendLine(tableName);
            sqlBuilder.AppendLine("WHERE id = ?id");

            var parameter = new SqlParameter("?id", id);

            return ExecuteNonQuery(sqlBuilder.ToString(), parameter);
        }

        internal static int UpdateRecord(string tableName, SqlParameter paramKey, List<SqlParameter> parameters)
        {
            var sql = GenerateUpdateStatement(tableName, parameters, paramKey);
            parameters.Add(paramKey);
            return ExecuteNonQuery(sql, parameters.ToArray());
        }

        internal static int CreateRecord(string tableName, List<SqlParameter> parameters)
        {
            var sql = GenerateInsertStatement(tableName, parameters);
            return ExecuteInsertQuery(sql, parameters.ToArray());
        }

        public static DataTable ExecuteStoredProcedure(string storedProcedure, string databaseName, params SqlParameter[] parameters)
        {
            #region -- setup database -- 

            var connectionBuilder = new MySqlConnectionStringBuilder
            {
                Server = Settings.Default.DatabaseServer,
                Port = Settings.Default.DatabasePort,
                Database = databaseName,
                UserID = Settings.Default.DatabaseUser,
                Password = Utilities.Password.Decrypt(Settings.Default.DatabasePassword),
                ConnectionTimeout = 60,
                DefaultCommandTimeout = 60,
                AllowZeroDateTime = false,
                ConvertZeroDateTime = true,
                RespectBinaryFlags = false
            };
            var sqlConnection = new MySqlConnection(connectionBuilder.ToString());

            #endregion


            var sqlCommand = new MySqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = storedProcedure,
                Connection = sqlConnection
            };
            foreach (SqlParameter parameter in parameters)
            {
                sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            var dataAdapter = new MySqlDataAdapter(sqlCommand);
            var dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            sqlConnection.Close();
            sqlConnection.Dispose();
            return dataTable;
        }

        internal static void SwitchDatabase(int year)
        {
            // generate databasename based on branch and year
            var database = string.Format("{0}_{1}_{2}", Settings.Default.BranchName, year,
                                         Settings.Default.DatabaseEnvironment);

            if (SharedDbConnection != null)
            {
                // do nothing if database is not changed
                if (database == SharedDbConnection.Database) return;

                if (SharedDbConnection.State != ConnectionState.Closed)
                {
                    SharedDbConnection.Close();
                    SharedDbConnection.Dispose();
                }
                SharedDbConnection.Close();
                SharedDbConnection.Dispose();
            }

            var connectionBuilder = new MySqlConnectionStringBuilder
            {
                Server = Settings.Default.DatabaseServer,
                Port = Settings.Default.DatabasePort,
                Database = database,
                UserID = Settings.Default.DatabaseUser,
                Password = Utilities.Password.Decrypt(Settings.Default.DatabasePassword),
                ConnectionTimeout = 60,
                DefaultCommandTimeout = 60,
                AllowZeroDateTime = false,
                ConvertZeroDateTime = true,
                RespectBinaryFlags = false
            };
            SharedDbConnection = new MySqlConnection(connectionBuilder.ToString());
        }
    }
}