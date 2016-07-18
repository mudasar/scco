using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;
using System.Data;
using SCCO.WPF.MVC.CS.Models.AccountVerifier;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Controllers
{
    public class ReportController
    {
        public static readonly string ReportFolder = GetReportFolderPath();

        public static string GetReportFolderPath()
        {
            // get the root folder
            string rootFolder = Environment.CommandLine;
            rootFolder = rootFolder.Substring(1, rootFolder.Length - 3);
            return Path.Combine(Path.GetDirectoryName(rootFolder), "ReportFiles");
        }

        #region Official Receipt

        public static class OfficialReceiptReports
        {
            public static void VoucherForm(int documentNumber)
            {
                DataTable ordoc1 = DataSource.OfficialReceipt.GetOfficialReceiptDocument(documentNumber);
                ordoc1.TableName = "ordoc1";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(ordoc1);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "or_form.rpt";
                ri.Title = "Official Receipt";
                ri.DataSource = dataSet;
                ri.LoadReport();
            }

            public static void DetailedReport(DateTime dateStart, DateTime dateEnd)
            {

                var titleBuilder = new StringBuilder();
                if (dateStart == dateEnd)
                {
                    titleBuilder.Append("As of ");
                    titleBuilder.Append(String.Format("{0:MMMM dd, yyyy}", dateStart));
                }
                else
                {
                    titleBuilder.Append("For the month of ");
                    titleBuilder.Append(String.Format("{0:MMMM yyyy}", dateStart));
                }

                var or = DataSource.OfficialReceipt.WhereDocumentDateBetween(dateStart, dateEnd);
                or.TableName = "or";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(or);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "or_detailed.rpt";
                ri.Title = titleBuilder.ToString();
                ri.DataSource = dataSet;
                ri.LoadReport();
            }

            internal static void PerCollectorDetailedReport(Views.VoucherReportViewModel reportViewModel)
            {
                var dateStart = reportViewModel.DateRange[0];
                var dateEnd = reportViewModel.DateRange[1];

                var titleBuilder = new StringBuilder();
                if (dateStart == dateEnd)
                {
                    titleBuilder.AppendFormat("Detailed Collection Report of {0} on {1:MMMM dd, yyyy}",
                                              reportViewModel.SelectedCollector, dateStart);
                }
                else
                {
                    titleBuilder.AppendFormat("Detailed Collection Report of {0} for the month of {1:MMMM yyyy}",
                                              reportViewModel.SelectedCollector, dateStart);
                }

                // Generate query statement
                var parameters = new List<SqlParameter>();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("SELECT * FROM `or` WHERE");

                // set filter by document date
                if (dateStart == dateEnd)
                {
                    queryBuilder.AppendLine("`DOC_DATE` = ?DateStart");
                    parameters.Add(new SqlParameter("?DateStart", dateStart));
                }
                else
                {
                    queryBuilder.AppendLine("`DOC_DATE` BETWEEN ?DateStart AND ?DateEnd");
                    parameters.Add(new SqlParameter("?DateStart", dateStart));
                    parameters.Add(new SqlParameter("?DateEnd", dateEnd));
                }

                // set filter by collector name except 'ALL'
                if (reportViewModel.SelectedCollector == null || reportViewModel.SelectedCollector.CollectorName.ToUpper() != "ALL")
                {
                    queryBuilder.AppendLine("AND `COLLECTOR` = ?CollectorName");
                    parameters.Add(new SqlParameter("?CollectorName", reportViewModel.SelectedCollector));
                }

                queryBuilder.AppendLine("ORDER BY COLLECTOR, DOC_NUM");
                var or = DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), parameters.ToArray());
                or.TableName = "or";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(or);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "or_detailed.rpt";
                ri.Title = titleBuilder.ToString();
                ri.DataSource = dataSet;
                ri.LoadReport();
            }

            internal static void PerCollectorSummaryReport(Views.VoucherReportViewModel reportViewModel)
            {
                var dateStart = reportViewModel.DateRange[0];
                var dateEnd = reportViewModel.DateRange[1];

                var titleBuilder = new StringBuilder();
                if (dateStart == dateEnd)
                {
                    titleBuilder.AppendFormat("Summary Collection Report of {0} on {1:MMMM dd, yyyy}",
                                              reportViewModel.SelectedCollector, dateStart);
                }
                else
                {
                    titleBuilder.AppendFormat("Summary Collection Report of {0} for the month of {1:MMMM yyyy}",
                                              reportViewModel.SelectedCollector, dateStart);
                }

                // Generate query statement
                var parameters = new List<SqlParameter>();
                var queryBuilder = new StringBuilder();


                // set filter by collector name except 'ALL'
                if (reportViewModel.SelectedCollector != null &&
                    reportViewModel.SelectedCollector.CollectorName.ToUpper() != "ALL")
                {
                    queryBuilder.AppendLine("SELECT * FROM `or`");

                    // set filter by document date
                    if (dateStart == dateEnd)
                    {
                        queryBuilder.AppendLine("WHERE `DOC_DATE` = ?DateStart");
                        parameters.Add(new SqlParameter("?DateStart", dateStart));
                    }
                    else
                    {
                        queryBuilder.AppendLine("WHERE `DOC_DATE` BETWEEN ?DateStart AND ?DateEnd");
                        parameters.Add(new SqlParameter("?DateStart", dateStart));
                        parameters.Add(new SqlParameter("?DateEnd", dateEnd));
                    }
                    queryBuilder.AppendLine("AND `COLLECTOR` = ?CollectorName");
                    parameters.Add(new SqlParameter("?CollectorName", reportViewModel.SelectedCollector));

                    var or = DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), parameters.ToArray());
                    or.TableName = "or";

                    DataTable comp = Company.GetData();
                    comp.TableName = "comp";

                    var dataSet = new DataSet();
                    dataSet.Tables.Add(or);
                    dataSet.Tables.Add(comp);

                    var ri = new ReportItem();
                    ri.ReportFile = "or_summary.rpt";
                    ri.Title = titleBuilder.ToString();
                    ri.DataSource = dataSet;
                    ri.LoadReport();
                }
                else
                {
                    queryBuilder.AppendLine("SELECT COLLECTOR as collector, SUM(CREDIT) as total");
                    queryBuilder.AppendLine("FROM `or`");

                    // set filter by document date
                    if (dateStart == dateEnd)
                    {
                        queryBuilder.AppendLine("WHERE `DOC_DATE` = ?DateStart");
                        parameters.Add(new SqlParameter("?DateStart", dateStart));
                    }
                    else
                    {
                        queryBuilder.AppendLine("WHERE `DOC_DATE` BETWEEN ?DateStart AND ?DateEnd");
                        parameters.Add(new SqlParameter("?DateStart", dateStart));
                        parameters.Add(new SqlParameter("?DateEnd", dateEnd));
                    }
                    queryBuilder.AppendLine("GROUP BY COLLECTOR");
                    queryBuilder.AppendLine("ORDER BY SUM(CREDIT) DESC");
                    var or = DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), parameters.ToArray());
                    or.TableName = "or_collector_summary";

                    DataTable comp = Company.GetData();
                    comp.TableName = "comp";

                    var dataSet = new DataSet();
                    dataSet.Tables.Add(or);
                    dataSet.Tables.Add(comp);

                    var ri = new ReportItem();
                    ri.ReportFile = "or_collector_summary.rpt";
                    ri.Title = titleBuilder.ToString();
                    ri.DataSource = dataSet;
                    ri.LoadReport();
                    
                }

            }

            internal static void SummaryReport(Views.VoucherReportViewModel reportViewModel)
            {
                var dateStart = reportViewModel.DateRange[0];
                var dateEnd = reportViewModel.DateRange[1];

                var titleBuilder = new StringBuilder();
                if (dateStart == dateEnd)
                {
                    titleBuilder.Append("As of ");
                    titleBuilder.Append(String.Format("{0:MMMM dd, yyyy}", dateStart));
                }
                else
                {
                    titleBuilder.Append("For the month of ");
                    titleBuilder.Append(String.Format("{0:MMMM yyyy}", dateStart));
                }

                var or = DataSource.OfficialReceipt.WhereDocumentDateBetween(dateStart, dateEnd);
                or.TableName = "or";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(or);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "or_summary.rpt";
                ri.Title = titleBuilder.ToString();
                ri.DataSource = dataSet;
                ri.LoadReport();
            }

            public static void Attachment(int documentNumber)
            {
                try
                {
                    DataTable or = DataSource.OfficialReceipt.WhereDocumentNumberEquals(documentNumber);
                    or.TableName = "or";

                    DataTable comp = Company.GetData();
                    comp.TableName = "comp";

                    var dataSet = new DataSet();
                    dataSet.Tables.Add(or);
                    dataSet.Tables.Add(comp);


                    var ri = new ReportItem();
                    ri.ReportFile = "or_attachment.rpt";
                    ri.Title = "Official Receipt Attachment";
                    ri.DataSource = dataSet;
                    ri.LoadReport();

                }
                catch (Exception e)
                {
                    Logger.ExceptionLogger(new ReportController(), e);
                }
            }

            internal static void PerAccount(Views.VoucherReportViewModel reportViewModel)
            {
                try
                {
                    var accountCode = reportViewModel.AccountCode;
                    var dateStart = reportViewModel.DateRange[0];
                    var dateEnd = reportViewModel.DateRange[1];
                    var titleBuilder = new StringBuilder();
                    if (dateStart == dateEnd)
                    {
                        titleBuilder.Append("As of ");
                        titleBuilder.Append(String.Format("{0:MMMM dd, yyyy}", dateStart));
                    }
                    else
                    {
                        titleBuilder.Append("For the month of ");
                        titleBuilder.Append(String.Format("{0:MMMM yyyy}", dateStart));
                    }

                    var sqlParameters = new List<SqlParameter>();
                    
                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT * FROM `or` WHERE `ACC_CODE` = ?AccountCode");
                    sqlParameters.Add(new SqlParameter("?AccountCode", accountCode));
                    sqlParameters.Add(new SqlParameter("?DateStart", dateStart));
                    if (dateStart == dateEnd)
                    {
                        queryBuilder.AppendLine("AND `DOC_DATE` = ?DateStart");
                    }
                    else
                    {
                        queryBuilder.AppendLine("AND `DOC_DATE` BETWEEN ?DateStart AND ?DateEnd");
                        sqlParameters.Add(new SqlParameter("?DateEnd", dateEnd));
                    }
                    
                    if (reportViewModel.SelectedCollector != null &&
                        reportViewModel.SelectedCollector.CollectorName.ToUpper() != "ALL")
                    {
                        
                        queryBuilder.AppendLine("AND `COLLECTOR` = ?CollectorName");
                        sqlParameters.Add(new SqlParameter("?CollectorName",
                                                           reportViewModel.SelectedCollector.CollectorName));
                    }

                    var or = DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), sqlParameters.ToArray());
                    or.TableName = "or";

                    DataTable comp = Company.GetData();
                    comp.TableName = "comp";

                    var dataSet = new DataSet();
                    dataSet.Tables.Add(or);
                    dataSet.Tables.Add(comp);


                    var ri = new ReportItem();
                    ri.ReportFile = "or_per_account.rpt";
                    ri.Title = titleBuilder.ToString();
                    ri.DataSource = dataSet;
                    ri.LoadReport();

                }
                catch (Exception e)
                {
                    Logger.ExceptionLogger(new ReportController(), e);
                }
            }

            public static Result TellerCollectorForm(int documentNumber)
            {
                DataTable ordoc1 = DataSource.OfficialReceipt.GetOfficialReceiptDocument(documentNumber);
                ordoc1.TableName = "ordoc1";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(ordoc1);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "teller_collector_form.rpt";
                ri.Title = "Official Receipt";
                ri.DataSource = dataSet;
                return ri.LoadReport();
            }

            internal static class DailyCollectionReport
            {
                public static void Summary(DateTime date, string collector)
                {
                    DataTable or = DataSource.OfficialReceipt.DailyCollectionPerCollector(date, collector);
                    or.TableName = "or";
                    DataTable comp = Company.GetData();
                    comp.TableName = "comp";

                    var dataSet = new DataSet();
                    dataSet.Tables.Add(or);
                    dataSet.Tables.Add(comp);

                    var ri = new ReportItem();
                    ri.ReportFile = "daily_collection_summary.rpt";
                    ri.Title = "Daily Collection Report - Account Summary";
                    ri.DataSource = dataSet;
                    ri.LoadReport();
                }

                public static void Detailed(DateTime date, string collector)
                {
                    DataTable or = DataSource.OfficialReceipt.DailyCollectionPerCollector(date, collector);
                    or.TableName = "or";
                    DataTable comp = Company.GetData();
                    comp.TableName = "comp";

                    var dataSet = new DataSet();
                    dataSet.Tables.Add(or);
                    dataSet.Tables.Add(comp);

                    var ri = new ReportItem();
                    ri.ReportFile = "daily_collection_detailed.rpt";
                    ri.Title = "Daily Collection Report Per OR No.";
                    ri.DataSource = dataSet;
                    ri.LoadReport();
                }
            }
        }

        #endregion

        #region Journal Voucher

        public static class JournalVoucherReports
        {
            public static void VoucherForm(int documentNumber)
            {
                DataTable jvdoc1 = DataSource.JournalVoucher.GetJournalVoucherDocument(documentNumber);
                jvdoc1.TableName = "jvdoc1";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(jvdoc1);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "jv_form.rpt";
                ri.Title = "Journal Voucher";
                ri.DataSource = dataSet;
                ri.LoadReport();
            }

            public static void DetailedReport(DateTime dateStart, DateTime dateEnd)
            {

                var titleBuilder = new StringBuilder();
                if (dateStart == dateEnd)
                {
                    titleBuilder.Append("As of ");
                    titleBuilder.Append(String.Format("{0:MMMM dd, yyyy}", dateStart));
                }
                else
                {
                    titleBuilder.Append("For the month of ");
                    titleBuilder.Append(String.Format("{0:MMMM yyyy}", dateStart));
                }

                var jv = DataSource.JournalVoucher.WhereDocumentDateBetween(dateStart, dateEnd);
                jv.TableName = "jv";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(jv);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "jv_detailed.rpt";
                ri.Title = titleBuilder.ToString();
                ri.DataSource = dataSet;
                ri.LoadReport();
            }

            internal static void SummaryReport(Views.VoucherReportViewModel reportViewModel)
            {
                var dateStart = reportViewModel.DateRange[0];
                var dateEnd = reportViewModel.DateRange[1];

                var titleBuilder = new StringBuilder();
                if (dateStart == dateEnd)
                {
                    titleBuilder.Append("As of ");
                    titleBuilder.Append(String.Format("{0:MMMM dd, yyyy}", dateStart));
                }
                else
                {
                    titleBuilder.Append("For the month of ");
                    titleBuilder.Append(String.Format("{0:MMMM yyyy}", dateStart));
                }

                var jv = DataSource.JournalVoucher.WhereDocumentDateBetween(dateStart, dateEnd);
                jv.TableName = "jv";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(jv);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "jv_summary.rpt";
                ri.Title = titleBuilder.ToString();
                ri.DataSource = dataSet;
                ri.LoadReport();
            }

            public static void Attachment(int documentNumber)
            {
                try
                {
                    DataTable jv = DataSource.JournalVoucher.WhereDocumentNumberEquals(documentNumber);
                    jv.TableName = "jv";

                    DataTable voucher_explanation = Voucher.GetExplanation(VoucherTypes.JV, documentNumber);
                    voucher_explanation.TableName = "voucher_explanation";

                    DataTable comp = Company.GetData();
                    comp.TableName = "comp";

                    var dataSet = new DataSet();
                    dataSet.Tables.Add(jv);
                    dataSet.Tables.Add(voucher_explanation);
                    dataSet.Tables.Add(comp);

                    var ri = new ReportItem();
                    ri.ReportFile = "jv_attachment.rpt";
                    ri.Title = "Journal Voucher Attachment";
                    ri.DataSource = dataSet;
                    ri.LoadReport();

                }
                catch (Exception e)
                {
                    Logger.ExceptionLogger(new ReportController(), e);
                }
            }

            internal static void PerAccount(Views.VoucherReportViewModel reportViewModel)
            {
                try
                {
                    var accountCode = reportViewModel.AccountCode;
                    var dateStart = reportViewModel.DateRange[0];
                    var dateEnd = reportViewModel.DateRange[1];

                    var titleBuilder = new StringBuilder();
                    if (dateStart == dateEnd)
                    {
                        titleBuilder.Append("As of ");
                        titleBuilder.Append(String.Format("{0:MMMM dd, yyyy}", dateStart));
                    }
                    else
                    {
                        titleBuilder.Append("For the month of ");
                        titleBuilder.Append(String.Format("{0:MMMM yyyy}", dateStart));
                    }

                    DataTable jv = DataSource.JournalVoucher.WhereAccountCodeIsAndDateIsBetwen(accountCode,
                                                                                               dateStart, dateEnd);
                    jv.TableName = "jv";

                    DataTable comp = Company.GetData();
                    comp.TableName = "comp";

                    var dataSet = new DataSet();
                    dataSet.Tables.Add(jv);
                    dataSet.Tables.Add(comp);


                    var ri = new ReportItem();
                    ri.ReportFile = "jv_per_account.rpt";
                    ri.Title = titleBuilder.ToString();
                    ri.DataSource = dataSet;
                    ri.LoadReport();

                }
                catch (Exception e)
                {
                    Logger.ExceptionLogger(new ReportController(), e);
                }
            }

        }

        #endregion

        #region Cash Voucher

        public static class CashVoucherReports
        {
            public static void VoucherForm(int documentNumber)
            {
                DataTable cvdoc1 = DataSource.CashVoucher.GetCashVoucherDocument(documentNumber);
                cvdoc1.TableName = "cvdoc1";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(cvdoc1);
                dataSet.Tables.Add(comp);


                var ri = new ReportItem();
                ri.ReportFile = "cv_form.rpt";
                ri.Title = "Cash Voucher";
                ri.DataSource = dataSet;
                ri.LoadReport();
            }

            public static void DetailedReport(DateTime dateStart, DateTime dateEnd)
            {
                var titleBuilder = new StringBuilder();
                if (dateStart == dateEnd)
                {
                    titleBuilder.Append("As of ");
                    titleBuilder.Append(String.Format("{0:MMMM dd, yyyy}", dateStart));
                }
                else
                {
                    titleBuilder.Append("For the month of ");
                    titleBuilder.Append(String.Format("{0:MMMM yyyy}", dateStart));
                }

                var cv = DataSource.CashVoucher.WhereDocumentDateBetween(dateStart, dateEnd);
                cv.TableName = "cv";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(cv);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "cv_detailed.rpt";
                ri.Title = titleBuilder.ToString();
                ri.DataSource = dataSet;
                ri.LoadReport();
            }

            internal static void SummaryReport(Views.VoucherReportViewModel reportViewModel)
            {
                var dateStart = reportViewModel.DateRange[0];
                var dateEnd = reportViewModel.DateRange[1];

                var titleBuilder = new StringBuilder();
                if (dateStart == dateEnd)
                {
                    titleBuilder.Append("As of ");
                    titleBuilder.Append(String.Format("{0:MMMM dd, yyyy}", dateStart));
                }
                else
                {
                    titleBuilder.Append("For the month of ");
                    titleBuilder.Append(String.Format("{0:MMMM yyyy}", dateStart));
                }

                var cv = DataSource.CashVoucher.WhereDocumentDateBetween(dateStart, dateEnd);
                cv.TableName = "cv";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(cv);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "cv_summary.rpt";
                ri.Title = titleBuilder.ToString();
                ri.DataSource = dataSet;
                ri.LoadReport();
            }

            public static void Attachment(int documentNumber)
            {
                try
                {
                    DataTable cv = DataSource.CashVoucher.WhereDocumentNumberEquals(documentNumber);
                    cv.TableName = "cv";

                    DataTable voucher_explanation = Voucher.GetExplanation(VoucherTypes.CV, documentNumber);
                    voucher_explanation.TableName = "voucher_explanation";

                    DataTable comp = Company.GetData();
                    comp.TableName = "comp";

                    var dataSet = new DataSet();
                    dataSet.Tables.Add(cv);
                    dataSet.Tables.Add(voucher_explanation);
                    dataSet.Tables.Add(comp);


                    var ri = new ReportItem();
                    ri.ReportFile = "cv_attachment.rpt";
                    ri.Title = "Cash Voucher Attachment";
                    ri.DataSource = dataSet;
                    ri.LoadReport();

                }
                catch (Exception e)
                {
                    Logger.ExceptionLogger(new ReportController(), e);
                }
            }

            internal static void PerAccount(Views.VoucherReportViewModel reportViewModel)
            {
                try
                {
                    var accountCode = reportViewModel.AccountCode;
                    var dateStart = reportViewModel.DateRange[0];
                    var dateEnd = reportViewModel.DateRange[1];

                    var titleBuilder = new StringBuilder();
                    if (dateStart == dateEnd)
                    {
                        titleBuilder.Append("As of ");
                        titleBuilder.Append(String.Format("{0:MMMM dd, yyyy}", dateStart));
                    }
                    else
                    {
                        titleBuilder.Append("For the month of ");
                        titleBuilder.Append(String.Format("{0:MMMM yyyy}", dateStart));
                    }

                    DataTable cv = DataSource.CashVoucher.WhereAccountCodeIsAndDateIsBetwen(accountCode, dateStart,
                                                                                            dateEnd);
                    cv.TableName = "cv";

                    DataTable comp = Company.GetData();
                    comp.TableName = "comp";

                    var dataSet = new DataSet();
                    dataSet.Tables.Add(cv);
                    dataSet.Tables.Add(comp);


                    var ri = new ReportItem();
                    ri.ReportFile = "cv_per_account.rpt";
                    ri.Title = titleBuilder.ToString();
                    ri.DataSource = dataSet;
                    ri.LoadReport();

                }
                catch (Exception e)
                {
                    Logger.ExceptionLogger(new ReportController(), e);
                }
            }
        }

        #endregion

        #region Loan Report

        public static Result GenerateLoanDetailsReport(DataTable loanDetails)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(25);


                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, loanDetails, reportInfo.Title, reportPath);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        public static Result GenerateLoanAmortizationReport(DataTable loanDetailsOfAmortization,
                                                            DataTable loanPaymentDetails)
        {
            try
            {
                //var reportInfo = new ReportItem();
                //reportInfo.Find(26);
                //DataTable header = Company.GetList("SCCO");
                //string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                //ReportCreator.DisplayReport(loanPaymentDetails, header, loanDetailsOfAmortization,
                //                            reportInfo.Title, reportPath);
                //return new Result(true, "Successfully Generated.");
                var dataTables = new DataTable[2];
                dataTables[0] = loanDetailsOfAmortization;
                dataTables[1] = loanPaymentDetails;
                var reportItem = new ReportItem();
                return reportItem.LoadReport(dataTables, "loan_amortization_schedule.rpt");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        #endregion

        #region Schedule Report

        /*  NOTE : Please set the Report Schedule in Chart of Accounts  */

        public static Result GenerateScheduleOfInterestOnLoans(DateTime asOf, string orderBy)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(10);
                string sqlCommandText = String.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcedure,
                                                      Converter.DateTimeToMySqlDate(asOf), orderBy);
                DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, details, reportInfo.Title, reportPath,
                                            "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        public static Result GenerateScheduleOfTimeDeposit(DateTime asOf, string orderBy)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(11);
                string sqlCommandText = String.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcedure,
                                                      Converter.DateTimeToMySqlDate(asOf), orderBy);
                DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, details, reportInfo.Title, reportPath,
                                            "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        public static Result GenerateScheduleOfLoans(DateTime asOf, string orderBy)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(12);
                string sqlCommandText = String.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcedure,
                                                      Converter.DateTimeToMySqlDate(asOf), orderBy);
                DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, details, reportInfo.Title, reportPath,
                                            "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        public static Result GenerateScheduleOfSavingsDeposit(DateTime asOf, string orderBy)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(13);
                string sqlCommandText = String.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcedure,
                                                      Converter.DateTimeToMySqlDate(asOf), orderBy);
                DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, details, reportInfo.Title, reportPath,
                                            "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        public static Result GenerateScheduleOfShareCapital(DateTime asOf, string orderBy)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(14);
                string sqlCommandText = String.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcedure,
                                                      Converter.DateTimeToMySqlDate(asOf), orderBy);
                DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header,
                                            details, reportInfo.Title, reportPath,
                                            "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        public static Result GenerateScheduleOfTimeDepositConsolidated(DateTime asOf, string orderBy)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(16);
                string sqlCommandText = String.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcedure,
                                                      Converter.DateTimeToMySqlDate(asOf), orderBy);
                DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, details, reportInfo.Title, reportPath,
                                            "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        public static Result GenerateScheduleOfFines(DateTime asOf, string orderBy)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(17);
                string sqlCommandText = String.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcedure,
                                                      Converter.DateTimeToMySqlDate(asOf), orderBy);
                DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, details, reportInfo.Title, reportPath,
                                            "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        public static Result GenerateScheduleReports(int reportScheduleId, string reportDescription, DateTime asOf,
                                                     string orderBy)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(22);
                string sqlCommandText = String.Format("CALL {0} ({1}, '{2}','{3}'  ) ", reportInfo.StoredProcedure,
                                                      reportScheduleId, Converter.DateTimeToMySqlDate(asOf), orderBy);
                DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, details, reportDescription, reportPath,
                                            "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        #endregion

        #region Member

        public static Result GenerateMemberReport(string memberCode)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(15);
                string sqlCommandText = String.Format("CALL {0} ('{1}') ", reportInfo.StoredProcedure,
                                                      memberCode.Trim());
                DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, details, reportInfo.Title, reportPath);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        public static Result GenerateMemberListReport(DataTable result, string reportTitle)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(21);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, result, reportTitle, reportPath);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        #endregion

        #region Verifier

        private static Result ShowStatementAccountSummary(string memberCode, DateTime transactionDate)
        {
            try
            {
                //var reportInfo = new ReportItem();
                //reportInfo.Find(19);
                //string sqlCommandText = String.Format("CALL {0} ('{1}','{2}') ", reportInfo.StoredProcedure,
                //                                      memberCode,
                //                                      Converter.DateTimeToMySqlDate(transactionDate));
                //DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                //DataTable header = Company.GetList("SCCO");
                //string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                //ReportCreator.DisplayReport(header, details, reportInfo.Title, reportPath);

                var spParams = new List<SqlParameter>();
                spParams.Add(new SqlParameter("?ps_member_code",memberCode));
                spParams.Add(new SqlParameter("?pd_as_of",transactionDate));
                var dtSoaSummary = DatabaseController.ExecuteStoredProcedure("sp_soa_summary", spParams.ToArray());
                var reportItem = new ReportItem();
                reportItem.Title = "Statement of Accounts - Summary";
                return reportItem.LoadReport(dtSoaSummary, "soa_summary.rpt");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        private static Result GenerateDetailedStatementAccount(string memberCode, string accountCode,
                                                      DateTime transactionDate, Int32 timeDepositId,
                                                      string accountTitle)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(18);
                string sqlCommandText = String.Format("CALL {0} ('{1}','{2}','{3}',{4}) ", reportInfo.StoredProcedure,
                                                      memberCode, accountCode,
                                                      Converter.DateTimeToMySqlDate(transactionDate), timeDepositId);
                DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, details, reportInfo.Title, reportPath, accountTitle);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        internal static Result ShowStatementAccountSummary(ObservableCollection<AccountSummary> accountSummaries, DateTime asOf)
        {
            if (null == accountSummaries || accountSummaries.Count == 0) return new Result(false, "No account summary");

            var dataTables = new List<DataTable>();
            var dtSoaDetailed = new DataTable("soa_summary");

            dtSoaDetailed.Columns.Add("member_code", typeof(string));
            dtSoaDetailed.Columns.Add("member_name", typeof(string));
            dtSoaDetailed.Columns.Add("account_code", typeof(string));
            dtSoaDetailed.Columns.Add("account_title", typeof(string));
            dtSoaDetailed.Columns.Add("debit_account", typeof(decimal));
            dtSoaDetailed.Columns.Add("credit_account", typeof(decimal));
            dtSoaDetailed.Columns.Add("ending_balance", typeof(decimal));

            foreach (var item in accountSummaries)
            {
                DataRow dr = dtSoaDetailed.NewRow();
                dr["member_code"] = item.MemberCode;
                dr["member_name"] = item.MemberName;
                dr["account_code"] = item.AccountCode;
                dr["account_title"] = item.AccountTitle;
                dr["debit_account"] = item.DebitAccount;
                dr["credit_account"] = item.CreditAccount;
                dr["ending_balance"] = item.Balance;
                dtSoaDetailed.Rows.Add(dr);
            }

            dataTables.Add(dtSoaDetailed);
            var reportItem = new ReportItem();
            
            reportItem.Title = "Statement of Accounts - Summary";
            return reportItem.LoadReport(asOf, dataTables.ToArray(), "soa_summary.rpt");
        }

        internal static Result ShowStatementAccountDetailed(ObservableCollection<AccountDetail> accountDetails, DateTime asOf)
        {
            if (null == accountDetails || accountDetails.Count == 0) return new Result(false,"No account details");

            var dataTables = new List<DataTable>();
            var dtSoaDetailed = new DataTable("soa_detailed");

            dtSoaDetailed.Columns.Add("member_code", typeof(string));
            dtSoaDetailed.Columns.Add("member_name", typeof(string));
            dtSoaDetailed.Columns.Add("account_code", typeof(string));
            dtSoaDetailed.Columns.Add("account_title", typeof(string));
            dtSoaDetailed.Columns.Add("transaction_date", typeof(DateTime));
            dtSoaDetailed.Columns.Add("reference", typeof(string));
            dtSoaDetailed.Columns.Add("debit_amount", typeof(decimal));
            dtSoaDetailed.Columns.Add("credit_amount", typeof(decimal));
            dtSoaDetailed.Columns.Add("ending_balance", typeof(decimal));
            dtSoaDetailed.Columns.Add("trace_code", typeof(string));

            foreach (var ad in accountDetails)
            {
                DataRow dr = dtSoaDetailed.NewRow();
                dr["member_code"] = ad.MemberCode;
                dr["member_name"] = ad.MemberName;
                dr["account_code"] = ad.AccountCode;
                dr["account_title"] = ad.Title;
                dr["transaction_date"] = ad.VoucherDate;
                dr["reference"] = string.Format("{0} {1}", ad.VoucherType, ad.VoucherNumber);
                dr["debit_amount"] = ad.Debit;
                dr["credit_amount"] = ad.Credit;
                dr["ending_balance"] = ad.EndingBalance;
                dr["trace_code"] = ad.Initial;
                dtSoaDetailed.Rows.Add(dr);
            }

            dataTables.Add(dtSoaDetailed);
            var reportItem = new ReportItem();
            reportItem.Title = "Statement of Accounts - Detailed";
            return reportItem.LoadReport(asOf, dataTables.ToArray(), "soa_detailed.rpt");
        }



        #endregion

        #region Voucher Attachment

        public static Result GenerateVoucherAttachment(string voucherType, Int32 documentNumber,
                                                       string acccountCode = "")
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(20);
                string sqlCommandText = String.Format("CALL {0} ('{1}',{2},'{3}' ) ", reportInfo.StoredProcedure,
                                                      voucherType, documentNumber, acccountCode);
                DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, details, reportInfo.Title, reportPath);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        #endregion

        #region Passbook

        public static Result GeneratePassbook(DataTable result)
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(24);
                DataTable header = Company.GetList("SCCO");
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, result, reportInfo.Title, reportPath);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        #endregion

        #region --- GeneralLedgerReport ---

        public static Result GenerateStatementOfOperation(DateTime asOf)
        {
            try
            {

                //var reportItem = new ReportItem();
                //reportItem.ReportFile = "financial_operation.rpt";
                //reportItem.Title = "Statement of Financial Operation";
                //reportItem.StoredProcedure = "sp_financial_operation";
                //reportItem.StoredProcedureParameters = new[] { new SqlParameter("td_as_of", asOf) };


                DataTable tblFinancialOperation =
                    DatabaseController.ExecuteStoredProcedure("sp_financial_operation",
                                                                       new SqlParameter("td_as_of", asOf));

                tblFinancialOperation.TableName = "financial_operation";

                DataTable tblGainOnLoss =
                    DatabaseController.ExecuteStoredProcedure("sp_financial_operation_gain_on_sale",
                                                                       new SqlParameter("td_as_of", asOf));

                tblGainOnLoss.TableName = "financial_operation_gain_on_sale";

                var dataTables = new DataTable[2];
                dataTables[0] = tblFinancialOperation;
                dataTables[1] = tblGainOnLoss;


                var reportItem = new ReportItem();
                reportItem.ReportFile = "financial_operation.rpt";
                reportItem.Title = "Statement of Financial Operation";

                return reportItem.LoadReport(dataTables, "financial_operation.rpt");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }


        public static Result GenerateStatementFinancialCondition(DateTime asOf)
        {
            try
            {
                //var reportItem = new ReportItem();
                //reportItem.ReportFile = "financial_condition.rpt";
                //reportItem.Title = "Statement of Financial Condition";
                //reportItem.StoredProcedure = "sp_financial_condition";
                //reportItem.StoredProcedureParameters = new[] {new SqlParameter("td_as_of", asOf)};

                //return reportItem.LoadReport();
                var sp = "sp_financial_condition_custom";
                var t = new SqlParameter("td_as_of", asOf);
                var dataTable = DatabaseController.ExecuteStoredProcedure(sp, t);
                var recordCount = dataTable.Rows.Count;

                for (int i = 0; i < recordCount; i++)
                {
                    var functionName = "fn_sum_end_balance";
                    var columnHeader = "end_balance_formula";
                    var columnValue = DataConverter.ToString(dataTable.Rows[i][columnHeader]);

                    if (!string.IsNullOrEmpty(columnValue))
                    {
                        // fs.end_balance_formula LIKE %fn_sum_end_balance% 
                        if (columnValue.Contains(functionName))
                        {
                            var parse = columnValue.Replace(functionName, "");
                            var ids = parse.Split(',').ToList();
                            if (!ids.Any()) continue;

                            var listIds = ids.Select(id => Convert.ToInt32(id)).ToList();
                            int lowBound = listIds.Min();
                            int highBound = listIds.Max();

                            var filtered = from row in dataTable.AsEnumerable()
                                           where row.Field<int>("id") >= lowBound &&
                                                 row.Field<int>("id") <= highBound
                                           select row;

                            var sumEndBalance = 0m;
                            foreach (DataRow dataRow in filtered)
                            {
                                int fId = DataConverter.ToInteger(dataRow["id"]);
                                if (listIds.Contains(fId))
                                {
                                    sumEndBalance += DataConverter.ToDecimal(dataRow["end_balance"]);
                                }
                            }
                            dataTable.Rows[i]["end_balance"] = sumEndBalance;

                        }

                    }

                    functionName = "fn_sum_end_balance";
                    columnHeader = "group_balance_formula";
                    columnValue = DataConverter.ToString(dataTable.Rows[i][columnHeader]);

                    if (!string.IsNullOrEmpty(columnValue))
                    {
                        #region --- Get Sum Group Balance in End Balance Column ---

                        // fs.group_balance_formula LIKE %fn_sum_end_balance% 
                        if (columnValue.Contains(functionName))
                        {
                            var parse = columnValue.Replace(functionName, "");
                            var ids = parse.Split(',').ToList();
                            if (!ids.Any()) continue;

                            var listIds = ids.Select(id => Convert.ToInt32(id)).ToList();
                            int lowBound = listIds.Min();
                            int highBound = listIds.Max();

                            var filtered = from row in dataTable.AsEnumerable()
                                           where row.Field<int>("id") >= lowBound &&
                                                 row.Field<int>("id") <= highBound
                                           select row;

                            var sumEndBalance = 0m;
                            foreach (DataRow dataRow in filtered)
                            {
                                int fId = DataConverter.ToInteger(dataRow["id"]);
                                if (listIds.Contains(fId))
                                {
                                    sumEndBalance += DataConverter.ToDecimal(dataRow["end_balance"]);
                                }
                            }
                            dataTable.Rows[i]["group_balance"] = sumEndBalance;
                        }

                        #endregion

                        #region --- Get Sum Group Balance in Group Balance Column ---

                        functionName = "fn_sum_group_balance";
                        columnHeader = "group_balance_formula";
                        columnValue = DataConverter.ToString(dataTable.Rows[i][columnHeader]);

                        // fs.group_balance_formula LIKE %fn_sum_group_balance% 
                        if (columnValue.Contains(functionName))
                        {
                            var parse = columnValue.Replace(functionName, "");
                            var ids = parse.Split(',').ToList();
                            if (!ids.Any()) continue;

                            var listIds = ids.Select(id => Convert.ToInt32(id)).ToList();
                            int lowBound = listIds.Min();
                            int highBound = listIds.Max();

                            var filtered = from row in dataTable.AsEnumerable()
                                           where row.Field<int>("id") >= lowBound &&
                                                 row.Field<int>("id") <= highBound
                                           select row;

                            var sumGroupEndBalance = 0m;
                            foreach (DataRow dataRow in filtered)
                            {
                                int fId = DataConverter.ToInteger(dataRow["id"]);
                                if (listIds.Contains(fId))
                                {
                                    sumGroupEndBalance += DataConverter.ToDecimal(dataRow["group_balance"]);
                                }
                            }
                            dataTable.Rows[i]["group_balance"] = sumGroupEndBalance;


                        }

                        #endregion
                    }

                }
                
                var reportItem = new ReportItem();
                var reportFile = "financial_condition_custom.rpt";
                reportItem.Title = "Statement of Financial Condition";
                return reportItem.LoadReport(dataTable, reportFile);


            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }


        public static Result GenerateStatementCashFlow()
        {
            try
            {
                var reportInfo = new ReportItem();
                reportInfo.Find(28);
                DataTable header = Company.GetList("SCCO");
                string sqlCommandText = String.Format("CALL {0} () ", reportInfo.StoredProcedure);
                DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
                string reportPath = Path.Combine(ReportFolder, reportInfo.ReportFile);
                ReportCreator.DisplayReport(header, details, reportInfo.Title, reportPath);
                return new Result(true, "Successfully Generated.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        #endregion

        #region --- Time Deposit ---

        public static Result ShowPretermComputationForInterestOnTimeDeposit(int timeDepositDetailId)
        {
            Action showReport = () =>
                                    {
                                        var ri = new ReportItem();
                                        ri.ReportFile = "PretermComputationForInterestOnTD.rpt";
                                        ri.Title = "Preterm Computation For Interest On Time Deposit";
                                        var dataSet = new DataSet();
                                        //DataTable dataTable =
                                        //    //DataGenerator.GetPretermComputationOnTimeDeposit(timeDepositDetailId);
                                        //dataTable.TableName = "PretermComputationForInterestOnTD";
                                        //dataSet.Tables.Add(dataTable);

                                        //dataSet.Tables.Add(
                                        //    DataGenerator.GetPretermComputationOnTimeDeposit(timeDepositDetailId));
                                        ////ri.DataSource.Tables.Add(
                                        //    DataGenerator.GetPretermComputationOnTimeDeposit(timeDepositDetailId));
                                        //ri.DataSource = DataGenerator.GetPretermComputationOnTimeDeposit(timeDepositDetailId);
                                        ri.DataSource = dataSet;
                                        ri.LoadReport();
                                    };

            return ActionController.InvokeAction(showReport);
        }

        public static class TimeDeposit
        {
            public static Result PrintCertificate(Nfmb member, Models.TimeDeposit.TimeDepositDetails timeDepositDetails)
            {
                Action showReport = () =>
                    {
                        const string reportFile = "time_deposit_certificate.rpt";

                        var sql = "SELECT @member_code as member_code, " +
                                  "@member_name as member_name, " +
                                  "@certificate_number as certificate_number, " +
                                  "@amount as amount, " +
                                  "@amount_in_words as amount_in_words, " +
                                  "@term as term, " +
                                  "CAST(@date_issued AS DATE) as date_issued, " +
                                  "CAST(@due_date AS DATE) as due_date, " +
                                  "@interest_rate as interest_rate, " +
                                  "@authorized_signature1 as authorized_signature1, " +
                                  "@authorized_signature2 as authorized_signature2 " +
                                  "FROM nfmb " +
                                  "LIMIT 1";
                        var parameters = new[]
                            {
                                new SqlParameter("@member_code", member.MemberCode), 
                                new SqlParameter("@member_name", member.MemberName),
                                new SqlParameter("@certificate_number", timeDepositDetails.CertificateNo), 
                                new SqlParameter("@amount", timeDepositDetails.Amount),
                                new SqlParameter("@amount_in_words", Converter.AmountToWords(timeDepositDetails.Amount)),
                                new SqlParameter("@term", string.Format("{0} {1}",timeDepositDetails.Term, timeDepositDetails.TermsMode)),
                                new SqlParameter("@date_issued", timeDepositDetails.DateIn),
                                new SqlParameter("@due_date", timeDepositDetails.Maturity), 
                                new SqlParameter("@interest_rate", Converter.ToPercentage(timeDepositDetails.Rate)),
                                new SqlParameter("@authorized_signature1", GlobalSettings.TimeDepositAuthorizedSignature1), 
                                new SqlParameter("@authorized_signature2", GlobalSettings.TimeDepositAuthorizedSignature2)
                            };
                        var dataTable = DatabaseController.ExecuteSelectQuery(sql, parameters);
                        dataTable.TableName = Path.GetFileNameWithoutExtension(reportFile);

                        ReportItem.Load(dataTable, reportFile);
                    };
                return ActionController.InvokeAction(showReport);
            }
        }

        #endregion

        #region --- Withdrawal ---

        public static Result PrintWitdrawalValidation(CashVoucher cashVoucher)
        {
            try
            {
                var statement = string.Format("SELECT * FROM `cv` WHERE ID = ?ID");
                var parameter = new SqlParameter("?ID", cashVoucher.ID);
                var dataTable = DatabaseController.ExecuteSelectQuery(statement, parameter);
                dataTable.Rows[0]["AMT_WORDS"] = Converter.AmountToWords(cashVoucher.Debit);
                dataTable.TableName = "cv";
                return ReportItem.Load(dataTable, "withdrawal_validation.rpt");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }

        #endregion

        public static void ShowReport(ReportItem reportItem)
        {

        }



        public static class DataSource
        {

            private static DataTable FindVoucherWhereDateBetween(string voucherType, DateTime dateStart,
                                                                 DateTime dateEnd)
            {
                var mysqlDateStart = Converter.DateTimeToMySqlDate(dateStart);
                var mysqlDateEnd = Converter.DateTimeToMySqlDate(dateEnd);

                var sqlBuilder = new StringBuilder();
                var sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("?DateStart", mysqlDateStart));

                if (mysqlDateStart == mysqlDateEnd)
                {
                    sqlBuilder.AppendFormat("SELECT * FROM `{0}` WHERE `DOC_DATE` = ?DateStart", voucherType);
                }
                else
                {
                    sqlBuilder.AppendFormat("SELECT * FROM `{0}` WHERE `DOC_DATE` BETWEEN ?DateStart AND ?DateEnd",
                                            voucherType);
                    sqlParams.Add(new SqlParameter("?DateEnd", mysqlDateEnd));
                }

                var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), sqlParams.ToArray());

                return dataTable;
            }

            private static DataTable FindVoucherWhereNumberEquals(string voucherType, int documentNumber)
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("SELECT * FROM `{0}` WHERE DOC_NUM = ?DOC_NUM", voucherType);
                var param = new SqlParameter("?DOC_NUM", documentNumber);
                return DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), param);
            }

            private static DataTable FindVoucherWhereAccountCodeEquals(string voucherType, string accountCode)
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat("SELECT * FROM `{0}` WHERE ACC_CODE = ?ACC_CODE", voucherType);
                var param = new SqlParameter("?ACC_CODE", accountCode);
                return DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), param);
            }

            private static DataTable FindVoucherWhereAccountCodeIsAndDateIsBetwen(string voucherType, string accountCode,
                                                                                  DateTime dateStart, DateTime dateEnd)
            {
                var mysqlDateStart = Converter.DateTimeToMySqlDate(dateStart);
                var mysqlDateEnd = Converter.DateTimeToMySqlDate(dateEnd);

                var sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("?ACC_CODE", accountCode));
                sqlParams.Add(new SqlParameter("?DateStart", mysqlDateStart));

                var queryBuilder = new StringBuilder();
                if (dateStart == dateEnd)
                {
                    queryBuilder.AppendFormat(
                        "SELECT * FROM `{0}` WHERE ACC_CODE = ?ACC_CODE AND DOC_DATE = ?DateStart", voucherType);
                }
                else
                {
                    queryBuilder.AppendFormat(
                        "SELECT * FROM `{0}` WHERE ACC_CODE = ?ACC_CODE AND `DOC_DATE` BETWEEN ?DateStart AND ?DateEnd",
                        voucherType);
                    sqlParams.Add(new SqlParameter("?DateEnd", mysqlDateEnd));
                }
                return DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), sqlParams.ToArray());
            }

            public static class JournalVoucher
            {
                public static DataTable GetJournalVoucherDocument(int documentNumber)
                {
                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append("SELECT * FROM `JV` WHERE DOC_NUM = ?DOC_NUM");
                    var param = new SqlParameter("?DOC_NUM", documentNumber);
                    DataTable journalVoucherTable = DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(),
                                                                                          param);
                    journalVoucherTable.TableName = "JV";

                    queryBuilder = new StringBuilder();
                    queryBuilder.Append("SELECT * FROM `JVDOC1` LIMIT 1");
                    DataTable jvdoc1Table = DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), param);

                    if (journalVoucherTable.Rows.Count == 0) return jvdoc1Table;

                    DataRow jvdoc1Row = jvdoc1Table.Rows[0];
                    jvdoc1Row["CODE"] = journalVoucherTable.Rows[0]["MEM_CODE"];
                    jvdoc1Row["NAME"] = journalVoucherTable.Rows[0]["MEM_NAME"];
                    jvdoc1Row["DATE"] = journalVoucherTable.Rows[0]["DOC_DATE"];
                    jvdoc1Row["DOCNO"] = journalVoucherTable.Rows[0]["DOC_NUM"];

                    //var explanation = DataConverter.ToString(journalVoucherTable.Rows[0]["EXPLAIN"]);
                    var explanation = "";

                    // reset debit and credit values
                    for (int i = 1; i <= 30; i++)
                    {
                        jvdoc1Row["COL" + (i)] = 0;
                    }
                    // reset account titles
                    for (int i = 1; i <= 15; i++)
                    {
                        jvdoc1Row["CNAME" + (i)] = "";
                    }

                    Nfmb member = Nfmb.FindByCode(DataConverter.ToString(jvdoc1Row["CODE"]));
                    var addressBuilder = new StringBuilder();
                    if (member.Address1.Length > 0)
                    {
                        addressBuilder.Append(member.Address1.Trim() + " ");
                    }
                    if (member.Address2.Length > 0)
                    {
                        addressBuilder.Append(member.Address2.Trim() + " ");
                    }
                    if (member.Address3.Length > 0)
                    {
                        addressBuilder.Append(member.Address3.Trim());
                    }
                    jvdoc1Row["ADDRESS"] = addressBuilder.ToString();

                    for (int i = 0; i < journalVoucherTable.Rows.Count; i++)
                    {
                        // maximum rows = 15
                        if (i < 15)
                        {
                            var colTitle = "CNAME" + (i + 1); //CNAME1
                            var colCredit = "COL" + (i + 1); //COL1
                            var colDebit = "COL" + (i + 16); //COL16
                            jvdoc1Row[colTitle] = journalVoucherTable.Rows[i]["TITLE"];
                            jvdoc1Row[colCredit] = DataConverter.ToDecimal(journalVoucherTable.Rows[i]["CREDIT"]);
                            jvdoc1Row[colDebit] = DataConverter.ToDecimal(journalVoucherTable.Rows[i]["DEBIT"]);

                            if (string.IsNullOrEmpty(explanation.Trim()) || DataConverter.ToString(jvdoc1Row["EXPLAIN"]) == String.Empty)
                            {
                                //jvdoc1Row["EXPLAIN"] = journalVoucherTable.Rows[i]["EXPLAIN"];
                                explanation = DataConverter.ToUtf8String(journalVoucherTable.Rows[i]["EXPLAIN"]);
                                jvdoc1Row["EXPLAIN"] = explanation;
                            }

                            if (DataConverter.ToString(jvdoc1Row["SUM_OF"]) == String.Empty)
                            {
                                jvdoc1Row["SUM_OF"] = journalVoucherTable.Rows[i]["AMT_WORDS"];
                                jvdoc1Row["CHECK"] = journalVoucherTable.Rows[i]["CHECK_NUM"];
                                jvdoc1Row["BANK"] = journalVoucherTable.Rows[i]["BANK_TITLE"];
                                jvdoc1Row["AMT"] = journalVoucherTable.Rows[i]["AMOUNT"];
                                jvdoc1Row["PAY_TO"] = journalVoucherTable.Rows[i]["CREDIT"];
                            }

                            //break;
                        }
                    }

                    return jvdoc1Table;
                }

                public static DataTable WhereDocumentNumberEquals(int documentNumber)
                {
                    return FindVoucherWhereNumberEquals("JV", documentNumber);
                }

                public static DataTable WhereDocumentDateBetween(DateTime dateStart, DateTime dateEnd)
                {
                    return FindVoucherWhereDateBetween("JV", dateStart, dateEnd);
                }

                public static DataTable WhereAccountCodeEquals(string accountCode)
                {
                    return FindVoucherWhereAccountCodeEquals("JV", accountCode);
                }

                public static DataTable WhereAccountCodeIsAndDateIsBetwen(string accountCode, DateTime dateStart,
                                                                          DateTime dateEnd)
                {
                    return FindVoucherWhereAccountCodeIsAndDateIsBetwen("JV", accountCode, dateStart, dateEnd);
                }
            }

            public static class CashVoucher
            {
                public static DataTable GetCashVoucherDocument(int documentNumber)
                {
                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append("SELECT * FROM `CV` WHERE DOC_NUM = ?DOC_NUM");
                    var param = new SqlParameter("?DOC_NUM", documentNumber);
                    DataTable cashVoucherTable = DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(),
                                                                                       param);
                    cashVoucherTable.TableName = "CV";

                    queryBuilder = new StringBuilder();
                    queryBuilder.Append("SELECT * FROM `CVDOC1` LIMIT 1");
                    DataTable cvdoc1Table = DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), param);

                    if (cashVoucherTable.Rows.Count == 0) return cvdoc1Table;

                    DataRow cvdoc1Row = cvdoc1Table.Rows[0];
                    cvdoc1Row["CODE"] = cashVoucherTable.Rows[0]["MEM_CODE"];
                    cvdoc1Row["NAME"] = cashVoucherTable.Rows[0]["MEM_NAME"];
                    cvdoc1Row["DATE"] = cashVoucherTable.Rows[0]["DOC_DATE"];
                    cvdoc1Row["DOCNO"] = cashVoucherTable.Rows[0]["DOC_NUM"];

                    //var explanation = DataConverter.ToString(cashVoucherTable.Rows[0]["EXPLAIN"]);
                    var explanation = "";


                    // reset debit and credit values
                    for (int i = 1; i <= 30; i++)
                    {
                        cvdoc1Row["COL" + (i)] = 0;
                    }
                    // reset account titles
                    for (int i = 1; i <= 15; i++)
                    {
                        cvdoc1Row["CNAME" + (i)] = "";
                    }

                    Nfmb member = Nfmb.FindByCode(DataConverter.ToString(cvdoc1Row["CODE"]));
                    var addressBuilder = new StringBuilder();
                    if (member.Address1.Length > 0)
                    {
                        addressBuilder.Append(member.Address1.Trim() + " ");
                    }
                    if (member.Address2.Length > 0)
                    {
                        addressBuilder.Append(member.Address2.Trim() + " ");
                    }
                    if (member.Address3.Length > 0)
                    {
                        addressBuilder.Append(member.Address3.Trim());
                    }
                    cvdoc1Row["ADDRESS"] = addressBuilder.ToString();

                    for (int i = 0; i < cashVoucherTable.Rows.Count; i++)
                    {
                        // maximum rows = 15
                        if (i < 15)
                        {
                            var colTitle = "CNAME" + (i + 1); //CNAME1
                            var colCredit = "COL" + (i + 1); //COL1
                            var colDebit = "COL" + (i + 16); //COL16
                            cvdoc1Row[colTitle] = cashVoucherTable.Rows[i]["TITLE"];
                            cvdoc1Row[colCredit] = DataConverter.ToDecimal(cashVoucherTable.Rows[i]["CREDIT"]);
                            cvdoc1Row[colDebit] = DataConverter.ToDecimal(cashVoucherTable.Rows[i]["DEBIT"]);

                            if (string.IsNullOrEmpty(explanation.Trim()) || DataConverter.ToString(cvdoc1Row["EXPLAIN"]) == String.Empty)
                            {
                                //cvdoc1Row["EXPLAIN"] = cashVoucherTable.Rows[i]["EXPLAIN"];
                                explanation = DataConverter.ToUtf8String(cashVoucherTable.Rows[i]["EXPLAIN"]);
                                cvdoc1Row["EXPLAIN"] = explanation;
                            }

                            if (DataConverter.ToString(cvdoc1Row["SUM_OF"]) == String.Empty)
                            {
                                cvdoc1Row["SUM_OF"] = cashVoucherTable.Rows[i]["AMT_WORDS"];
                                cvdoc1Row["CHECK"] = cashVoucherTable.Rows[i]["CHECK_NUM"];
                                cvdoc1Row["BANK"] = cashVoucherTable.Rows[i]["BANK_TITLE"];
                                var debit = DataConverter.ToDecimal(cashVoucherTable.Rows[i]["DEBIT"]);
                                var credit = DataConverter.ToDecimal(cashVoucherTable.Rows[i]["CREDIT"]);
                                cvdoc1Row["AMT"] = debit + credit;
                                cvdoc1Row["PAY_TO"] = cashVoucherTable.Rows[i]["CREDIT"];
                            }
                           
                            //break;
                        }
                    }

                    return cvdoc1Table;
                }

                public static DataTable WhereDocumentNumberEquals(int documentNumber)
                {
                    return FindVoucherWhereNumberEquals("CV", documentNumber);
                }

                public static DataTable WhereDocumentDateBetween(DateTime dateStart, DateTime dateEnd)
                {
                    return FindVoucherWhereDateBetween("CV", dateStart, dateEnd);
                }

                public static DataTable WhereAccountCodeEquals(string accountCode)
                {
                    return FindVoucherWhereAccountCodeEquals("CV", accountCode);
                }

                public static DataTable WhereAccountCodeIsAndDateIsBetwen(string accountCode, DateTime dateStart,
                                                                          DateTime dateEnd)
                {
                    return FindVoucherWhereAccountCodeIsAndDateIsBetwen("CV", accountCode, dateStart, dateEnd);
                }

            }

            public static class OfficialReceipt
            {
                public static DataTable GetOfficialReceiptDocument(int documentNumber)
                {
                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append("SELECT * FROM `OR` WHERE DOC_NUM = ?DOC_NUM");
                    var param = new SqlParameter("?DOC_NUM", documentNumber);
                    DataTable officialReceiptTable = DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(),
                                                                                           param);
                    officialReceiptTable.TableName = "OR";

                    queryBuilder = new StringBuilder();
                    queryBuilder.Append("SELECT * FROM `ORDOC1` LIMIT 1");
                    DataTable ordoc1Table = DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), param);

                    if (officialReceiptTable.Rows.Count == 0) return ordoc1Table;

                    DataRow ordoc1Row = ordoc1Table.Rows[0];
                    ordoc1Row["CODE"] = officialReceiptTable.Rows[0]["MEM_CODE"];
                    ordoc1Row["NAME"] = officialReceiptTable.Rows[0]["MEM_NAME"];
                    ordoc1Row["DATE"] = officialReceiptTable.Rows[0]["DOC_DATE"];

                    //ordoc1Row["DOCNO"] = officialReceiptTable.Rows[0]["DOC_NUM"];
                    //ordoc1Row["EXPLAIN"] = officialReceiptTable.Rows[0]["EXPLAIN"];

                    // reset credit values
                    for (int i = 1; i <= 15; i++)
                    {
                        ordoc1Row["COL" + (i)] = 0;
                    }
                    // reset account titles
                    for (int i = 1; i <= 15; i++)
                    {
                        ordoc1Row["CNAME" + (i)] = "";
                    }

                    Nfmb member = Nfmb.FindByCode(DataConverter.ToString(ordoc1Row["CODE"]));
                    var addressBuilder = new StringBuilder();
                    if (member.Address1.Length > 0)
                    {
                        addressBuilder.Append(member.Address1.Trim() + " ");
                    }
                    if (member.Address2.Length > 0)
                    {
                        addressBuilder.Append(member.Address2.Trim() + " ");
                    }
                    if (member.Address3.Length > 0)
                    {
                        addressBuilder.Append(member.Address3.Trim());
                    }
                    ordoc1Row["ADDRESS"] = addressBuilder.ToString();

                    var cashOnHand = GlobalSettings.CodeOfCashOnHand;

                    for (int i = 0; i < officialReceiptTable.Rows.Count; i++)
                    {
                        // maximum rows = 15
                        if (i < 15)
                        {
                            var orRow = officialReceiptTable.Rows[i];

                            if (DataConverter.ToString(orRow["ACC_CODE"]) == cashOnHand)
                            {
                                ordoc1Row["SUM_OF"] =
                                    Converter.AmountToWords(DataConverter.ToDecimal(orRow["DEBIT"]));

                                #region --- Get Cash Denomination ---

                                var denomination = new Dictionary<decimal, int>();

                                if (DataConverter.ToInteger(orRow["DEN1"]) > 0)
                                {
                                    denomination.Add(1000m, DataConverter.ToInteger(orRow["DEN1"]));
                                }
                                if (DataConverter.ToInteger(orRow["DEN2"]) > 0)
                                {
                                    denomination.Add(500m, DataConverter.ToInteger(orRow["DEN2"]));
                                }
                                if (DataConverter.ToInteger(orRow["DEN9"]) > 0)
                                {
                                    denomination.Add(200m, DataConverter.ToInteger(orRow["DEN9"])); // replaces .5
                                }
                                if (DataConverter.ToInteger(orRow["DEN3"]) > 0)
                                {
                                    denomination.Add(100m, DataConverter.ToInteger(orRow["DEN3"]));
                                }
                                if (DataConverter.ToInteger(orRow["DEN4"]) > 0)
                                {
                                    denomination.Add(50m, DataConverter.ToInteger(orRow["DEN4"]));
                                }
                                if (DataConverter.ToInteger(orRow["DEN5"]) > 0)
                                {
                                    denomination.Add(20m, DataConverter.ToInteger(orRow["DEN5"]));
                                }
                                if (DataConverter.ToInteger(orRow["DEN6"]) > 0)
                                {
                                    denomination.Add(10m, DataConverter.ToInteger(orRow["DEN6"]));
                                }
                                if (DataConverter.ToInteger(orRow["DEN7"]) > 0)
                                {
                                    denomination.Add(5m, DataConverter.ToInteger(orRow["DEN7"]));
                                }
                                if (DataConverter.ToInteger(orRow["DEN8"]) > 0)
                                {
                                    denomination.Add(1m, DataConverter.ToInteger(orRow["DEN8"]));
                                }
                                if (DataConverter.ToInteger(orRow["DEN10"]) > 0)
                                {
                                    denomination.Add(.25m, DataConverter.ToInteger(orRow["DEN10"]));
                                }

                                var denoLimit = 0;
                                foreach (var deno in denomination)
                                {
                                    denoLimit++;

                                    if (denoLimit > 5) break;
                                    switch (denoLimit)
                                    {
                                        case 1:
                                            ordoc1Row["DENO_1"] = deno.Key;
                                            ordoc1Row["DENO1"] = deno.Value;
                                            break;

                                        case 2:
                                            ordoc1Row["DENO_2"] = deno.Key;
                                            ordoc1Row["DENO2"] = deno.Value;
                                            break;

                                        case 3:
                                            ordoc1Row["DENO_3"] = deno.Key;
                                            ordoc1Row["DENO3"] = deno.Value;
                                            break;

                                        case 4:
                                            ordoc1Row["DENO_4"] = deno.Key;
                                            ordoc1Row["DENO4"] = deno.Value;
                                            break;

                                        case 5:
                                            ordoc1Row["DENO_5"] = deno.Key;
                                            ordoc1Row["DENO5"] = deno.Value;
                                            break;
                                    }
                                }

                                #endregion --- Get Cash Denomination ---

                                #region --- Get Check Denomination ---

                                ordoc1Row["BNK1"] = orRow["BNAME1"];
                                ordoc1Row["CHK1"] = orRow["BCHECK1"];
                                ordoc1Row["AMT1"] = orRow["BAMT1"];

                                ordoc1Row["BNK2"] = orRow["BNAME2"];
                                ordoc1Row["CHK2"] = orRow["BCHECK2"];
                                ordoc1Row["AMT2"] = orRow["BAMT2"];

                                ordoc1Row["BNK3"] = orRow["BNAME3"];
                                ordoc1Row["CHK3"] = orRow["BCHECK3"];
                                ordoc1Row["AMT3"] = orRow["BAMT3"];

                                ordoc1Row["BNK4"] = orRow["BNAME4"];
                                ordoc1Row["CHK4"] = orRow["BCHECK4"];
                                ordoc1Row["AMT4"] = orRow["BAMT4"];

                                ordoc1Row["BNK5"] = orRow["BNAME5"];
                                ordoc1Row["CHK5"] = orRow["BCHECK5"];
                                ordoc1Row["AMT5"] = orRow["BAMT5"];

                                #endregion --- Get Check Denomination ---
                            }
                            else
                            {
                                var colTitle = "CNAME" + (i + 1); //CNAME1
                                var colCredit = "COL" + (i + 1); //COL1
                                ordoc1Row[colTitle] = officialReceiptTable.Rows[i]["TITLE"];
                                ordoc1Row[colCredit] =
                                    DataConverter.ToDecimal(officialReceiptTable.Rows[i]["CREDIT"]);
                            }
                        }
                    }

                    return ordoc1Table;
                }

                public static DataTable WhereDocumentNumberEquals(int documentNumber)
                {
                    return FindVoucherWhereNumberEquals("OR", documentNumber);
                }

                public static DataTable WhereDocumentDateBetween(DateTime dateStart, DateTime dateEnd)
                {
                    return FindVoucherWhereDateBetween("OR", dateStart, dateEnd);
                }

                public static DataTable WhereAccountCodeEquals(string accountCode)
                {
                    return FindVoucherWhereAccountCodeEquals("OR", accountCode);
                }

                public static DataTable WhereAccountCodeIsAndDateIsBetwen(string accountCode, DateTime dateStart,
                                                                          DateTime dateEnd)
                {
                    return FindVoucherWhereAccountCodeIsAndDateIsBetwen("OR", accountCode, dateStart, dateEnd);
                }

                internal static DataTable DailyCollectionPerCollector(DateTime date, string collector)
                {
                    var mysqlDocumentDate = Converter.DateTimeToMySqlDate(date);

                    var sqlParams = new List<SqlParameter>();
                    sqlParams.Add(new SqlParameter("?DOC_DATE", mysqlDocumentDate));
                    sqlParams.Add(new SqlParameter("?COLLECTOR", collector));
                    sqlParams.Add(new SqlParameter("?COH", GlobalSettings.CodeOfCashOnHand));

                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendFormat(
                        "SELECT * FROM `OR` WHERE `DOC_DATE` = ?DOC_DATE AND `COLLECTOR` = ?COLLECTOR AND ACC_CODE <> ?COH");
                    return DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), sqlParams.ToArray());
                }
            }
        }

        #region 

        //#region Official Receipt

        //public static Result GenerateOfficialReceiptDetailsReportByDateCollector(DateTime voucherDate, int collectorId)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(1);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}',{3} ) ", reportInfo.StoredProcName,
        //                                              GlobalSettings.CashOnHandCode,
        //                                              Converter.DateTimeToMySqlDate(voucherDate), collectorId);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateOfficialReceiptSummaryReportByDateCollector(DateTime voucherDate, int collectorId)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(2);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}',{3} ) ", reportInfo.StoredProcName,
        //                                              GlobalSettings.CashOnHandCode,
        //                                              Converter.DateTimeToMySqlDate(voucherDate), collectorId);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateOfficialReceiptReportByDateCollector(int collectorId, DateTime dateStart,
        //                                                                  DateTime dateEnd, string accountCode = "")
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(3);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}','{3}','{4}' ) ", reportInfo.StoredProcName,
        //                                              collectorId, Converter.DateTimeToMySqlDate(dateStart),
        //                                              Converter.DateTimeToMySqlDate(dateEnd), accountCode);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath,
        //                                    ReportCreator.GetCriteriaDateRange(dateStart, dateEnd));
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //#endregion

        //#region Journal Voucher

        //public static Result GenerateJournalVoucherReportByDateRange(DateTime dateStart, DateTime dateEnd,
        //                                                             string accountCode = "")
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(7);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}','{3}' ) ", reportInfo.StoredProcName,
        //                                              Converter.DateTimeToMySqlDate(dateStart),
        //                                              Converter.DateTimeToMySqlDate(dateEnd), accountCode);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath,
        //                                    ReportCreator.GetCriteriaDateRange(dateStart, dateEnd));
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateJournalVoucherReportByVoucherNumber(int voucherNumber)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(8);
        //        string sqlCommandText = string.Format("CALL {0} ({1})", reportInfo.StoredProcName, voucherNumber);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateJournalVoucherSummaryReportByDateRange(DateTime dateStart, DateTime dateEnd,
        //                                                                    string accountCode = "")
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(9);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}','{3}')", reportInfo.StoredProcName,
        //                                              Converter.DateTimeToMySqlDate(dateStart),
        //                                              Converter.DateTimeToMySqlDate(dateEnd), accountCode);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath,
        //                                    ReportCreator.GetCriteriaDateRange(dateStart, dateEnd));
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //#endregion

        //#region Cash Voucher

        //public static Result GenerateCashVoucherReportByDateRange(DateTime dateStart, DateTime dateEnd,
        //                                                          string accountCode = "")
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(4);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}','{3}' ) ", reportInfo.StoredProcName,
        //                                              Converter.DateTimeToMySqlDate(dateStart),
        //                                              Converter.DateTimeToMySqlDate(dateEnd), accountCode);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath,
        //                                    ReportCreator.GetCriteriaDateRange(dateStart, dateEnd));
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateCashVoucherReportByVoucherNumber(int voucherNumber)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(5);
        //        string sqlCommandText = string.Format("CALL {0} ({1})", reportInfo.StoredProcName, voucherNumber);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        string amountInWords = Converter.AmountToWords(Convert.ToDecimal(details.Rows[0]["Amount"]));
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath, amountInWords);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.ExceptionLogger(new ReportController(), e);
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateCashVoucherSummaryReportByDateRange(DateTime dateStart, DateTime dateEnd,
        //                                                                 string accountCode = "")
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(6);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}','{3}')", reportInfo.StoredProcName,
        //                                              Converter.DateTimeToMySqlDate(dateStart),
        //                                              Converter.DateTimeToMySqlDate(dateEnd), accountCode);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath,
        //                                    ReportCreator.GetCriteriaDateRange(dateStart, dateEnd));
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateCashVoucherSummaryReport(DateTime dateStart, DateTime dateEnd,
        //                                                      string accountCode = "")
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(23);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}','{3}')", reportInfo.StoredProcName,
        //                                              Converter.DateTimeToMySqlDate(dateStart),
        //                                              Converter.DateTimeToMySqlDate(dateEnd), accountCode);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath,
        //                                    ReportCreator.GetCriteriaDateRange(dateStart, dateEnd));
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}


        //#endregion

        //#region Loan Report

        //public static Result GenerateLoanDetailsReport(DataTable loanDetails)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(25);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, loanDetails, reportInfo.ReportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateLoanAmortizationReport(DataTable loanDetailsOfAmortization,
        //                                                    DataTable loanPaymentDetails)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(26);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(loanPaymentDetails, header, loanDetailsOfAmortization,
        //                                    reportInfo.ReportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //#endregion

        //#region Schedule Report


        ///*  NOTE : Please set the Report Schedule in Chart of Accounts  */

        //public static Result GenerateScheduleOfInterestOnLoans(DateTime asOf, string orderBy)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(10);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcName,
        //                                              Converter.DateTimeToMySqlDate(asOf), orderBy);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath,
        //                                    "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateScheduleOfTimeDeposit(DateTime asOf, string orderBy)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(11);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcName,
        //                                              Converter.DateTimeToMySqlDate(asOf), orderBy);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath,
        //                                    "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateScheduleOfLoans(DateTime asOf, string orderBy)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(12);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcName,
        //                                              Converter.DateTimeToMySqlDate(asOf), orderBy);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath,
        //                                    "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateScheduleOfSavingsDeposit(DateTime asOf, string orderBy)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(13);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcName,
        //                                              Converter.DateTimeToMySqlDate(asOf), orderBy);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath,
        //                                    "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateScheduleOfShareCapital(DateTime asOf, string orderBy)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(14);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcName,
        //                                              Converter.DateTimeToMySqlDate(asOf), orderBy);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header,
        //                                    details, reportInfo.ReportTitle, reportPath,
        //                                    "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateScheduleOfTimeDepositConsolidated(DateTime asOf, string orderBy)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(16);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcName,
        //                                              Converter.DateTimeToMySqlDate(asOf), orderBy);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath,
        //                                    "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}


        //public static Result GenerateScheduleOfFines(DateTime asOf, string orderBy)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(17);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}'  ) ", reportInfo.StoredProcName,
        //                                              Converter.DateTimeToMySqlDate(asOf), orderBy);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath,
        //                                    "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}


        //public static Result GenerateScheduleReports(int reportScheduleId, string reportDescription, DateTime asOf,
        //                                             string orderBy)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(22);
        //        string sqlCommandText = string.Format("CALL {0} ({1}, '{2}','{3}'  ) ", reportInfo.StoredProcName,
        //                                              reportScheduleId, Converter.DateTimeToMySqlDate(asOf), orderBy);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportDescription, reportPath,
        //                                    "AsOf " + asOf.Date.ToString("MM/dd/yyyy"), "By " + orderBy);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //#endregion

        //#region Member

        //public static Result GenerateMemberReport(string memberCode)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(15);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}') ", reportInfo.StoredProcName, memberCode.Trim());
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateMemberListReport(DataTable result, string reportTitle)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(21);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, result, reportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //#endregion

        //#region Verifier 

        //public static Result GenerateSummaryStatementAccount(string memberCode, DateTime transactionDate)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(19);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}') ", reportInfo.StoredProcName,
        //                                              memberCode,
        //                                              Converter.DateTimeToMySqlDate(transactionDate));
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //public static Result GenerateDetailedStatementAccount(string memberCode, string accountCode,
        //                                                      DateTime transactionDate, Int32 timeDepositId,
        //                                                      string accountTitle)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(18);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}','{2}','{3}',{4}) ", reportInfo.StoredProcName,
        //                                              memberCode, accountCode,
        //                                              Converter.DateTimeToMySqlDate(transactionDate), timeDepositId);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath, accountTitle);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //#endregion

        //#region Voucher Attachment

        //public static Result GenerateVoucherAttachment(string voucherType, Int32 voucherNumber, string acccountCode = "")
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(20);
        //        string sqlCommandText = string.Format("CALL {0} ('{1}',{2},'{3}' ) ", reportInfo.StoredProcName,
        //                                              voucherType, voucherNumber, acccountCode);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //#endregion

        //#region Passbook 

        //public static Result GeneratePassbook(DataTable result)
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(24);
        //        DataTable header = Company.GetList("SCCO");
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, result, reportInfo.ReportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //#endregion

        //#region GeneralLedgerReport

        //public static Result GenerateStatementOfOperation()
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(29);
        //        DataTable header = Company.GetList("SCCO");
        //        string sqlCommandText = string.Format("CALL {0} () ", reportInfo.StoredProcName);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}


        //public static Result GenerateStatementFinancialCondition()
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(27);
        //        DataTable header = Company.GetList("SCCO");
        //        string sqlCommandText = string.Format("CALL {0} () ", reportInfo.StoredProcName);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}


        //public static Result GenerateStatementCashFlow()
        //{
        //    try
        //    {
        //        var reportInfo = new ReportInformation(28);
        //        DataTable header = Company.GetList("SCCO");
        //        string sqlCommandText = string.Format("CALL {0} () ", reportInfo.StoredProcName);
        //        DataTable details = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        string reportPath = Path.Combine(ReportFolder, reportInfo.ReportName);
        //        ReportCreator.DisplayReport(header, details, reportInfo.ReportTitle, reportPath);
        //        return new Result(true, "Successfully Generated.");
        //    }
        //    catch (Exception e)
        //    {
        //        return new Result(false, e.Message);
        //    }
        //}

        //#endregion

        #endregion


        internal static Result ShowBorrowerMakers(string code, DateTime asOf)
        {
            try
            {
                string sp = "sp_member";
                var param = new SqlParameter("tc_member_code", code);

                var dtMember = DatabaseController.ExecuteStoredProcedure(sp, param);
                dtMember.TableName = "member";
                sp = "sp_borrower_makers";
                var paramList = new List<SqlParameter>();
                paramList.Add(param);
                paramList.Add(new SqlParameter("td_as_of_date", asOf));
                var paramArray = paramList.ToArray();
                var dtComakers = DatabaseController.ExecuteStoredProcedure(sp, paramArray);
                dtComakers.TableName = "borrower_makers";

                var dtCompany = Company.GetData();
                dtCompany.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(dtMember);
                dataSet.Tables.Add(dtComakers);
                dataSet.Tables.Add(dtCompany);

                var ri = new ReportItem();
                ri.ReportFile = "borrower_makers.rpt";
                ri.Title = "Borrower Makers";
                ri.DataSource = dataSet;
                return ri.LoadReport();
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }
    }
}