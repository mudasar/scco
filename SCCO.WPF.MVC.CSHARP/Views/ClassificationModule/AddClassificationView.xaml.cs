using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.ClassificationModule
{
    public partial class AddClassificationView
    {
        private Classification _newItem;

        public AddClassificationView()
        {
            InitializeComponent();
            _newItem = new Classification();
            DataContext = _newItem;

            btnAdd.Click += btnAdd_Click;

            Loaded += (sender, args) => txtClassificationName.Focus();
        }

        public Classification NewItem
        {
            get { return _newItem; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var item = Classification.FindByName(_newItem.Description);
            if (item == null)
            {
                var result = _newItem.Create();
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
                MessageWindow.ShowNotifyMessage("Classification already exists!");
            }
        }
    }
}
