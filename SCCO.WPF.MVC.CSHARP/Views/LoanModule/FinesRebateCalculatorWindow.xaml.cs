using System;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    /// <summary>
    /// Interaction logic for FinesRebateCalculatorWindow.xaml
    /// </summary>
    public partial class FinesRebateCalculatorWindow
    {
        private readonly FinesRebateCalculatorViewModel _viewModel;

        public FinesRebateCalculatorWindow()
        {
            InitializeComponent();
            CalculateButton.Click += CalculateButtonOnClick;
            PrintButton.Click += PrintButtonOnClick;
        }

        private void PrintButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var reportItem= new ReportItem();
            reportItem.Title = string.Format("{0} assessment as of {1:D}", _viewModel.ReportTitle, _viewModel.ProcessDate);
            reportItem.LoadReport(_viewModel.GetReportData(), "fines_rebate.rpt");
        }

        private void CalculateButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.Calculate();
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            if (_viewModel.Fines > 0)
            {
                RebateStackPanel.Visibility = Visibility.Collapsed;
                InterestStackPanel.Visibility = Visibility.Visible;
                FinesStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                RebateStackPanel.Visibility = Visibility.Visible;
                InterestStackPanel.Visibility = Visibility.Collapsed;
                FinesStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        public FinesRebateCalculatorWindow(FinesRebateCalculatorViewModel viewModel):this()
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            RefreshDisplay();
        }
    }
}
