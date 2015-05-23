using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class Contact : INotifyPropertyChanged, IModel
    {
        private string _accountType;
        private string _firstName;
        private int _id;
        private string _lastName;
        private string _memberCode;
        private string _memberName;
        private string _middleName;
        private byte[] _picture;
        private byte[] _signature;
        private string _suffix;
        private string _telephone;
        private string _businessPhone;
        private string _mobilePhone;
        private string _email;

        #region Properties

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string FullName
        {
            get
            {
                string fullname = string.Format("{0}, {1} {2} {3}.", LastName, FirstName, Suffix,
                                                MiddleName.Substring(0, 1));
                while (fullname.Contains("  "))
                {
                    fullname = fullname.Replace("  ", " ");
                }
                return fullname;
            }
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

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }


        public string AccountType
        {
            get { return _accountType; }
            set
            {
                _accountType = value;
                OnPropertyChanged("AccountType");
            }
        }

        public string MemberCode
        {
            get { return _memberCode; }
            set
            {
                _memberCode = value;
                OnPropertyChanged("MemberCode");
            }
        }

        public string MemberName
        {
            get { return _memberName; }
            set
            {
                _memberName = value;
                OnPropertyChanged("MemberName");
            }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value;
                OnPropertyChanged("MiddleName");
            }
        }

        public byte[] Picture
        {
            get { return _picture; }
            set
            {
                _picture = value;
                OnPropertyChanged("Picture");
            }
        }

        public byte[] Signature
        {
            get { return _signature; }
            set
            {
                _signature = value;
                OnPropertyChanged("Signature");
            }
        }

        public string Suffix
        {
            get { return _suffix; }
            set
            {
                _suffix = value;
                OnPropertyChanged("Suffix");
            }
        }

        public string Telephone
        {
            get { return _telephone; }
            set
            {
                _telephone = value;
                OnPropertyChanged("Telephone");
            }
        }

        public string BusinessPhone
        {
            get { return _businessPhone; }
            set
            {
                _businessPhone = value;
                OnPropertyChanged("BusinessPhone");
            }
        }

        public string MobilePhone
        {
            get { return _mobilePhone; }
            set
            {
                _mobilePhone = value;
                OnPropertyChanged("MobilePhone");
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        #endregion Properties

        #region --- CRUD ---

        private const string TABLE_NAME = "contacts";

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();

                ModelController.AddParameter(sqlParameters, "?MEM_CODE", MemberCode);
                ModelController.AddParameter(sqlParameters, "?MEM_NAME", MemberName);
                ModelController.AddParameter(sqlParameters, "?FIRST_NAME", FirstName);
                ModelController.AddParameter(sqlParameters, "?MIDDLE_NAME", MiddleName);
                ModelController.AddParameter(sqlParameters, "?LAST_NAME", LastName);
                ModelController.AddParameter(sqlParameters, "?PICTURE", Picture);
                ModelController.AddParameter(sqlParameters, "?SIGNATURE", Signature);
                ModelController.AddParameter(sqlParameters, "?ACCOUNT_TYPE", AccountType);
                ModelController.AddParameter(sqlParameters, "?TELEPHONE", Telephone);
                ModelController.AddParameter(sqlParameters, "?MOBILE_PHONE", MobilePhone);
                ModelController.AddParameter(sqlParameters, "?BUSINESS_PHONE", BusinessPhone);
                ModelController.AddParameter(sqlParameters, "?EMAIL", Email);

                return sqlParameters;
            }
        }

        private SqlParameter ParamKey
        {
            get { return new SqlParameter("?ID", ID); }
        }

        public Result Create()
        {
            Action createRecord = () =>
                {
                    var sqlParameter = Parameters;

                    var sql = DatabaseController.GenerateInsertStatement(TABLE_NAME, sqlParameter);
                    ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
                };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
                {
                    var key = ParamKey;

                    var sql = DatabaseController.GenerateDeleteStatement(TABLE_NAME, key);

                    DatabaseController.ExecuteNonQuery(sql, key);
                };

            return ActionController.InvokeAction(deleteRecord);
        }

        public Result Find(int id)
        {
            Action findRecord = () =>
                {
                    ResetProperties();
                    ID = id;

                    var key = ParamKey;
                    var sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

                    DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        SetPropertiesFromDataRow(dataRow);
                    }
                };

            return ActionController.InvokeAction(findRecord);
        }

        public void ResetProperties()
        {
            ID = 0;
            MemberCode = string.Empty;
            MemberName = string.Empty;
            AccountType = string.Empty;

            FirstName = string.Empty;
            MiddleName = string.Empty;
            LastName = string.Empty;
            Suffix = string.Empty;

            Picture = null;
            Signature = null;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            MemberCode = DataConverter.ToString(dataRow["MEM_CODE"]);
            MemberName = DataConverter.ToString(dataRow["MEM_NAME"]);
            FirstName = DataConverter.ToString(dataRow["FIRST_NAME"]);
            LastName = DataConverter.ToString(dataRow["LAST_NAME"]);
            MiddleName = DataConverter.ToString(dataRow["MIDDLE_NAME"]);
            Suffix = DataConverter.ToString(dataRow["SUFFIX"]);
            AccountType = DataConverter.ToString(dataRow["ACCOUNT_TYPE"]);
            Picture = DataConverter.ToByteArray(dataRow["PICTURE"]);
            Signature = DataConverter.ToByteArray(dataRow["SIGNATURE"]);

            Telephone = DataConverter.ToString(dataRow["TELEPHONE"]);
            MobilePhone = DataConverter.ToString(dataRow["MOBILE_PHONE"]);
            BusinessPhone = DataConverter.ToString(dataRow["BUSINESS_PHONE"]);
            Email = DataConverter.ToString(dataRow["EMAIL"]);
        }

        public Result Update()
        {
            Action updateRecord = () =>
                {
                    var key = ParamKey;

                    var sqlParameter = Parameters;
                    sqlParameter.Add(key);

                    var sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME, sqlParameter,
                                                                         key);

                    DatabaseController.ExecuteNonQuery(sql, sqlParameter.ToArray());
                };

            return ActionController.InvokeAction(updateRecord);
        }

        #endregion --- CRUD ---

        //public static byte[] GetImagesByMemberCode(string memberCode)
        //{
        //    var sql = string.Format("SELECT * FROM `{0}` WHERE MemberCode = {1}", TableName, memberCode);
        //    var dataTable = DatabaseController.ExecuteSelectQuery(sql);
        //    var specimen = new Biometrics();
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        specimen.SetPropertiesFromDataRow(dataRow);
        //    }
        //    return specimen.Picture;
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        internal static Contact WhereMemberCodeIs(string memberCode)
        {
            var sqlBuilder = new System.Text.StringBuilder();
            sqlBuilder.AppendFormat("SELECT * FROM {0} WHERE MEM_CODE = ?MEM_CODE LIMIT 1", TABLE_NAME);

            var param = new SqlParameter("?MEM_CODE", memberCode);
            var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), param);

            var item = new Contact();
            item.MemberCode = memberCode;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                item.SetPropertiesFromDataRow(dataRow);
            }
            return item;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}