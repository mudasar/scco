using System.Windows;

namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    /// <summary>
    /// Interaction logic for TimeDepositDetailWindow.xaml
    /// </summary>
    public partial class TimeDepositEditView
    {
        private const int MONTHS_IN_THREE_YEARS = 12*3;

        private bool _isReadOnly;

        public TimeDepositEditView()
        {
            InitializeComponent();
            for (int i = 0; i < (MONTHS_IN_THREE_YEARS);)
            {
                i ++;
                cboTerm.Items.Add(i);
            }
        }

        public TimeDepositEditView(Models.TimeDeposit.TimeDepositDetails timeDepositDetail) :this()
        {
            DataContext = timeDepositDetail;
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value;
                UpdateButton.IsEnabled = !IsReadOnly;
            }
        }

        private void UpdateOnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
