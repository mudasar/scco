using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.FinancialConditionReportConfigurationModule
{
    public partial class EditItemView
    {
        private readonly FinancialConditionReportConfiguration _updateItem;

        public EditItemView()
        {
            InitializeComponent();
            UpdateButton.Click += (sender, args) => Update();
        }

        public EditItemView(int id) : this()
        {
            _updateItem = new FinancialConditionReportConfiguration();
            _updateItem.Find(id);
            DataContext = _updateItem;
        }

        private void Update()
        {
            var result = _updateItem.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            DialogResult = true;
            Close();
        }
    }
}
