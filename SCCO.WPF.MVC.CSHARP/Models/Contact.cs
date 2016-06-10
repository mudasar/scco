using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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

        private static string _imagesFolder;

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
                //ModelController.AddParameter(sqlParameters, "?PICTURE", Picture);
                //ModelController.AddParameter(sqlParameters, "?SIGNATURE", Signature);
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
            //Picture = DataConverter.ToByteArray(dataRow["PICTURE"]);
            //Signature = DataConverter.ToByteArray(dataRow["SIGNATURE"]);

            Telephone = DataConverter.ToString(dataRow["TELEPHONE"]);
            MobilePhone = DataConverter.ToString(dataRow["MOBILE_PHONE"]);
            BusinessPhone = DataConverter.ToString(dataRow["BUSINESS_PHONE"]);
            Email = DataConverter.ToString(dataRow["EMAIL"]);

            LoadPicture();
            LoadSignature();
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

        public event PropertyChangedEventHandler PropertyChanged;

        internal static Contact FindByMemberCode(string memberCode)
        {
            var sqlBuilder = new System.Text.StringBuilder();
            sqlBuilder.AppendFormat("SELECT * FROM {0} WHERE MEM_CODE = ?MEM_CODE LIMIT 1", TABLE_NAME);

            var param = new SqlParameter("?MEM_CODE", memberCode);
            var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), param);

            var item = new Contact {MemberCode = memberCode};
            foreach (DataRow dataRow in dataTable.Rows)
            {
                item.SetPropertiesFromDataRow(dataRow);
            }
            return item;
        }

        internal static byte[] FindPictureByMemberCode(string memberCode)
        {
            var sqlBuilder = new System.Text.StringBuilder();
            sqlBuilder.AppendFormat("SELECT PICTURE FROM {0} WHERE MEM_CODE = ?MEM_CODE LIMIT 1", TABLE_NAME);

            var param = new SqlParameter("?MEM_CODE", memberCode);
            var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), param);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                return DataConverter.ToByteArray(dataRow["PICTURE"]);
            }
            return new byte[0];
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private byte[] GetPictureBytesFromImagesFolder()
        {
            var image = new byte[0];
            var imagesFolder = GetImagesFolder();

            if (!Directory.Exists(imagesFolder))
            {
                return image;
            }
            var pictureFile = PictureFileName();
            if (File.Exists(pictureFile))
            {
                var imageBytes = ImageTool.GetBytesFromImageFile(pictureFile);
                var decryptedBytes = RijndaelHelper.DecryptBytes(imageBytes, "SensitivePhrase", "SodiumChloride");
                image = decryptedBytes;
            }
            return image;
        }

        private byte[] GetSignatureBytesFromImagesFolder()
        {
            var image = new byte[0];
            var imagesFolder = GetImagesFolder();

            if (!Directory.Exists(imagesFolder))
            {
                return image;
            }
            var signatureFile = SignatureFileName();
            if (File.Exists(signatureFile))
            {
                var imageBytes = ImageTool.GetBytesFromImageFile(signatureFile);
                var decryptedBytes = RijndaelHelper.DecryptBytes(imageBytes, "SensitivePhrase", "SodiumChloride");
                image = decryptedBytes;
            }
            return image;
        }

        internal void SavePicture(string imageFile)
        {
            var imagesFolder = CreateImagesFolder();
            if (!string.IsNullOrEmpty(imagesFolder))
            {
                var imageBytes = ImageTool.GetBytesFromImageFile(imageFile);
                var encryptedBytes = RijndaelHelper.EncryptBytes(imageBytes, "SensitivePhrase", "SodiumChloride");
                File.WriteAllBytes(PictureFileName(), encryptedBytes);
            }
        }

        internal void SaveSignature(string imageFile)
        {
            var imagesFolder = CreateImagesFolder();
            if (!string.IsNullOrEmpty(imagesFolder))
            {
                var imageBytes = ImageTool.GetBytesFromImageFile(imageFile);
                var encryptedBytes = RijndaelHelper.EncryptBytes(imageBytes, "SensitivePhrase", "SodiumChloride");
                File.WriteAllBytes(SignatureFileName(), encryptedBytes);
            }
        }

        internal static string GetImagesFolder()
        {
            if (string.IsNullOrEmpty(_imagesFolder))
            {
                var networkPath = Path.Combine(Properties.Settings.Default.DatabaseServer, GlobalSettings.SharedFolder);
                _imagesFolder = @"\\" + Path.Combine(networkPath, "CONTACTS", "IMAGES");

            }
            return _imagesFolder;
        }

        internal static void RefreshImagesFolder()
        {
            var networkPath = Path.Combine(Properties.Settings.Default.DatabaseServer, GlobalSettings.SharedFolder);
            _imagesFolder = @"\\" + Path.Combine(networkPath, "CONTACTS", "IMAGES");
        }

        private bool CreateFolder(string directory)
        {
            if (Directory.Exists(directory))
            {
                return true;
            }
            try
            {
                Directory.CreateDirectory(directory);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string CreateImagesFolder()
        {
            var networkPath = Path.Combine(Properties.Settings.Default.DatabaseServer, GlobalSettings.SharedFolder);
            if (string.IsNullOrEmpty(networkPath))
            {
                return "";
            }

            networkPath = @"\\" + networkPath;

            var contactsFolder = Path.Combine(networkPath, "CONTACTS");
            if (!CreateFolder(contactsFolder))
            {
                return "";
            }

            var signaturesFolder = Path.Combine(contactsFolder, "IMAGES");
            if (!CreateFolder(signaturesFolder))
            {
                return "";
            }
            return Directory.Exists(signaturesFolder) ? signaturesFolder : "";
        }

        private string PictureFileName()
        {
            return Path.ChangeExtension(Path.Combine(GetImagesFolder(), MemberCode), ".pict");
        }

        private string SignatureFileName()
        {
            return Path.ChangeExtension(Path.Combine(GetImagesFolder(), MemberCode), ".sign");
        }

        private void LoadPicture()
        {
            // load if image file exist
            if (File.Exists(PictureFileName()))
            {
                Picture = GetPictureBytesFromImagesFolder();
                return;
            }

            // else check if image is in database
            var sql = "SELECT PICTURE FROM contacts WHERE MEM_CODE = ?MemberCode";
            var parameter = new SqlParameter("?MemberCode", MemberCode);
            var dataTable = DatabaseController.ExecuteSelectQuery(sql, parameter);
            if (dataTable.Rows.Count > 0)
            {
                var imageBytes = DataConverter.ToByteArray(dataTable.Rows[0]["PICTURE"]);
                if (imageBytes.Length <= 0)
                {
                    return;
                }

                Picture = imageBytes;

                // then save to image folder
                var imagesFolder = CreateImagesFolder();

                if (string.IsNullOrEmpty(imagesFolder))
                {
                    return;
                }

                if (!Directory.Exists(imagesFolder))
                {
                    return;
                }

                var encryptedBytes = RijndaelHelper.EncryptBytes(imageBytes, "SensitivePhrase", "SodiumChloride");
                File.WriteAllBytes(PictureFileName(), encryptedBytes);

                // then remove blob
                sql = "UPDATE contacts SET PICTURE = ?Picture WHERE MEM_CODE = ?MemberCode";
                var parameters = new List<SqlParameter> {new SqlParameter("?Picture", null), parameter};

                DatabaseController.ExecuteNonQuery(sql, parameters.ToArray());
            }
        }

        private void LoadSignature()
        {
            // load if image file exist
            if (File.Exists(SignatureFileName()))
            {
                Signature = GetSignatureBytesFromImagesFolder();
                return;
            }

            // else check if image is in database
            var sql = "SELECT SIGNATURE FROM contacts WHERE MEM_CODE = ?MemberCode";
            var parameter = new SqlParameter("?MemberCode", MemberCode);
            var dataTable = DatabaseController.ExecuteSelectQuery(sql, parameter);
            if (dataTable.Rows.Count > 0)
            {
                var imageBytes = DataConverter.ToByteArray(dataTable.Rows[0]["SIGNATURE"]);
                if (imageBytes.Length <= 0)
                {
                    return;
                }

                Signature = imageBytes;

                // then save to image folder
                var imagesFolder = CreateImagesFolder();

                if (string.IsNullOrEmpty(imagesFolder))
                {
                    return;
                }

                if (!Directory.Exists(imagesFolder))
                {
                    return;
                }

                var encryptedBytes = RijndaelHelper.EncryptBytes(imageBytes, "SensitivePhrase", "SodiumChloride");
                File.WriteAllBytes(SignatureFileName(), encryptedBytes);

                // then remove blob
                sql = "UPDATE contacts SET SIGNATURE = ?Signature WHERE MEM_CODE = ?MemberCode";
                var parameters = new List<SqlParameter> {new SqlParameter("?Signature", null), parameter};

                DatabaseController.ExecuteNonQuery(sql, parameters.ToArray());
            }
        }
    }
}