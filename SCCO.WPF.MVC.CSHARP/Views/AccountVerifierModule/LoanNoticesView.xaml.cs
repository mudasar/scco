using System;
using System.Collections.Generic;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Extensions;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
{
    /// <summary>
    ///     Interaction logic for LoanNoticesView.xaml
    /// </summary>
    public partial class LoanNoticesView
    {
        private readonly LoanDetails _loanDetails;

        public LoanNoticesView(LoanDetails loanDetails)
        {
            InitializeComponent();
            _loanDetails = loanDetails;

            LoansNonperformingButton.Click += (sender, args) => GenerateNoticesForLoansNonPerforming();

            LoansNearMaturityButton.Click += (sender, args) => GenerateNoticesForLoansNearMaturity();

            LoansOverdueButton.Click += (sender, args) => GenerateNoticesForLoansOverdue();

            LoansOverdueNonResponsiveButton.Click += (sender, args) => GenerateNoticesForNonResponsiveLoansOverdue();

            LoanNoticeForComakersButton.Click += (sender, args) => GenerateNoticesForComakers();
        }

        private void GenerateNoticesForLoansNonPerforming()
        {
            try
            {
                var asOf = MainController.LoggedUser.TransactionDate;
                var notices = new List<LoanNoticesViewModel> {new LoanNoticesViewModel(_loanDetails, asOf)};
                var loanDetails = notices.ToDataTable();

                loanDetails.TableName = "notice_loan_non_performing";

                var comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                {
                    ReportFile = "notice_loan_non_performing.rpt",
                    Title = "Notice for non-performing loans",
                    DataSource = dataSet
                };
                var result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception e)
            {
                MessageWindow.ShowAlertMessage(e.Message);
            }
        }

        private void GenerateNoticesForLoansNearMaturity()
        {
            try
            {
                var asOf = MainController.LoggedUser.TransactionDate;
                var notices = new List<LoanNoticesViewModel> {new LoanNoticesViewModel(_loanDetails, asOf)};
                var loanDetails = notices.ToDataTable();

                loanDetails.TableName = "notice_loan_details";

                var comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                {
                    ReportFile = "notice_loan_near_maturity.rpt",
                    Title = "Notice for loan near maturity",
                    DataSource = dataSet
                };
                var result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception e)
            {
                MessageWindow.ShowAlertMessage(e.Message);
            }
        }

        private void GenerateNoticesForLoansOverdue()
        {
            try
            {
                var asOf = MainController.LoggedUser.TransactionDate;
                var notices = new List<LoanNoticesViewModel> {new LoanNoticesViewModel(_loanDetails, asOf)};
                var loanDetails = notices.ToDataTable();

                loanDetails.TableName = "notice_loan_details";

                var comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                {
                    ReportFile = "notice_loan_overdue.rpt",
                    Title = "Notice for overdue loans",
                    DataSource = dataSet
                };
                var result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception e)
            {
                MessageWindow.ShowAlertMessage(e.Message);
            }
        }

        private void GenerateNoticesForNonResponsiveLoansOverdue()
        {
            try
            {
                var asOf = MainController.LoggedUser.TransactionDate;
                var notices = new List<LoanNoticesViewModel> {new LoanNoticesViewModel(_loanDetails, asOf)};
                var loanDetails = notices.ToDataTable();

                loanDetails.TableName = "notice_loan_details";

                var comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                {
                    ReportFile = "notice_loan_overdue_non_responsive.rpt",
                    Title = "Notice for overdue loans - Non-responsive",
                    DataSource = dataSet
                };
                var result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception e)
            {
                MessageWindow.ShowAlertMessage(e.Message);
            }
        }

        private void GenerateNoticesForComakers()
        {
            try
            {
                var asOf = MainController.LoggedUser.TransactionDate;
                var notices = new List<LoanNoticesViewModel>();

                foreach (var coMaker in _loanDetails.CoMakers)
                {
                    if (string.IsNullOrEmpty(coMaker.MemberName)) continue;
                    var item = new LoanNoticesViewModel(_loanDetails, asOf)
                    {
                        comaker_code = coMaker.MemberCode,
                        comaker_name = coMaker.MemberName
                    };

                    var member = Nfmb.FindByCode(coMaker.MemberCode);
                    if (member == null || member.ID < 0) continue;
                    item.address1 = member.Address1;
                    item.address2 = member.Address2;
                    item.address3 = member.Address3;

                    notices.Add(item);
                }

                if (notices.Count < 1)
                {
                    MessageWindow.ShowAlertMessage("No Co-Makers!");
                    return;
                }

                var loanDetails = notices.ToDataTable();

                loanDetails.TableName = "notice_comakers";

                var comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                {
                    ReportFile = "notice_comakers.rpt",
                    Title = "Notice for Comakers",
                    DataSource = dataSet
                };
                var result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception e)
            {
                MessageWindow.ShowAlertMessage(e.Message);
            }
        }
    }
}