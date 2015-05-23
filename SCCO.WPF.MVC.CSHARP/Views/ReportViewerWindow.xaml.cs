using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Reporting.WinForms;

namespace SCCO.WPF.MVC.CS.Views
{
    /// <summary>
    /// Interaction logic for ReportViewerWindow.xaml
    /// </summary>
    public partial class ReportViewerWindow : Window
    {
        public ReportViewerWindow()
        {
            InitializeComponent();
            _reportViewer.Load += ReportViewerOnLoad;
        }

        public ReportViewerWindow(DataTable dataTable):this()
        {
            _dataTable = dataTable;
        }

        private bool _isReportViewerLoaded;
        private DataTable _dataTable;

        public void ReportViewerOnLoad (object sender, EventArgs e)
        {
            if(!_isReportViewerLoaded)
            {
                var reportDataSource = new ReportDataSource();
                var dataset = new DataTable();

                dataset.BeginInit();
                reportDataSource.Value = _dataTable;
                _reportViewer.LocalReport.DataSources.Add(reportDataSource);

                _reportViewer.LocalReport.ReportPath = System.IO.Path.Combine(Properties.Settings.Default.ReportFolderPath, "MemberList.rpt");
                _reportViewer.RefreshReport();
                _isReportViewerLoaded = true;

            }
        }
    }
}
