using System.Windows;

namespace SCCO.WPF.MVC.CS.Views.ReportsModule {
    /// <summary>
    /// Interaction logic for ReportsWindow.xaml
    /// </summary>
    public partial class ReportsWindow {
        public ReportsWindow() {
            InitializeComponent();
            //this.SetButtonCaption();
            btnLoanReports.Click += (s, e) => ShowLoanReportsView();

            btnLoanNotices.Click += (s, e) => ShowLoanNoticesView();
        }


        private void ShowLoanReportsView()
        {
            var view = new LoanReportsView();
            view.ShowDialog();
        }

        private void ShowLoanNoticesView()
        {
            var view = new LoanNotices();
            view.ShowDialog();
        }
        //private Result SetButtonCaption()
        //{
        //    try
        //    {
        //        this.btnReport8.Content = Models.ReportSchedule.GetScheduleDescription(8).Description;
        //        this.BtnReport9.Content = Models.ReportSchedule.GetScheduleDescription(9).Description;
        //        this.BtnReport10.Content = Models.ReportSchedule.GetScheduleDescription(10).Description;
        //        this.BtnReport11.Content = Models.ReportSchedule.GetScheduleDescription(11).Description;
        //        this.BtnReport12.Content = Models.ReportSchedule.GetScheduleDescription(12).Description;
        //        this.BtnReport13.Content = Models.ReportSchedule.GetScheduleDescription(13).Description;
        //        this.BtnReport14.Content = Models.ReportSchedule.GetScheduleDescription(14).Description;
        //        this.BtnReport15.Content = Models.ReportSchedule.GetScheduleDescription(15).Description;
        //        this.BtnReport16.Content = Models.ReportSchedule.GetScheduleDescription(16).Description;
        //        this.BtnReport17.Content = Models.ReportSchedule.GetScheduleDescription(17).Description;
        //        this.BtnReport18.Content = Models.ReportSchedule.GetScheduleDescription(18).Description;
        //        this.BtnReport19.Content = Models.ReportSchedule.GetScheduleDescription(19).Description;
        //        this.BtnReport20.Content = Models.ReportSchedule.GetScheduleDescription(20).Description;
        //        return new Result(true, "Success", this, "SetButtonCaption");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false,e.Message, this,"SetButtonCaption");
        //    }
        //}

        //private void GenerateLoanReport(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleOfLoans(GlobalSettings.TransactionDate,"Code");
        //    ReportController.GenerateScheduleOfLoans(GlobalSettings.TransactionDate, "Name");
        //    ReportController.GenerateMemberListReport(Models.Member.GetMemberList());
        //}

        //private void GenerateSavingsDepositReport(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleOfSavingsDeposit(GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleOfSavingsDeposit(GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateInterestOnLoansReport(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleOfInterestOnLoans(GlobalSettings.TransactionDate,"Code");
        //    ReportController.GenerateScheduleOfInterestOnLoans(GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateTimeDepositReport(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleOfTimeDeposit(GlobalSettings.TransactionDate,"Code");
        //    ReportController.GenerateScheduleOfTimeDeposit(GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateShareCapitalReport(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleOfShareCapital(GlobalSettings.TransactionDate,"Code");
        //    ReportController.GenerateScheduleOfShareCapital(GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateFinesReport(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleOfFines(GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleOfFines(GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateTimesDepositConsolidatedReport(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleOfTimeDepositConsolidated(GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleOfTimeDepositConsolidated(GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport8(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleReports(8, (string)this.btnReport8.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(8, (string)this.btnReport8.Content, GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport9(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleReports(9, (string)this.BtnReport9.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(9, (string)this.BtnReport9.Content, GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport10(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleReports(10, (string)this.BtnReport10.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(10, (string)this.BtnReport10.Content, GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport11(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleReports(11, (string)this.BtnReport11.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(11, (string)this.BtnReport11.Content, GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport12(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleReports(12, (string)this.BtnReport12.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(12, (string)this.BtnReport12.Content, GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport13(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleReports(13, (string)this.BtnReport13.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(13, (string)this.BtnReport13.Content, GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport14(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleReports(14, (string)this.BtnReport14.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(14, (string)this.BtnReport14.Content, GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport15(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleReports(15, (string)this.BtnReport15.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(15, (string)this.BtnReport15.Content, GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport16(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleReports(16, (string)this.BtnReport16.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(16, (string)this.BtnReport16.Content, GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport17(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleReports(17, (string)this.BtnReport17.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(17, (string)this.BtnReport17.Content, GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport18(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleReports(18, (string)this.BtnReport18.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(18, (string)this.BtnReport18.Content, GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport19(object sender, RoutedEventArgs e)
        //{
        //    ReportController.GenerateScheduleReports(19, (string)this.BtnReport19.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(19, (string)this.BtnReport19.Content, GlobalSettings.TransactionDate, "Name");
        //}

        //private void GenerateReport20(object sender, RoutedEventArgs e)
        //{
        //    var schedulesReportWindow = new SchedulesReportWindow();
        //    schedulesReportWindow.ShowDialog();
        //    return;
            
        //    ReportController.GenerateScheduleReports(20, (string)this.BtnReport20.Content, GlobalSettings.TransactionDate, "Code");
        //    ReportController.GenerateScheduleReports(20, (string)this.BtnReport20.Content, GlobalSettings.TransactionDate, "Name");
        //}

        private void MemberInformationReportButtonOnClick(object sender, RoutedEventArgs e)
        {
            var memberInformationReport = new ReportPickerWindow("MIS");
            memberInformationReport.ShowDialog();
        }
        private void AccountScheduleReportButtonOnClick(object sender, RoutedEventArgs e)
        {
            var view = new ScheduleOfAccountsView();
            view.ShowDialog();
        }

    }
}
