using System;
using SAPBusinessObjects.WPF.Viewer;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.CrystalReportViewer
{
    public class ReportWindowForm
    {
        public static Result AddControl(CrystalReportsViewer crystalReportViewer)
        {
            try
            {
                var mainWindow = new MainReportWindow();
                mainWindow.AddControl(crystalReportViewer);
                mainWindow.Show();
                return new Result(true, "Success");
            }
            catch (Exception e)
            {
                return  new Result(false,e.Message);
            }
        }
    }
}
