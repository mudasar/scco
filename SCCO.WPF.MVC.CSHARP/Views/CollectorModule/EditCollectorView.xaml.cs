using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.CollectorModule
{
    public partial class EditCollectorView
    {
        private readonly Collector _collector;

        public EditCollectorView()
        {
            InitializeComponent();
            btnUpdate.Click += btnUpdate_Click;
            Loaded += (sender, args) => txtCollectorName.Focus();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var result = _collector.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            DialogResult = true;
            Close();
        }

        public EditCollectorView(int collectorId)
            : this()
        {
            _collector = new Collector();
            _collector.Find(collectorId);
            DataContext = _collector;
        }
    }
}
