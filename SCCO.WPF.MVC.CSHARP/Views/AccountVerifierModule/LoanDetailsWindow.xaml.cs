using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
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
            CoMaker1Label.DataContext = count >= 1 ? _loanDetails.CoMakers[0] : new CoMaker();

            CoMaker2Label.DataContext = count >= 2 ? _loanDetails.CoMakers[1] : new CoMaker();

            CoMaker3Label.DataContext = count >= 3 ? _loanDetails.CoMakers[2] : new CoMaker();

            

            btnNotices.Click += (s, e) =>
            {
                var view = new LoanNoticesView(_loanDetails);
                view.ShowDialog();
            };
        }     
    }
}
