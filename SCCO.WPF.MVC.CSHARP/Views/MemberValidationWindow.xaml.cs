using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views
{
    internal enum AccountTypes
    {
        SingleAccount,
        JointAccount,
        CorporateAccount
    }

    /// <summary>
    /// Interaction logic for MemberNameSetupWindow.xaml
    /// </summary>
    public partial class MemberValidationWindow
    {
        private static ModelController.DataManipulationType _dataManipulationType;
        private readonly Contact _biometrics;
        private List<string> _possibleNames;
        private AccountTypes _selectedAccountType;

        public MemberValidationWindow()
        {
            InitializeComponent();
        }

        public MemberValidationWindow(Contact biometrics, ModelController.DataManipulationType dataManipulationType)
        {
            InitializeComponent();
            _biometrics = biometrics;
            _dataManipulationType = dataManipulationType;
            DataContext = _biometrics;
        }

        public Contact ValidatedBiometrics { get; set; }

        private static Result ValidateMemberName(string memberCode, string memberName)
        {
            if (string.IsNullOrEmpty(memberCode) || string.IsNullOrEmpty(memberName))
                return new Result(false, "Member Code or Member Name must not be empty.");

            if (_dataManipulationType == ModelController.DataManipulationType.Create)
            {
                Nfmb foundMember = Nfmb.FindByCode(memberCode);
                if (foundMember != null && foundMember.ID > 0)
                {
                    return new Result(false, "Member Code already exists.");
                }

                foundMember = Nfmb.FindByName(memberName);

                if (foundMember != null && foundMember.ID > 0)
                {
                    if (foundMember.ID != 0)
                        return new Result(false, "Member Name already exists.");
                }
            }
            else
            {
                Nfmb foundMember = Nfmb.FindByName(memberName);
                if (foundMember != null && foundMember.ID > 0)
                {
                    if (foundMember.MemberCode != memberCode)
                        return new Result(false, "Member Name already exists.");
                }
            }

            return new Result(true, "ValidateMemberName successful.");
        }

        private void AccountTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = cboAccountType.SelectedIndex;
            if (selectedIndex < 0) return;

            switch (selectedIndex)
            {
                case 0:
                    _selectedAccountType = AccountTypes.SingleAccount;
                    break;

                case 1:
                    _selectedAccountType = AccountTypes.JointAccount;
                    break;

                case 2:
                    _selectedAccountType = AccountTypes.CorporateAccount;
                    break;
            }
            RefreshDisplay();
            GeneratePossibleMemberNames();
        }

        private void btnGeneratePossibleNames_Click(object sender, RoutedEventArgs e)
        {
            GeneratePossibleMemberNames();
        }

        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            Result result = ValidateMemberName(_biometrics.MemberCode, _biometrics.MemberName);
            if (result.Success)
            {
                _biometrics.AccountType = cboAccountType.Text;
                ValidatedBiometrics = _biometrics;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }

        private void GeneratePossibleMemberNames()
        {
            _possibleNames = new List<string>();
            if (!(string.IsNullOrEmpty(_biometrics.LastName)) && (!string.IsNullOrEmpty(_biometrics.FirstName)))
            {
                switch (_selectedAccountType)
                {
                    #region --- SINGLE ACCOUNT ---

                    case AccountTypes.SingleAccount:
                        _possibleNames.Add(string.Format("{0}, {1} {2}", _biometrics.LastName,
                                                         _biometrics.FirstName, _biometrics.MiddleName));
                        if (_biometrics.MiddleName != null)
                            _possibleNames.Add(string.Format("{0}, {1} {2}.", _biometrics.LastName,
                                                             _biometrics.FirstName,
                                                             _biometrics.MiddleName.Length > 0
                                                                 ? _biometrics.MiddleName.Substring(0, 1)
                                                                 : ""));
                        break;

                    #endregion --- SINGLE ACCOUNT ---

                    #region --- JOINT ACCOUNT ---

                    case AccountTypes.JointAccount:
                        _possibleNames.Add(string.Format("{0} AND {1}", _biometrics.LastName,
                                                         _biometrics.FirstName));
                        _possibleNames.Add(string.Format("{0} OR {1}", _biometrics.LastName,
                                                         _biometrics.FirstName));
                        _possibleNames.Add(string.Format("{0} AND OR {1}", _biometrics.LastName,
                                                         _biometrics.FirstName));
                        break;

                    #endregion --- JOINT ACCOUNT ---

                    #region --- CORPORATE ACCOUNT ---

                    // nothing to generate

                    #endregion --- CORPORATE ACCOUNT ---
                }
            }
            cboMemberName.ItemsSource = _possibleNames;
        }

        private void RefreshDisplay()
        {
            switch (_selectedAccountType)
            {
                case AccountTypes.SingleAccount:
                    lblMemberName.Content = "Member Name";
                    lblLastName.Content = "Last Name";
                    lblFirstName.Content = "First Name";
                    lblMiddleName.Content = "Middle Name";
                    lblMiddleName.Visibility = Visibility.Visible;
                    txtMiddleName.Visibility = Visibility.Visible;

                    txtMemberName.Visibility = Visibility.Collapsed;
                    cboMemberName.Visibility = Visibility.Visible;
                    btnGeneratePossibleNames.Visibility = Visibility.Visible;
                    break;

                case AccountTypes.JointAccount:
                    lblMemberName.Content = "Joint Account Name";
                    lblLastName.Content = "Last Name, First Name MI.";
                    lblFirstName.Content = "Last Name, First Name MI.";
                    lblMiddleName.Visibility = Visibility.Collapsed;
                    txtMiddleName.Visibility = Visibility.Collapsed;

                    txtMemberName.Visibility = Visibility.Collapsed;
                    cboMemberName.Visibility = Visibility.Visible;
                    btnGeneratePossibleNames.Visibility = Visibility.Visible;
                    break;

                case AccountTypes.CorporateAccount:
                    lblMemberName.Content = "Corporate Account Name";
                    lblLastName.Content = "Last Name, First Name MI.";
                    lblFirstName.Content = "Last Name, First Name MI.";
                    lblMiddleName.Content = "Last Name, First Name MI.";
                    lblMiddleName.Visibility = Visibility.Visible;
                    txtMiddleName.Visibility = Visibility.Visible;

                    txtMemberName.Visibility = Visibility.Visible;
                    cboMemberName.Visibility = Visibility.Collapsed;
                    btnGeneratePossibleNames.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}