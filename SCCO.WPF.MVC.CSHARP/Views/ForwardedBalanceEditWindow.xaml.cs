using System.Windows;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views
{
    /// <summary>
    /// Interaction logic for ForwardedBalanceEditWindow.xaml
    /// </summary>
    public partial class ForwardedBalanceEditWindow
    {
        private ForwardedBalanceOld _forwardedBalance;
        private readonly ForwardedBalanceOld _originalforwardedBalance;

        public ForwardedBalanceEditWindow(ForwardedBalanceOld currentRecord)
        {
            InitializeComponent();
            ModelGrid.DataContext = _originalforwardedBalance = _forwardedBalance = currentRecord;
            UpdateButton.Content = _forwardedBalance.ForwardedBalanceId > 0 ? "Update" : "Add";
        }

        public Result ActionResult { get; set; }

        private void UpdateModel()
        {
            ActionResult = _forwardedBalance.ForwardedBalanceId > 0
                               ? _forwardedBalance.Update()
                               : _forwardedBalance.Create();

            if (ActionResult.Success)
                Close();
            else
                MessageWindow.ShowAlertMessage(ActionResult.Message);
        }

        //private void MoveFocusToNextControl(object sender, KeyEventArgs e)
        //{
        //    MainController.MoveFocusToNextControlOnEnter(sender, e);
        //}

        #region --- Events ---

        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
            UpdateModel();
        }

        private void CancelButtonOnClick(object sender, RoutedEventArgs e)
        {
            _forwardedBalance = _originalforwardedBalance;
            ModelGrid.DataContext = _forwardedBalance;
            ActionResult = new Result(false, "Cancelled by user");
            Close();
        }

        #endregion --- Events ---

        private void MemberCodeTextBoxOnLostFocus(object sender, RoutedEventArgs e)
        {
            var member = Nfmb.WhereMemberCodeIs(MemberCodeTextBox.Text);
            MemberNameTextBox.Text = member.MemberName;
        }

        private void AccountCodeTextBoxOnLostFocus(object sender, RoutedEventArgs e)
        {
            var account = Account.WhereAccountCodeIs(AccountCodeTextBox.Text);
            AccountTitleTextBox.Text = account.AccountTitle;
        }


    }
}
