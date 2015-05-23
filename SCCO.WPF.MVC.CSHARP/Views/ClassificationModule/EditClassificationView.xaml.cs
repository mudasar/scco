using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.ClassificationModule
{
    public partial class EditClassificationView
    {
        private readonly Classification _classification;

        public EditClassificationView()
        {
            InitializeComponent();
            btnUpdate.Click += btnUpdate_Click;
            Loaded += (sender, args) => txtClassificationName.Focus();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var result = _classification.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            DialogResult = true;
            Close();
        }

        public EditClassificationView(int id)
            : this()
        {
            _classification = new Classification();
            _classification.Find(id);
            DataContext = _classification;
        }
    }
}
