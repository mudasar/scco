using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Views.SearchModule;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    public partial class EditLoanProductView
    {
        private readonly LoanProduct _loanProduct = new LoanProduct();
        private readonly LoanCharge _loanCharge1;
        private readonly LoanCharge _loanCharge2;

        private readonly LoanCharge _loanCharge3;

        private readonly LoanCharge _loanCharge4;

        private readonly LoanCharge _loanCharge5;

        private readonly LoanCharge _loanCharge6;

        private readonly LoanDeduction _loanDeduction1;
        private readonly LoanDeduction _loanDeduction2;
        private readonly LoanDeduction _loanDeduction3;

        public EditLoanProductView()
        {
            InitializeComponent();

            btnOk.Click += delegate { OkButtonOnClick(); };

            foreach (var lt in Enum.GetValues(typeof (Enums.LoanTypes)))
            {
                cboLoanType.Items.Add(lt.ToString());
            }

            foreach (var mop in Enum.GetValues(typeof (Enums.ModeOfPayment)))
            {
                cboModeOfPayment.Items.Add(mop.ToString());
            }

            stbCode1.Click += delegate
                                  {
                                      var account = FindAccount();
                                      if (account == null)
                                      {
                                          _loanCharge1.AccountCode = "";
                                          _loanCharge1.AccountTitle = "";
                                          _loanCharge1.Amount = 0;
                                          return;
                                      }
                                      _loanCharge1.AccountCode = account.AccountCode;
                                      _loanCharge1.AccountTitle = account.AccountTitle;
                                  };

            stbCode2.Click += delegate
                                  {
                                      var account = FindAccount();
                                      if (account == null)
                                      {
                                          _loanCharge2.AccountCode = "";
                                          _loanCharge2.AccountTitle = "";
                                          _loanCharge2.Amount = 0;
                                          return;
                                      }
                                      _loanCharge2.AccountCode = account.AccountCode;
                                      _loanCharge2.AccountTitle = account.AccountTitle;
                                  };

            stbCode3.Click += delegate
                                  {
                                      var account = FindAccount();
                                      if (account == null)
                                      {
                                          _loanCharge3.AccountCode = "";
                                          _loanCharge3.AccountTitle = "";
                                          _loanCharge3.Amount = 0;
                                          return;
                                      }
                                      _loanCharge3.AccountCode = account.AccountCode;
                                      _loanCharge3.AccountTitle = account.AccountTitle;
                                  };

            stbCode4.Click += delegate
                                  {
                                      var account = FindAccount();
                                      if (account == null)
                                      {
                                          _loanCharge4.AccountCode = "";
                                          _loanCharge4.AccountTitle = "";
                                          _loanCharge4.Amount = 0;
                                          return;
                                      }
                                      _loanCharge4.AccountCode = account.AccountCode;
                                      _loanCharge4.AccountTitle = account.AccountTitle;
                                  };

            stbCode5.Click += delegate
                                  {
                                      var account = FindAccount();
                                      if (account == null)
                                      {
                                          _loanCharge5.AccountCode = "";
                                          _loanCharge5.AccountTitle = "";
                                          _loanCharge5.Amount = 0;
                                          return;
                                      }
                                      _loanCharge5.AccountCode = account.AccountCode;
                                      _loanCharge5.AccountTitle = account.AccountTitle;
                                  };

            stbCode6.Click += delegate
                                  {
                                      var account = FindAccount();
                                      if (account == null)
                                      {
                                          _loanCharge6.AccountCode = "";
                                          _loanCharge6.AccountTitle = "";
                                          _loanCharge6.Amount = 0;
                                          return;
                                      }
                                      _loanCharge6.AccountCode = account.AccountCode;
                                      _loanCharge6.AccountTitle = account.AccountTitle;
                                  };

            stbDeductCode1.Click += delegate
                                        {
                                            var account = FindAccount();
                                            if (account == null)
                                            {
                                                _loanDeduction1.AccountCode = "";
                                                _loanDeduction1.AccountTitle = "";
                                                _loanDeduction1.Amount = 0;
                                                return;
                                            }
                                            _loanDeduction1.AccountCode = account.AccountCode;
                                            _loanDeduction1.AccountTitle = account.AccountTitle;
                                        };

            stbDeductCode2.Click += delegate
                                        {
                                            var account = FindAccount();
                                            if (account == null)
                                            {
                                                _loanDeduction2.AccountCode = "";
                                                _loanDeduction2.AccountTitle = "";
                                                _loanDeduction2.Amount = 0;
                                                return;
                                            }
                                            _loanDeduction2.AccountCode = account.AccountCode;
                                            _loanDeduction2.AccountTitle = account.AccountTitle;
                                        };

            stbDeductCode3.Click += delegate
                                        {
                                            var account = FindAccount();
                                            if (account == null)
                                            {
                                                _loanDeduction3.AccountCode = "";
                                                _loanDeduction3.AccountTitle = "";
                                                _loanDeduction3.Amount = 0;
                                                return;
                                            }
                                            _loanDeduction3.AccountCode = account.AccountCode;
                                            _loanDeduction3.AccountTitle = account.AccountTitle;
                                        };
        }

        public EditLoanProductView(int id)
            : this()
        {
            _loanProduct.Find(id);
            var loanCharges = LoanCharge.GetListByLoanProductId(_loanProduct.ID);

            int countCharges = loanCharges.Count;

            _loanCharge1 = countCharges >= 1 ? loanCharges[0] : new LoanCharge();

            _loanCharge2 = countCharges >= 2 ? loanCharges[1] : new LoanCharge();

            _loanCharge3 = countCharges >= 3 ? loanCharges[2] : new LoanCharge();

            _loanCharge4 = countCharges >= 4 ? loanCharges[3] : new LoanCharge();

            _loanCharge5 = countCharges >= 5 ? loanCharges[4] : new LoanCharge();

            _loanCharge6 = countCharges >= 6 ? loanCharges[5] : new LoanCharge();

            List<LoanDeduction> loanDeductions = LoanDeduction.GetListByLoanProductId(_loanProduct.ID);
            var countDeductions = loanDeductions.Count;

            _loanDeduction1 = countDeductions >= 1 ? loanDeductions[0] : new LoanDeduction();

            _loanDeduction2 = countDeductions >= 2 ? loanDeductions[1] : new LoanDeduction();

            _loanDeduction3 = countDeductions >= 3 ? loanDeductions[2] : new LoanDeduction();

            DataContext = _loanProduct;
            grdDetails.DataContext = _loanProduct;

            grdCharge1.DataContext = _loanCharge1;
            grdCharge2.DataContext = _loanCharge2;
            grdCharge3.DataContext = _loanCharge3;
            grdCharge4.DataContext = _loanCharge4;
            grdCharge5.DataContext = _loanCharge5;
            grdCharge6.DataContext = _loanCharge6;

            grdDeduction1.DataContext = _loanDeduction1;
            grdDeduction2.DataContext = _loanDeduction2;
            grdDeduction3.DataContext = _loanDeduction3;
        }

        public EditLoanProductView(string newLoanProductName) : this()
        {
            _loanProduct = new LoanProduct();
            _loanProduct.Name = newLoanProductName;


            _loanCharge1 = new LoanCharge();

            _loanCharge2 = new LoanCharge();

            _loanCharge3 = new LoanCharge();

            _loanCharge4 = new LoanCharge();

            _loanCharge5 = new LoanCharge();

            _loanCharge6 = new LoanCharge();

            _loanDeduction1 = new LoanDeduction();

            _loanDeduction2 = new LoanDeduction();

            _loanDeduction3 = new LoanDeduction();

            DataContext = _loanProduct;
            grdDetails.DataContext = _loanProduct;

            grdCharge1.DataContext = _loanCharge1;
            grdCharge2.DataContext = _loanCharge2;
            grdCharge3.DataContext = _loanCharge3;
            grdCharge4.DataContext = _loanCharge4;
            grdCharge5.DataContext = _loanCharge5;
            grdCharge6.DataContext = _loanCharge6;

            grdDeduction1.DataContext = _loanDeduction1;
            grdDeduction2.DataContext = _loanDeduction2;
            grdDeduction3.DataContext = _loanDeduction3;
        }

        private void OkButtonOnClick()
        {
            string successMessage = "Loan Product has been updated!";
            try
            {
                Result result = _loanProduct.ValidateProperties();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                    return;
                }
                if (_loanProduct.ID > 0)
                    result = _loanProduct.Update();
                else
                {
                    result = _loanProduct.Create();
                    successMessage = "Loan Product has been created!";
                }

                var loanCharges = LoanCharge.GetListByLoanProductId(_loanProduct.ID);
                foreach (LoanCharge loanCharge in loanCharges)
                {
                    loanCharge.Destroy();
                }

                var loanDeductions = LoanDeduction.GetListByLoanProductId(_loanProduct.ID);
                foreach (LoanDeduction loanDeduction in loanDeductions)
                {
                    loanDeduction.Destroy();
                }

                if (!string.IsNullOrEmpty(_loanCharge1.AccountCode))
                {
                    _loanCharge1.LoanProductId = _loanProduct.ID;
                    _loanCharge1.Create();
                }
                if (!string.IsNullOrEmpty(_loanCharge2.AccountCode))
                {
                    _loanCharge2.LoanProductId = _loanProduct.ID;
                    _loanCharge2.Create();
                }
                if (!string.IsNullOrEmpty(_loanCharge3.AccountCode))
                {
                    _loanCharge3.LoanProductId = _loanProduct.ID;
                    _loanCharge3.Create();
                }
                if (!string.IsNullOrEmpty(_loanCharge4.AccountCode))
                {
                    _loanCharge4.LoanProductId = _loanProduct.ID;
                    _loanCharge4.Create();
                }
                if (!string.IsNullOrEmpty(_loanCharge5.AccountCode))
                {
                    _loanCharge5.LoanProductId = _loanProduct.ID;
                    _loanCharge5.Create();
                }
                if (!string.IsNullOrEmpty(_loanCharge6.AccountCode))
                {
                    _loanCharge6.LoanProductId = _loanProduct.ID;
                    _loanCharge6.Create();
                }

                if (!string.IsNullOrEmpty(_loanDeduction1.AccountCode))
                {
                    _loanDeduction1.LoanProductId = _loanProduct.ID;
                    _loanDeduction1.Create();
                }
                if (!string.IsNullOrEmpty(_loanDeduction2.AccountCode))
                {
                    _loanDeduction2.LoanProductId = _loanProduct.ID;
                    _loanDeduction2.Create();
                }
                if (!string.IsNullOrEmpty(_loanDeduction3.AccountCode))
                {
                    _loanDeduction3.LoanProductId = _loanProduct.ID;
                    _loanDeduction3.Create();
                }
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                    return;
                }
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }

            MessageWindow.ShowNotifyMessage(successMessage);
            DialogResult = true;
            Close();
        }

        private void SearchProductCode(object sender, RoutedEventArgs e)
        {
            var loanAccounts = Account.GetListOfLoanReceivables();
            var searchItems =
                loanAccounts.Select(
                    loan =>
                    new SearchItem(loan.ID, loan.AccountTitle) {ItemCode = loan.AccountCode}).ToList();

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();
            if (searchByCodeWindow.DialogResult != true) return;

            _loanProduct.ProductCode = searchByCodeWindow.SelectedItem.ItemCode;
        }

        private Account FindAccount()
        {
            var accounts = Account.GetList();
            var searchItems =
                accounts.Select(
                    loan =>
                    new SearchItem(loan.ID, loan.AccountTitle) {ItemCode = loan.AccountCode}).ToList();

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();
            if (searchByCodeWindow.DialogResult != true)
            {
                return null;
            }
            var account = accounts.SingleOrDefault(ac => ac.ID == searchByCodeWindow.SelectedItem.ItemId);

            return account;
        }

        public LoanProduct CurrentItem { get { return _loanProduct; } }
    }
}
