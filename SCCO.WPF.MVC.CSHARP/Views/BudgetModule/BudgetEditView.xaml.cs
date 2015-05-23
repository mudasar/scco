using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.BudgetModule
{
    /// <summary>
    /// Interaction logic for NewBudgetView.xaml
    /// </summary>
    public partial class BudgetEditView
    {
        private Budget _model;
        public BudgetEditView(Budget model)
        {
            InitializeComponent();

            _model = model;
            DataContext = _model;

            btnUpdate.Click += (sender, args) => Update();
        }

        void Update()
        {
            var result = Controllers.BudgetsController.Save(_model);
            DialogResult = result.Success;
            if (result.Success)
            {
                MessageWindow.ShowNotifyMessage(result.Message);
                Close();
            }
            else
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }
    }
}
