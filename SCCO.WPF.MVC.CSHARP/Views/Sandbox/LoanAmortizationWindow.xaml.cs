using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.Sandbox
{
    /// <summary>
    /// Interaction logic for LoanComputationMaintenanceWindow.xaml
    /// </summary>
    public partial class LoanAmortizationWindow
    {
		private LoanAmortization _loanAmortization;

		//private LoanDetail _loanDetail;

		private List<LoanProduct> _loanProducts;

        private LoanProduct _loanProduct;
        private Nfmb _member;

        public LoanAmortizationWindow(Nfmb member)
            : this()
        {
            _member = member;
        }

        public LoanAmortizationWindow()
        {
            InitializeComponent();

            InitializeData();

            // data binding
			//MainGrid.DataContext = _loanDetail = new LoanDetail();

			_loanAmortization = new LoanAmortization();

            LoanModuleGrid.DataContext = _loanAmortization;

            cboLoanTypes.ItemsSource = _loanProducts.Select(lp => lp.LoanType).Distinct();

            cboLoanProducts.ItemsSource = _loanProducts;

            cboLoanTerms.ItemsSource = null;
            cboLoanTerms.Items.Add(string.Format("{0} month", 1));
            for (int i = 2; i <= 36; i++)
            {
                cboLoanTerms.Items.Add(string.Format("{0} months", i));
            }

        }

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			GenerateDiminishingAmortization();
		}

		private void button2_Click(object sender, RoutedEventArgs e)
		{
			GenerateStraightLineAmortization();
		}


		private void GenerateDiminishingAmortization()
	    {
			DataGridAmortizationSchedule.ItemsSource = null;
		    _loanAmortization.CreateDiminishingAmortizationSchedule();
		    var x = _loanAmortization.AmortizationSchedule;
			DataGridAmortizationSchedule.ItemsSource = x;
		    LoanModuleGrid.DataContext = null;
		    LoanModuleGrid.DataContext = _loanAmortization;
	    }
		private void GenerateStraightLineAmortization()
		{
			var result = ValidateEntries();
            if(!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
		    DataGridAmortizationSchedule.ItemsSource = null;
			var selectedLoanProduct = _loanProduct = (LoanProduct)cboLoanProducts.SelectedItem;

			_loanAmortization.AnnualInterestRate = selectedLoanProduct.AnnualInterestRate;
			//_loanAmortization.LoanAmount = _loanDetail.LoanAmount;
			_loanAmortization.StartDate = Convert.ToDateTime(DatePickerStartDate.SelectedDate);

			_loanAmortization.CreateStraightLineAmortizationSchedule();
			var x = _loanAmortization.AmortizationSchedule;
			DataGridAmortizationSchedule.ItemsSource = x;
			LoanModuleGrid.DataContext = null;
			LoanModuleGrid.DataContext = _loanAmortization;

            //foreach (LoanCharge loanCharge in selectedLoanProduct.LoanCharges)
            //{
            //    loanCharge.Amount = loanCharge.Rate*_loanAmortization.LoanAmount;
            //}

            //grdLoanCharges.ItemsSource = null;
            //grdLoanCharges.ItemsSource = selectedLoanProduct.LoanCharges;
		}

		private Result ValidateEntries()
		{
            //if(_loanDetail.LoanAmount <= 0)
            //{
            //    return new Result(false, "Invalid loan amount entered.");
            //}

            var selectedLoanProduct = (LoanProduct)cboLoanProducts.SelectedItem;
			if (selectedLoanProduct == null)
			{
				return new Result(false,"No loan product selected.");
			}

            return new Result(true,"ValidateEntries");
		}


		private void InitializeData()
		{
            _loanAmortization = new LoanAmortization();
		    _loanProducts = LoanProduct.GetList();
            _member = new Nfmb();
		}

        private void cboLoanTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedLoanTye =
                cboLoanTypes.SelectedItem.ToString();
            cboLoanProducts.ItemsSource = _loanProducts.Where(lp => lp.LoanType == selectedLoanTye);
        }

        private void cboLoanProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedLoanProduct = (LoanProduct)cboLoanProducts.SelectedItem;
            if (selectedLoanProduct == null) return;

            TextBoxAnnualInterestRate.Text = selectedLoanProduct.AnnualInterestRate.ToString("P");
            cboLoanTerms.SelectedIndex = selectedLoanProduct.MinimumTerm - 1;
        }

        private void cboLoanTerms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _loanAmortization.NumberOfPayments = cboLoanTerms.SelectedIndex + 1;
        }

        private void btnLoanComputation_Click(object sender, RoutedEventArgs e)
        {
            //var account = new Account(_loanProduct.AccountCode);
            ////_loanDetail = new LoanDetail(_member,account,_loanAmortization);
            //_loanDetail = new LoanDetail();
            //_loanDetail.MemberCode = _member.MemberCode;
            //_loanDetail.MemberName = _member.MemberName;
            
            //_loanDetail.AccountCode = _loanProduct.AccountCode;
            //_loanDetail.AccountTitle = _loanProduct.AccountTitle;

            //_loanDetail.LoanAmount = _loanAmortization.LoanAmount;
            //_loanDetail.AnnualInterestRate = _loanProduct.AnnualInterestRate;
            //_loanDetail.Term = _loanAmortization.Term;


            //var loanComputationWindow = new LoanComputationWindow(_loanDetail, _loanProduct);
            //loanComputationWindow.ShowDialog();

            //TODO: initialize LoanComputation based on LoanProductId and LoanAmount
            //var loanComputation = new LoanComputation(_loanAmortization.LoanAmount, _loanProduct.LoanProductId);
            //var loanComputationWindow = new LoanComputationWindow(loanComputation);
            //loanComputationWindow.ShowDialog();
        }
    }
}
