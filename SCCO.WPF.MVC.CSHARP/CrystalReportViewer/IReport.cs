using System.Data;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.CrystalReportViewer
{
    public interface IReport
    {
        string ServerName { get; set; }
        string ReportPath { get; set; }
        string ReportTitle { get; set; }
        string Criteria1 { get; set; }
        string Criteria2 { get; set; }
        string Criteria3 { get; set; }
        DataTable Details { get; set; }
        DataTable Header { get; set; }
        Result ShowReport();
    }
}