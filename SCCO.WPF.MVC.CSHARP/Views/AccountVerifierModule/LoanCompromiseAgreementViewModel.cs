namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
{
    internal class LoanCompromiseAgreementViewModel : ViewModelBase
    {
        private int _journalVoucherNumber;
        private decimal _loanBalance;
        //private decimal _interestApplied;
        private int _loanTerm;
        private decimal _finesAndPenalty;

        public int JournalVoucherNumber
        {
            get { return _journalVoucherNumber; }
            set
            {
                _journalVoucherNumber = value;
                OnPropertyChanged("JournalVoucherNumber");
            }
        }

        public decimal LoanBalance
        {
            get { return _loanBalance; }
            set
            {
                _loanBalance = value;
                OnPropertyChanged("LoanBalance");
            }
        }

        public int LoanTerm
        {
            get { return _loanTerm; }
            set
            {
                _loanTerm = value;
                OnPropertyChanged("LoanTerm");
            }
        }

        public decimal FinesAndPenalty
        {
            get { return _finesAndPenalty; }
            set
            {
                _finesAndPenalty = value;
                OnPropertyChanged("FinesAndPenalty");
            }
        }

        public Models.Loan.LoanDetails LoanDetails { get; set; }
    }
}