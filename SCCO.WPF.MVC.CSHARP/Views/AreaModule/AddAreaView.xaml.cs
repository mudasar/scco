using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AreaModule
{
    public partial class AddAreaView
    {
        private readonly Area _newItem;
        public AddAreaView()
        {
            InitializeComponent();
            _newItem = new Area();
            DataContext = _newItem;

            btnAdd.Click += btnAdd_Click;
            Loaded += (sender, args) => txtAreaName.Focus();
        }

        public Area NewItem
        {
            get { return _newItem; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Area item = Area.FindByName(_newItem.AreaName);
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
                MessageWindow.ShowNotifyMessage("Area already exists!");
            }
        }
    }
}
