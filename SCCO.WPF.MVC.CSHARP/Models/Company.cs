using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Data;
using System.Windows.Media.Imaging;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;


namespace SCCO.WPF.MVC.CS.Models
{
    public class Company : INotifyPropertyChanged, IModel
    {
        private const string COMPANY_NAME = "Sta. Cruz Savings and Credit Cooperative";
        private const string TABLE_NAME = "company";
        private string _address;
        private BitmapImage _bitmapImage;
        private byte[] _companyIcon;
        private string _companyName;
        private string _contactNo;
        private string _description1;
        private string _description2;
        private int _id;
        private string _tin;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged("Address"); }
        }

        public BitmapImage BitmapImage
        {
            get { return _bitmapImage; }
            set { _bitmapImage = value; OnPropertyChanged("BitmapImage"); }
        }

        public string CompanyCode { get; set; }

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; OnPropertyChanged("CompanyName"); }
        }

        public string ContactNo
        {
            get { return _contactNo; }
            set { _contactNo = value; OnPropertyChanged("ContactNo"); }
        }

        public string Description1
        {
            get { return _description1; }
            set { _description1 = value; OnPropertyChanged("CompanyIcon"); }
        }

        public string Description2
        {
            get { return _description2; }
            set { _description2 = value; OnPropertyChanged("Description2"); }
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        public byte[] Image
        {
            get { return _companyIcon; }
            set
            {
                _companyIcon = value; OnPropertyChanged("Image"); if (Image == null)
                {
                    BitmapImage = null;
                    return;
                }
                try
                {
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = new MemoryStream(Image);
                    imageSource.EndInit();
                    BitmapImage = imageSource;
                }
                catch (Exception exception)
                {
                    Logger.ExceptionLogger(new ImageTool(), exception);
                }
            }
        }

        public string Tin
        {
            get { return _tin; }
            set { _tin = value; OnPropertyChanged("Tin"); }
        }

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?CompanyName", CompanyName);
                ModelController.AddParameter(sqlParameters, "?Address", Address);
                ModelController.AddParameter(sqlParameters, "?Tin", Tin);
                ModelController.AddParameter(sqlParameters, "?ContactNo", ContactNo);
                ModelController.AddParameter(sqlParameters, "?Image", Image);
                ModelController.AddParameter(sqlParameters, "?Description1", Description1);
                ModelController.AddParameter(sqlParameters, "?Description2", Description2);
                ModelController.AddParameter(sqlParameters, "?CompanyCode", CompanyCode);
                return sqlParameters;
            }
        }

        private SqlParameter ParamKey
        {
            get { return new SqlParameter("?ID", ID); }
        }

        public static DataTable GetData()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendLine("SELECT * FROM");
            queryBuilder.AppendLine(TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(queryBuilder);
            dataTable.TableName = TABLE_NAME;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                dataRow["CompanyName"] = COMPANY_NAME;    
            }
            
            return dataTable;
        }

        public static DataTable GetList(string companyCode)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendLine("SELECT * FROM");
            queryBuilder.AppendLine(TABLE_NAME);
            queryBuilder.AppendLine("WHERE CompanyCode = ?CompanyCode");

            var param = new SqlParameter("?CompanyCode", companyCode);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(queryBuilder, param);
            return dataTable;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Implementation of IModel

        public Result Create()
        {
            Action createRecord = () =>
            {
                List<SqlParameter> sqlParameter = Parameters;

                string sql = DatabaseController.GenerateInsertStatement(TABLE_NAME, sqlParameter);
                ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
            };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
            {
                SqlParameter key = ParamKey;

                string sql = DatabaseController.GenerateDeleteStatement(TABLE_NAME, key);

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

                SqlParameter key = ParamKey;
                string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

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
            CompanyName = COMPANY_NAME;
            Address = "";
            Tin = "";
            ContactNo = "";
            Image = null;
            Description1 = "";
            Description2 = "";
            CompanyCode = "";
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            CompanyName = COMPANY_NAME;
            Address = DataConverter.ToString(dataRow["Address"]);
            Tin = DataConverter.ToString(dataRow["Tin"]);
            ContactNo = DataConverter.ToString(dataRow["ContactNo"]);
            Image = DataConverter.ToByteArray(dataRow["Image"]);
            Description1 = DataConverter.ToString(dataRow["Description1"]);
            Description2 = DataConverter.ToString(dataRow["Description2"]);
            CompanyCode = DataConverter.ToString(dataRow["CompanyCode"]);
        }

        public Result Update()
        {
            Action updateRecord = () =>
            {
                SqlParameter key = ParamKey;

                List<SqlParameter> sqlParameter = Parameters;
                sqlParameter.Add(key);

                string sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME, sqlParameter, key);

                DatabaseController.ExecuteNonQuery(sql, sqlParameter.ToArray());
            };

            return ActionController.InvokeAction(updateRecord);
        }

        #endregion

        public static Company FirstOrDefault()
        {
            var dataTable = GetData();
            var cooperative = new Company {CompanyName = COMPANY_NAME};
            foreach (DataRow dataRow in dataTable.Rows)
            {
                cooperative.SetPropertiesFromDataRow(dataRow);
            }
            return cooperative;
        }
    }
}
