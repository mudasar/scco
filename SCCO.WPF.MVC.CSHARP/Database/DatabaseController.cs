﻿using System;
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

        public static MySqlConnection SharedDbConnection { get; set; }

        public string DatabaseName { get; set; }

        public string Password { get; set; }

        public uint Port { get; set; }

        public string Server { get; set; }

        public string UserName { get; set; }

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

        public static DataTable ExecuteStoredProcedure(string storedProcedure, List<SqlParameter> parameters)
        {
            return ExecuteStoredProcedure(storedProcedure, parameters.ToArray());
        }

        public static string GenerateDeleteStatement(string tableName, SqlParameter whereParameter)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("DELETE FROM ");
            queryBuilder.Append(tableName);

            // where condition
            queryBuilder.Append(" WHERE ");
            string whereCondition = whereParameter.Key.Replace("?", "") + "=" + whereParameter.Key;
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
            List<string> fields = parameters.Select(parameter => parameter.Key.Replace("?", "")).ToList();
            queryBuilder.Append(string.Join("`,`", fields));
            queryBuilder.Append("`) VALUES (");

            // generate values
            List<string> values = parameters.Select(parameter => parameter.Key).ToList();
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
            string whereCondition = whereParameter.Key.Replace("?", "") + "=" + whereParameter.Key;
            queryBuilder.Append(whereCondition);
            queryBuilder.Append(" LIMIT 1");

            return queryBuilder.ToString();
        }

        public static string GenerateSelectStatement(string tableName)
        {
            return string.Format("SELECT * FROM `{0}`", tableName);
        }

        public static string GenerateUpdateStatement(string tableName, List<SqlParameter> parameters,
                                                     SqlParameter whereParameter)
        {
            #region --- AUTO-GENERATE QUERY BASED ON SQLPARAMETERS ---

            var queryBuilder = new StringBuilder();
            queryBuilder.Append("UPDATE ");
            queryBuilder.Append("`" + tableName + "` ");

            // set values for each field
            queryBuilder.Append("SET ");
            List<string> fields =
                parameters.Select(parameter => "`" + parameter.Key.Replace("?", "") + "`" + "=" + parameter.Key)
                          .ToList();
            queryBuilder.Append(string.Join(",", fields));

            // where condition
            queryBuilder.Append(" WHERE ");
            string whereCondition = whereParameter.Key.Replace("?", "") + "=" + whereParameter.Key;
            queryBuilder.Append(whereCondition);

            #endregion --- AUTO-GENERATE QUERY BASED ON SQLPARAMETERS ---

            return queryBuilder.ToString();
        }

        // static methods
        public static bool IsServerConnected()
        {
            var connectionBuilder = new MySqlConnectionStringBuilder
                {
                    Server = Settings.Default.DatabaseServer,
                    Port = Settings.Default.DatabasePort,
                    UserID = Settings.Default.DatabaseUser,
                    Password = Utilities.Password.Decrypt(Settings.Default.DatabasePassword),
                    ConnectionTimeout = 60,
                    DefaultCommandTimeout = 60,
                    AllowZeroDateTime = false,
                    ConvertZeroDateTime = true,
                    RespectBinaryFlags = false
                };
            using (var connection = new MySqlConnection(connectionBuilder.ToString()))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine(@"MySQL version : {0}", connection.ServerVersion);
                    return true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(@"Error: {0}", ex);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static bool IsDatabaseExist(string databaseName)
        {
            string sql =
                string.Format("SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = ?db_name");
            var connectionBuilder = new MySqlConnectionStringBuilder
                {
                    Server = Settings.Default.DatabaseServer,
                    Port = Settings.Default.DatabasePort,
                    UserID = Settings.Default.DatabaseUser,
                    Password = Utilities.Password.Decrypt(Settings.Default.DatabasePassword),
                    ConnectionTimeout = 60,
                    DefaultCommandTimeout = 60,
                    AllowZeroDateTime = false,
                    ConvertZeroDateTime = true,
                    RespectBinaryFlags = false
                };
            using (var connection = new MySqlConnection(connectionBuilder.ToString()))
            {
                try
                {
                    var sqlCommand = new MySqlCommand
                        {
                            CommandType = CommandType.Text,
                            CommandText = sql,
                            Connection = connection
                        };
                    sqlCommand.Parameters.AddWithValue("?db_name", databaseName);

                    var dataAdapter = new MySqlDataAdapter(sqlCommand);
                    var dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataTable.Rows.Count > 0;
                }
                catch (MySqlException)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static bool Validate()
        {
            string database = string.Format("{0}_{1}_{2}", Settings.Default.BranchName, DateTime.Now.Year,
                                            Settings.Default.DatabaseEnvironment);
            try
            {
                if (SharedDbConnection == null)
                {
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

                    SharedDbConnection = sqlConnection;
                }
                if (SharedDbConnection.State != ConnectionState.Open)
                {
                    SharedDbConnection.Open();
                }
                return true;
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(new DatabaseController(), exception);
            }
            return false;
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
            string sql = GenerateUpdateStatement(tableName, parameters, paramKey);
            parameters.Add(paramKey);
            return ExecuteNonQuery(sql, parameters.ToArray());
        }

        internal static int CreateRecord(string tableName, List<SqlParameter> parameters)
        {
            string sql = GenerateInsertStatement(tableName, parameters);
            return ExecuteInsertQuery(sql, parameters.ToArray());
        }

        public static DataTable ExecuteStoredProcedure(string storedProcedure, string databaseName,
                                                       params SqlParameter[] parameters)
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
            string database = string.Format("{0}_{1}_{2}", Settings.Default.BranchName, year,
                                            Settings.Default.DatabaseEnvironment);

            SwitchDatabase(database);
        }

        private static void SwitchDatabase(string database)
        {
            if (SharedDbConnection != null)
            {
                // do nothing if database is not changed
                if (database == SharedDbConnection.Database)
                {
                    if (SharedDbConnection.State == ConnectionState.Closed)
                    {
                        SharedDbConnection.Open();
                    }
                    return;
                }

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
            SharedDbConnection.Open();
        }

        internal static string GetDatabaseByYear(int year)
        {
            return string.Format("{0}_{1}_{2}",
                                 Settings.Default.BranchName,
                                 year,
                                 Settings.Default.DatabaseEnvironment).ToLower();
        }

        internal static void CloseDatabase()
        {
            try
            {
                if (SharedDbConnection.State != ConnectionState.Closed)
                {
                    SharedDbConnection.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        internal static void UseDatabase(string databaseName)
        {
            try
            {
                if (SharedDbConnection == null)
                {
                    SwitchDatabase(databaseName);
                }
                else
                {
                    if (SharedDbConnection.State != ConnectionState.Open)
                    {
                        SharedDbConnection.Open();
                    }
                }
                var sqlCommand = new MySqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = string.Format("USE {0}", databaseName),
                        Connection = SharedDbConnection
                    };
                sqlCommand.Prepare();
                sqlCommand.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
            }
        }

        internal static void Backup(string database, string backupFile)
        {
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

            using (var conn = new MySqlConnection(connectionBuilder.ToString()))
            {
                using (var cmd = new MySqlCommand())
                {
                    using (var mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(backupFile);
                        conn.Close();
                    }
                }
            }
        }

        internal static void Restore(string database, string backupFile)
        {
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
            using (var conn = new MySqlConnection(connectionBuilder.ToString()))
            {
                using (var cmd = new MySqlCommand())
                {
                    using (var mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportFromFile(backupFile);
                        conn.Close();
                    }
                }
            }
        }
    }
}