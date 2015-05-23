using System;
using System.Collections.Generic;
using System.Linq;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    /// <summary>
    /// Interaction logic for DividendDistributionWindow.xaml
    ///
    /// 1. Get the total monthly average of individual's Share Capital for the previous year
    ///     - total monthly average = sum of individual average monthly share capital end balance
    /// 2. Get the rate
    ///     - rate = amount allocated (for distribution) / total monthly average
    /// 3. Get the dividend
    ///     - dividend = monthly average x rate
    /// </summary>
    public partial class DividendDistributionWindow
    {
        private readonly DividendDistributionViewModel _viewModel;

        public DividendDistributionWindow()
        {
            InitializeComponent();

            _viewModel = new DividendDistributionViewModel();
            _viewModel.Initialize();

            DataContext = _viewModel;

            PostButton.Click += (sender, args) =>
            {
                var valid = _viewModel.Validate();
                if (valid.Success)
                {
                    _viewModel.Process();
                    _viewModel.SaveSettings();

                    var message = "Interest on Share Capital succesfully posted! ";
                    message += string.Format("Please check JV {0}.", _viewModel.JournalVoucherNumber);
                    MessageWindow.ShowNotifyMessage(message);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageWindow.ShowAlertMessage(valid.Message);
                }
            };
        }
    }
}
