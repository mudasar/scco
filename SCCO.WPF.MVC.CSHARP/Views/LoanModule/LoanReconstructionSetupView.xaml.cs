using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    public partial class LoanReconstructionSetupView
    {
        public LoanReconstructionSetupView()
        {
            InitializeComponent();
            InitializeControls();
            RefreshDisplay();
        }

        private void InitializeControls()
        {
            stbFinesAndPenalty.Click += delegate
                {
                    var account = FindAccount();
                    if (account == null) return;
                    var jea = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfFinesAndPenalty);
                    jea.CurrentValue = account.AccountCode;
                    jea.Update();
                    stbFinesAndPenalty.Text = account.AccountCode;
                };
            stbInterestRebate.Click += delegate
            {
                var account = FindAccount();
                if (account == null) return;
                var jea = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfInterestRebate);
                jea.CurrentValue = account.AccountCode;
                jea.Update();
                stbInterestRebate.Text = account.AccountCode;
            };
            stbUnearnedIncome.Click += delegate
            {
                var account = FindAccount();
                if (account == null) return;
                var jea = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfUnearnedIncome);
                jea.CurrentValue = account.AccountCode;
                jea.Update();
                stbUnearnedIncome.Text = account.AccountCode;
            };
            stbSeniorMembersAssistanceProgram.Click += delegate
            {
                var account = FindAccount();
                if (account == null) return;
                var jea = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfSeniorMembersAssistanceProgram);
                jea.CurrentValue = account.AccountCode;
                jea.Update();
                stbSeniorMembersAssistanceProgram.Text = account.AccountCode;
            };
        }

        private Account FindAccount()
        {
            return Controllers.MainController.SearchAccount();
        }

        private void RefreshDisplay()
        {
            stbFinesAndPenalty.Text = GlobalSettings.CodeOfFinesAndPenalty;
            stbInterestRebate.Text = GlobalSettings.CodeOfInterestRebate;
            stbUnearnedIncome.Text = GlobalSettings.CodeOfUnearnedIncome;
            stbSeniorMembersAssistanceProgram.Text = GlobalSettings.CodeOfSeniorMembersAssistanceProgram;
        }
    }
}
