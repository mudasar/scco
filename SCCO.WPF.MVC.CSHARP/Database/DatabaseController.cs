using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using SCCO.WPF.MVC.CS.Properties;

namespace SCCO.WPF.MVC.CS.Database
{
    public class DatabaseController
    {
        #region --- PROPERTIES ---

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

        #endregion

        /// <summary>
        /// Executes an insert query
        /// </summary>
        /// <param name="sqlInsert">SQL insert statement</param>
        /// <param name="sqlParameters">SQL parameters</param>
        /// <returns>Last Insert ID</returns>
        public static int ExecuteInsertQuery(string sqlInsert, params SqlParameter[] sqlParameters)
        {
            try
            {
                OpenConnection();
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
                Log(sqlCommand);
                var sqlResult = sqlCommand.ExecuteNonQuery();
                if (sqlResult > 0)
                {
                    sqlCommand.CommandText = "SELECT LAST_INSERT_ID()";
                    sqlCommand.Parameters.Clear();
                    sqlResult = Convert.ToInt32(sqlCommand.ExecuteScalar());
                }
                return sqlResult;
            }
            finally
            {
                CloseConnection();
            }
        }

        public static int ExecuteNonQuery(string sqlStatement, params SqlParameter[] sqlParameters)
        {
            try
            {
                OpenConnection();
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
                Log(sqlCommand);
                var sqlResult = sqlCommand.ExecuteNonQuery();
                return sqlResult;
            }
            finally
            {
                CloseConnection();
            }
        }

        public static DataTable ExecuteSelectQuery(string sqlSelect, params SqlParameter[] parameters)
        {
            try
            {
                OpenConnection();
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
                Log(sqlCommand);
                var dataAdapter = new MySqlDataAdapter(sqlCommand);
                var dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            finally
            {
                CloseConnection();
            }
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
            try
            {
                OpenConnection();
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
                Log(sqlCommand);
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            finally
            {
                CloseConnection();
            }
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
            using (var connection = new MySqlConnection(ConnectionString()))
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
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
            }
        }

        public static bool IsDatabaseExist(string databaseName)
        {
            const string sql = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = ?databaseName";
            using (var connection = new MySqlConnection(ConnectionString()))
            {
                try
                {
                    var sqlCommand = new MySqlCommand
                        {
                            CommandType = CommandType.Text,
                            CommandText = sql,
                            Connection = connection
                        };
                    sqlCommand.Parameters.AddWithValue("?databaseName", databaseName);

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

        //public static bool Validate()
        //{
        //    try
        //    {
        //        if (SharedDbConnection == null)
        //        {
        //            var sqlConnection = new MySqlConnection(ConnectionString(database));
        //            SharedDbConnection = sqlConnection;
        //        }
        //        if (SharedDbConnection.State != ConnectionState.Open)
        //        {
        //            SharedDbConnection.Open();
        //        }
        //        return true;
        //    }
        //    catch (Exception exception)
        //    {
        //        Logger.ExceptionLogger(new DatabaseController(), exception);
        //    }
        //    return false;
        //}

        public static DataTable FindRecord(string tableName, int id)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM");
            sqlBuilder.AppendLine(tableName);
            sqlBuilder.AppendLine("WHERE id = ?id");

            var parameter = new SqlParameter("?id", id);
            return ExecuteSelectQuery(sqlBuilder, parameter);
        }

        public static DataTable ExecuteStoredProcedure(string storedProcedure, string databaseName,
                                                       params SqlParameter[] parameters)
        {
            var sqlConnection = new MySqlConnection(ConnectionString(databaseName));
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
            Log(sqlCommand);
            dataAdapter.Fill(dataTable);
            sqlConnection.Close();
            sqlConnection.Dispose();
            return dataTable;
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

        internal static void SwitchDatabase(int year)
        {
            // generate databasename based on branch and year
            string database = string.Format("{0}_{1}_{2}", Settings.Default.BranchName, year,
                                            Settings.Default.DatabaseEnvironment);

            SwitchDatabase(database);
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
            using (var conn = new MySqlConnection(ConnectionString(database)))
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
            using (var conn = new MySqlConnection(ConnectionString(database)))
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

        internal static string ConnectionString()
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
            return connectionBuilder.ToString();
        }

        internal static string ConnectionString(string database)
        {
            var connectionBuilder = new MySqlConnectionStringBuilder
                {
                    Server = Settings.Default.DatabaseServer,
                    Port = Settings.Default.DatabasePort,
                    UserID = Settings.Default.DatabaseUser,
                    Password = Utilities.Password.Decrypt(Settings.Default.DatabasePassword),
                    Database = database,
                    ConnectionTimeout = 60,
                    DefaultCommandTimeout = 60,
                    AllowZeroDateTime = false,
                    ConvertZeroDateTime = true,
                    RespectBinaryFlags = false
                };
            return connectionBuilder.ToString();
        }

        internal static DataTable ShowDatabases()
        {
            // "SHOW DATABASES"
            const string commandText = "SHOW DATABASES";
            using (var connection = new MySqlConnection(ConnectionString()))
            {
                try
                {
                    var sqlCommand = new MySqlCommand
                        {
                            CommandType = CommandType.Text,
                            CommandText = commandText,
                            Connection = connection
                        };
                    var dataAdapter = new MySqlDataAdapter(sqlCommand);
                    var dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
                catch (MySqlException)
                {
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        internal static void DropDatabase(string database)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendLine("DROP DATABASE IF EXISTS");
            queryBuilder.AppendLine(database);

            var conn = new MySqlConnection(ConnectionString());
            try
            {
                Console.WriteLine(@"Connecting to MySQL...");
                conn.Open();

                var sql = queryBuilder.ToString();
                var cmd = new MySqlCommand(sql, conn);
                Console.WriteLine(@"Execute: {0}", sql);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conn.Close();
                Console.WriteLine(@"Done.");
            }
        }

        internal static void CreateDatabase(string database)
        {
            DropDatabase(database);

            var queryBuilder = new StringBuilder();
            queryBuilder.AppendLine("CREATE DATABASE");
            queryBuilder.AppendLine(database);

            var conn = new MySqlConnection(ConnectionString());
            try
            {
                Console.WriteLine(@"Connecting to MySQL...");
                conn.Open();

                var sql = queryBuilder.ToString();
                var cmd = new MySqlCommand(sql, conn);
                Console.WriteLine(@"Execute: {0}", sql);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conn.Close();
                Console.WriteLine(@"Done.");
            }
        }

        internal static MySqlConnection GenericConnection(string database)
        {
            if (database == null)
            {
                return new MySqlConnection(ConnectionString());
            }
            return string.IsNullOrEmpty(database)
                       ? new MySqlConnection(ConnectionString())
                       : new MySqlConnection(ConnectionString(database));
        }

        internal static void ExecuteNonQuery(MySqlConnection conn, string sql, params SqlParameter[] parameters)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Console.WriteLine(@"Connecting to MySQL...");
                    conn.Open();
                }
                var cmd = new MySqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = sql,
                        Connection = conn
                    };
                foreach (SqlParameter parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                Console.WriteLine(cmd.CommandText);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conn.Close();
                Console.WriteLine(@"Done.");
            }
        }

        internal static DataTable ExecuteSelectQuery(MySqlConnection conn, string sql, params SqlParameter[] parameters)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Console.WriteLine(@"Connecting to MySQL...");
                    conn.Open();
                }
                var cmd = new MySqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = sql,
                        Connection = conn
                    };
                foreach (SqlParameter parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                var dataAdapter = new MySqlDataAdapter(cmd);
                var dataTable = new DataTable();
                Console.WriteLine(cmd.CommandText);
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            finally
            {
                conn.Close();
                Console.WriteLine(@"Done.");
            }
        }

        internal static int ExecuteInsertQuery(MySqlConnection conn, string sql, params SqlParameter[] parameters)
        {
            try
            {
                var cmd = new MySqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = sql,
                        Connection = conn
                    };

                foreach (SqlParameter parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                if (conn.State != ConnectionState.Open)
                {
                    Console.WriteLine(@"Connecting to MySQL...");
                    conn.Open();
                }
                Console.WriteLine(cmd.CommandText);
                var sqlResult = cmd.ExecuteNonQuery();
                return sqlResult;
            }
            finally
            {
                conn.Close();
                Console.WriteLine(@"Done.");
            }
        }

        internal static string BaseDirectory()
        {
            const string query = "SHOW VARIABLES WHERE Variable_Name = 'basedir'";
            DataTable dataTable = ExecuteSelectQuery(GenericConnection(null), query);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                return dataRow[1].ToString();
            }
            return "";
        }

        internal static string ProgramDataDirectory()
        {
            const string query = "SHOW VARIABLES WHERE Variable_Name = 'datadir'";
            DataTable dataTable = ExecuteSelectQuery(GenericConnection(null), query);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                return dataRow[1].ToString();
            }
            return "";
        }

        #region --- PRIVATES ---


        /// <summary>
        /// Ensure connection is closed
        /// </summary>
        private static void CloseConnection()
        {
            if (SharedDbConnection.State != ConnectionState.Closed)
            {
                SharedDbConnection.Close();
            }
        }

        /// <summary>
        /// Ensure connection is open
        /// </summary>
        private static void OpenConnection()
        {
            if (SharedDbConnection == null)
            {
                SharedDbConnection = new MySqlConnection(ConnectionString(DefaultDatabase()));
            }
            if (SharedDbConnection.State != ConnectionState.Open)
            {
                SharedDbConnection.Open();
            }
        }

        private static string DefaultDatabase()
        {
            return string.Format("{0}_{1}_{2}",
                                 Settings.Default.BranchName,
                                 DateTime.Now.Year,
                                 Settings.Default.DatabaseEnvironment);
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

            SharedDbConnection = new MySqlConnection(ConnectionString(database));
            SharedDbConnection.Open();
        }

        private static void Log(MySqlCommand sqlCommand)
        {
            Console.WriteLine(sqlCommand.CommandText);
        }

        #endregion
    }
}