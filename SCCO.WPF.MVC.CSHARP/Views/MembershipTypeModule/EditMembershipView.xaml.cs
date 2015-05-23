using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.MembershipTypeModule
{
    public partial class EditMembershipTypeView
    {
        private readonly MembershipType _membershipType;

        public EditMembershipTypeView()
        {
            InitializeComponent();
            btnUpdate.Click += btnUpdate_Click;
            Loaded += (sender, args) => txtMembershipTypeName.Focus();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var result = _membershipType.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            DialogResult = true;
            Close();
        }

        public EditMembershipTypeView(int collectorId)
            : this()
        {
            _membershipType = new MembershipType();
            _membershipType.Find(collectorId);
            DataContext = _membershipType;
        }
    }
}
