using System;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.FinancialConditionReportConfigurationModule
{
    public partial class AddItemView
    {
        private readonly FinancialConditionReportConfiguration _newItem;

        public FinancialConditionReportConfiguration NewItem { get { return _newItem; } }

        public AddItemView()
        {
            InitializeComponent();

            AddButton.Click += (sender, args) => Add();

            _newItem = new FinancialConditionReportConfiguration();
            DataContext = _newItem;
        }

        private void Add()
        {
            try
            {
                _newItem.Create();
                DialogResult = true;
                Close();
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
        }
    }
}
