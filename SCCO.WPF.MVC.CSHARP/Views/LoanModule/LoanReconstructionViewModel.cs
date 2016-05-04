using System;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    internal class LoanReconstructionViewModel : ViewModelBase
    {
        private LoanDetails _loanDetails;

        /// <summary>
        /// The reconstructed Loan Details
        /// </summary>
        public LoanDetails LoanDetails
        {
            get { return _loanDetails; }
            set { _loanDetails = value; OnPropertyChanged("LoanDetails"); }
        }

        /// <summary>
        /// Journal Voucher Number
        /// </summary>
        public int DocumentNumber { get; set; }

        /// <summary>
        /// Posting Date
        /// </summary>
        public DateTime DocumentDate { get; set; }

        /// <summary>
        /// OR number if interest and penalty are paid
        /// </summary>
        public string OfficialReceiptNumber { get; set; }

        public decimal InterestRebate { get; set; }

        public string InterestRebateAccountCode { get; set; }

        public Nfmb Member { get; set; }

        public Account LoanApplied { get; set; }

        public ReconstructionTypes ReconstructionTypes { get; set; }

        public decimal LoanBalance { get; set; }

        public LoanComputation LoanComputation { get; set; }

        public void ReconstructLoan()
        {
            switch (ReconstructionTypes)
            {
                case ReconstructionTypes.PaidInterest:
                    PaidInterestReconstruction();
                    break;

                case ReconstructionTypes.AddOnInterest:
                    AddOnInterestReconstruction();
                    break;

                case ReconstructionTypes.Restructed:
                    RestructureLoan();
                    break;
            }
        }

        private Result RestructureLoan()
        {
            return new Result(false, "Not yet implemented");
        }

        private Result PaidInterestReconstruction()
        {
            string explanation = GetReconstructionExplanation(ReconstructionTypes.PaidInterest);
            var document = new VoucherDocument { Date = DocumentDate, Type = VoucherTypes.JV, Number = DocumentNumber };

            // enter first loan settlement entry
            var jv = new JournalVoucher();
            jv.SetMember(Member);
            jv.SetAccount(LoanApplied);
            jv.SetDocument(document);
            jv.Credit = LoanBalance;
            var result = jv.Create();
            if (result.Success)
            {
                JournalVoucher.DeleteAll(document.Number);
                return result;
            }

            jv = new JournalVoucher();
            jv.SetMember(Member);
            jv.SetAccount(LoanApplied);
            jv.SetDocument(document);
            jv.Debit = LoanBalance;
            jv.LoanDetails = LoanDetails;
            jv.Explanation = explanation + Environment.NewLine + LoanDetails.GenerateExplanation();
            result = jv.Create();
            if (!result.Success)
            {
                JournalVoucher.DeleteAll(document.Number);
                return result;
            }

            #region --- Voucher Log ---

            var voucherLog = new VoucherLog();
            voucherLog.Find(document.Type.ToString(), document.Number);
            voucherLog.Date = document.Date;
            voucherLog.Initials = MainController.LoggedUser.Initials;

            voucherLog.Save();

            #endregion

            return result;
        }

        private Result AddOnInterestReconstruction()
        {
            string explanation = GetReconstructionExplanation(ReconstructionTypes.PaidInterest);
            var document = new VoucherDocument { Date = DocumentDate, Type = VoucherTypes.JV, Number = DocumentNumber };

            // entry to settle loan
            var result = AddVoucherEntry(LoanApplied, LoanBalance, "Credit");
            if (result.Success)
            {
                JournalVoucher.DeleteAll(document.Number);
                return result;
            }

            // entry for fines/penalty
            decimal charges = PostCharges();
            decimal deductions = PostDeductions();

            // entry for reconstructed loan
            var jv = new JournalVoucher();
            jv.SetMember(Member);
            jv.SetAccount(LoanApplied);
            jv.SetDocument(document);
            jv.Debit = LoanBalance + charges + deductions;
            jv.LoanDetails = LoanDetails;
            jv.Explanation = explanation + Environment.NewLine + LoanDetails.GenerateExplanation();
            result = jv.Create();
            if (!result.Success)
            {
                JournalVoucher.DeleteAll(document.Number);
                return result;
            }

            #region --- Voucher Log ---

            var voucherLog = new VoucherLog();
            voucherLog.Find(document.Type.ToString(), document.Number);
            voucherLog.Date = document.Date;
            voucherLog.Initials = MainController.LoggedUser.Initials;

            voucherLog.Save();

            #endregion
            return result;
        }

        private string GetReconstructionExplanation(ReconstructionTypes reconstructionTypes)
        {
            switch (reconstructionTypes)
            {
                case ReconstructionTypes.PaidInterest:
                    return "Paid Interest Loan Reconstruction, Reference OR#" + OfficialReceiptNumber;

                case ReconstructionTypes.AddOnInterest:
                    return "Add-On Interest Loan Reconstruction";

                case ReconstructionTypes.Restructed:
                    return "Loan Restructured";
            }
            return string.Empty;
        }

        private decimal PostCharges()
        {
            // TODO: post to journal voucher all charges
            var accounts = new string[6];
            accounts[0] = LoanComputation.ChargeCode1;
            accounts[1] = LoanComputation.ChargeCode2;
            accounts[2] = LoanComputation.ChargeCode3;
            accounts[3] = LoanComputation.ChargeCode4;
            accounts[4] = LoanComputation.ChargeCode5;
            accounts[5] = LoanComputation.ChargeCode6;

            var amounts = new decimal[6];
            amounts[0] = LoanComputation.ChargeAmount1;
            amounts[1] = LoanComputation.ChargeAmount2;
            amounts[2] = LoanComputation.ChargeAmount3;
            amounts[3] = LoanComputation.ChargeAmount4;
            amounts[4] = LoanComputation.ChargeAmount5;
            amounts[5] = LoanComputation.ChargeAmount6;

            var charges = 0m;
            for (var i = 0; i < amounts.Length; i++)
            {
                if (amounts[i] == 0m)
                {
                    continue;
                }
                if (AddVoucherEntry(Account.FindByCode(accounts[i]), amounts[i], "Credit").Success)
                {
                    charges += amounts[i];
                }
            }
            return charges;
        }

        private Result AddVoucherEntry(Account account, decimal amount, string nature)
        {
            var document = new VoucherDocument { Date = DocumentDate, Type = VoucherTypes.JV, Number = DocumentNumber };

            var jv = new JournalVoucher();
            jv.SetMember(Member);
            jv.SetAccount(account);
            jv.SetDocument(document);
            if (nature.ToUpper().Substring(1, 1) == "D")
            {
                jv.Debit = amount;
            }
            return jv.Create();
        }

        private decimal PostDeductions()
        {
            // TODO: post to journal voucher all charges
            var accounts = new string[6];
            accounts[0] = LoanComputation.DeductCode1;
            accounts[1] = LoanComputation.DeductCode2;
            accounts[2] = LoanComputation.DeductCode3;
            accounts[3] = LoanComputation.DeductCode4;
            accounts[4] = LoanComputation.DeductCode5;
            accounts[5] = LoanComputation.DeductCode6;

            var amounts = new decimal[6];
            amounts[0] = LoanComputation.DeductAmount1;
            amounts[1] = LoanComputation.DeductAmount2;
            amounts[2] = LoanComputation.DeductAmount3;
            amounts[3] = LoanComputation.DeductAmount4;
            amounts[4] = LoanComputation.DeductAmount5;
            amounts[5] = LoanComputation.DeductAmount6;

            var deductions = 0m;
            for (var i = 0; i < amounts.Length; i++)
            {
                if (amounts[i] == 0m)
                {
                    continue;
                }
                if (AddVoucherEntry(Account.FindByCode(accounts[i]), amounts[i], "Credit").Success)
                {
                    deductions += amounts[i];
                }
            }
            return deductions;
        }
    }

    internal enum ReconstructionTypes
    {
        PaidInterest,
        AddOnInterest,
        Restructed
    }
}
