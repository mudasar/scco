
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.CrystalReportViewer
{
    public abstract class AReport : IReport
    {
        public string ServerName { get; set; }
        public string ReportPath { get; set; }
        public string ReportTitle { get; set; }
        public string Criteria1 { get; set; }
        public string Criteria2 { get; set; }
        public string Criteria3 { get; set; }
        public System.Data.DataTable Details { get; set; }
        public System.Data.DataTable Header { get; set; }
        public abstract Result ShowReport();
    }
}