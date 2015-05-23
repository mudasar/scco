using System;
using System.Collections.Generic;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    public class LoanComputation : ModelBase, IModel
    {
        private decimal _chargeAmount1;
        private decimal _chargeAmount2;
        private decimal _chargeAmount3;
        private decimal _chargeAmount4;
        private decimal _chargeAmount5;
        private decimal _chargeAmount6;
        private decimal _chargeAmountTotal;
        private string _chargeCode1;
        private string _chargeCode2;
        private string _chargeCode3;
        private string _chargeCode4;
        private string _chargeCode5;
        private string _chargeCode6;
        private string _chargeTitle1;
        private string _chargeTitle2;
        private string _chargeTitle3;
        private string _chargeTitle4;
        private string _chargeTitle5;
        private string _chargeTitle6;
        private decimal _deductAmount1;
        private decimal _deductAmount2;
        private decimal _deductAmount3;
        private decimal _deductAmount4;
        private decimal _deductAmount5;
        private decimal _deductAmount6;
        private decimal _deductAmountTotal;
        private string _deductCode1;
        private string _deductCode2;
        private string _deductCode3;
        private string _deductCode4;
        private string _deductCode5;
        private string _deductCode6;
        private string _deductTitle1;
        private string _deductTitle2;
        private string _deductTitle3;
        private string _deductTitle4;
        private string _deductTitle5;
        private string _deductTitle6;
        private decimal _netProceedsAmount;
        private string _netProceedsCode;
        private string _netProceedsTitle;
        private decimal _loanAmount;
        private string _loanDescription;
        private LoanDetails _loanDetails;

        #region --- CONSTRUCTORS ---

        public LoanComputation():base("loan_computation")
        {
            LoanDetails = new LoanDetails();
        }

        public LoanComputation(decimal loanAmount, int loanProductId):this()
        {
            LoanAmount = loanAmount;

            var loanProduct = new LoanProduct();
            loanProduct.Find(loanProductId);


            var loanApplied = Account.FindByCode(loanProduct.ProductCode);
            LoanDescription = string.Format("{0} - {1}", loanApplied.AccountCode, loanApplied.AccountTitle);

            LoanDetails.AccountCode = loanApplied.AccountCode;
            

            #region --- Populate Charges ---

            var loanCharges = LoanCharge.GetListByLoanProductId(loanProduct.ID);
            int i = 0;
            foreach (var loanCharge in loanCharges)
            {
                i++;
                if (i == 1)
                {
                    ChargeCode1 = loanCharge.AccountCode;
                    ChargeTitle1 = loanCharge.AccountTitle;
                    ChargeAmount1 = loanCharge.Rate * LoanAmount;
                    continue;
                }
                if (i == 2)
                {
                    ChargeCode2 = loanCharge.AccountCode;
                    ChargeTitle2 = loanCharge.AccountTitle;
                    ChargeAmount2 = loanCharge.Rate * LoanAmount;
                    continue;
                }
                if (i == 3)
                {
                    ChargeCode3 = loanCharge.AccountCode;
                    ChargeTitle3 = loanCharge.AccountTitle;
                    ChargeAmount3 = loanCharge.Rate * LoanAmount;
                    continue;
                }
                if (i == 4)
                {
                    ChargeCode4 = loanCharge.AccountCode;
                    ChargeTitle4 = loanCharge.AccountTitle;
                    ChargeAmount4 = loanCharge.Rate * LoanAmount;
                    continue;
                }
                if (i == 5)
                {
                    ChargeCode5 = loanCharge.AccountCode;
                    ChargeTitle5 = loanCharge.AccountTitle;
                    ChargeAmount5 = loanCharge.Rate * LoanAmount;
                    continue;
                }
                if (i == 6)
                {
                    ChargeCode6 = loanCharge.AccountCode;
                    ChargeTitle6 = loanCharge.AccountTitle;
                    ChargeAmount6 = loanCharge.Rate * LoanAmount;
                    break;
                }
            }

            #endregion

            #region --- Populate Deductions ---

            var loanDeductions = LoanDeduction.GetListByLoanProductId(loanProduct.ID);
            i = 0;
            foreach (var loanDeduct in loanDeductions)
            {
                i++;
                if (i == 1)
                {
                    DeductCode1 = loanDeduct.AccountCode;
                    DeductTitle1 = loanDeduct.AccountTitle;
                    DeductAmount1 = loanDeduct.Amount;
                    continue;
                }
                if (i == 2)
                {
                    DeductCode2 = loanDeduct.AccountCode;
                    DeductTitle2 = loanDeduct.AccountTitle;
                    DeductAmount2 = loanDeduct.Amount;
                    continue;
                }
                if (i == 3)
                {
                    DeductCode3 = loanDeduct.AccountCode;
                    DeductTitle3 = loanDeduct.AccountTitle;
                    DeductAmount3 = loanDeduct.Amount;
                    continue;
                }
                if (i == 4)
                {
                    DeductCode4 = loanDeduct.AccountCode;
                    DeductTitle4 = loanDeduct.AccountTitle;
                    DeductAmount4 = loanDeduct.Amount;
                    continue;
                }
                if (i == 5)
                {
                    DeductCode5 = loanDeduct.AccountCode;
                    DeductTitle5 = loanDeduct.AccountTitle;
                    DeductAmount5 = loanDeduct.Amount;
                    continue;
                }
                if (i == 6)
                {
                    DeductCode6 = loanDeduct.AccountCode;
                    DeductTitle6 = loanDeduct.AccountTitle;
                    DeductAmount6 = loanDeduct.Amount;
                    break;
                }
            }

            #endregion

            #region --- INITIALIZE DEFAULT NET PROCEEDS ---

            var netProceedsAccount = Account.FindByCode(GlobalSettings.CodeOfCashOnHand);
            NetProceedsCode = netProceedsAccount.AccountCode;
            //NetProceedsTitle = netProceedsAccount.AccountTitle;

            #endregion --- INITIALIZE DEFAULT NET PROCEEDS ---
        }

        #endregion --- CONSTRUCTORS ---

        #region --- PROPERTIES ---

        public decimal LoanAmount
        {
            get { return _loanAmount; }
            set { _loanAmount = value; OnPropertyChanged("LoanAmount"); }
        }

        public string LoanDescription
        {
            get { return _loanDescription; }
            set { _loanDescription = value; OnPropertyChanged("LoanDescription"); }
        }

        public LoanDetails LoanDetails
        {
            get { return _loanDetails; }
            set { _loanDetails = value; OnPropertyChanged("LoanDetails"); }
        }

        #region --- CHARGES ---

        public decimal ChargeAmount1
        {
            get { return _chargeAmount1; }
            set
            {
                _chargeAmount1 = value;
                OnPropertyChanged("ChargeAmount1");
                CalculateTotalCharges();
            }
        }

        public decimal ChargeAmount2
        {
            get { return _chargeAmount2; }
            set
            {
                _chargeAmount2 = value;
                OnPropertyChanged("ChargeAmount2");
                CalculateTotalCharges();
            }
        }

        public decimal ChargeAmount3
        {
            get { return _chargeAmount3; }
            set
            {
                _chargeAmount3 = value;
                OnPropertyChanged("ChargeAmount3");
                CalculateTotalCharges();
            }
        }

        public decimal ChargeAmount4
        {
            get { return _chargeAmount4; }
            set
            {
                _chargeAmount4 = value;
                OnPropertyChanged("ChargeAmount4");
                CalculateTotalCharges();
            }
        }

        public decimal ChargeAmount5
        {
            get { return _chargeAmount5; }
            set
            {
                _chargeAmount5 = value;
                OnPropertyChanged("ChargeAmount5");
                CalculateTotalCharges();
            }
        }

        public decimal ChargeAmount6
        {
            get { return _chargeAmount6; }
            set
            {
                _chargeAmount6 = value;
                OnPropertyChanged("ChargeAmount6");
                CalculateTotalCharges();
            }
        }

        public decimal ChargeAmountTotal
        {
            get { return _chargeAmountTotal; }
            set
            {
                _chargeAmountTotal = value;
                OnPropertyChanged("ChargeAmountTotal");
                CalculateNetProceeds();
            }
        }

        public string ChargeCode1
        {
            get { return _chargeCode1; }
            set
            {
                _chargeCode1 = value;
                OnPropertyChanged("ChargeCode1");
                if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    ChargeTitle1 = account.AccountTitle;
                }
                else
                {
                    ChargeTitle1 = "";
                }
            }
        }

        public string ChargeCode2
        {
            get { return _chargeCode2; }
            set
            {
                _chargeCode2 = value; OnPropertyChanged("ChargeCode2"); if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    ChargeTitle2 = account.AccountTitle;
                }
                else
                {
                    ChargeTitle2 = "";
                }
            }
        }

        public string ChargeCode3
        {
            get { return _chargeCode3; }
            set
            {
                _chargeCode3 = value; OnPropertyChanged("ChargeCode3"); if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    ChargeTitle3 = account.AccountTitle;
                }
                else
                {
                    ChargeTitle3 = "";
                }
            }
        }

        public string ChargeCode4
        {
            get { return _chargeCode4; }
            set
            {
                _chargeCode4 = value;
                OnPropertyChanged("ChargeCode4");
                if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    ChargeTitle4 = account.AccountTitle;
                }
                else
                {
                    ChargeTitle4 = "";
                }

            }
        }

        public string ChargeCode5
        {
            get { return _chargeCode5; }
            set
            {
                _chargeCode5 = value; OnPropertyChanged("ChargeCode5");
                if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    ChargeTitle5 = account.AccountTitle;
                }
                else
                {
                    ChargeTitle5 = "";
                }
            }
        }

        public string ChargeCode6
        {
            get { return _chargeCode6; }
            set
            {
                _chargeCode6 = value; OnPropertyChanged("ChargeCode6");
                if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    ChargeTitle6 = account.AccountTitle;
                }
                else
                {
                    ChargeTitle6 = "";
                }

            }
        }

        public string ChargeTitle1
        {
            get { return _chargeTitle1; }
            set { _chargeTitle1 = value; OnPropertyChanged("ChargeTitle1"); }
        }

        public string ChargeTitle2
        {
            get { return _chargeTitle2; }
            set { _chargeTitle2 = value; OnPropertyChanged("ChargeTitle2"); }
        }

        public string ChargeTitle3
        {
            get { return _chargeTitle3; }
            set { _chargeTitle3 = value; OnPropertyChanged("ChargeTitle3"); }
        }

        public string ChargeTitle4
        {
            get { return _chargeTitle4; }
            set { _chargeTitle4 = value; OnPropertyChanged("ChargeTitle4"); }
        }

        public string ChargeTitle5
        {
            get { return _chargeTitle5; }
            set { _chargeTitle5 = value; OnPropertyChanged("ChargeTitle5"); }
        }

        public string ChargeTitle6
        {
            get { return _chargeTitle6; }
            set { _chargeTitle6 = value; OnPropertyChanged("ChargeTitle6"); }
        }

        private void CalculateTotalCharges()
        {
            var totalCharges = _chargeAmount1 + _chargeAmount2 + _chargeAmount3 + _chargeAmount4 + _chargeAmount5 +
                               _chargeAmount6;

            ChargeAmountTotal = totalCharges;
        }

        #endregion --- CHARGES ---

        #region ---- DEDUCTIONS ---

        public decimal DeductAmount1
        {
            get { return _deductAmount1; }
            set
            {
                _deductAmount1 = value; OnPropertyChanged("DeductAmount1");
                CalculateTotalDeductions();
            }
        }

        public decimal DeductAmount2
        {
            get { return _deductAmount2; }
            set { _deductAmount2 = value; OnPropertyChanged("DeductAmount2"); CalculateTotalDeductions(); }
        }

        public decimal DeductAmount3
        {
            get { return _deductAmount3; }
            set { _deductAmount3 = value; OnPropertyChanged("DeductAmount3"); CalculateTotalDeductions(); }
        }

        public decimal DeductAmount4
        {
            get { return _deductAmount4; }
            set { _deductAmount4 = value; OnPropertyChanged("DeductAmount4"); CalculateTotalDeductions(); }
        }

        public decimal DeductAmount5
        {
            get { return _deductAmount5; }
            set { _deductAmount5 = value; OnPropertyChanged("DeductAmount5"); CalculateTotalDeductions(); }
        }

        public decimal DeductAmount6
        {
            get { return _deductAmount6; }
            set
            {
                _deductAmount6 = value;
                OnPropertyChanged("DeductAmount6");
                CalculateTotalDeductions();
            }
        }

        public decimal DeductAmountTotal
        {
            get { return _deductAmountTotal; }
            set
            {
                _deductAmountTotal = value;
                OnPropertyChanged("DeductAmountTotal");
                CalculateNetProceeds();
            }

        }

        public string DeductCode1
        {
            get { return _deductCode1; }
            set
            {
                _deductCode1 = value;
                OnPropertyChanged("DeductCode1");
                if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    DeductTitle1 = account.AccountTitle;
                }
                else
                {
                    DeductTitle1 = "";
                }
            }
        }

        public string DeductCode2
        {
            get { return _deductCode2; }
            set
            {
                _deductCode2 = value;
                OnPropertyChanged("DeductCode2");
                if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    DeductTitle2 = account.AccountTitle;
                }
                else
                {
                    DeductTitle2 = "";
                }
            }
        }

        public string DeductCode3
        {
            get { return _deductCode3; }
            set
            {
                _deductCode3 = value;
                OnPropertyChanged("DeductCode3");
                if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    DeductTitle3 = account.AccountTitle;
                }
                else
                {
                    DeductTitle3 = "";
                }
            }
        }

        public string DeductCode4
        {
            get { return _deductCode4; }
            set
            {
                _deductCode4 = value;
                OnPropertyChanged("DeductCode4");
                if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    DeductTitle4 = account.AccountTitle;
                }
                else
                {
                    DeductTitle4 = "";
                }
            }
        }

        public string DeductCode5
        {
            get { return _deductCode5; }
            set
            {
                _deductCode5 = value;
                OnPropertyChanged("DeductCode5");
                if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    DeductTitle5 = account.AccountTitle;
                }
                else
                {
                    DeductTitle5 = "";
                }
            }
        }

        public string DeductCode6
        {
            get { return _deductCode6; }
            set
            {
                _deductCode6 = value;
                OnPropertyChanged("DeductCode6");
                if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    DeductTitle6 = account.AccountTitle;
                }
                else
                {
                    DeductTitle6 = "";
                }
            }
        }

        public string DeductTitle1
        {
            get { return _deductTitle1; }
            set { _deductTitle1 = value; OnPropertyChanged("DeductTitle1");}
        }

        public string DeductTitle2
        {
            get { return _deductTitle2; }
            set { _deductTitle2 = value; OnPropertyChanged("DeductTitle2"); }
        }

        public string DeductTitle3
        {
            get { return _deductTitle3; }
            set { _deductTitle3 = value; OnPropertyChanged("DeductTitle3"); }
        }

        public string DeductTitle4
        {
            get { return _deductTitle4; }
            set { _deductTitle4 = value; OnPropertyChanged("DeductTitle4"); }
        }

        public string DeductTitle5
        {
            get { return _deductTitle5; }
            set { _deductTitle5 = value; OnPropertyChanged("DeductTitle5"); }
        }

        public string DeductTitle6
        {
            get { return _deductTitle6; }
            set { _deductTitle6 = value; OnPropertyChanged("DeductTitle6"); }
        }

        private void CalculateTotalDeductions()
        {
            var totalDeductions = _deductAmount1 + _deductAmount2 + _deductAmount3 + _deductAmount4 + _deductAmount5 +
                                  _deductAmount6;

            DeductAmountTotal = totalDeductions;
        }

        #endregion ---- DEDUCTIONS ---

        #region --- NET PROCEEDS ---

        public decimal NetProceedsAmount
        {
            get { return _netProceedsAmount; }
            set { _netProceedsAmount = value; OnPropertyChanged("NetProceedsAmount"); }
        }

        public string NetProceedsCode
        {
            get { return _netProceedsCode; }
            set
            {
                _netProceedsCode = value;
                OnPropertyChanged("NetProceedsCode");
                if (value.Length >= 3)
                {
                    var account = Account.FindByCode(value);
                    NetProceedsTitle = account.AccountTitle;
                }
                else
                {
                    NetProceedsTitle = "";
                }
            }
        }

        public string NetProceedsTitle
        {
            get { return _netProceedsTitle; }
            set
            {
                _netProceedsTitle = value;
                OnPropertyChanged("NetProceedsTitle");
            }
        }

        private void CalculateNetProceeds()
        {
            NetProceedsAmount = LoanAmount - ChargeAmountTotal - DeductAmountTotal;
        }

        #endregion --- NET PROCEEDS ---

        #endregion --- PROPERTIES ---

        public List<ComputationDetail> Charges
        {
            
            get
            {
                var charges = new List<ComputationDetail>();
                if (ChargeAmount1 != 0)
                    charges.Add(new ComputationDetail(ChargeCode1, ChargeTitle1, ChargeAmount1));
                if (ChargeAmount2 != 0)
                    charges.Add(new ComputationDetail(ChargeCode2, ChargeTitle2, ChargeAmount2));
                if (ChargeAmount3 != 0)
                    charges.Add(new ComputationDetail(ChargeCode3, ChargeTitle3, ChargeAmount3));
                if (ChargeAmount4 != 0)
                    charges.Add(new ComputationDetail(ChargeCode4, ChargeTitle4, ChargeAmount4));
                if (ChargeAmount5 != 0)
                    charges.Add(new ComputationDetail(ChargeCode5, ChargeTitle5, ChargeAmount5));
                if (ChargeAmount6 != 0)
                    charges.Add(new ComputationDetail(ChargeCode6, ChargeTitle6, ChargeAmount6));

                return charges;
            }
        }

        public List<ComputationDetail> Deductions
        {

            get
            {
                var deductions = new List<ComputationDetail>();
                if (DeductAmount1 != 0)
                    deductions.Add(new ComputationDetail(DeductCode1, DeductTitle1, DeductAmount1));
                if (DeductAmount2 != 0)
                    deductions.Add(new ComputationDetail(DeductCode2, DeductTitle2, DeductAmount2));
                if (DeductAmount3 != 0)
                    deductions.Add(new ComputationDetail(DeductCode3, DeductTitle3, DeductAmount3));
                if (DeductAmount4 != 0)
                    deductions.Add(new ComputationDetail(DeductCode4, DeductTitle4, DeductAmount4));
                if (DeductAmount5 != 0)
                    deductions.Add(new ComputationDetail(DeductCode5, DeductTitle5, DeductAmount5));
                if (DeductAmount6 != 0)
                    deductions.Add(new ComputationDetail(DeductCode6, DeductTitle6, DeductAmount6));

                return deductions;
            }
        }

        public DataTable CreateReportData()
        {
            if (ID > 0)
            {
                Update();
            }else
            {
                Create();
            }

            var key = new SqlParameter("?ID", ID);
            var sql = DatabaseController.GenerateSelectStatement(_tableName, key);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);

            dataTable.TableName = _tableName;
            return dataTable;
        }

        #region Implementation of IModel

        #region --- CRUD ---

        public Result Create()
        {
            Action createRecord = () =>
            {
                var sqlParameter = Parameters;
                var sql = DatabaseController.GenerateInsertStatement(_tableName, sqlParameter);

                ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
            };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            Action updateRecord = () =>
            {
                var key = _paramKey;
                var sqlParameters = Parameters;
                var sql = DatabaseController.GenerateUpdateStatement(_tableName, Parameters,
                                                                     _paramKey);
                sqlParameters.Add(key);
                DatabaseController.ExecuteNonQuery(sql, sqlParameters.ToArray());
            };

            return ActionController.InvokeAction(updateRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
            {
                var key = _paramKey;
                var sql = DatabaseController.GenerateDeleteStatement(_tableName, key);
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

                var key = new SqlParameter("?ID", ID);
                var sql = DatabaseController.GenerateSelectStatement(_tableName, key);

                DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SetPropertiesFromDataRow(dataRow);
                }
            };

            return ActionController.InvokeAction(findRecord);
        }

        #endregion

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?id", ID);

                ModelController.AddParameter(sqlParameters, "?charge_code1", ChargeCode1);
                ModelController.AddParameter(sqlParameters, "?charge_title1", ChargeTitle1);
                ModelController.AddParameter(sqlParameters, "?charge_amount1", ChargeAmount1);

                ModelController.AddParameter(sqlParameters, "?charge_code2", ChargeCode2);
                ModelController.AddParameter(sqlParameters, "?charge_title2", ChargeTitle2);
                ModelController.AddParameter(sqlParameters, "?charge_amount2", ChargeAmount2);                

                ModelController.AddParameter(sqlParameters, "?charge_code3", ChargeCode3);
                ModelController.AddParameter(sqlParameters, "?charge_title3", ChargeTitle3);
                ModelController.AddParameter(sqlParameters, "?charge_amount3", ChargeAmount3);

                ModelController.AddParameter(sqlParameters, "?charge_code4", ChargeCode4);
                ModelController.AddParameter(sqlParameters, "?charge_title4", ChargeTitle4);
                ModelController.AddParameter(sqlParameters, "?charge_amount4", ChargeAmount4);
                
                ModelController.AddParameter(sqlParameters, "?charge_code5", ChargeCode5);
                ModelController.AddParameter(sqlParameters, "?charge_title5", ChargeTitle5);
                ModelController.AddParameter(sqlParameters, "?charge_amount5", ChargeAmount5);

                ModelController.AddParameter(sqlParameters, "?charge_code6", ChargeCode6);
                ModelController.AddParameter(sqlParameters, "?charge_title6", ChargeTitle6);
                ModelController.AddParameter(sqlParameters, "?charge_amount6", ChargeAmount6);

                ModelController.AddParameter(sqlParameters, "?deduct_code1", DeductCode1);
                ModelController.AddParameter(sqlParameters, "?deduct_title1", DeductTitle1);
                ModelController.AddParameter(sqlParameters, "?deduct_amount1", DeductAmount1);

                ModelController.AddParameter(sqlParameters, "?deduct_code2", DeductCode2);
                ModelController.AddParameter(sqlParameters, "?deduct_title2", DeductTitle2);
                ModelController.AddParameter(sqlParameters, "?deduct_amount2", DeductAmount2);

                ModelController.AddParameter(sqlParameters, "?deduct_code3", DeductCode3);
                ModelController.AddParameter(sqlParameters, "?deduct_title3", DeductTitle3);
                ModelController.AddParameter(sqlParameters, "?deduct_amount3", DeductAmount3);

                ModelController.AddParameter(sqlParameters, "?deduct_code4", DeductCode4);
                ModelController.AddParameter(sqlParameters, "?deduct_title4", DeductTitle4);
                ModelController.AddParameter(sqlParameters, "?deduct_amount4", DeductAmount4);

                ModelController.AddParameter(sqlParameters, "?deduct_code5", DeductCode5);
                ModelController.AddParameter(sqlParameters, "?deduct_title5", DeductTitle5);
                ModelController.AddParameter(sqlParameters, "?deduct_amount5", DeductAmount5);

                ModelController.AddParameter(sqlParameters, "?deduct_code6", DeductCode6);
                ModelController.AddParameter(sqlParameters, "?deduct_title6", DeductTitle6);
                ModelController.AddParameter(sqlParameters, "?deduct_amount6", DeductAmount6);

                ModelController.AddParameter(sqlParameters, "?total_charges", ChargeAmountTotal);
                ModelController.AddParameter(sqlParameters, "?total_deductions", DeductAmountTotal);

                ModelController.AddParameter(sqlParameters, "?net_code", NetProceedsCode);
                ModelController.AddParameter(sqlParameters, "?net_title", NetProceedsTitle);
                ModelController.AddParameter(sqlParameters, "?net_amount", NetProceedsAmount);

                ModelController.AddParameter(sqlParameters, "?member_code", LoanDetails.MemberCode);
                ModelController.AddParameter(sqlParameters, "?member_name", LoanDetails.MemberName);

                ModelController.AddParameter(sqlParameters, "?loan_code", LoanDetails.AccountCode);
                ModelController.AddParameter(sqlParameters, "?loan_title", LoanDetails.AccountTitle);
                ModelController.AddParameter(sqlParameters, "?loan_amount", LoanDetails.LoanAmount);

                var loanTerms = LoanDetails.LoanTerms +" " + Utilities.DataConverter.ToTermsMode(LoanDetails.LoanTerms, LoanDetails.TermsMode);
                ModelController.AddParameter(sqlParameters, "?loan_term", loanTerms);

                ModelController.AddParameter(sqlParameters, "?mode_payment", LoanDetails.ModeOfPayment.ToString());
                ModelController.AddParameter(sqlParameters, "?payment", LoanDetails.Payment);

                ModelController.AddParameter(sqlParameters, "?release_date", LoanDetails.DateReleased);
                ModelController.AddParameter(sqlParameters, "?granted_date", LoanDetails.GrantedDate);
                ModelController.AddParameter(sqlParameters, "?maturity_date", LoanDetails.MaturityDate);

                ModelController.AddParameter(sqlParameters, "?interest_rate", LoanDetails.InterestRate);
                ModelController.AddParameter(sqlParameters, "?interest_amount", LoanDetails.InterestAmount);
                ModelController.AddParameter(sqlParameters, "?interest_amortization", LoanDetails.InterestAmortization);

                ModelController.AddParameter(sqlParameters, "?comaker_code1", LoanDetails.CoMakers[0].MemberCode);
                ModelController.AddParameter(sqlParameters, "?comaker_name1", LoanDetails.CoMakers[0].MemberName);
                ModelController.AddParameter(sqlParameters, "?comaker_code2", LoanDetails.CoMakers[1].MemberCode);
                ModelController.AddParameter(sqlParameters, "?comaker_name2", LoanDetails.CoMakers[1].MemberName);
                ModelController.AddParameter(sqlParameters, "?comaker_code3", LoanDetails.CoMakers[2].MemberCode);
                ModelController.AddParameter(sqlParameters, "?comaker_name3", LoanDetails.CoMakers[2].MemberName);

                ModelController.AddParameter(sqlParameters, "?collateral_description", LoanDetails.Description);

                return sqlParameters;
            }
        }

        public void ResetProperties()
        {
            throw new System.NotImplementedException();
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

   
}
