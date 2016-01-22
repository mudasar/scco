using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Media.Imaging;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class ProductImage : INotifyPropertyChanged , IModel
    {
        private int _id;
        private string _productCode;
        private byte[] _image;
        private string _title;
        private string _description;
        private BitmapImage _bitmapImage;


        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        public string ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; OnPropertyChanged("ProductCode"); }
        }


        public byte[] Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged("Image");

                BitmapImage = ImageTool.CreateImageSourceFromBytes(Image);
            }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title");}
        }

        
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged("Description"); }
        }

        public BitmapImage BitmapImage
        {
            get { return _bitmapImage; }
            set { _bitmapImage = value; OnPropertyChanged("BitmapImage"); }
        }


        private const string TABLE_NAME = "image_products";
        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?ProductCode", ProductCode);
                ModelController.AddParameter(sqlParameters, "?Image", Image);
                return sqlParameters;
            }
        }

        private SqlParameter ParamKey
        {
            get { return new SqlParameter("?ID", ID); }
        }
        
        
        public event PropertyChangedEventHandler PropertyChanged;
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
            ProductCode = "";
            Image = new[] {new byte()};
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            ProductCode = DataConverter.ToString(dataRow["ProductCode"]);
            Image = DataConverter.ToByteArray(dataRow["Image"]);

            var account = Account.FindByCode(ProductCode);

            Title = account.AccountCode;
            Description = account.AccountTitle;
        }

        #endregion

        internal static ProductImage WhereProductCodeIs(string productCode)
        {
            string sqlCommandText = string.Format("SELECT * FROM {0} WHERE ProductCode = ?ProductCode", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText,
                                                                        new SqlParameter("?ProductCode",productCode));

            if (dataTable.Rows.Count == 0) return null;

            var item = new ProductImage();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                item = new ProductImage();
                item.SetPropertiesFromDataRow(dataRow);
            }
            return item;
        }

        internal static ProductImageCollection CollectAll()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT * FROM " + TABLE_NAME);
            var collection = new ProductImageCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new ProductImage();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }
    }

    public class ProductImageCollection : ObservableCollection<ProductImage>
    {
    }
}
