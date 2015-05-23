using System;
using System.Data;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Controllers
{
    public class ReportCreator
    {
        private static readonly string ReportFolder = ReportController.GetReportFolderPath();

        public static Result DisplayReport(DataTable reportDetail, ReportItem reportInformation)
        {
            try
            {
                string reportPath = System.IO.Path.Combine(ReportFolder, reportInformation.ReportFile);
                var reportViewerWindow = new CrystalReportViewer.ReportViewer
                {
                    Criteria1 = "",
                    Criteria2 = "",
                    Criteria3 = "",
                    Header = null,
                    Details = reportDetail,
                    ReportTitle = reportInformation.Title,
                    ReportPath = reportPath,
                    ServerName = ""
                };

                reportViewerWindow.ShowReport();
                return new Result(true, "Successfully Generated", new ReportCreator(), "GenerateReport");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message, new ReportCreator(), "GenerateReport");
            }
        }

        public static Result DisplayReport(DataTable reportHeader, DataTable reportDetail, string reportTitle, string reportPath, string optionalCriteria1 = "", string optionalCriteria2 = "", string optionalCriteria3 = "", string optionalServerName = "")
        {
            try
            {
                var reportViewerWindow = new CrystalReportViewer.ReportViewer
                                             {
                                                 Criteria1 = optionalCriteria1,
                                                 Criteria2 = optionalCriteria2,
                                                 Criteria3 = optionalCriteria3,
                                                 Header = reportHeader,
                                                 Details = reportDetail,
                                                 ReportTitle = reportTitle,
                                                 ReportPath = reportPath,
                                                 ServerName = optionalServerName
                                             };

                reportViewerWindow.ShowReport();
                return new Result(true, "Successfully Generated", new ReportCreator(), "GenerateReport");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message, new ReportCreator(), "GenerateReport");
            }
        }

        public static Result DisplayReport(DataTable loanPaymentDetails, DataTable reportHeader, DataTable reportDetail, string reportTitle, string reportPath, string optionalCriteria1 = "", string optionalCriteria2 = "", string optionalCriteria3 = "", string optionalServerName = "")
        {
            try
            {
                var reportViewerWindow = new CrystalReportViewer.LoanAmortizationReportViewer()
                {
                    Criteria1 = optionalCriteria1,
                    Criteria2 = optionalCriteria2,
                    Criteria3 = optionalCriteria3,
                    Header = reportHeader,
                    Details = reportDetail,
                    ReportTitle = reportTitle,
                    ReportPath = reportPath,
                    ServerName = optionalServerName,
                    LoanPaymentDetails = loanPaymentDetails
                };
                reportViewerWindow.ShowReport();
                return new Result(true, "Successfully Generated", new ReportCreator(), "GenerateReport");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message, new ReportCreator(), "GenerateReport");
            }
        }

        public static string GetCriteriaDateRange(DateTime dateStart, DateTime dateEnd)
        {
            string returnValue = "";
            try
            {
                if (dateStart < dateEnd)
                    returnValue = string.Format("AsOf {0} - {1} ", dateStart.ToString("MM/dd/yyyy"), dateEnd.ToString("MM/dd/yyyy"));
                else if (dateStart == dateEnd)
                    returnValue = string.Format("AsOf {0} ", dateStart.ToString("MM/dd/yyyy"));

                return returnValue;
            }
            catch (Exception )
            {
                return returnValue;
            }
        }

    }
}
