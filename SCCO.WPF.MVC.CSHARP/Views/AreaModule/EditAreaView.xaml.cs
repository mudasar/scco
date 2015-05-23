using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AreaModule
{
    public partial class EditAreaView
    {
        private readonly Area _area;

        public EditAreaView()
        {
            InitializeComponent();
            btnUpdate.Click += btnUpdate_Click;
            Loaded += (sender, args) => txtAreaName.Focus();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var result = _area.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            DialogResult = true;
            Close();
        }

        public EditAreaView(int id)
            : this()
        {
            _area = new Area();
            _area.Find(id);
            DataContext = _area;
        }
    }
}
