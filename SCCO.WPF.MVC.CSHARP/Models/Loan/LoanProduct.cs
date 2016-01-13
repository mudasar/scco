using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;
using System.Windows.Media.Imaging;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    public class LoanProduct : IEquatable<LoanProduct>, INotifyPropertyChanged, IModel
    {
        private string _productCode;
        private decimal _annualInterestRate;
        private string _name;
        private List<LoanCharge> _loanCharges;
        private int _id;
        private int _minimumTerm;
        private string _loanType;
        private string _modeOfPayment;
        private decimal _minimumLoanableAmount;
        private decimal _maximumLoanableAmount;
        private string _description;

        private BitmapImage _bitmapImage;
        private int _maximumTerm;
        private decimal _monthlyCapitalBuildUp;

        #region --PROPERTIES--

        public string ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; OnPropertyChanged("ProductCode"); }
        }

        public decimal AnnualInterestRate
        {
            get { return _annualInterestRate; }
            set { _annualInterestRate = value; OnPropertyChanged("AnnualInterestRate"); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public List<LoanCharge> LoanCharges
        {
            get { return _loanCharges; }
            set { _loanCharges = value; OnPropertyChanged("LoanCharges"); }
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        public int MinimumTerm
        {
            get { return _minimumTerm; }
            set { _minimumTerm = value; OnPropertyChanged("MinimumTerm"); }
        }

        public int MaximumTerm
        {
            get { return _maximumTerm; }
            set { _maximumTerm = value; OnPropertyChanged("MaximumTerm"); }
        }

        public string LoanType
        {
            get { return _loanType; }
            set { _loanType = value; OnPropertyChanged("LoanType"); }
        }

        public string ModeOfPayment
        {
            get { return _modeOfPayment; }
            set { _modeOfPayment = value; OnPropertyChanged("ModeOfPayment"); }
        }

        public decimal MinimumLoanableAmount
        {
            get { return _minimumLoanableAmount; }
            set { _minimumLoanableAmount = value; OnPropertyChanged("MinimumLoanableAmount"); }
        }

        public decimal MaximumLoanableAmount
        {
            get { return _maximumLoanableAmount; }
            set { _maximumLoanableAmount = value; OnPropertyChanged("MaximumLoanableAmount"); }
        }


        public decimal MonthlyCapitalBuildUp
        {
            get { return _monthlyCapitalBuildUp; }
            set { _monthlyCapitalBuildUp = value; OnPropertyChanged("MonthlyCapitalBuildUp"); }
        }

        #region --- List Item Properties ----

        public BitmapImage BitmapImage
        {
            get { return _bitmapImage; }
            set
            {
                _bitmapImage = value;
                OnPropertyChanged("BitmapImage");
            }
        }

        public string Title
        {
            get { return Name; }
            set { Name = value; }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        #endregion





        #endregion --PROPERTIES--

        #region --- CRUD ---

        private const string TABLE_NAME = "loan_products";


        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?Name", Name);
                ModelController.AddParameter(sqlParameters, "?ProductCode", ProductCode);
                ModelController.AddParameter(sqlParameters, "?LoanType", LoanType);
                ModelController.AddParameter(sqlParameters, "?ModeOfPayment", ModeOfPayment);
                
                // Do not nullify these fields...
                sqlParameters.Add(new SqlParameter("?AnnualInterestRate", AnnualInterestRate));
                sqlParameters.Add(new SqlParameter("?MinimumTerm", MinimumTerm));
                sqlParameters.Add(new SqlParameter("?MaximumTerm", MaximumTerm));
                sqlParameters.Add(new SqlParameter("?MinimumLoanableAmount", MinimumLoanableAmount));
                sqlParameters.Add(new SqlParameter("?MaximumLoanableAmount", MaximumLoanableAmount));
                sqlParameters.Add(new SqlParameter("?MonthlyCapitalBuildUp", MonthlyCapitalBuildUp));

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
            Name = "";
            LoanType = string.Empty;
            MinimumTerm = 0;
            ModeOfPayment = string.Empty;
            AnnualInterestRate = 0m;
            LoanCharges = new List<LoanCharge>();
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = (int)dataRow["ID"];
            Name = (string)dataRow["Name"];
            LoanType = (string)dataRow["LoanType"];
            ModeOfPayment = (string)dataRow["ModeOfPayment"];
            AnnualInterestRate = (decimal)dataRow["AnnualInterestRate"];
            ProductCode = DataConverter.ToString(dataRow["ProductCode"]);
            MinimumTerm = DataConverter.ToInteger(dataRow["MinimumTerm"]);
            MaximumTerm = DataConverter.ToInteger(dataRow["MaximumTerm"]);
            MinimumLoanableAmount = DataConverter.ToDecimal(dataRow["MinimumLoanableAmount"]);
            MaximumLoanableAmount = DataConverter.ToDecimal(dataRow["MaximumLoanableAmount"]);
            MonthlyCapitalBuildUp = DataConverter.ToDecimal(dataRow["MonthlyCapitalBuildUp"]);
            
            LoanCharges = LoanCharge.GetListByLoanProductId(ID);

            Description = string.Format("Loanable amount of P{0:N} - P{1:N}", MinimumLoanableAmount,
                                        MaximumLoanableAmount);

            var productImage = ProductImage.WhereProductCodeIs(ProductCode);
            if (null == productImage) return;

            BitmapImage = productImage.BitmapImage;
        }

        public Result Update()
        {
            Action updateRecord = () =>
                                      {
                                          SqlParameter key = ParamKey;

                                          List<SqlParameter> sqlParameter = Parameters;
                                          sqlParameter.Add(key);

                                          string sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME,
                                                                                                  sqlParameter,
                                                                                                  key);

                                          DatabaseController.ExecuteNonQuery(sql, sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(updateRecord);
        }

        #endregion --- CRUD ---

        public event PropertyChangedEventHandler PropertyChanged;

        public static List<LoanProduct> GetListByLoanAccount(Account loanAccount)
        {
            string sqlCommandText = string.Format("SELECT * FROM {0} WHERE AccountCode = ?AccountCode", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText,
                                                                        new SqlParameter("?AccountCode",
                                                                                         loanAccount.AccountCode));
            var result = new List<LoanProduct>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var lp = new LoanProduct();
                lp.SetPropertiesFromDataRow(dataRow);
                result.Add(lp);
            }
            return result;
        }

        internal static List<LoanProduct> GetList()
        {
            string sqlCommandText = string.Format("SELECT * FROM {0}", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText); 
            var result = new List<LoanProduct>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var lp = new LoanProduct(); 
                lp.SetPropertiesFromDataRow(dataRow);
                result.Add(lp);
            }
            return result;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public static LoanProduct WhereTitleIs(string title)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM");
            sqlBuilder.AppendLine(TABLE_NAME);
            sqlBuilder.AppendLine("WHERE Name = ?Name LIMIT 1");

            var sqlParam = new SqlParameter("?Name", title);

            var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, sqlParam);

            if (dataTable.Rows.Count == 0) return null;

            var loanProduct = new LoanProduct();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                loanProduct.SetPropertiesFromDataRow(dataRow);
            }
            return loanProduct;
        }

        public Result ValidateProperties()
        {
            if (string.IsNullOrEmpty(Name)) return new Result(false, "Product name must not be empty.");

            if(string.IsNullOrEmpty(ProductCode)) return new Result(false, "No Product Code specified.");

            if(string.IsNullOrEmpty(LoanType)) return new Result(false,"No Loan Type specified.");
            
            if(string.IsNullOrEmpty(ModeOfPayment)) return new Result(false, "No Mode of Payment specified.");
            
            if(AnnualInterestRate < 0) return new Result(false,"Invalid Interest Rate.");
            
            if(MinimumTerm <= 0) return new Result(false,"Invalid Minimum Term.");

            if(MinimumTerm > MaximumTerm) return new Result(false,"Minimum Term must not be greater than Maximum Term.");
            
            if(MaximumTerm <= 0) return new Result(false,"Invalid Maximum Term.");

            if(MaximumTerm < MinimumTerm) return new Result(false, "Maximum Term must not be less than Minimum Term.");

            if (MinimumLoanableAmount <= 0) return new Result(false, "Invalid Minimum Loanable Amount.");

            if (MinimumLoanableAmount > MaximumLoanableAmount) return new Result(false, "Minimum Loanable Amount must not be greater than Maximum Loanable Amount.");

            if (MaximumLoanableAmount <= 0) return new Result(false, "Invalid Maximum Loanable Amount.");

            if (MaximumLoanableAmount < MinimumLoanableAmount) return new Result(false, "Maximum Loanable Amount must not be less than Minimum Loanable Amount.");

            return new Result(true,"All properties are valid.");
        }

        #region Implementation of IEquatable<LoanProduct>

        public bool Equals(LoanProduct other)
        {
            Type thisType = GetType();
            PropertyInfo[] thisProperties = thisType.GetProperties();

            var countProperties = thisProperties.Length;
            Type otherType = other.GetType();
            PropertyInfo[] otherProperties = otherType.GetProperties();

            for (int i = 0; i < countProperties; i++)
            {
                if (thisProperties[i].Name != otherProperties[i].Name)
                    return false;
            }

            return true;
        }

        #endregion

        internal static LoanProductCollection CollectAll()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT * FROM " + TABLE_NAME + " ORDER BY `Name`");
            var collection = new LoanProductCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new LoanProduct();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }

        internal static LoanProduct FindBy(string column, string value)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendLine("SELECT * FROM");
            queryBuilder.AppendLine(TABLE_NAME);
            queryBuilder.AppendFormat("WHERE `{0}` = ?{0} LIMIT 1", column);
            var param = new SqlParameter(string.Format("?{0}", column), value);
            var dataTable = DatabaseController.ExecuteSelectQuery(queryBuilder, param);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new LoanProduct();
                item.SetPropertiesFromDataRow(dataRow);
                return item;
            }
            return null;
        }
    }

    public class LoanProductCollection : System.Collections.ObjectModel.ObservableCollection<LoanProduct>
    {
    }
}