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
                var settings = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfFinesAndPenalty);
                settings.CurrentValue = account.AccountCode;
                settings.Update();
                stbFinesAndPenalty.Text = account.AccountCode;
            };

            stbInterestRebate.Click += delegate
            {
                var account = FindAccount();
                if (account == null) return;
                var settings = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfInterestRebate);
                settings.CurrentValue = account.AccountCode;
                settings.Update();
                stbInterestRebate.Text = account.AccountCode;
            };

            stbUnearnedIncome.Click += delegate
            {
                var account = FindAccount();
                if (account == null) return;
                var settings = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfUnearnedIncome);
                settings.CurrentValue = account.AccountCode;
                settings.Update();
                stbUnearnedIncome.Text = account.AccountCode;
            };

            stbSeniorMembersAssistanceProgram.Click += delegate
            {
                var account = FindAccount();
                if (account == null) return;
                var settings = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfSeniorMembersAssistanceProgram);
                settings.CurrentValue = account.AccountCode;
                settings.Update();
                stbSeniorMembersAssistanceProgram.Text = account.AccountCode;
            };

            stbGoNegosyo.Click += delegate
            {
                var account = FindAccount();
                if (account == null) return;
                var settings = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfGoNegosyo);
                settings.CurrentValue = account.AccountCode;
                settings.Update();
                stbGoNegosyo.Text = account.AccountCode;
            };

            stbCoopPurchaseOrder.Click += delegate
            {
                var account = FindAccount();
                if (account == null) return;
                var settings = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfCoopPurchaseOrder);
                settings.CurrentValue = account.AccountCode;
                settings.Update();
                stbCoopPurchaseOrder.Text = account.AccountCode;
            };

            stbMiscellaneousIncome.Click += delegate
            {
                var account = FindAccount();
                if (account == null) return;
                var settings = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfMiscellaneousIncome);
                settings.CurrentValue = account.AccountCode;
                settings.Update();
                stbMiscellaneousIncome.Text = account.AccountCode;
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
            stbGoNegosyo.Text = GlobalSettings.CodeOfGoNegosyo;
            stbCoopPurchaseOrder.Text = GlobalSettings.CodeOfCoopPurchaseOrder;
            stbMiscellaneousIncome.Text = GlobalSettings.CodeOfMiscellaneousIncome;
        }
    }
}
