using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    /// <summary>
    /// Interaction logic for LoanDetailsWindow.xaml
    /// </summary>
    public partial class LoanDetailsWindow
    {
        private readonly LoanDetails _loanDetails;
        public LoanDetailsWindow(LoanDetails loanDetails)
        {
            InitializeComponent();
            _loanDetails = loanDetails;
            DataContext = _loanDetails;
            
            var count = _loanDetails.CoMakers.Length;
            if (count >= 1)
                lblCoMaker1.DataContext = _loanDetails.CoMakers[0];
            else
                lblCoMaker1.DataContext = new CoMaker();

            if (count >= 2)
                lblCoMaker2.DataContext = _loanDetails.CoMakers[1];
            else
                lblCoMaker2.DataContext = new CoMaker();

            if (count >= 3)
                lblCoMaker3.DataContext = _loanDetails.CoMakers[2];
            else
                lblCoMaker3.DataContext = new CoMaker();

            //lblBorrower.Content = string.Format("{0} - {1}", loanDetails.MemberCode, loanDetails.MemberName);
            //lblLoanApplied.Content = string.Format("{0} - {1}", loanDetails.AccountCode, loanDetails.AccountTitle);

            //lblReference.Content = string.Format("{0} {1}", loanDetails.DocumentType, loanDetails.DocumentNo);
            //lblDate.Content = string.Format("{0:MM/dd/yyyy}", loanDetails.DocumentDate);

            //lblBank.Content = string.Format("{0}", loanDetails.BankName);
            //lblCheckNo.Content = string.Format("{0}", loanDetails.CheckNo);

            //lblLoanAmount.Content = string.Format("{0:N2}", loanDetails.LoanAmount);
            //lblPayment.Content = string.Format("{0:N2}", loanDetails.Payment);
            //lblInterestAmount.Content = string.Format("{0:N2}", loanDetails.InterestAmount);
            //lblInterestAmort.Content = string.Format("{0:N2}", loanDetails.InterestAmortization);

            //lblDateGranted.Content = string.Format("{0:MM/dd/yyyy}", loanDetails.GrantedDate);
            //lblMaturity.Content = string.Format("{0:MM/dd/yyyy}", loanDetails.MaturityDate);
            //lblCutOffDate.Content = string.Format("{0:MM/dd/yyyy}", loanDetails.CutOffDate);

            //chkWithCollateral.IsChecked = loanDetails.IsWithCollateral;
            //chkWithCollateral.IsEnabled = false;
            //lblCollateralDescription.Content = loanDetails.Description;

            //lblRealeaseNo.Content = string.Format("{0}", loanDetails.ReleaseNo);
            //lblReleaseDate.Content = string.Format("{0:MM/dd/yyyy}", loanDetails.DateReleased);
            //lblInterestRate.Content = string.Format("{0:P}", loanDetails.InterestRate);
            //lblTerms.Content = string.Format("{0} {1}", loanDetails.LoanTerms, loanDetails.TermsMode);
            //lblModeOfPayment.Content = string.Format("{0}", loanDetails.ModeOfPayment);

            //var count = loanDetails.CoMakers.Length;
            //if(count >=1)
            //    lblCoMaker1.Content = string.Format("{0} - {1}", loanDetails.CoMakers[0].MemberCode, loanDetails.CoMakers[0].MemberName);

            //if (count >= 2)
            //    lblCoMaker2.Content = string.Format("{0} - {1}", loanDetails.CoMakers[1].MemberCode, loanDetails.CoMakers[1].MemberName);

            //if (count >= 3)
            //    lblCoMaker3.Content = string.Format("{0} - {1}", loanDetails.CoMakers[2].MemberCode, loanDetails.CoMakers[2].MemberName);

        }
        //public LoanDetailsWindow(DataRow dataRow)
        //{
        //    InitializeComponent();
        //    lblBorrower.Content = string.Format("{0} - {1}", dataRow["MEM_CODE"], dataRow["MEM_NAME"]);
        //    lblLoanApplied.Content = string.Format("{0} - {1}", dataRow["ACC_CODE"], dataRow["TITLE"]);

        //    lblReference.Content = string.Format("{0} {1}", dataRow["DOC_TYPE"], dataRow["DOC_NUM"]);
        //    lblDate.Content = string.Format("{0:MM/dd/yyyy}", dataRow["DOC_DATE"]);

        //    lblBank.Content = string.Format("{0}", dataRow["BANK_TITLE"]);
        //    lblCheckNo.Content = string.Format("{0}", dataRow["CHECK_NUM"]);
            
        //    lblLoanAmount.Content = string.Format("{0:N2}", dataRow["LOAN_AMT"]);
        //    lblPayment.Content = string.Format("{0:N2}", dataRow["PAYMENT"]);
        //    lblInterestAmount.Content = string.Format("{0:N2}", dataRow["INT_AMT"]);
        //    lblInterestAmort.Content = string.Format("{0:N2}", dataRow["INT_AMORT"]);

        //    lblDateGranted.Content = string.Format("{0:MM/dd/yyyy}", dataRow["DATE_GRANT"]);
        //    lblMaturity.Content = string.Format("{0:MM/dd/yyyy}", dataRow["MATURITY"]);
        //    lblCutOffDate.Content = string.Format("{0:MM/dd/yyyy}", dataRow["MATURITY"]);

        //    chkWithCollateral.IsChecked = DataConverter.ToBoolean(dataRow["COLLAT"]);
        //    chkWithCollateral.IsEnabled = false;
        //    lblCollateralDescription.Content = string.Format("{0}", dataRow["DESC"]);

        //    lblRealeaseNo.Content = string.Format("{0}", dataRow["RELEASE_NO"]);
        //    lblReleaseDate.Content = string.Format("{0:MM/dd/yyyy}", dataRow["RELEASED"]);
        //    lblInterestRate.Content = string.Format("{0:P}", dataRow["INT_RATE"]);
        //    lblTerms.Content = string.Format("{0}", dataRow["TERMS"]);
        //    lblModeOfPayment.Content = string.Format("{0:N2}", dataRow["MODE_PAY"]);

        //    lblCoMaker1.Content = string.Format("{0} - {1}", dataRow["CO_CODE1"], dataRow["CO_NAME1"]);
        //    lblCoMaker2.Content = string.Format("{0} - {1}", dataRow["CO_CODE2"], dataRow["CO_NAME2"]);
        //    lblCoMaker3.Content = string.Format("{0} - {1}", dataRow["CO_CODE3"], dataRow["CO_NAME3"]);
        //}

        //public LoanDetailsWindow(Models.CashVoucher currentItem)
        //{
        //    InitializeComponent();
        //    lblBorrower.Content = string.Format("{0} - {1}", currentItem.MemberCode, currentItem.MemberName);
        //    lblLoanApplied.Content = string.Format("{0} - {1}", currentItem.AccountCode, currentItem.AccountTitle);

        //    lblReference.Content = string.Format("{0} {1}", currentItem.VoucherType, currentItem.VoucherNo);
        //    lblDate.Content = string.Format("{0:MM/dd/yyyy}", currentItem.VoucherDate);

        //    lblBank.Content = string.Format("{0}", currentItem.BankTitle);
        //    lblCheckNo.Content = string.Format("{0}", currentItem.CheckNo1);

        //    lblLoanAmount.Content = string.Format("{0:N2}", currentItem.LoanDetails.LoanAmount);
        //    lblPayment.Content = string.Format("{0:N2}", currentItem.LoanDetails.Payment);
        //    lblInterestAmount.Content = string.Format("{0:N2}", currentItem.LoanDetails.InterestAmount);
        //    lblInterestAmort.Content = string.Format("{0:N2}", currentItem.LoanDetails.InterestAmortization);

        //    lblDateGranted.Content = string.Format("{0:MM/dd/yyyy}", currentItem.LoanDetails.GrantedDate);
        //    lblMaturity.Content = string.Format("{0:MM/dd/yyyy}", currentItem.LoanDetails.MaturityDate);
        //    lblCutOffDate.Content = string.Format("{0:MM/dd/yyyy}", currentItem.LoanDetails.CutOffDate);

        //    chkWithCollateral.IsChecked = currentItem.LoanDetails.IsWithCollateral;
        //    chkWithCollateral.IsEnabled = false;
        //    lblCollateralDescription.Content = string.Format("{0}", currentItem.LoanDetails.Description);

        //    lblRealeaseNo.Content = string.Format("{0}", currentItem.LoanDetails.ReleaseNo);
        //    lblReleaseDate.Content = string.Format("{0:MM/dd/yyyy}", currentItem.LoanDetails.DateReleased);
        //    lblInterestRate.Content = string.Format("{0:P}", currentItem.LoanDetails.InterestRate);
        //    lblTerms.Content = string.Format("{0} {1}", currentItem.LoanDetails.LoanTerms,
        //                                     currentItem.LoanDetails.TermsMode);
        //    lblModeOfPayment.Content = string.Format("{0}", currentItem.LoanDetails.ModeOfPayment);

        //    lblCoMaker1.Content = string.Format("{0} - {1}", currentItem.LoanDetails.CoMakers[0].MemberCode, currentItem.LoanDetails.CoMakers[0].MemberName);
        //    lblCoMaker2.Content = string.Format("{0} - {1}", currentItem.LoanDetails.CoMakers[1].MemberCode, currentItem.LoanDetails.CoMakers[1].MemberName);
        //    lblCoMaker3.Content = string.Format("{0} - {1}", currentItem.LoanDetails.CoMakers[2].MemberCode, currentItem.LoanDetails.CoMakers[2].MemberName);
        //}

        //public LoanDetailsWindow(Models.JournalVoucher currentItem)
        //{
        //    InitializeComponent();
        //    lblBorrower.Content = string.Format("{0} - {1}", currentItem.MemberCode, currentItem.MemberName);
        //    lblLoanApplied.Content = string.Format("{0} - {1}", currentItem.AccountCode, currentItem.AccountTitle);

        //    lblReference.Content = string.Format("{0} {1}", currentItem.VoucherType, currentItem.VoucherNo);
        //    lblDate.Content = string.Format("{0:MM/dd/yyyy}", currentItem.VoucherDate);

        //    lblBank.Content = string.Format("{0}", currentItem.BankTitle);
        //    lblCheckNo.Content = string.Format("{0}", currentItem.CheckNo1);

        //    lblLoanAmount.Content = string.Format("{0:N2}", currentItem.LoanDetails.LoanAmount);
        //    lblPayment.Content = string.Format("{0:N2}", currentItem.LoanDetails.Payment);
        //    lblInterestAmount.Content = string.Format("{0:N2}", currentItem.LoanDetails.InterestAmount);
        //    lblInterestAmort.Content = string.Format("{0:N2}", currentItem.LoanDetails.InterestAmortization);

        //    lblDateGranted.Content = string.Format("{0:MM/dd/yyyy}", currentItem.LoanDetails.GrantedDate);
        //    lblMaturity.Content = string.Format("{0:MM/dd/yyyy}", currentItem.LoanDetails.MaturityDate);
        //    lblCutOffDate.Content = string.Format("{0:MM/dd/yyyy}", currentItem.LoanDetails.CutOffDate);

        //    chkWithCollateral.IsChecked = currentItem.LoanDetails.IsWithCollateral;
        //    chkWithCollateral.IsEnabled = false;
        //    lblCollateralDescription.Content = string.Format("{0}", currentItem.LoanDetails.Description);

        //    lblRealeaseNo.Content = string.Format("{0}", currentItem.LoanDetails.ReleaseNo);
        //    lblReleaseDate.Content = string.Format("{0:MM/dd/yyyy}", currentItem.LoanDetails.DateReleased);
        //    lblInterestRate.Content = string.Format("{0:P}", currentItem.LoanDetails.InterestRate);
        //    lblTerms.Content = string.Format("{0}", currentItem.LoanDetails.LoanTerms);
        //    lblModeOfPayment.Content = string.Format("{0}", currentItem.LoanDetails.ModeOfPayment);

        //    lblCoMaker1.Content = string.Format("{0} - {1}", currentItem.LoanDetails.CoMakers[0].MemberCode, currentItem.LoanDetails.CoMakers[0].MemberName);
        //    lblCoMaker2.Content = string.Format("{0} - {1}", currentItem.LoanDetails.CoMakers[1].MemberCode, currentItem.LoanDetails.CoMakers[1].MemberName);
        //    lblCoMaker3.Content = string.Format("{0} - {1}", currentItem.LoanDetails.CoMakers[2].MemberCode, currentItem.LoanDetails.CoMakers[2].MemberName);
        //}

        //public LoanDetailsWindow(Models.AccountVerifier.AccountDetail currentItem)
        //{
        //    InitializeComponent();
        //    lblBorrower.Content = string.Format("{0} - {1}", currentItem.MemberCode, currentItem.MemberName);
        //    lblLoanApplied.Content = string.Format("{0} - {1}", currentItem.AccountCode, currentItem.Title);

        //    lblReference.Content = string.Format("{0} {1}", currentItem.VoucherType, currentItem.VoucherNumber);
        //    lblDate.Content = string.Format("{0:MM/dd/yyyy}", currentItem.VoucherDate);

        //    lblBank.Content = string.Format("{0}", currentItem.BankTitle);
        //    lblCheckNo.Content = string.Format("{0}", currentItem.CheckNum1);

        //    lblLoanAmount.Content = string.Format("{0:N2}", currentItem.LoanDetails.LoanAmount);
        //    lblPayment.Content = string.Format("{0:N2}", currentItem.LoanDetails.Payment);
        //    lblInterestAmount.Content = string.Format("{0:N}", currentItem.LoanDetails.InterestAmount);
        //    lblInterestAmort.Content = string.Format("{0:N}", currentItem.LoanDetails.InterestAmortization);

        //    lblDateGranted.Content = string.Format("{0:MM/dd/yyyy}", currentItem.LoanDetails.GrantedDate);
        //    lblMaturity.Content = string.Format("{0:MM/dd/yyyy}", currentItem.LoanDetails.MaturityDate);
        //    lblCutOffDate.Content = string.Format("{0:MM/dd/yyyy}", currentItem.LoanDetails.CutOffDate);

        //    chkWithCollateral.IsChecked = currentItem.LoanDetails.IsWithCollateral;
        //    chkWithCollateral.IsEnabled = false;
        //    lblCollateralDescription.Content = string.Format("{0}", currentItem.LoanDetails.Description);

        //    lblRealeaseNo.Content = string.Format("{0}", currentItem.LoanDetails.ReleaseNo);
        //    lblReleaseDate.Content = string.Format("{0:MM/dd/yyyy}", currentItem.LoanDetails.DateReleased);
        //    lblInterestRate.Content = string.Format("{0:P}", currentItem.LoanDetails.InterestRate);
        //    lblTerms.Content = string.Format("{0} {1}", currentItem.LoanDetails.LoanTerms,
        //                                     currentItem.LoanDetails.TermsMode);
        //    lblModeOfPayment.Content = string.Format("{0}", currentItem.LoanDetails.ModeOfPayment);

        //    lblCoMaker1.Content = string.Format("{0} - {1}", currentItem.LoanDetails.CoMakers[0].MemberCode, currentItem.LoanDetails.CoMakers[0].MemberName);
        //    lblCoMaker2.Content = string.Format("{0} - {1}", currentItem.LoanDetails.CoMakers[1].MemberCode, currentItem.LoanDetails.CoMakers[1].MemberName);
        //    lblCoMaker3.Content = string.Format("{0} - {1}", currentItem.LoanDetails.CoMakers[2].MemberCode, currentItem.LoanDetails.CoMakers[2].MemberName);
        //}
    }
}
