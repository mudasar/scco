using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class User : INotifyPropertyChanged, IModel
    {
        #region --- original ---

//private string _fullName;
        //private string _userName;
        //private string _password;
        //private int _ID;
        //private string _initial;
        //private string _level;


        //public string FullName
        //{
        //    get { return _fullName; }
        //    set
        //    {
        //        _fullName = value;
        //        OnPropertyChanged("FullName");
        //    }
        //}

        //public string UserName
        //{
        //    get { return _userName; }
        //    set
        //    {
        //        _userName = value;
        //        OnPropertyChanged("UserName");
        //    }
        //}
        //public string Password
        //{
        //    get { return _password; }
        //    set
        //    {
        //        _password = value;
        //        OnPropertyChanged("Password");
        //    }
        //}
        //public int ID
        //{
        //    get { return _ID; }
        //    set
        //    {
        //        _ID = value;
        //        OnPropertyChanged("ID");
        //    }
        //}
        //public string Initial
        //{
        //    get { return _initial; }
        //    set
        //    {
        //        _initial = value;
        //        OnPropertyChanged("Initial");
        //    }
        //}
        //public string Level
        //{
        //    get { return _level; }
        //    set
        //    {
        //        _level = value;
        //        OnPropertyChanged("Level");
        //    }
        //}


        //public void ValidateUser()
        //{
        //    string sqlCommandText =
        //        string.Format(
        //            "SELECT * FROM {0} WHERE UserName = ?UserName AND Password = PASSWORD(?Password)",
        //            TABLE_NAME);
        //    var parameters = new List<SqlParameter>
        //                         {new SqlParameter("?UserName", UserName), new SqlParameter("?Password", Password)};

        //    DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText, parameters.ToArray());
        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        ID = Convert.ToInt32(row["ID"]);
        //        UserName = Convert.ToString(row["UserName"]);
        //        FullName = Convert.ToString(row["FullName"]);
        //        Initial = Convert.ToString(row["Initial"]);
        //    }
        //}

        //#region --- CRUD ---

        //private const string TABLE_NAME = "Collection";

        //private List<SqlParameter> Parameters
        //{
        //    get
        //    {
        //        // DO NOT INCLUDE KEY !!!
        //        var list = new List<SqlParameter>();
        //        list.Add(new SqlParameter("?UserName", UserName));
        //        list.Add(new SqlParameter("?FullName", FullName));
        //        list.Add(new SqlParameter("?Initial", Initial));
        //        return list;
        //    }
        //}

        //private SqlParameter ParamKey
        //{
        //    get { return new SqlParameter("?ID", ID); }
        //}

        //public string CollectorName
        //{
        //    get { return "Must Modify User Class To Get CollectorName"; }
        //    //set { //throw new NotImplementedException(); 
        //    //}
        //}

        //public Result Create()
        //{
        //    Action createRecord = () =>
        //                              {
        //                                  List<SqlParameter> sqlParameter = Parameters;
        //                                  string sql = DatabaseController.GenerateInsertStatement(TABLE_NAME, sqlParameter);
        //                                  ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());

        //                                  Password = "password";
        //                                  UpdatePassword();
        //                              };

        //    return ActionController.InvokeAction(createRecord);
        //}

        //public Result Update()
        //{
        //    Action updateRecord = () =>
        //                              {
        //                                  SqlParameter key = ParamKey;

        //                                  List<SqlParameter> sqlParameter = Parameters;
        //                                  sqlParameter.Add(key);

        //                                  string sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME,
        //                                                                                          sqlParameter,
        //                                                                                          key);

        //                                  DatabaseController.ExecuteNonQuery(sql, sqlParameter.ToArray());

        //                              };

        //    return ActionController.InvokeAction(updateRecord);
        //}

        //public Result Destroy()
        //{
        //    Action deleteRecord = () =>
        //                              {
        //                                  SqlParameter key = ParamKey;

        //                                  string sql = DatabaseController.GenerateDeleteStatement(TABLE_NAME, key);

        //                                  DatabaseController.ExecuteNonQuery(sql, key);
        //                              };

        //    return ActionController.InvokeAction(deleteRecord);
        //}

        //public Result Find(int id)
        //{
        //    Action findRecord = () =>
        //                            {
        //                                ResetProperties();
        //                                ID = id;

        //                                SqlParameter key = ParamKey;
        //                                string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

        //                                DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
        //                                foreach (DataRow dataRow in dataTable.Rows)
        //                                {
        //                                    SetPropertiesFromDataRow(dataRow);
        //                                }
        //                            };

        //    return ActionController.InvokeAction(findRecord);
        //}

        //public void ResetProperties()
        //{
        //    ID = 0;
        //    UserName = "";
        //    FullName = "";
        //    Initial = "";
        //    Level = "";
        //}

        //public void SetPropertiesFromDataRow(DataRow dataRow)
        //{
        //    ID = Convert.ToInt32(dataRow["ID"]);
        //    UserName = Convert.ToString(dataRow["UserName"]);
        //    FullName = Convert.ToString(dataRow["FullName"]);
        //    Initial = Convert.ToString(dataRow["Initial"]);
        //}

        //#endregion

        //#region --- CRUD ---

        //public Result UpdatePassword()
        //{
        //    try
        //    {
        //        string sqlCommandText =
        //            string.Format(
        //                "UPDATE {0} SET Password = PASSWORD(?Password) WHERE ID = ?ID",
        //                TABLE_NAME);

        //        var sqlParameters = new List<SqlParameter>
        //                                {
        //                                    new SqlParameter("?Password", Password),
        //                                    new SqlParameter("?ID", ID),
        //                                };
        //        DatabaseController.ExecuteNonQuery(sqlCommandText, sqlParameters.ToArray());
        //        return new Result(true, "Update successful");
        //    }
        //    catch (Exception exception)
        //    {
        //        Logger.ExceptionLogger(this, exception);
        //        return new Result(false, exception.Message);
        //    }
        //}

        //#endregion

        //public static bool IsExists(string fullName)
        //{
        //    string sqlCommandText = string.Format("SELECT * FROM {0} WHERE Description = ?Description", TABLE_NAME);
        //    DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText,
        //                                                                new SqlParameter("?Description", fullName));
        //    return dataTable.Rows.Count > 0;
        //}

        //public static bool IsValidUser(string username, string password)
        //{
        //    string sqlCommandText =
        //        string.Format("SELECT * FROM {0} WHERE UserName = ?UserName AND Password = PASSWORD(?Password)",
        //                      TABLE_NAME);

        //    DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText,
        //                                                                new SqlParameter("?UserName", username),
        //                                                                new SqlParameter("?Password", password));
        //    return dataTable.Rows.Count > 0;
        //}

        //public static List<User> GetList()
        //{
        //    string sqlCommandText = string.Format("SELECT * FROM {0}", TABLE_NAME);
        //    DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);

        //    return (from DataRow row in dataTable.Rows
        //            select new User
        //                       {
        //                           ID = Convert.ToInt32(row["ID"]),
        //                           UserName = (string) row["UserName"],
        //                           FullName = (string) row["FullName"],
        //                           Password = (string) row["Password"],
        //                           Initial = (string) row["Initial"]
        //                       }).ToList();
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        //}

        #endregion

        private const string TABLE_NAME = "secure";

        public static string DefaultPassword
        {
            get { return "password"; }
        }

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string LoginName
        {
            get { return _loginName; }
            set
            {
                _loginName = value;
                OnPropertyChanged("LoginName");
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        public string CollectorName
        {
            get { return _collectorName; }
            set
            {
                _collectorName = value;
                OnPropertyChanged("CollectorName");
            }
        }

        public string Initials
        {
            get { return _initials; }
            set
            {
                _initials = value;
                OnPropertyChanged("Initials");
            }
        }

        public bool CanAccessAccountVerifier
        {
            //MODULE1
            get { return _canAccessAccountVerifier; }
            set
            {
                _canAccessAccountVerifier = value;
                OnPropertyChanged("CanAccessAccountVerifier");
            }
        }

        public bool CanAccessOfficialReceipts
        {
            //MODULE2
            get { return _canAccessOfficialReceipts; }
            set
            {
                _canAccessOfficialReceipts = value;
                OnPropertyChanged("CanAccessOfficialReceipts");
            }
        }

        public bool CanAccessCashVoucher
        {
            //MODULE3
            get { return _canAccessCashVoucher; }
            set
            {
                _canAccessCashVoucher = value;
                OnPropertyChanged("CanAccessCashVoucher");
            }
        }

        public bool CanAccessJournalVoucher
        {
            //MODULE4
            get { return _canAccessJournalVoucher; }
            set
            {
                _canAccessJournalVoucher = value;
                OnPropertyChanged("CanAccessJournalVoucher");
            }
        }

        public bool IsAdministrator
        {
            get { return _isAdministrator; }
            set
            {
                _isAdministrator = value;
                OnPropertyChanged("IsAdministrator");
            }
        }

        public bool Module5; //MODULE5
        public bool Module6; //MODULE6

        public bool CanAccessDatabaseBackup
        {
            //MODULE7
            get { return _canAccessDatabaseBackup; }
            set
            {
                _canAccessDatabaseBackup = value;
                OnPropertyChanged("CanAccessDatabaseBackup");
            }
        }

        public bool CanAccessInitialSetup
        {
            //MODULE8
            get { return _canAccessInitialSetup; }
            set
            {
                _canAccessInitialSetup = value;
                OnPropertyChanged("CanAccessInitialSetup");
            }
        }

        public bool Module9; //MODULE9

        public bool CanAccessTellerCollector
        {
            //MODULE10
            get { return _canAccessTellerCollector; }
            set
            {
                _canAccessTellerCollector = value;
                OnPropertyChanged("CanAccessTellerCollector");
            }
        }

        public bool Module11; //MODULE11
        public bool Module12; //MODULE12
        public bool Module13; //MODULE13
        public bool Module14; //MODULE14
        public bool Module15; //MODULE15
        public bool Module16; //MODULE16
        public int Year; //YEAR
        public bool Beg; //BEG
        public bool Jan; //JAN
        public bool Feb; //FEB
        public bool Mar; //MAR
        public bool Apr; //APR
        public bool May; //MAY
        public bool Jun; //JUN
        public bool Jul; //JUL
        public bool Aug; //AUG
        public bool Sep; //SEP
        public bool Oct; //OCT
        public bool Nov; //NOV
        public bool Dec; //DEC
        public bool Cyear; //CYEAR
        public bool Module30; // MODULE30
        public bool Module31; // MODULE31
        public bool Module32; //MODULE32
        public bool Module33; //MODULE33
        public bool Module34; //MODULE34
        public bool Module35; //MODULE35
        public bool Module36; //MODULE36
        public bool Module37; //MODULE37
        public bool Module38; //MODULE38
        public bool Module39; //MODULE39
        public bool Module40; //MODULE40
        public bool Module41; //MODULE41
        public bool Posted1; //POSTED1
        public bool Posted2; //POSTED2
        public bool Posted3; //POSTED3
        public bool Posted4; //POSTED4
        public bool Posted5; //POSTED5
        public bool Posted6; //POSTED6
        public bool Post; //POST
        public bool UnPost; //UNPOST
        public bool Sl; //SL
        public bool Gl; //GL
        public bool Gl1; //GL1
        public bool Gl2; //GL2
        public bool Load; //LOAD
        private int _id;
        private string _password;
        private string _userName;
        private string _collectorName;
        private string _initials;
        private bool _canAccessTellerCollector;
        private bool _canAccessInitialSetup;
        private string _loginName;
        private bool _canAccessDatabaseBackup;
        private bool _canAccessMemberInformation;
        private bool _canAccessGeneralLedgerReports;
        private bool _canAccessOtherReports;
        private bool _canAccessCashVoucher;
        private bool _canAccessAccountVerifier;
        private bool _canAccessOfficialReceipts;
        private bool _canAccessJournalVoucher;
        private bool _isAdministrator;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Implementation of IModel

        private List<SqlParameter> SqlParameters
        {
            get
            {
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?ACCESS", Password);
                ModelController.AddParameter(sqlParameters, "?LOGIN", LoginName);
                ModelController.AddParameter(sqlParameters, "?NAME", UserName);
                ModelController.AddParameter(sqlParameters, "?COLLECTOR", CollectorName);
                ModelController.AddParameter(sqlParameters, "?INITIAL", Initials);

                ModelController.AddParameter(sqlParameters, "?MODULE1", CanAccessAccountVerifier);
                ModelController.AddParameter(sqlParameters, "?MODULE2", CanAccessOfficialReceipts);
                ModelController.AddParameter(sqlParameters, "?MODULE3", CanAccessCashVoucher);
                ModelController.AddParameter(sqlParameters, "?MODULE4", CanAccessJournalVoucher);
                ModelController.AddParameter(sqlParameters, "?MODULE5", CanAccessGeneralLedgerReports);

                ModelController.AddParameter(sqlParameters, "?MODULE8", CanAccessInitialSetup);
                ModelController.AddParameter(sqlParameters, "?MODULE9", CanAccessOtherReports);
                ModelController.AddParameter(sqlParameters, "?MODULE10", CanAccessTellerCollector);

                ModelController.AddParameter(sqlParameters, "?MODULE31", CanAccessMemberInformation);
                ModelController.AddParameter(sqlParameters, "?MODULE14", IsAdministrator);

                return sqlParameters;
            }
        }

        public bool CanAccessMemberInformation
        {
            get { return _canAccessMemberInformation; }
            set
            {
                _canAccessMemberInformation = value;
                OnPropertyChanged("CanAccessMemberInformation");
            }
        }

        public bool CanAccessGeneralLedgerReports
        {
            get { return _canAccessGeneralLedgerReports; }
            set
            {
                _canAccessGeneralLedgerReports = value;
                OnPropertyChanged("CanAccessGeneralLedgerReports");
            }
        }

        public bool CanAccessOtherReports
        {
            get { return _canAccessOtherReports; }
            set
            {
                _canAccessOtherReports = value;
                OnPropertyChanged("CanAccessOtherReports");
            }
        }

        public DateTime TransactionDate
        {
            get { return MainController.UserTransactionDate; }
        }


        public Result Create()
        {
            Action createRecord = () =>
                {
                    ID = DatabaseController.CreateRecord(TABLE_NAME, SqlParameters);
                };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            Action updateRecord = () =>
                {
                    var key = new SqlParameter("?ID", ID);
                    DatabaseController.UpdateRecord(TABLE_NAME, key, SqlParameters);
                };

            return ActionController.InvokeAction(updateRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () => DatabaseController.DeleteRecord(TABLE_NAME, ID);

            return ActionController.InvokeAction(deleteRecord);
        }

        public Result Find(int id)
        {
            Action findRecord = () =>
                {
                    ResetProperties();
                    ID = id;

                    DataTable dataTable = DatabaseController.FindRecord(TABLE_NAME, ID);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        SetPropertiesFromDataRow(dataRow);
                    }
                };

            return ActionController.InvokeAction(findRecord);
        }

        public void ResetProperties()
        {
            UserName = string.Empty;
            LoginName = string.Empty;
            Password = string.Empty;
            CollectorName = string.Empty;
            Initials = string.Empty;

            CanAccessAccountVerifier = false;
            CanAccessOfficialReceipts = false;
            CanAccessCashVoucher = false;
            CanAccessJournalVoucher = false;

            CanAccessDatabaseBackup = false;
            CanAccessInitialSetup = false;
            CanAccessTellerCollector = false;

            CanAccessMemberInformation = false;
            CanAccessGeneralLedgerReports = false;
            CanAccessOtherReports = false;

            IsAdministrator = false;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            UserName = DataConverter.ToString(dataRow["NAME"]);
            LoginName = DataConverter.ToString(dataRow["LOGIN"]);
            CollectorName = DataConverter.ToString(dataRow["COLLECTOR"]);
            Password = DataConverter.ToString(dataRow["ACCESS"]);
            Initials = DataConverter.ToString(dataRow["INITIAL"]);

            CanAccessAccountVerifier = DataConverter.ToBoolean(dataRow["MODULE1"]);
            CanAccessOfficialReceipts = DataConverter.ToBoolean(dataRow["MODULE2"]);
            CanAccessCashVoucher = DataConverter.ToBoolean(dataRow["MODULE3"]);
            CanAccessJournalVoucher = DataConverter.ToBoolean(dataRow["MODULE4"]);
            CanAccessGeneralLedgerReports = DataConverter.ToBoolean(dataRow["MODULE5"]);

            CanAccessDatabaseBackup = DataConverter.ToBoolean(dataRow["MODULE7"]);
            CanAccessInitialSetup = DataConverter.ToBoolean(dataRow["MODULE8"]);
            CanAccessOtherReports = DataConverter.ToBoolean(dataRow["MODULE9"]);
            CanAccessTellerCollector = DataConverter.ToBoolean(dataRow["MODULE10"]);

            CanAccessMemberInformation = DataConverter.ToBoolean(dataRow["MODULE31"]);

            IsAdministrator = DataConverter.ToBoolean(dataRow["MODULE14"]);
        }

        #endregion

        public static int FindMatch(string userName, string password)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT ID FROM SECURE WHERE LOGIN = ?LOGIN AND ACCESS = PASSWORD(?ACCESS) LIMIT 1");
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("?ACCESS", password));
            parameters.Add(new SqlParameter("?LOGIN", userName));
            var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), parameters.ToArray());
            if (dataTable.Rows.Count > 0)
            {
                return DataConverter.ToInteger(dataTable.Rows[0][0]);
            }
            return 0;
        }

        public Result ResetPassword()
        {
            Password = DefaultPassword;
            return UpdatePassword();
        }

        public Result UpdatePassword()
        {
            Action updatePassword = () =>
                {
                    var sqlBuilder = new StringBuilder();
                    sqlBuilder.AppendFormat(
                        "UPDATE {0} SET ACCESS = PASSWORD(?Password) WHERE ID = ?ID", TABLE_NAME);

                    var sqlParameters = new List<SqlParameter>
                        {
                            new SqlParameter("?Password",
                                             Password),
                            new SqlParameter("?ID", ID),
                        };
                    DatabaseController.ExecuteNonQuery(sqlBuilder.ToString(),
                                                       sqlParameters.ToArray());
                };
            return ActionController.InvokeAction(updatePassword);
        }

        public Result UpdateLoginNameAndPassword()
        {
            Action updateLogin = () =>
                {
                    var sqlBuilder = new StringBuilder();
                    sqlBuilder.AppendFormat(
                        "UPDATE {0} SET LOGIN = ?LoginName, ACCESS = PASSWORD(?Password) WHERE ID = ?ID", TABLE_NAME);

                    var sqlParameters = new List<SqlParameter>
                        {
                            new SqlParameter("?LoginName", LoginName),
                            new SqlParameter("?Password", Password),
                            new SqlParameter("?ID", ID),
                        };
                    DatabaseController.ExecuteNonQuery(sqlBuilder.ToString(), sqlParameters.ToArray());
                };
            return ActionController.InvokeAction(updateLogin);
        }

        public static User FindByName(string logginName)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM");
            sqlBuilder.AppendLine(TABLE_NAME);
            sqlBuilder.AppendLine("WHERE LOGIN = ?LOGIN");
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("?LOGIN", logginName));
            var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), parameters.ToArray());
            if (dataTable.Rows.Count > 0)
            {
                var user = new User();
                user.SetPropertiesFromDataRow(dataTable.Rows[0]);
                return user;
            }
            return null;
        }

        public static List<User> GetList()
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM");
            sqlBuilder.AppendLine(TABLE_NAME);

            var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder);

            var list = new List<User>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var user = new User();
                user.SetPropertiesFromDataRow(dataRow);
                list.Add(user);
            }
            return list;
        }

        internal static UserCollection CollectAll()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT * FROM " + TABLE_NAME);
            var collection = new UserCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new User();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }
    }

    public class UserCollection : System.Collections.ObjectModel.ObservableCollection<User>
    {
    }
}