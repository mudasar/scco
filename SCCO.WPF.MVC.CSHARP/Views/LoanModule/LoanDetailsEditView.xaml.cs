using System;
using System.Windows;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    public partial class LoanDetailsEditView
    {
        private const int MONTHS_IN_THREE_YEARS = 12*3;
        private readonly LoanDetails _loanDetails;

        public LoanDetailsEditView(LoanDetails loanDetails)
        {
            InitializeComponent();
            _loanDetails = loanDetails;
            DataContext = _loanDetails;

            btnUpdate.Click += UpdateButtonOnClick;

            txtLoanAmount.LostFocus += (sender, args) => RefreshFields();
            cboTerm.SelectionChanged += (sender, args) => RefreshFields();
            cboTermsMode.SelectionChanged += (sender, args) => RefreshFields();
            cboModePay.SelectionChanged += (sender, args) => RefreshFields();

            dtpDateGranted.LostFocus += (sender, args) => RefreshFields();
            txtAnnualInterestRate.LostFocus += (sender, args) => RefreshFields();


            for (int i = 0; i < (MONTHS_IN_THREE_YEARS);)
            {
                i++;
                cboTerm.Items.Add(i);
            }

            cboTermsMode.Items.Add("Days");
            cboTermsMode.Items.Add("Month");
            cboTermsMode.Items.Add("Months");

            cboModePay.Items.Add(ModeOfPayments.Daily);
            cboModePay.Items.Add(ModeOfPayments.Weekly);
            cboModePay.Items.Add(ModeOfPayments.SemiMonthly);
            cboModePay.Items.Add(ModeOfPayments.Monthly);
        }

        private void RefreshFields()
        {
            if (_loanDetails.LoanAmount == 0) return;
            if (_loanDetails.LoanTerms == 0) return;
            if (string.IsNullOrEmpty(_loanDetails.TermsMode)) return;

            switch (_loanDetails.TermsMode)
            {
                case "Days":
                case "Day":
                    UpdateFieldsForDayTermsMode();
                    break;
                default:
                    UpdateFieldsForMonthTermsMode();
                    break;
            }
        }

        private void UpdateFieldsForDayTermsMode()
        {
            _loanDetails.MaturityDate = _loanDetails.GrantedDate.AddDays(_loanDetails.LoanTerms);
            _loanDetails.InterestAmount = _loanDetails.LoanAmount * _loanDetails.InterestRate;
            _loanDetails.InterestAmortization = _loanDetails.InterestAmount;

            var days = _loanDetails.LoanTerms;

            switch (_loanDetails.ModeOfPayment)
            {
                case ModeOfPayments.Daily:
                    _loanDetails.Payment = _loanDetails.LoanAmount/_loanDetails.LoanTerms;
                    break;

                case ModeOfPayments.Weekly:
                    _loanDetails.Payment = _loanDetails.LoanAmount/(Math.Floor(days/7m));
                    break;
                case ModeOfPayments.SemiMonthly:
                    _loanDetails.Payment = _loanDetails.LoanAmount/(days/15m);
                    break;
                default:
                    _loanDetails.Payment = _loanDetails.LoanAmount;
                    break;
            }
        }

        private void UpdateFieldsForMonthTermsMode()
        {
            _loanDetails.MaturityDate = _loanDetails.GrantedDate.AddMonths(_loanDetails.LoanTerms);
            _loanDetails.InterestAmount = (_loanDetails.LoanAmount * _loanDetails.InterestRate / 12) * _loanDetails.LoanTerms;
            _loanDetails.InterestAmortization = _loanDetails.InterestAmount / _loanDetails.LoanTerms;

            switch (_loanDetails.ModeOfPayment)
            {
                case ModeOfPayments.Daily:
                    _loanDetails.Payment = _loanDetails.LoanAmount /
                                           _loanDetails.MaturityDate.Subtract(_loanDetails.GrantedDate).Days;
                    break;

                case ModeOfPayments.Weekly:
                    _loanDetails.Payment = _loanDetails.LoanAmount /
                                           GetWeekDifference(_loanDetails.GrantedDate, _loanDetails.MaturityDate);
                    break;
                case ModeOfPayments.SemiMonthly:
                    _loanDetails.Payment = _loanDetails.LoanAmount /
                                           (GetMonthDifference(_loanDetails.GrantedDate, _loanDetails.MaturityDate) * 2);
                    break;
                default:
                    _loanDetails.Payment = _loanDetails.LoanAmount /
                                           GetMonthDifference(_loanDetails.GrantedDate, _loanDetails.MaturityDate);
                    break;
            }
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            DialogResult = true;
            Close();
        }

        private int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12*(startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        private int GetWeekDifference(DateTime startDate, DateTime endDate)
        {
            return (endDate.Subtract(startDate)).Days/7;
        }

        private enum TermsMode
        {
            Days,
            Months
        }
    }
}