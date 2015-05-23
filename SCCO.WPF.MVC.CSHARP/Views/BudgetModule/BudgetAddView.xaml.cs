using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.BudgetModule
{
    /// <summary>
    /// Interaction logic for BudgetAddView.xaml
    /// </summary>
    public partial class BudgetAddView
    {
        private Budget _model;

        public BudgetAddView(Budget model)
        {
            InitializeComponent();

            _model = model;
            DataContext = _model;
            txtAmount.Focus();
            btnCreate.Click += (sender, args) => Create();
        }

        void Create()
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