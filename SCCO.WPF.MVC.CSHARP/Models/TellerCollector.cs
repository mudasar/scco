using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Media;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class TellerCollector : INotifyPropertyChanged, IModel
    {
        private string _accountCode01;

        private string _accountCode02;

        private string _accountCode03;

        private string _accountCode04;

        private string _accountCode05;

        private string _accountCode06;

        private string _accountCode07;

        private string _accountCode08;

        private string _accountCode09;

        private string _accountCode10;

        private string _accountCode11;

        private string _accountCode12;

        private string _accountTitle01;

        private string _accountTitle02;

        private string _accountTitle03;

        private string _accountTitle04;

        private string _accountTitle05;

        private string _accountTitle06;

        private string _accountTitle07;

        private string _accountTitle08;

        private string _accountTitle09;

        private string _accountTitle10;

        private string _accountTitle11;

        private string _accountTitle12;

        private string _amountInWords;

        private string _collectorName;

        private decimal _creditAmount01;

        private decimal _creditAmount02;

        private decimal _creditAmount03;

        private decimal _creditAmount04;

        private decimal _creditAmount05;

        private decimal _creditAmount06;

        private decimal _creditAmount07;

        private decimal _creditAmount08;

        private decimal _creditAmount09;

        private decimal _creditAmount10;

        private decimal _creditAmount12;

        private string _memberCode;

        private string _memberName;

        private decimal _totalAmount;

        private DateTime _voucherDate;

        private int _voucherNumber;
        private decimal _creditAmount11;
        private bool _isCancelled;
        private Brush _creditAmount01Brush;
        private Brush _creditAmount02Brush;
        private Brush _creditAmount03Brush;
        private Brush _creditAmount04Brush;
        private Brush _creditAmount05Brush;
        private Brush _creditAmount06Brush;
        private Brush _creditAmount07Brush;
        private Brush _creditAmount08Brush;
        private Brush _creditAmount09Brush;
        private Brush _creditAmount10Brush;
        private Brush _creditAmount11Brush;
        private Brush _creditAmount12Brush;
        private List<string> _listTimeDepositCode;
        private Account _cashOnHandAccount;

        public event PropertyChangedEventHandler PropertyChanged;

        public string AccountCode01
        {
            get { return _accountCode01; }
            set
            {
                _accountCode01 = value;
                OnPropertyChanged("AccountCode01");
                CreditAmount01Brush = Brushes.Red;

                //AccountTitle01 = string.Empty;
                //if (value == string.Empty) return;
                //var account = Account.FindByCode(value);
                //if (account != null)
                //{
                //    AccountTitle01 = account.AccountTitle;
                //}
            }
        }

        public string AccountCode02
        {
            get { return _accountCode02; }
            set
            {
                _accountCode02 = value;
                OnPropertyChanged("AccountCode02");
                CreditAmount02Brush = Brushes.Red;

                //AccountTitle02 = string.Empty;
                //if (value == string.Empty) return;
                //var account = Account.FindByCode(value);
                //if (account != null)
                //{
                //    AccountTitle02 = account.AccountTitle;
                //}
            }
        }

        public string AccountCode03
        {
            get { return _accountCode03; }
            set
            {
                _accountCode03 = value;
                OnPropertyChanged("AccountCode03");
                CreditAmount03Brush = Brushes.Red;

                //AccountTitle03 = string.Empty;
                //if (value == string.Empty) return;
                //var account = Account.FindByCode(value);
                //if (account != null)
                //{
                //    AccountTitle03 = account.AccountTitle;
                //}
            }
        }

        public string AccountCode04
        {
            get { return _accountCode04; }
            set
            {
                _accountCode04 = value;
                OnPropertyChanged("AccountCode04");
                CreditAmount04Brush = Brushes.Red;

                //AccountTitle04 = string.Empty;
                //if (value == string.Empty) return;
                //var account = Account.FindByCode(value);
                //if (account != null)
                //{
                //    AccountTitle04 = account.AccountTitle;
                //}
            }
        }

        public string AccountCode05
        {
            get { return _accountCode05; }
            set
            {
                _accountCode05 = value;
                OnPropertyChanged("AccountCode05");
                CreditAmount05Brush = Brushes.Red;

                //AccountTitle05 = string.Empty;
                //if (value == string.Empty) return;
                //var account = Account.FindByCode(value);
                //if (account != null)
                //{
                //    AccountTitle05 = account.AccountTitle;
                //}
            }
        }

        public string AccountCode06
        {
            get { return _accountCode06; }
            set
            {
                _accountCode06 = value;
                OnPropertyChanged("AccountCode06");
                CreditAmount06Brush = Brushes.Red;

                //AccountTitle06 = string.Empty;
                //if (value == string.Empty) return;
                //var account = Account.FindByCode(value);
                //if (account != null)
                //{
                //    AccountTitle06 = account.AccountTitle;
                //}
            }
        }

        public string AccountCode07
        {
            get { return _accountCode07; }
            set
            {
                _accountCode07 = value;
                OnPropertyChanged("AccountCode07");
                CreditAmount07Brush = Brushes.Red;

                //AccountTitle07 = string.Empty;
                //if (value == string.Empty) return;
                //var account = Account.FindByCode(value);
                //if (account != null)
                //{
                //    AccountTitle07 = account.AccountTitle;
                //}
            }
        }

        public string AccountCode08
        {
            get { return _accountCode08; }
            set
            {
                _accountCode08 = value;
                OnPropertyChanged("AccountCode08");
                CreditAmount08Brush = Brushes.Red;

                //AccountTitle08 = string.Empty;
                //if (value == string.Empty) return;
                //var account = Account.FindByCode(value);
                //if (account != null)
                //{
                //    AccountTitle08 = account.AccountTitle;
                //}
            }
        }

        public string AccountCode09
        {
            get { return _accountCode09; }
            set
            {
                _accountCode09 = value;
                OnPropertyChanged("AccountCode09");
                CreditAmount09Brush = Brushes.Red;

                //AccountTitle09 = string.Empty;
                //if (value == string.Empty) return;
                //var account = Account.FindByCode(value);
                //if (account != null)
                //{
                //    AccountTitle09 = account.AccountTitle;
                //}
            }
        }

        public string AccountCode10
        {
            get { return _accountCode10; }
            set
            {
                _accountCode10 = value;
                OnPropertyChanged("AccountCode10");
                CreditAmount10Brush = Brushes.Red;

                //AccountTitle10 = string.Empty;
                //if (value == string.Empty) return;
                //var account = Account.FindByCode(value);
                //if (account != null)
                //{
                //    AccountTitle10 = account.AccountTitle;
                //}
            }
        }

        public string AccountCode11
        {
            get { return _accountCode11; }
            set
            {
                _accountCode11 = value;
                OnPropertyChanged("AccountCode11");
                CreditAmount11Brush = Brushes.Red;

                //AccountTitle11 = string.Empty;
                //if (value == string.Empty) return;
                //var account = Account.FindByCode(value);
                //if (account != null)
                //{
                //    AccountTitle11 = account.AccountTitle;
                //}
            }
        }

        public string AccountCode12
        {
            get { return _accountCode12; }
            set
            {
                _accountCode12 = value;
                OnPropertyChanged("AccountCode12");
                CreditAmount12Brush = Brushes.Red;

                //AccountTitle12 = string.Empty;
                //if (value == string.Empty) return;
                //var account = Account.FindByCode(value);
                //if (account != null)
                //{
                //    AccountTitle12 = account.AccountTitle;
                //}
            }
        }

        public string AccountTitle01
        {
            get { return _accountTitle01; }
            set
            {
                _accountTitle01 = value;
                OnPropertyChanged("AccountTitle01");
            }
        }

        public string AccountTitle02
        {
            get { return _accountTitle02; }
            set
            {
                _accountTitle02 = value;
                OnPropertyChanged("AccountTitle02");
            }
        }

        public string AccountTitle03
        {
            get { return _accountTitle03; }
            set
            {
                _accountTitle03 = value;
                OnPropertyChanged("AccountTitle03");
            }
        }

        public string AccountTitle04
        {
            get { return _accountTitle04; }
            set
            {
                _accountTitle04 = value;
                OnPropertyChanged("AccountTitle04");
            }
        }

        public string AccountTitle05
        {
            get { return _accountTitle05; }
            set
            {
                _accountTitle05 = value;
                OnPropertyChanged("AccountTitle05");
            }
        }

        public string AccountTitle06
        {
            get { return _accountTitle06; }
            set
            {
                _accountTitle06 = value;
                OnPropertyChanged("AccountTitle06");
            }
        }

        public string AccountTitle07
        {
            get { return _accountTitle07; }
            set
            {
                _accountTitle07 = value;
                OnPropertyChanged("AccountTitle07");
            }
        }

        public string AccountTitle08
        {
            get { return _accountTitle08; }
            set
            {
                _accountTitle08 = value;
                OnPropertyChanged("AccountTitle08");
            }
        }

        public string AccountTitle09
        {
            get { return _accountTitle09; }
            set
            {
                _accountTitle09 = value;
                OnPropertyChanged("AccountTitle09");
            }
        }

        public string AccountTitle10
        {
            get { return _accountTitle10; }
            set
            {
                _accountTitle10 = value;
                OnPropertyChanged("AccountTitle10");
            }
        }

        public string AccountTitle11
        {
            get { return _accountTitle11; }
            set
            {
                _accountTitle11 = value;
                OnPropertyChanged("AccountTitle11");
            }
        }

        public string AccountTitle12
        {
            get { return _accountTitle12; }
            set
            {
                _accountTitle12 = value;
                OnPropertyChanged("AccountTitle12");
            }
        }

        public string AmountInWords
        {
            get { return _amountInWords; }
            set
            {
                _amountInWords = value;
                OnPropertyChanged("AmountInWords");
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

        public decimal CreditAmount01
        {
            get { return _creditAmount01; }
            set
            {
                _creditAmount01 = value;
                OnPropertyChanged("CreditAmount01");

                CreditAmount01Brush = Brushes.Red;
                UpdateTotalAmount();
            }
        }

        public Brush CreditAmount01Brush
        {
            get { return _creditAmount01Brush; }
            set
            {
                _creditAmount01Brush = value;

                if (CreditAmount01 == 0m && AccountCode01.Trim().Length == 0)
                {
                    _creditAmount01Brush = Brushes.Transparent;
                }
                else
                {
                    _creditAmount01Brush = Brushes.White;
                }

                OnPropertyChanged("CreditAmount01Brush");
            }
        }

        public decimal CreditAmount02
        {
            get { return _creditAmount02; }
            set
            {
                _creditAmount02 = value;
                OnPropertyChanged("CreditAmount02");

                CreditAmount02Brush = Brushes.Red;
                UpdateTotalAmount();
            }
        }

        public Brush CreditAmount02Brush
        {
            get { return _creditAmount02Brush; }
            set
            {
                _creditAmount02Brush = value;

                if (CreditAmount02 == 0m && AccountCode02.Trim().Length == 0)
                {
                    _creditAmount02Brush = Brushes.Transparent;
                }
                else
                {
                    _creditAmount02Brush = Brushes.White;
                }
                OnPropertyChanged("CreditAmount02Brush");
            }
        }

        public decimal CreditAmount03
        {
            get { return _creditAmount03; }
            set
            {
                _creditAmount03 = value;
                OnPropertyChanged("CreditAmount03");

                CreditAmount03Brush = Brushes.Red;
                UpdateTotalAmount();
            }
        }

        public Brush CreditAmount03Brush
        {
            get { return _creditAmount03Brush; }
            set
            {
                _creditAmount03Brush = value;

                if (CreditAmount03 == 0m && AccountCode03.Trim().Length == 0)
                {
                    _creditAmount03Brush = Brushes.Transparent;
                }
                else
                {
                    _creditAmount03Brush = Brushes.White;
                }
                OnPropertyChanged("CreditAmount03Brush");
            }
        }

        public decimal CreditAmount04
        {
            get { return _creditAmount04; }
            set
            {
                _creditAmount04 = value;
                OnPropertyChanged("CreditAmount04");

                CreditAmount04Brush = Brushes.Red;
                UpdateTotalAmount();
            }
        }

        public Brush CreditAmount04Brush
        {
            get { return _creditAmount04Brush; }
            set
            {
                _creditAmount04Brush = value;

                if (CreditAmount04 == 0m && AccountCode04.Trim().Length == 0)
                {
                    _creditAmount04Brush = Brushes.Transparent;
                }
                else
                {
                    _creditAmount04Brush = Brushes.White;
                }
                OnPropertyChanged("CreditAmount04Brush");
            }
        }

        public decimal CreditAmount05
        {
            get { return _creditAmount05; }
            set
            {
                _creditAmount05 = value;
                OnPropertyChanged("CreditAmount05");

                CreditAmount05Brush = Brushes.Red;
                UpdateTotalAmount();
            }
        }

        public Brush CreditAmount05Brush
        {
            get { return _creditAmount05Brush; }
            set
            {
                _creditAmount05Brush = value;

                if (CreditAmount05 == 0m && AccountCode05.Trim().Length == 0)
                {
                    _creditAmount05Brush = Brushes.Transparent;
                }
                else
                {
                    _creditAmount05Brush = Brushes.White;
                }
                OnPropertyChanged("CreditAmount05Brush");
            }
        }

        public decimal CreditAmount06
        {
            get { return _creditAmount06; }
            set
            {
                _creditAmount06 = value;
                OnPropertyChanged("CreditAmount06");

                CreditAmount06Brush = Brushes.Red;
                UpdateTotalAmount();
            }
        }

        public Brush CreditAmount06Brush
        {
            get { return _creditAmount06Brush; }
            set
            {
                _creditAmount06Brush = value;

                if (CreditAmount06 == 0m && AccountCode06.Trim().Length == 0)
                {
                    _creditAmount06Brush = Brushes.Transparent;
                }
                else
                {
                    _creditAmount06Brush = Brushes.White;
                }
                OnPropertyChanged("CreditAmount06Brush");
            }
        }

        public decimal CreditAmount07
        {
            get { return _creditAmount07; }
            set
            {
                _creditAmount07 = value;
                OnPropertyChanged("CreditAmount07");

                CreditAmount07Brush = Brushes.Red;
                UpdateTotalAmount();
            }
        }

        public Brush CreditAmount07Brush
        {
            get { return _creditAmount07Brush; }
            set
            {
                _creditAmount07Brush = value;

                if (CreditAmount07 == 0m && AccountCode07.Trim().Length == 0)
                {
                    _creditAmount07Brush = Brushes.Transparent;
                }
                else
                {
                    _creditAmount07Brush = Brushes.White;
                }
                OnPropertyChanged("CreditAmount07Brush");
            }
        }

        public decimal CreditAmount08
        {
            get { return _creditAmount08; }
            set
            {
                _creditAmount08 = value;
                OnPropertyChanged("CreditAmount08");

                CreditAmount08Brush = Brushes.Red;
                UpdateTotalAmount();
            }
        }

        public Brush CreditAmount08Brush
        {
            get { return _creditAmount08Brush; }
            set
            {
                _creditAmount08Brush = value;

                if (CreditAmount08 == 0m && AccountCode08.Trim().Length == 0)
                {
                    _creditAmount08Brush = Brushes.Transparent;
                }
                else
                {
                    _creditAmount08Brush = Brushes.White;
                }
                OnPropertyChanged("CreditAmount08Brush");
            }
        }

        public decimal CreditAmount09
        {
            get { return _creditAmount09; }
            set
            {
                _creditAmount09 = value;
                OnPropertyChanged("CreditAmount09");

                CreditAmount09Brush = Brushes.Red;
                UpdateTotalAmount();
            }
        }

        public Brush CreditAmount09Brush
        {
            get { return _creditAmount09Brush; }
            set
            {
                _creditAmount09Brush = value;

                if (CreditAmount09 == 0m && AccountCode09.Trim().Length == 0)
                {
                    _creditAmount09Brush = Brushes.Transparent;
                }
                else
                {
                    _creditAmount09Brush = Brushes.White;
                }
                OnPropertyChanged("CreditAmount09Brush");
            }
        }

        public decimal CreditAmount10
        {
            get { return _creditAmount10; }
            set
            {
                _creditAmount10 = value;
                OnPropertyChanged("CreditAmount10");

                CreditAmount10Brush = Brushes.Red;
                UpdateTotalAmount();
            }
        }

        public Brush CreditAmount10Brush
        {
            get { return _creditAmount10Brush; }
            set
            {
                _creditAmount10Brush = value;

                if (CreditAmount10 == 0m && AccountCode10.Trim().Length == 0)
                {
                    _creditAmount10Brush = Brushes.Transparent;
                }
                else
                {
                    _creditAmount10Brush = Brushes.White;
                }
                OnPropertyChanged("CreditAmount10Brush");
            }
        }

        public decimal CreditAmount11
        {
            get { return _creditAmount11; }
            set
            {
                _creditAmount11 = value;
                OnPropertyChanged("CreditAmount11");

                CreditAmount11Brush = Brushes.Red;
                UpdateTotalAmount();
            }
        }

        public Brush CreditAmount11Brush
        {
            get { return _creditAmount11Brush; }
            set
            {
                _creditAmount11Brush = value;

                if (CreditAmount11 == 0m && AccountCode11.Trim().Length == 0)
                {
                    _creditAmount11Brush = Brushes.Transparent;
                }
                else
                {
                    _creditAmount11Brush = Brushes.White;
                }
                OnPropertyChanged("CreditAmount11Brush");
            }
        }

        public decimal CreditAmount12
        {
            get { return _creditAmount12; }
            set
            {
                _creditAmount12 = value;
                OnPropertyChanged("CreditAmount12");

                CreditAmount12Brush = Brushes.Red;
                UpdateTotalAmount();
            }
        }

        public Brush CreditAmount12Brush
        {
            get { return _creditAmount12Brush; }
            set
            {
                _creditAmount12Brush = value;

                if (CreditAmount12 == 0m && AccountCode12.Trim().Length == 0)
                {
                    _creditAmount12Brush = Brushes.Transparent;
                }
                else
                {
                    _creditAmount12Brush = Brushes.White;
                }
                OnPropertyChanged("CreditAmount12Brush");
            }
        }

        public string MemberCode
        {
            get { return _memberCode; }
            set
            {
                _memberCode = value;
                OnPropertyChanged("MemberCode");
                //MemberName = string.Empty;
                //if (value != string.Empty)
                //{
                //    var member = Nfmb.FindByCode(value);
                //    if (member != null)
                //    {
                //        MemberName = member.MemberName;
                //    }
                //}
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

        public decimal TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                _totalAmount = value;
                OnPropertyChanged("TotalAmount");
            }
        }

        public DateTime VoucherDate
        {
            get { return _voucherDate; }
            set
            {
                _voucherDate = value;
                OnPropertyChanged("VoucherDate");
            }
        }

        public int VoucherNumber
        {
            get { return _voucherNumber; }
            set
            {
                _voucherNumber = value;
                OnPropertyChanged("VoucherNumber");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Implementation of IModel

        public Result Create()
        {
            Action create = delegate
                                {
                                    var totalAmount = 0m;
                                    for (int i = 1; i < 12; i++)
                                    {
                                        if (AccountCodes(i) == string.Empty)
                                        {
                                            continue;  // ignore empty account
                                        }

                                        if (AccountCodes(i).ToUpper().Contains("CANCEL"))
                                        {
                                            continue; // ignore cancelled
                                        }

                                        if (AccountCodes(i).Trim() == _cashOnHandAccount.AccountCode)
                                        {
                                            continue; // ignore cash on hand
                                        }

                                        if (CreditAmounts(i) > 0)
                                        {
                                            totalAmount += CreditAmounts(i);
                                            InsertOfficialReceipt(AccountCodes(i), AccountTitles(i), CreditAmounts(i));
                                        }
                                    }

                                    if (totalAmount > 0)
                                    {
                                        var coh = _cashOnHandAccount;
                                        InsertOfficialReceipt(coh.AccountCode, coh.AccountTitle, totalAmount);
                                    }

                                    #region --- Voucher Log ---

                                    var voucherLog = new VoucherLog();
                                    voucherLog.Find("OR", VoucherNumber);
                                    voucherLog.Date = VoucherDate;
                                    voucherLog.Initials = MainController.LoggedUser.Initials;
                                    if (CashAndCheckDenomimation != null && CashAndCheckDenomimation.HasCheckDeposit)
                                    {
                                        voucherLog.Remarks = "CHK";
                                    }

                                    voucherLog.Save();

                                    #endregion
                                };
            return ActionController.InvokeAction(create);
        }

        public Result Destroy()
        {
            Action delete = () => OfficialReceipt.DeleteAll(_voucherNumber);
            return ActionController.InvokeAction(delete);
        }

        public Result Find(int docNum)
        {
            Action find = delegate
                              {
                                  var sqlBuilder = new StringBuilder();
                                  sqlBuilder.AppendFormat("SELECT * FROM `OR` WHERE DOC_NUM = ?DOC_NUM");

                                  var sqlParam = new SqlParameter("?DOC_NUM", docNum);

                                  DataTable result = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(),
                                                                                           sqlParam);

                                  ResetProperties();
                                  VoucherNumber = docNum;

                                  int index = 0;
                                  foreach (DataRow dataRow in result.Rows)
                                  {
                                      if (index == 0)
                                      {
                                          VoucherDate = DataConverter.ToDateTime(dataRow["DOC_DATE"]);
                                          MemberCode = DataConverter.ToString(dataRow["MEM_CODE"]);
                                          MemberName = DataConverter.ToString(dataRow["MEM_NAME"]);
                                          IsCancelled = MemberCode.ToUpper().Contains("CANCEL");
                                          CollectorName = DataConverter.ToString(dataRow["COLLECTOR"]);
                                      }

                                      index++;

                                      string accountCode = DataConverter.ToString(dataRow["ACC_CODE"]);
                                      string accountTitle = DataConverter.ToString(dataRow["TITLE"]);
                                      decimal credit = DataConverter.ToDecimal(dataRow["CREDIT"]);

                                      if (accountCode != _cashOnHandAccount.AccountCode)
                                      {
                                          SetAccountCode(index, accountCode);
                                          SetAccountTitle(index, accountTitle);
                                          SetCreditAmount(index, credit);
                                          if (_listTimeDepositCode.Contains(accountCode))
                                          {
                                              TimeDepositDetail = TimeDepositDetails.ExtractFromDataRow(dataRow);
                                          }
                                      }
                                      else
                                      {
                                          CashAndCheckDenomimation = CashAndCheckBreakDown.ExtractFromDataRow(dataRow);
                                      }
                                  }

                                  //UpdateTotalAmount();
                              };

            return ActionController.InvokeAction(find);
        }

        public bool IsCancelled
        {
            get { return _isCancelled; }
            set
            {
                _isCancelled = value;
                OnPropertyChanged("IsCancelled");
            }
        }

        public void ResetProperties()
        {
            MemberCode = string.Empty;
            MemberName = string.Empty;

            VoucherNumber = 0;
            VoucherDate = MainController.UserTransactionDate;

            CollectorName = MainController.LoggedUser.CollectorName;

            AccountCode01 = string.Empty;
            AccountTitle01 = string.Empty;
            CreditAmount01 = 0m;

            AccountCode02 = string.Empty;
            AccountTitle02 = string.Empty;
            CreditAmount02 = 0m;

            AccountCode03 = string.Empty;
            AccountTitle03 = string.Empty;
            CreditAmount03 = 0m;

            AccountCode04 = string.Empty;
            AccountTitle04 = string.Empty;
            CreditAmount04 = 0m;

            AccountCode05 = string.Empty;
            AccountTitle05 = string.Empty;
            CreditAmount05 = 0m;

            AccountCode06 = string.Empty;
            AccountTitle06 = string.Empty;
            CreditAmount06 = 0m;

            AccountCode07 = string.Empty;
            AccountTitle07 = string.Empty;
            CreditAmount07 = 0m;

            AccountCode08 = string.Empty;
            AccountTitle08 = string.Empty;
            CreditAmount08 = 0m;

            AccountCode09 = string.Empty;
            AccountTitle09 = string.Empty;
            CreditAmount09 = 0m;

            AccountCode10 = string.Empty;
            AccountTitle10 = string.Empty;
            CreditAmount10 = 0m;

            AccountCode11 = string.Empty;
            AccountTitle11 = string.Empty;
            CreditAmount11 = 0m;

            AccountCode12 = string.Empty;
            AccountTitle12 = string.Empty;
            CreditAmount12 = 0m;

            CashAndCheckDenomimation = null;
            TimeDepositDetail = null;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            throw new NotImplementedException();
        }

        public Result Update()
        {
            throw new NotImplementedException();
        }

        #endregion

        public string AccountCodes(int index)
        {
            switch (index)
            {
                case 1:
                    return AccountCode01;
                case 2:
                    return AccountCode02;
                case 3:
                    return AccountCode03;
                case 4:
                    return AccountCode04;
                case 5:
                    return AccountCode05;
                case 6:
                    return AccountCode06;
                case 7:
                    return AccountCode07;
                case 8:
                    return AccountCode08;
                case 9:
                    return AccountCode09;
                case 10:
                    return AccountCode10;
                case 11:
                    return AccountCode11;
                case 12:
                    return AccountCode12;
                default:
                    return "";
            }
        }

        private string AccountTitles(int index)
        {
            switch (index)
            {
                case 1:
                    return AccountTitle01;
                case 2:
                    return AccountTitle02;
                case 3:
                    return AccountTitle03;
                case 4:
                    return AccountTitle04;
                case 5:
                    return AccountTitle05;
                case 6:
                    return AccountTitle06;
                case 7:
                    return AccountTitle07;
                case 8:
                    return AccountTitle08;
                case 9:
                    return AccountTitle09;
                case 10:
                    return AccountTitle10;
                case 11:
                    return AccountTitle11;
                case 12:
                    return AccountTitle12;
                default:
                    return "";
            }
        }

        private decimal CreditAmounts(int index)
        {
            switch (index)
            {
                case 1:
                    return CreditAmount01;
                case 2:
                    return CreditAmount02;
                case 3:
                    return CreditAmount03;
                case 4:
                    return CreditAmount04;
                case 5:
                    return CreditAmount05;
                case 6:
                    return CreditAmount06;
                case 7:
                    return CreditAmount07;
                case 8:
                    return CreditAmount08;
                case 9:
                    return CreditAmount09;
                case 10:
                    return CreditAmount10;
                case 11:
                    return CreditAmount11;
                case 12:
                    return CreditAmount12;
                default:
                    return 0m;
            }
        }

        private void SetAccountCode(int index, string value)
        {
            switch (index)
            {
                case 1:
                    AccountCode01 = value;
                    break;
                case 2:
                    AccountCode02 = value;
                    break;
                case 3:
                    AccountCode03 = value;
                    break;
                case 4:
                    AccountCode04 = value;
                    break;
                case 5:
                    AccountCode05 = value;
                    break;
                case 6:
                    AccountCode06 = value;
                    break;
                case 7:
                    AccountCode07 = value;
                    break;
                case 8:
                    AccountCode08 = value;
                    break;
                case 9:
                    AccountCode09 = value;
                    break;
                case 10:
                    AccountCode10 = value;
                    break;
                case 11:
                    AccountCode11 = value;
                    break;
                case 12:
                    AccountCode12 = value;
                    break;
            }
        }

        internal void SetAccountTitle(int index, string value)
        {
            switch (index)
            {
                case 1:
                    AccountTitle01 = value;
                    break;
                case 2:
                    AccountTitle02 = value;
                    break;
                case 3:
                    AccountTitle03 = value;
                    break;
                case 4:
                    AccountTitle04 = value;
                    break;
                case 5:
                    AccountTitle05 = value;
                    break;
                case 6:
                    AccountTitle06 = value;
                    break;
                case 7:
                    AccountTitle07 = value;
                    break;
                case 8:
                    AccountTitle08 = value;
                    break;
                case 9:
                    AccountTitle09 = value;
                    break;
                case 10:
                    AccountTitle10 = value;
                    break;
                case 11:
                    AccountTitle11 = value;
                    break;
                case 12:
                    AccountTitle12 = value;
                    break;
            }
        }

        private void SetCreditAmount(int index, decimal value)
        {
            switch (index)
            {
                case 1:
                    CreditAmount01 = value;
                    break;
                case 2:
                    CreditAmount02 = value;
                    break;
                case 3:
                    CreditAmount03 = value;
                    break;
                case 4:
                    CreditAmount04 = value;
                    break;
                case 5:
                    CreditAmount05 = value;
                    break;
                case 6:
                    CreditAmount06 = value;
                    break;
                case 7:
                    CreditAmount07 = value;
                    break;
                case 8:
                    CreditAmount08 = value;
                    break;
                case 9:
                    CreditAmount09 = value;
                    break;
                case 10:
                    CreditAmount10 = value;
                    break;
                case 11:
                    CreditAmount11 = value;
                    break;
                case 12:
                    CreditAmount12 = value;
                    break;
            }
        }

        private void InsertOfficialReceipt(string accountCode, string accountTitle, decimal amount)
        {
            var or = new OfficialReceipt
                         {
                             MemberCode = MemberCode,
                             MemberName = MemberName,
                             AccountCode = accountCode,
                             AccountTitle = accountTitle
                         };

            if (accountCode == _cashOnHandAccount.AccountCode)
            {
                // insert denomination details
                or.Debit = amount;

                #region --- Cash CashAndCheckBreakDown ---

                if (CashAndCheckDenomimation != null)
                {
                    //JEA: Since deno09 is not available (0.50), I use it as lieu to 200
                    or.Deno01 = CashAndCheckDenomimation.Deno01; //1000
                    or.Deno02 = CashAndCheckDenomimation.Deno02; //500
                    or.Deno03 = CashAndCheckDenomimation.Deno03; //100
                    or.Deno04 = CashAndCheckDenomimation.Deno04; //50
                    or.Deno05 = CashAndCheckDenomimation.Deno05; //20
                    or.Deno06 = CashAndCheckDenomimation.Deno06; //10
                    or.Deno07 = CashAndCheckDenomimation.Deno07; //5
                    or.Deno08 = CashAndCheckDenomimation.Deno08; //1
                    or.Deno09 = CashAndCheckDenomimation.Deno09; //.5 -> 200
                    or.Deno10 = CashAndCheckDenomimation.Deno10; //.25

                    #endregion --- Cash CashAndCheckBreakDown ---

                    #region --- Check CashAndCheckBreakDown ---

                    or.BankName1 = CashAndCheckDenomimation.BankName1;
                    or.BankDate1 = CashAndCheckDenomimation.BankDate1;
                    or.BankCheck1 = CashAndCheckDenomimation.BankCheck1;
                    or.BankAmount1 = CashAndCheckDenomimation.BankAmount1;

                    or.BankName2 = CashAndCheckDenomimation.BankName2;
                    or.BankDate2 = CashAndCheckDenomimation.BankDate2;
                    or.BankCheck2 = CashAndCheckDenomimation.BankCheck2;
                    or.BankAmount2 = CashAndCheckDenomimation.BankAmount2;

                    or.BankName3 = CashAndCheckDenomimation.BankName3;
                    or.BankDate3 = CashAndCheckDenomimation.BankDate3;
                    or.BankCheck3 = CashAndCheckDenomimation.BankCheck3;
                    or.BankAmount3 = CashAndCheckDenomimation.BankAmount3;

                    or.BankName4 = CashAndCheckDenomimation.BankName4;
                    or.BankDate4 = CashAndCheckDenomimation.BankDate4;
                    or.BankCheck4 = CashAndCheckDenomimation.BankCheck4;
                    or.BankAmount4 = CashAndCheckDenomimation.BankAmount4;

                    or.BankName5 = CashAndCheckDenomimation.BankName5;
                    or.BankDate5 = CashAndCheckDenomimation.BankDate5;
                    or.BankCheck5 = CashAndCheckDenomimation.BankCheck5;
                    or.BankAmount5 = CashAndCheckDenomimation.BankAmount5;
                }

                #endregion

                or.AmountInWords = AmountInWords;
                or.Amount = TotalAmount;
            }
            else
            {
                or.Credit = amount;
            }

            or.VoucherDate = VoucherDate;
            or.VoucherNo = VoucherNumber;
            or.VoucherType = VoucherTypes.OR;

            or.Collector = CollectorName;
            or.IsPosted = true;

            if (_listTimeDepositCode.Contains(accountCode))
            {
                if (TimeDepositDetail != null)
                    or.TimeDepositDetails = TimeDepositDetail;
            }

            or.Create();
        }

        private void UpdateTotalAmount()
        {
            TotalAmount = CreditAmount01 +
                          CreditAmount02 +
                          CreditAmount03 +
                          CreditAmount04 +
                          CreditAmount05 +
                          CreditAmount06 +
                          CreditAmount07 +
                          CreditAmount08 +
                          CreditAmount09 +
                          CreditAmount10 +
                          CreditAmount11 +
                          CreditAmount12;

            AmountInWords = Converter.AmountToWords(TotalAmount);
        }

        public TimeDepositDetails TimeDepositDetail { get; set; }

        public CashAndCheckBreakDown CashAndCheckDenomimation { get; set; }

        public string Signature
        {
            get
            {
                // used to compare changes
                var signatureBuilder = new StringBuilder();
                signatureBuilder.Append(VoucherDate);
                signatureBuilder.Append(VoucherNumber);

                signatureBuilder.Append(MemberCode);
                signatureBuilder.Append(CollectorName);

                signatureBuilder.Append(AccountCode01);
                signatureBuilder.Append(CreditAmount01);

                signatureBuilder.Append(AccountCode02);
                signatureBuilder.Append(CreditAmount02);

                signatureBuilder.Append(AccountCode03);
                signatureBuilder.Append(CreditAmount03);

                signatureBuilder.Append(AccountCode04);
                signatureBuilder.Append(CreditAmount04);

                signatureBuilder.Append(AccountCode05);
                signatureBuilder.Append(CreditAmount05);

                signatureBuilder.Append(AccountCode06);
                signatureBuilder.Append(CreditAmount06);

                signatureBuilder.Append(AccountCode07);
                signatureBuilder.Append(CreditAmount07);

                signatureBuilder.Append(AccountCode08);
                signatureBuilder.Append(CreditAmount08);

                signatureBuilder.Append(AccountCode09);
                signatureBuilder.Append(CreditAmount09);

                signatureBuilder.Append(AccountCode10);
                signatureBuilder.Append(CreditAmount10);

                signatureBuilder.Append(AccountCode11);
                signatureBuilder.Append(CreditAmount11);

                signatureBuilder.Append(AccountCode12);
                signatureBuilder.Append(CreditAmount12);

                if (TimeDepositDetail != null)
                {
                    signatureBuilder.Append(TimeDepositDetail.DateIn);
                    signatureBuilder.Append(TimeDepositDetail.CertificateNo);
                    signatureBuilder.Append(TimeDepositDetail.Term);
                    signatureBuilder.Append(TimeDepositDetail.Rate);
                }

                if (CashAndCheckDenomimation != null)
                {
                    signatureBuilder.Append(CashAndCheckDenomimation.Deno01);
                    signatureBuilder.Append(CashAndCheckDenomimation.Deno02);
                    signatureBuilder.Append(CashAndCheckDenomimation.Deno03);
                    signatureBuilder.Append(CashAndCheckDenomimation.Deno04);
                    signatureBuilder.Append(CashAndCheckDenomimation.Deno05);
                    signatureBuilder.Append(CashAndCheckDenomimation.Deno06);
                    signatureBuilder.Append(CashAndCheckDenomimation.Deno07);
                    signatureBuilder.Append(CashAndCheckDenomimation.Deno08);
                    signatureBuilder.Append(CashAndCheckDenomimation.Deno09);
                    signatureBuilder.Append(CashAndCheckDenomimation.Deno10);

                    signatureBuilder.Append(CashAndCheckDenomimation.BankName1);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankDate1);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankCheck1);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankAmount1);

                    signatureBuilder.Append(CashAndCheckDenomimation.BankName2);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankDate2);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankCheck2);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankAmount2);

                    signatureBuilder.Append(CashAndCheckDenomimation.BankName3);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankDate3);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankCheck3);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankAmount3);

                    signatureBuilder.Append(CashAndCheckDenomimation.BankName4);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankDate4);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankCheck4);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankAmount4);

                    signatureBuilder.Append(CashAndCheckDenomimation.BankName5);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankDate5);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankCheck5);
                    signatureBuilder.Append(CashAndCheckDenomimation.BankAmount5);
                }
                return signatureBuilder.ToString();
            }
        }

        public void InitializeLookup()
        {
            _cashOnHandAccount = Account.FindByCode(GlobalSettings.CodeOfCashOnHand);
            _listTimeDepositCode = Account.GetListOfTimeDepositCode();
        }
    }
}