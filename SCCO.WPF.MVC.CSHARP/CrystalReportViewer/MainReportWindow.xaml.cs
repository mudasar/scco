using System.Windows;
using SAPBusinessObjects.WPF.Viewer;

namespace SCCO.WPF.MVC.CS.CrystalReportViewer
{
    public partial class MainReportWindow
    {
        public MainReportWindow()
        {
            InitializeComponent();
        }

        public void AddControl(CrystalReportsViewer crystalReports)
        {
            CrystalReportsViewer crystalReportViewer = crystalReports;
            crystalReportViewer.Width = GridReport.Width;
            crystalReportViewer.Height = GridReport.Height;
            GridReport.Children.Add(crystalReportViewer);
            crystalReportViewer.Owner = this;
        }
    }
}
