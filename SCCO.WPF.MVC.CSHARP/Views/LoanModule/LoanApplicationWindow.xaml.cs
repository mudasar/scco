using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    /// <summary>
    /// Interaction logic for LoanApplicationWindow.xaml
    /// </summary>
    public partial class LoanApplicationWindow
    {
        //private readonly LoanAmortization _loanAmortization;
        private readonly LoanDetails _loanDetails;
        private readonly List<LoanProduct> _loanProducts;
        private Nfmb _borrower;
        private LoanAmortizationHeader _loanAmortizationHeader;
        private LoanProduct _loanProduct;
        //private Models.Account _loanApplied;

        public LoanApplicationWindow()
        {
            InitializeComponent();

            _loanProducts = LoanProduct.GetList();
            _loanDetails = new LoanDetails {GrantedDate = DateTime.Now, DateReleased = DateTime.Now, TermsMode = "MO"};

            InitializeLookups();
            DataContext = _loanDetails;

            LoanAmountBox.LostFocus += (sender, args) => UpdateSummaryFields();
            LoanTermsCombo.SelectionChanged += (sender, args) => LoanTermsComboOnSelectionChanged();
            LoanProductsCombo.SelectionChanged += LoanProductsComboOnSelectionChanged;
            GrantedDatePicker.SelectedDateChanged += (sender, args) => UpdateSummaryFields();
        }

        public LoanApplicationWindow(Nfmb member)
            : this()
        {
            _borrower = member;
            RefreshBorrowerInformation();
        }

        private void FindBorrowerButtonClick(object sender, RoutedEventArgs e)
        {
            _borrower = MainController.SearchMember();
            RefreshBorrowerInformation();
        }

        //private void GenerateStraightLineAmortization()
        //{
        //    Result result = ValidateEntries();
        //    if (!result.Success)
        //    {
        //        MessageWindow.ShowAlertMessage(result.Message);
        //        return;
        //    }

        //    _loanAmortizationHeader = GenerateLoanAmortizationHeader();
        //}

        private void InitializeLookups()
        {
            #region --- LOAN TYPES ---

            LoanTypesCombo.ItemsSource = _loanProducts.Select(lp => lp.LoanType).Distinct();

            #endregion --- LOAN TYPES ---

            #region --- LOAN TERMS ---

            LoanTermsCombo.Items.Add(string.Format("{0} month", 1));
            for (int i = 2; i <= 36; i++)
            {
                LoanTermsCombo.Items.Add(string.Format("{0} months", i));
            }

            #endregion --- LOAN TERMS ---

            #region --- DATE GRANTED ---

            GrantedDatePicker.SelectedDate = DateTime.Now;

            #endregion --- DATE GRANTED ---
        }

        private void LoanProductsComboOnSelectionChanged(object sender,
                                                         SelectionChangedEventArgs selectionChangedEventArgs)
        {
            LoanProduct selectedLoanProduct = _loanProduct = (LoanProduct) LoanProductsCombo.SelectedItem;
            if (selectedLoanProduct == null) return;

            Account loanAccount = Account.FindByCode(selectedLoanProduct.ProductCode);
            _loanDetails.AccountCode = loanAccount.AccountCode;
            _loanDetails.AccountTitle = loanAccount.AccountTitle;
            _loanDetails.LoanTerms = _loanProduct.MinimumTerm;
            _loanDetails.LoanAmount = selectedLoanProduct.MinimumLoanableAmount;
            _loanDetails.InterestRate = selectedLoanProduct.AnnualInterestRate;

            switch (selectedLoanProduct.ModeOfPayment)
            {
                case "Daily":
                    _loanDetails.ModeOfPayment = ModeOfPayments.Daily;
                    break;

                case "Weekly":
                    _loanDetails.ModeOfPayment = ModeOfPayments.Weekly;
                    break;

                case "SemiMonthly":
                    _loanDetails.ModeOfPayment = ModeOfPayments.SemiMonthly;
                    break;

                case "Monthly":
                    _loanDetails.ModeOfPayment = ModeOfPayments.Monthly;
                    break;

                default:
                    _loanDetails.ModeOfPayment = ModeOfPayments.NotSpecified;
                    break;
            }

            LoanTermsCombo.Items.Clear();
            for (int i = selectedLoanProduct.MinimumTerm; i <= selectedLoanProduct.MaximumTerm; i++)
            {
                LoanTermsCombo.Items.Add(string.Format("{0} months", i));
            }
            if (LoanTermsCombo.Items.Count > 0)
                LoanTermsCombo.SelectedIndex = 0;
        }

        private void LoanTermsComboOnSelectionChanged()
        {
            if (LoanTermsCombo.SelectedItem == null) return;

            var selectedTerm = (string) LoanTermsCombo.SelectedItem;

            string[] jea = selectedTerm.Split();
            short loanTerms = Convert.ToInt16(jea[0]);
            _loanDetails.LoanTerms = loanTerms;
            UpdateSummaryFields();
        }

        private void LoanTypesComboOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedLoanTye = LoanTypesCombo.SelectedItem.ToString();
            LoanProductsCombo.ItemsSource = _loanProducts.Where(lp => lp.LoanType == selectedLoanTye);

            _loanDetails.InterestRate = 0m;
            _loanDetails.LoanAmount = 0m;
        }

        private void RefreshBorrowerInformation()
        {
            BorrowerSearchBox.Text = string.Format("{0} - {1}", _borrower.MemberCode, _borrower.MemberName);
            AddressBox.Text = string.Format("{0} {1} {2}", _borrower.Address1, _borrower.Address2, _borrower.Address3);
            _loanDetails.MemberCode = _borrower.MemberCode;
            _loanDetails.MemberName = _borrower.MemberName;
        }

        private LoanProduct SetLoanProduct()
        {
            // member information
            _loanDetails.MemberCode = _borrower.MemberCode;
            _loanDetails.MemberName = _borrower.MemberName;

            // loan product
            var loanProduct = (LoanProduct) LoanProductsCombo.SelectionBoxItem;
            return loanProduct;
        }

        private void ShowLoanComputationFormButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (_borrower == null)
            {
                MessageWindow.ShowAlertMessage("No member information entered!");
                return;
            }

            if (LoanProductsCombo.SelectedItem == null ||
                string.IsNullOrEmpty(LoanProductsCombo.SelectionBoxItem.ToString()))
            {
                MessageWindow.ShowAlertMessage("No loan information entered!");
                return;
            }

            decimal loanAmount = _loanDetails.LoanAmount;
            if (loanAmount < _loanProduct.MinimumLoanableAmount)
            {
                MessageWindow.ShowAlertMessage(string.Format("Loan amount must not be less than P{0:N}!",
                                                             _loanProduct.MinimumLoanableAmount));
                return;
            }
            if (loanAmount > _loanProduct.MaximumLoanableAmount)
            {
                MessageWindow.ShowAlertMessage(string.Format("Loan amount must not be more than P{0:N}!",
                                                             _loanProduct.MaximumLoanableAmount));
                return;
            }


            LoanProduct loanProduct = SetLoanProduct();


            var loanComputationWindow = new LoanComputationWindow(_loanDetails, loanProduct, _loanAmortizationHeader);
            loanComputationWindow.ShowDialog();
        }

        private Result ValidateEntries()
        {
            if (_loanDetails.LoanAmount <= 0)
            {
                return new Result(false, "Invalid loan amount entered.");
            }

            var selectedLoanProduct = (LoanProduct) LoanProductsCombo.SelectedItem;
            if (selectedLoanProduct == null)
            {
                return new Result(false, "No loan product selected.");
            }

            return new Result(true, "ValidateEntries");
        }

        #region --- METHOD INVOKED BY USER CONTROLS ---

        private void FindComaker1ButtonClick(object sender, RoutedEventArgs e)
        {
            Nfmb member = MainController.SearchMember();
            if (member == null)
            {
                _loanDetails.CoMakers[0] = new CoMaker();
                CoMakerSearchBox1.Text = "";
            }
            else
            {
                _loanDetails.CoMakers[0] = new CoMaker {MemberCode = member.MemberCode, MemberName = member.MemberName};
                CoMakerSearchBox1.Text = _loanDetails.CoMakers[0].ToString();
            }
        }

        private void FindComaker2ButtonClick(object sender, RoutedEventArgs e)
        {
            Nfmb member = MainController.SearchMember();
            if (member == null)
            {
                _loanDetails.CoMakers[1] = new CoMaker();
                CoMakerSearchBox2.Text = "";
            }
            else
            {
                _loanDetails.CoMakers[1] = new CoMaker {MemberCode = member.MemberCode, MemberName = member.MemberName};
                CoMakerSearchBox2.Text = _loanDetails.CoMakers[1].ToString();
            }
        }

        private void FindComaker3ButtonClick(object sender, RoutedEventArgs e)
        {
            Nfmb member = MainController.SearchMember();
            if (member == null)
            {
                _loanDetails.CoMakers[2] = new CoMaker();
                CoMakerSearchBox3.Text = "";
            }
            else
            {
                _loanDetails.CoMakers[2] = new CoMaker {MemberCode = member.MemberCode, MemberName = member.MemberName};
                CoMakerSearchBox3.Text = _loanDetails.CoMakers[2].ToString();
            }
        }

        private void LoanAmortizationScheduleButtonClick(object sender, RoutedEventArgs e)
        {
            //GenerateStraightLineAmortization();
            //SetLoanProduct();
            //ShowReport();
            LoanAmortizationHeader schedule = GenerateLoanAmortizationHeader();

            ShowReport(schedule);
        }

        private LoanAmortizationHeader GenerateLoanAmortizationHeader()
        {
            string codeMember = _borrower.MemberCode;
            string codeAccount = _loanProduct.ProductCode;
            decimal amountLoan = _loanDetails.LoanAmount;
            int termLoan = _loanDetails.LoanTerms;
            decimal rateAnnualInterest = _loanProduct.AnnualInterestRate;
            DateTime grantedDate = _loanDetails.GrantedDate;
            decimal monthlyCapitalBuildUp = _loanProduct.MonthlyCapitalBuildUp;

            LoanAmortizationHeader schedule = LoanAmortizationController.GenerateLoanAmortization(
                codeMember,
                codeAccount,
                amountLoan,
                termLoan,
                rateAnnualInterest,
                grantedDate,
                monthlyCapitalBuildUp);
            return schedule;
        }

        private void ShowReport(LoanAmortizationHeader scheduleLoanAmortization)
        {
            if (scheduleLoanAmortization == null)
                return;

            var dtLoanAmortizationDetails = new DataTable {TableName = "loan_amortization_details"};
            dtLoanAmortizationDetails.Columns.Add("payment_date", typeof (DateTime));
            dtLoanAmortizationDetails.Columns.Add("payment_no", typeof (int));
            dtLoanAmortizationDetails.Columns.Add("beginning_balance", typeof (decimal));
            dtLoanAmortizationDetails.Columns.Add("payment", typeof (decimal));
            dtLoanAmortizationDetails.Columns.Add("interest", typeof (decimal));
            dtLoanAmortizationDetails.Columns.Add("capital_build_up", typeof (decimal));
            dtLoanAmortizationDetails.Columns.Add("amortization", typeof (decimal));
            dtLoanAmortizationDetails.Columns.Add("ending_balance", typeof (decimal));

            foreach (LoanAmortizationDetail schedule in scheduleLoanAmortization.PaymentSchedules)
            {
                DataRow dtRowDetails = dtLoanAmortizationDetails.NewRow();
                dtRowDetails["payment_date"] = schedule.PaymentDate;
                dtRowDetails["payment_no"] = schedule.PaymentNo;
                dtRowDetails["beginning_balance"] = schedule.BeginningBalance;
                dtRowDetails["payment"] = schedule.Payment;
                dtRowDetails["interest"] = schedule.Interest;
                dtRowDetails["capital_build_up"] = schedule.CapitalBuildUp;
                dtRowDetails["amortization"] = schedule.Amortization;
                dtRowDetails["ending_balance"] = schedule.EndingBalance;
                dtLoanAmortizationDetails.Rows.Add(dtRowDetails);
            }

            var dtLoanAmortizationHeader = new DataTable();
            dtLoanAmortizationDetails.TableName = "loan_amortization_header";
            dtLoanAmortizationHeader.Columns.Add("member_code", typeof (string));
            dtLoanAmortizationHeader.Columns.Add("member_name", typeof (string));
            dtLoanAmortizationHeader.Columns.Add("member_address", typeof (string));

            dtLoanAmortizationHeader.Columns.Add("account_code", typeof (string));
            dtLoanAmortizationHeader.Columns.Add("account_title", typeof (string));

            dtLoanAmortizationHeader.Columns.Add("loan_amount", typeof (decimal));
            dtLoanAmortizationHeader.Columns.Add("monthly_amortization", typeof (decimal));
            dtLoanAmortizationHeader.Columns.Add("monthly_capital_build_up", typeof (decimal));
            dtLoanAmortizationHeader.Columns.Add("annual_interest_rate", typeof (decimal));

            dtLoanAmortizationHeader.Columns.Add("loan_term", typeof (int));
            dtLoanAmortizationHeader.Columns.Add("mode_of_payment", typeof (string));

            dtLoanAmortizationHeader.Columns.Add("date_granted", typeof (DateTime));
            dtLoanAmortizationHeader.Columns.Add("date_maturity", typeof (DateTime));
            dtLoanAmortizationHeader.Columns.Add("first_payment_date", typeof (DateTime));

            DataRow dtRowHeader = dtLoanAmortizationHeader.NewRow();
            dtRowHeader["member_code"] = scheduleLoanAmortization.MemberCode;
            dtRowHeader["member_name"] = scheduleLoanAmortization.MemberName;
            dtRowHeader["member_address"] = scheduleLoanAmortization.MemberAddress;

            dtRowHeader["account_code"] = scheduleLoanAmortization.AccountCode;
            dtRowHeader["account_title"] = scheduleLoanAmortization.AccountTitle;

            dtRowHeader["loan_amount"] = scheduleLoanAmortization.LoanAmount;
            dtRowHeader["monthly_amortization"] = scheduleLoanAmortization.MonthlyAmortization;
            dtRowHeader["monthly_capital_build_up"] = scheduleLoanAmortization.MonthlyCapitalBuildUp;
            dtRowHeader["annual_interest_rate"] = scheduleLoanAmortization.AnnualInterestRate;

            dtRowHeader["loan_term"] = scheduleLoanAmortization.LoanTerm;
            dtRowHeader["mode_of_payment"] = scheduleLoanAmortization.ModeOfPayment;

            dtRowHeader["date_granted"] = scheduleLoanAmortization.DateGranted;
            dtRowHeader["date_maturity"] = scheduleLoanAmortization.DateMaturity;
            dtRowHeader["first_payment_date"] = scheduleLoanAmortization.FirstPaymentDate;

            dtLoanAmortizationHeader.Rows.Add(dtRowHeader);

            //ReportController.GenerateLoanAmortizationReport(dtLoanAmortizationHeader, dtLoanAmortizationDetails);
            try
            {
                var dataTables = new DataTable[2];
                dataTables[0] = dtLoanAmortizationHeader;
                dataTables[1] = dtLoanAmortizationDetails;
                var reportItem = new ReportItem {Title = "Loan Amortization Schedule"};
                dataTables[0].TableName = "loan_amortization_header";
                dataTables[1].TableName = "loan_amortization_details";

                Result result = reportItem.LoadReport(dataTables, "loan_amortization_schedule.rpt");
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception e)
            {
                MessageWindow.ShowAlertMessage(e.Message);
            }

            dtLoanAmortizationHeader.Dispose();
            dtLoanAmortizationDetails.Dispose();
        }


        //private void ShowReport()
        //{
        //    if (_loanAmortization.AmortizationSchedule == null)
        //        return;

        //    var dtLoanAmortizationDetails = new DataTable();
        //    dtLoanAmortizationDetails.TableName = "loan_amortization_details";
        //    dtLoanAmortizationDetails.Columns.Add("PaymentDate", typeof(DateTime));
        //    dtLoanAmortizationDetails.Columns.Add("PaymentNumber", typeof(int));
        //    dtLoanAmortizationDetails.Columns.Add("BeginningBalance", typeof(decimal));
        //    dtLoanAmortizationDetails.Columns.Add("ScheduledPayment", typeof(decimal));
        //    dtLoanAmortizationDetails.Columns.Add("Interest", typeof(decimal));
        //    dtLoanAmortizationDetails.Columns.Add("CapitalBuildUp", typeof(decimal));
        //    dtLoanAmortizationDetails.Columns.Add("MonthlyAmortization", typeof(decimal));
        //    dtLoanAmortizationDetails.Columns.Add("EndingBalance", typeof(decimal));

        //    var beginningBalance = _loanAmortization.LoanAmount;
        //    foreach (var schedule in _loanAmortization.AmortizationSchedule)
        //    {
        //        DataRow dtRowDetails = dtLoanAmortizationDetails.NewRow();
        //        dtRowDetails["PaymentDate"] = schedule.Date;
        //        dtRowDetails["PaymentNumber"] = schedule.PaymentNo;
        //        dtRowDetails["BeginningBalance"] = beginningBalance;
        //        dtRowDetails["ScheduledPayment"] = schedule.Amount;
        //        dtRowDetails["Interest"] = schedule.Interest;
        //        dtRowDetails["CapitalBuildUp"] = schedule.CapitalBuildUp;
        //        dtRowDetails["MonthlyAmortization"] = schedule.Principal;
        //        dtRowDetails["EndingBalance"] = schedule.Balance;
        //        dtLoanAmortizationDetails.Rows.Add(dtRowDetails);
        //        beginningBalance = schedule.Balance; 
        //    }

        //    var dtLoanAmortizationHeader = new DataTable();
        //    dtLoanAmortizationDetails.TableName = "loan_amortization_header";
        //    dtLoanAmortizationHeader.Columns.Add("MemberCode", typeof(string));
        //    dtLoanAmortizationHeader.Columns.Add("MemberName", typeof(string));
        //    dtLoanAmortizationHeader.Columns.Add("MemberAddress", typeof(string));
        //    dtLoanAmortizationHeader.Columns.Add("AccountCode", typeof(string));
        //    dtLoanAmortizationHeader.Columns.Add("Title", typeof(string));
        //    dtLoanAmortizationHeader.Columns.Add("Term", typeof(string));
        //    dtLoanAmortizationHeader.Columns.Add("Principal", typeof(decimal));
        //    dtLoanAmortizationHeader.Columns.Add("LoanAmount", typeof(decimal));
        //    dtLoanAmortizationHeader.Columns.Add("DateGranted", typeof(DateTime));
        //    dtLoanAmortizationHeader.Columns.Add("InterestRate", typeof(decimal));
        //    dtLoanAmortizationHeader.Columns.Add("Terms", typeof(string));
        //    dtLoanAmortizationHeader.Columns.Add("ModeOfPayment", typeof(string));
        //    dtLoanAmortizationHeader.Columns.Add("MaturityDate", typeof(DateTime));
        //    dtLoanAmortizationHeader.Columns.Add("Payment", typeof(decimal));
        //    dtLoanAmortizationHeader.Columns.Add("NumberOfPayment", typeof(decimal));
        //    dtLoanAmortizationHeader.Columns.Add("MonthlySD", typeof(decimal));

        //    DataRow dtRowHeader = dtLoanAmortizationHeader.NewRow();
        //    dtRowHeader["MemberCode"] = _loanDetails.MemberCode;
        //    dtRowHeader["MemberName"] = _loanDetails.MemberName;
        //    dtRowHeader["MemberAddress"] = AddressBox.Text;
        //    dtRowHeader["AccountCode"] = _loanDetails.AccountCode;
        //    dtRowHeader["Title"] = _loanDetails.AccountTitle;
        //    dtRowHeader["Term"] = LoanTermsCombo.SelectedItem;
        //    dtRowHeader["Principal"] = _loanDetails.LoanAmount;
        //    dtRowHeader["LoanAmount"] = _loanDetails.LoanAmount;
        //    dtRowHeader["DateGranted"] = _loanDetails.GrantedDate;
        //    dtRowHeader["InterestRate"] = _loanDetails.InterestRate * 100;
        //    dtRowHeader["Terms"] = LoanTermsCombo.SelectedItem;
        //    dtRowHeader["ModeOfPayment"] = _loanDetails.ModeOfPayment;
        //    dtRowHeader["MaturityDate"] = _loanDetails.MaturityDate;
        //    dtRowHeader["Payment"] = _loanDetails.Payment;
        //    dtRowHeader["NumberOfPayment"] = _loanAmortization.NumberOfPayments;
        //    dtRowHeader["MonthlySD"] = _loanAmortization.MonthlyPayment;

        //    dtLoanAmortizationHeader.Rows.Add(dtRowHeader);

        //    //ReportController.GenerateLoanAmortizationReport(dtLoanAmortizationHeader, dtLoanAmortizationDetails);
        //    try
        //    {

        //        var dataTables = new DataTable[2];
        //        dataTables[0] = dtLoanAmortizationHeader;
        //        dataTables[1] = dtLoanAmortizationDetails;
        //        var reportItem = new ReportItem();
        //        reportItem.Title = "Loan Amortization Schedule";
        //        dataTables[0].TableName = "loan_amortization_header";
        //        dataTables[1].TableName = "loan_amortization_details";

        //        var result = reportItem.LoadReport(dataTables, "loan_amortization_schedule.rpt");
        //        if(!result.Success)
        //        {
        //            MessageWindow.ShowAlertMessage(result.Message);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageWindow.ShowAlertMessage(e.Message);
        //    }

        //    dtLoanAmortizationHeader.Dispose();
        //    dtLoanAmortizationDetails.Dispose();
        //}

        private void UpdateSummaryFields()
        {
            #region --- REFRESH DATE FIELDS & LOAN TERM ---

            //_loanAmortization.StartDate = _loanDetails.GrantedDate;
            //_loanAmortization.PayOffDate = _loanDetails.GrantedDate.AddMonths(_loanDetails.LoanTerms);

            _loanAmortizationHeader = GenerateLoanAmortizationHeader();

            #endregion --- DATE FIELDS ---

            #region --- CHECK IF WE CAN PERFORM CALCULATION ---

            Result result = ValidateEntries();
            if (result.Success)
            {
                //GenerateStraightLineAmortization();
                _loanDetails.Payment = _loanAmortizationHeader.MonthlyAmortization;
                _loanDetails.InterestAmount = Math.Round(_loanAmortizationHeader.PaymentSchedules.Sum(sched => sched.Interest),2);
                _loanDetails.InterestAmortization = _loanAmortizationHeader.PaymentSchedules.First().Interest;
                _loanDetails.CutOffDate = _loanAmortizationHeader.FirstPaymentDate;
                _loanDetails.MaturityDate = _loanAmortizationHeader.DateMaturity;
            }
            else
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }

            #endregion --- CHECK IF WE CAN PERFORM CALCULATION ---
        }

        #endregion --- METHOD INVOKED BY USER CONTROLS ---
    }
}