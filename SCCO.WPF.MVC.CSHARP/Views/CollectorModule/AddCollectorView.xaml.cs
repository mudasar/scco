using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.CollectorModule
{
    public partial class AddCollectorView
    {
        private readonly Collector _newItem;
        public AddCollectorView()
        {
            InitializeComponent();
            _newItem = new Collector();
            DataContext = _newItem;

            btnAdd.Click += btnAdd_Click;
            Loaded += (sender, args) => txtCollectorName.Focus();
        }

        public Collector NewItem
        {
            get { return _newItem; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var item = Collector.FindByName(_newItem.CollectorName);
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
            }else
            {
                MessageWindow.ShowNotifyMessage("Collector already exists!");
            }
        }
    }
}
