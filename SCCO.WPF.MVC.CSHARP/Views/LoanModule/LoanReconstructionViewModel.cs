using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Extensions;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    internal class LoanReconstructionViewModel : ViewModelBase
    {
        private decimal _amountForReconstruction;
        private Account _finesAndPenaltyAccount;
        private Account _interestRebateAccount;
        private Account _loanAccount;
        private decimal _loanBalance;
        private LoanComputation _loanComputation;
        private LoanProduct _loanProduct;
        private List<DefaultModel> _loanProductTerms;
        private LoanDetails _newLoanDetails;
        private ObservableCollection<Particular> _particulars;
        private LoanDetails _previousLoanDetails;
        private DateTime _reconstructionDate;
        private Particular _selectedParticular;
        private Account _seniorMembersAssistanceProgramAccount;
        private Account _unearnedIncomeAccount;
        private decimal _totalChargesAndDeductions;
        private int _orNumber;

        public decimal LoanBalance
        {
            get { return _loanBalance; }
            set
            {
                _loanBalance = value;
                OnPropertyChanged("LoanBalance");
            }
        }

        public decimal AmountForReconstruction
        {
            get { return _amountForReconstruction; }
            set
            {
                _amountForReconstruction = value;
                OnPropertyChanged("AmountForReconstruction");
            }
        }

        public LoanDetails PreviousLoanDetails
        {
            get { return _previousLoanDetails; }
            set
            {
                _previousLoanDetails = value;
                OnPropertyChanged("PreviousLoanDetails");
            }
        }

        public LoanDetails NewLoanDetails
        {
            get { return _newLoanDetails; }
            set
            {
                _newLoanDetails = value;
                OnPropertyChanged("NewLoanDetails");
            }
        }

        public LoanProduct LoanProduct
        {
            get { return _loanProduct; }
            set
            {
                _loanProduct = value;
                OnPropertyChanged("LoanProduct");
            }
        }

        public LoanComputation LoanComputation
        {
            get { return _loanComputation; }
            set
            {
                _loanComputation = value;
                OnPropertyChanged("LoanComputation");
            }
        }

        public ObservableCollection<Particular> Particulars
        {
            get { return _particulars; }
            set
            {
                _particulars = value;
                OnPropertyChanged("Particulars");
            }
        }

        public List<DefaultModel> LoanProductTerms
        {
            get { return _loanProductTerms; }
            set
            {
                _loanProductTerms = value;
                OnPropertyChanged("LoanProductTerms");
            }
        }

        public Nfmb Borrower { get; set; }

        public Account LoanAccount
        {
            get { return _loanAccount; }
            set
            {
                _loanAccount = value;
                OnPropertyChanged("LoanAccount");
            }
        }

        public DateTime ReconstructionDate
        {
            get { return _reconstructionDate; }
            set
            {
                _reconstructionDate = value;
                OnPropertyChanged("ReconstructionDate");
            }
        }

        public Account InterestRebateAccount
        {
            get { return _interestRebateAccount; }
            set
            {
                _interestRebateAccount = value;
                OnPropertyChanged("InterestRebateAccount");
            }
        }

        public Account FinesAndPenaltyAccount
        {
            get { return _finesAndPenaltyAccount; }
            set
            {
                _finesAndPenaltyAccount = value;
                OnPropertyChanged("FinesAndPenaltyAccount");
            }
        }

        public Account UnearnedIncomeAccount
        {
            get { return _unearnedIncomeAccount; }
            set
            {
                _unearnedIncomeAccount = value;
                OnPropertyChanged("UnearnedIncomeAccount");
            }
        }

        public Account SeniorMembersAssistanceProgramAccount
        {
            get { return _seniorMembersAssistanceProgramAccount; }
            set
            {
                _seniorMembersAssistanceProgramAccount = value;
                OnPropertyChanged("SeniorMembersAssistanceProgramAccount");
            }
        }

        public Account CoopPurchaseOrderAccount { get; set; }

        public Account GoNegosyoAccount { get; set; }

        public Particular SelectedParticular
        {
            get { return _selectedParticular; }
            set
            {
                _selectedParticular = value;
                OnPropertyChanged("SelectedParticular");
            }
        }

        public User LoanManager { get; set; }

        public ReconstructionTypes ReconstructionType { get; set; }

        public int OrNumber
        {
            get { return _orNumber; }
            set
            {
                _orNumber = value;
                OnPropertyChanged("OrNumber");
            }
        }

        public decimal TotalChargesAndDeductions
        {
            get { return _totalChargesAndDeductions; }
            set { _totalChargesAndDeductions = value; OnPropertyChanged("TotalChargesAndDeductions"); }
        }

        public void InitializeContext()
        {
            // properties initialization
            Borrower = Nfmb.FindByCode(PreviousLoanDetails.MemberCode);
            LoanAccount = Account.FindByCode(PreviousLoanDetails.AccountCode);
            ConfigureAccounts();

            Particulars = new ObservableCollection<Particular>();
            //1. find closest loan product based on previous loan details
            SetLoanProduct();
            SetLoanProductTerms();

            //2. initialize new loan detail before setting loan computation
            SetNewLoanDetail();

            //3. let the LoanComputation model do the work for charges and deductions
            SetLoanComputation();

            InsertChargesAndDeductions();

            //4. append Rebates/Penalty
            AppendInterestRebateOrPenalty();

            // insert other deductions
            AddOrEditSmap();

            //5. Update loan amount for reconstruction
            UpdateLoanAmountForReconstruction();

            //6. Update New Loan Details
            UpdateLoanDetails();
        }

        public void RemoveSelectedParticular()
        {
            if (SelectedParticular != null)
            {
                Particulars.Remove(SelectedParticular);
                SelectedParticular = Particulars.LastOrDefault();
                UpdateLoanAmountForReconstruction();
                UpdateLoanDetails();
            }
        }

        internal Nfmb FindCoMaker(string code)
        {
            return Nfmb.FindByCode(code);
        }

        public void AddOrEditSmap()
        {
            if (IsSpecialLoan())
            {
                return;
            }
            if (SeniorMembersAssistanceProgramAccount == null)
            {
                return;
            }
            
            var totalDeductions = LoanBalance;
            Particular smap = null;
            foreach (var particular in Particulars)
            {
                if (particular.AccountCode != SeniorMembersAssistanceProgramAccount.AccountCode)
                {
                    totalDeductions += particular.Amount;
                }
                else
                {
                    smap = particular;
                }
            }

            var amount = 10; // default amount

            // if loan term is 1 year or above
            // smap = P1 for every P1000
            if (NewLoanDetails.LoanTerms >= 12) //gteq 1 year
            {
                var jea = (int) (totalDeductions/1000);
                amount = jea;
            }
            else
            {
                // if loan amount is P20K or above, P20, 
                if (totalDeductions > 20000)
                {
                    amount = 20;
                }
            }

            if (smap == null)
            {
                smap = new Particular
                    {
                        AccountCode = SeniorMembersAssistanceProgramAccount.AccountCode,
                        AccountTitle = SeniorMembersAssistanceProgramAccount.AccountTitle,
                        Amount = amount
                    };
                Particulars.Add(smap);
            }
            else
            {
                smap.Amount = amount;
            }
        }

        private void SetLoanProduct()
        {
            var conditions = new Dictionary<string, object>
                {
                    {"ProductCode", PreviousLoanDetails.AccountCode},
                    {"AnnualInterestRate", PreviousLoanDetails.InterestRate}
                };
            foreach (var item in LoanProduct.Where(conditions))
            {
                LoanProduct = item;
                return;
            }
            // if no loan product was found, provide default
            conditions = new Dictionary<string, object>
                {
                    {"ProductCode", PreviousLoanDetails.AccountCode},
                };
            foreach (var item in LoanProduct.Where(conditions))
            {
                LoanProduct = item;
                return;
            }
        }

        private void SetLoanProductTerms()
        {
            var terms = new List<DefaultModel>();
            for (var i = LoanProduct.MinimumTerm; i <= LoanProduct.MaximumTerm; i++)
            {
                var item = new DefaultModel {ID = i, Name = string.Format("{0} {1}", i, i == 1 ? "month" : "months")};
                terms.Add(item);
            }
            LoanProductTerms = terms;
        }

        private void SetNewLoanDetail()
        {
            NewLoanDetails = new LoanDetails
                {
                    MemberCode = PreviousLoanDetails.MemberCode,
                    MemberName = PreviousLoanDetails.MemberName,
                    AccountCode = PreviousLoanDetails.AccountCode,
                    AccountTitle = PreviousLoanDetails.AccountTitle,
                    LoanAmount =
                        ReconstructionType == ReconstructionTypes.AddOnInterest
                            ? LoanBalance
                            : PreviousLoanDetails.LoanAmount,
                    InterestRate = LoanProduct.AnnualInterestRate,
                    GrantedDate = ReconstructionDate,
                    LoanTerms =
                        PreviousLoanDetails.LoanTerms.IsBetween(LoanProduct.MinimumTerm, LoanProduct.MaximumTerm)
                            ? PreviousLoanDetails.LoanTerms
                            : LoanProduct.MinimumTerm
                };

            for (var i = 0; i < 3; i++)
            {
                NewLoanDetails.CoMakers[i] = PreviousLoanDetails.CoMakers[i];
            }
        }

        private void SetLoanComputation()
        {
            LoanComputation = new LoanComputation(NewLoanDetails, LoanProduct);
        }

        private void InsertChargesAndDeductions()
        {
            Particulars.Clear();

            if (IsSpecialLoan())
            {
                return;
            }

            foreach (var charge in LoanComputation.Charges.OrderBy(t => t.AccountCode))
            {
                var item = new Particular
                    {
                        AccountCode = charge.AccountCode,
                        AccountTitle = charge.AccountTitle,
                        Amount = charge.Amount
                    };
                Particulars.Add(item);
            }

            foreach (var deduction in LoanComputation.Deductions.OrderBy(t => t.AccountCode))
            {
                var item = new Particular
                    {
                        AccountCode = deduction.AccountCode,
                        AccountTitle = deduction.AccountTitle,
                        Amount = deduction.Amount
                    };
                Particulars.Add(item);
            }
        }

        private void AppendInterestRebateOrPenalty()
        {
            if (IsSpecialLoan())
            {
                AppendSpecialLoanCharges();
                return;
            }

            var model = new FinesRebateCalculatorViewModel
                {
                    LoanDetails = PreviousLoanDetails,
                    ProcessDate = ReconstructionDate,
                    LoanBalance = IsOverDue(PreviousLoanDetails.MaturityDate, ReconstructionDate) ? LoanBalance : 0
                };

            model.Calculate();

            if (model.Rebate != 0)
            {
                var rebate = InterestRebateAccount;
                var item = new Particular
                    {
                        AccountCode = rebate.AccountCode,
                        AccountTitle = rebate.AccountTitle,
                        Amount = Math.Abs(Math.Round(model.Rebate, 2))*-1
                    };
                Particulars.Add(item);
            }
            if (model.Fines != 0)
            {
                var fines = FinesAndPenaltyAccount;
                var item = new Particular
                    {
                        AccountCode = fines.AccountCode,
                        AccountTitle = fines.AccountTitle,
                        Amount = Math.Round(model.Fines, 2) + Math.Round(model.Interest)
                    };
                Particulars.Add(item);
            }
        }

        private bool IsOverDue(DateTime maturityDate, DateTime presentDate)
        {
            return presentDate > maturityDate;
        }

        private void UpdateLoanAmountForReconstruction()
        {
            AmountForReconstruction = LoanBalance;
            TotalChargesAndDeductions = 0m;
            foreach (var particular in Particulars)
            {
                TotalChargesAndDeductions += particular.Amount;
            }
        }

        public void UpdateLoanDetails()
        {
            NewLoanDetails.LoanAmount = AmountForReconstruction;
            var lah = LoanAmortizationController.GenerateLoanAmortization(
                NewLoanDetails.MemberCode,
                NewLoanDetails.AccountCode,
                NewLoanDetails.LoanAmount,
                NewLoanDetails.LoanTerms,
                NewLoanDetails.InterestRate,
                NewLoanDetails.GrantedDate,
                LoanProduct.MonthlyCapitalBuildUp
                );

            NewLoanDetails.InterestAmount = lah.PaymentSchedules.Sum(t => t.Interest);
            var firstPayment = lah.PaymentSchedules.First();

            NewLoanDetails.InterestAmortization = 0m;
            NewLoanDetails.Payment = 0m;

            if (firstPayment != null)
            {
                NewLoanDetails.InterestAmortization = firstPayment.Interest;
                var interest = firstPayment.Interest;
                if (IsSpecialLoan())
                {
                    interest = 0m;
                }
                NewLoanDetails.Payment = firstPayment.Payment + interest;
            }

            NewLoanDetails.MaturityDate = NewLoanDetails.GrantedDate.AddMonths(NewLoanDetails.LoanTerms);
            NewLoanDetails.CutOffDate = lah.FirstPaymentDate;
            NewLoanDetails.ModeOfPayment = ModeOfPayments.Monthly;
            NewLoanDetails.TermsMode = "MO";
        }

        public Result PostAddOnInterestReconstruction(int documentNumber, DateTime documentDate)
        {
            //1. loan
            var document = new VoucherDocument(VoucherTypes.JV, documentNumber, documentDate);
            var jv = new JournalVoucher();
            jv.SetDocument(document);
            jv.SetMember(Borrower);
            jv.SetAccount(Account.FindByCode(PreviousLoanDetails.AccountCode));
            jv.Credit = LoanBalance;

            jv.IsPosted = true;
            var result = jv.Create();
            if (!result.Success)
            {
                JournalVoucher.DeleteAll(documentNumber);
                return result;
            }

            //2. charges and deductions
            foreach (var particular in Particulars)
            {
                jv = new JournalVoucher();
                jv.SetDocument(document);
                jv.SetMember(Borrower);
                jv.AccountCode = particular.AccountCode;
                jv.AccountTitle = particular.AccountTitle;
                jv.Credit = particular.Amount;

                jv.IsPosted = true;
                result = jv.Create();
                if (!result.Success)
                {
                    JournalVoucher.DeleteAll(documentNumber);
                    return result;
                }
            }

            //3. Unearned Income
            jv = new JournalVoucher();
            jv.SetDocument(document);
            jv.SetMember(Borrower);
            jv.SetAccount(UnearnedIncomeAccount);
            jv.Credit = NewLoanDetails.InterestAmount;

            jv.IsPosted = true;
            result = jv.Create();
            if (!result.Success)
            {
                JournalVoucher.DeleteAll(documentNumber);
                return result;
            }

            //4. Reconstructed Loan
            jv = new JournalVoucher();
            jv.SetDocument(document);
            jv.SetMember(Borrower);
            jv.SetAccount(LoanAccount);
            jv.Debit = NewLoanDetails.LoanAmount + NewLoanDetails.InterestAmount + TotalChargesAndDeductions;

            NewLoanDetails.ReleaseNo = ModelController.Releases.MaxReleaseNumber() + 1;
            NewLoanDetails.DateReleased = ReconstructionDate;
            jv.LoanDetails = NewLoanDetails;
            jv.Explanation = "Add-On-Interest Loan Reconstruction: " + NewLoanDetails.GenerateExplanation();
            jv.Amount = jv.Debit;
            jv.AmountInWords = Converter.AmountToWords(jv.Debit);
            jv.IsPosted = true;
            result = jv.Create();
            if (!result.Success)
            {
                JournalVoucher.DeleteAll(documentNumber);
                return result;
            }

            #region --- Voucher Log ---

            var voucherLog = new VoucherLog();
            voucherLog.Find("JV", documentNumber);
            voucherLog.Date = ReconstructionDate;
            voucherLog.Initials = LoanManager.Initials;

            voucherLog.Save();

            #endregion

            return result;
        }

        public Result PostPaidInterestReconstruction(int documentNumber, DateTime documentDate)
        {
            //1. loan
            var document = new VoucherDocument(VoucherTypes.JV, documentNumber, documentDate);
            var jv = new JournalVoucher();
            jv.SetDocument(document);
            jv.SetMember(Borrower);
            jv.SetAccount(Account.FindByCode(PreviousLoanDetails.AccountCode));
            jv.Credit = LoanBalance;

            jv.IsPosted = true;
            var result = jv.Create();
            if (!result.Success)
            {
                JournalVoucher.DeleteAll(documentNumber);
                return result;
            }

            //2. charges and deductions
            foreach (var particular in Particulars)
            {
                jv = new JournalVoucher();
                jv.SetDocument(document);
                jv.SetMember(Borrower);
                jv.AccountCode = particular.AccountCode;
                jv.AccountTitle = particular.AccountTitle;
                jv.Credit = particular.Amount;

                jv.IsPosted = true;
                result = jv.Create();
                if (!result.Success)
                {
                    JournalVoucher.DeleteAll(documentNumber);
                    return result;
                }
            }

            var interestAmount = 0m;
            if (!IsSpecialLoan())
            {
                interestAmount = NewLoanDetails.InterestAmount;

                //3. Unearned Income
                jv = new JournalVoucher();
                jv.SetDocument(document);
                jv.SetMember(Borrower);
                jv.SetAccount(UnearnedIncomeAccount);
                jv.Credit = NewLoanDetails.InterestAmount;

                jv.IsPosted = true;
                result = jv.Create();
                if (!result.Success)
                {
                    JournalVoucher.DeleteAll(documentNumber);
                    return result;
                }
            }

            //4. Reconstructed Loan
            jv = new JournalVoucher();
            jv.SetDocument(document);
            jv.SetMember(Borrower);
            jv.SetAccount(LoanAccount);
            jv.Debit = NewLoanDetails.LoanAmount + interestAmount + TotalChargesAndDeductions;

            NewLoanDetails.ReleaseNo = ModelController.Releases.MaxReleaseNumber() + 1;
            NewLoanDetails.DateReleased = ReconstructionDate;
            jv.LoanDetails = NewLoanDetails;
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Paid Interest Loan Reconstruction: OR# " + OrNumber);
            messageBuilder.AppendLine(NewLoanDetails.GenerateExplanation());
            jv.Explanation = messageBuilder.ToString();
            jv.Amount = jv.Debit;
            jv.AmountInWords = Converter.AmountToWords(jv.Debit);
            jv.IsPosted = true;
            result = jv.Create();
            if (!result.Success)
            {
                JournalVoucher.DeleteAll(documentNumber);
                return result;
            }

            VoucherLog.Log(VoucherTypes.JV, documentNumber, ReconstructionDate, LoanManager.Initials);
            return result;
        }

        public List<SearchItem> GetLoanProductsSearchItems()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT ID, `Name`, ProductCode FROM loan_products");
            return (from DataRow dataRow in dataTable.Rows
                    let id = DataConverter.ToInteger(dataRow["ID"])
                    let name = DataConverter.ToString(dataRow["Name"])
                    let productCode = DataConverter.ToString(dataRow["ProductCode"])
                    select new SearchItem(id, name) {ItemCode = productCode}).ToList();
        }

        public void SetLoanProduct(LoanProduct loanProduct)
        {
            LoanProduct = loanProduct;
            LoanAccount = Account.FindByCode(LoanProduct.ProductCode);
            SetLoanProductTerms();
            NewLoanDetails.InterestRate = LoanProduct.AnnualInterestRate;
            if (!NewLoanDetails.LoanTerms.IsBetween(LoanProduct.MinimumTerm, LoanProduct.MaximumTerm))
            {
                NewLoanDetails.LoanTerms = LoanProduct.MinimumTerm;
            }
            LoanComputation = new LoanComputation(PreviousLoanDetails, LoanProduct);

            InsertChargesAndDeductions();
            AppendInterestRebateOrPenalty();
            AddOrEditSmap();
            UpdateLoanAmountForReconstruction();
            UpdateLoanDetails();
        }

        public Result Validate()
        {
            if (PreviousLoanDetails == null || (PreviousLoanDetails.LoanAmount == 0m))
            {
                return new Result(false, "Previous Loan Details not set.");
            }
            if (FinesAndPenaltyAccount == null || string.IsNullOrEmpty(FinesAndPenaltyAccount.AccountCode))
            {
                return new Result(false, "Fines and Penalty account not set.");
            }
            if (InterestRebateAccount == null || string.IsNullOrEmpty(InterestRebateAccount.AccountCode))
            {
                return new Result(false, "Interest Rebate account not set.");
            }
            if (UnearnedIncomeAccount == null || string.IsNullOrEmpty(UnearnedIncomeAccount.AccountCode))
            {
                return new Result(false, "Unearned Income account not set.");
            }
            if (SeniorMembersAssistanceProgramAccount == null ||
                string.IsNullOrEmpty(SeniorMembersAssistanceProgramAccount.AccountCode))
            {
                return new Result(false, "Senior Member Assistance Program account not set.");
            }

            if (GoNegosyoAccount == null || string.IsNullOrEmpty(GoNegosyoAccount.AccountCode))
            {
                return new Result(false, "Go Negosyo account not set.");
            }

            if (CoopPurchaseOrderAccount == null || string.IsNullOrEmpty(CoopPurchaseOrderAccount.AccountCode))
            {
                return new Result(false, "COOP Purchase Order account not set.");
            }
            return new Result(true, "Valid");
        }

        public void ConfigureAccounts()
        {
            var code = GlobalSettings.CodeOfFinesAndPenalty;
            if (!string.IsNullOrEmpty(code))
            {
                FinesAndPenaltyAccount = Account.FindByCode(code);
            }

            code = GlobalSettings.CodeOfInterestRebate;
            if (!string.IsNullOrEmpty(code))
            {
                InterestRebateAccount = Account.FindByCode(code);
            }

            code = GlobalSettings.CodeOfUnearnedIncome;
            if (!string.IsNullOrEmpty(code))
            {
                UnearnedIncomeAccount = Account.FindByCode(code);
            }

            code = GlobalSettings.CodeOfSeniorMembersAssistanceProgram;
            if (!string.IsNullOrEmpty(code))
            {
                SeniorMembersAssistanceProgramAccount = Account.FindByCode(code);
            }

            code = GlobalSettings.CodeOfGoNegosyo;
            if (!string.IsNullOrEmpty(code))
            {
                GoNegosyoAccount = Account.FindByCode(code);
            }

            code = GlobalSettings.CodeOfCoopPurchaseOrder;
            if (!string.IsNullOrEmpty(code))
            {
                CoopPurchaseOrderAccount = Account.FindByCode(code);
            }
        }

        internal void AddParticular(Particular particular)
        {
            Particulars.Add(particular);
            SelectedParticular = Particulars.Last();
            UpdateLoanAmountForReconstruction();
            UpdateLoanDetails();
        }

        /// <summary>
        /// Loans that are 1 month in term and has special computation
        /// </summary>
        private bool IsSpecialLoan()
        {
            if (LoanAccount.AccountCode == GoNegosyoAccount.AccountCode)
            {
                return true;
            }
            if (LoanAccount.AccountCode == CoopPurchaseOrderAccount.AccountCode)
            {
                return true;
            }
            return false;
        }

        private void AppendSpecialLoanCharges()
        {
            if (LoanAccount.AccountCode == CoopPurchaseOrderAccount.AccountCode)
            {
                ApplyCoopPurchaseCharges();
                return;
            }

            if (LoanAccount.AccountCode == GoNegosyoAccount.AccountCode)
            {
                ApplyGoNegosyoCharges();
            }
        }

        private void ApplyCoopPurchaseCharges()
        {
            var interest = LoanBalance * (LoanProduct.AnnualInterestRate / 12m);
            const decimal finesRate = 3.5m/100m;
            var penalty = 0m;
            if (IsOverDue(PreviousLoanDetails.MaturityDate, ReconstructionDate))
            {
                penalty = LoanBalance*finesRate;
            }
            var fines = FinesAndPenaltyAccount;

            var item = new Particular
            {
                AccountCode = fines.AccountCode,
                AccountTitle = fines.AccountTitle,
                Amount = Math.Round(penalty, 2) + Math.Round(interest)
            };
            Particulars.Add(item);
        }

        private void ApplyGoNegosyoCharges()
        {
            const decimal finesRate = 5/100m;
            var penalty = 0m;
            if (IsOverDue(PreviousLoanDetails.MaturityDate, ReconstructionDate))
            {
                penalty = LoanBalance * finesRate;
            }
            var fines = FinesAndPenaltyAccount;

            var item = new Particular
            {
                AccountCode = fines.AccountCode,
                AccountTitle = fines.AccountTitle,
                Amount = Math.Round(penalty, 2)
            };
            Particulars.Add(item);
        }
    }

    internal class Particular : ViewModelBase
    {
        private string _accountCode;
        private string _accountTitle;
        private decimal _amount;

        public string AccountCode
        {
            get { return _accountCode; }
            set
            {
                _accountCode = value;
                OnPropertyChanged("AccountCode");
            }
        }

        public string AccountTitle
        {
            get { return _accountTitle; }
            set
            {
                _accountTitle = value;
                OnPropertyChanged("AccountTitle");
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }
    }

    internal class DefaultModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    internal enum ReconstructionTypes
    {
        PaidInterest,
        AddOnInterest,
        Restructed
    }
}