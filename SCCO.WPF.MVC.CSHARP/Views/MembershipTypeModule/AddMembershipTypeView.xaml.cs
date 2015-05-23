using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.MembershipTypeModule
{
    public partial class AddMembershipTypeView
    {
        private readonly MembershipType newItem;
        public AddMembershipTypeView()
        {
            InitializeComponent();
            newItem = new MembershipType();
            DataContext = newItem;

            btnAdd.Click += btnAdd_Click;
            Loaded += (sender, args) => txtMembershipTypeName.Focus();
        }

        public MembershipType NewItem
        {
            get { return newItem; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MembershipType item = MembershipType.FindByName(newItem.Description);
            if (item == null)
            {
                var result = newItem.Create();
                if (result.Success)
                {
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            else
            {
                MessageWindow.ShowNotifyMessage("MembershipType already exists!");
            }
        }
    }
}
