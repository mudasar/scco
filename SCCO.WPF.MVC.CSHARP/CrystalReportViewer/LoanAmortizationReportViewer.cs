using System;
using SAPBusinessObjects.WPF.Viewer;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.CrystalReportViewer
{
    public class LoanAmortizationReportViewer : AReport, ILoanAmortization
    {
        public override Result ShowReport()
        {
            try
            {
                var crystalReportViewer = new CrystalReportsViewer();
                var myReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                myReport.Load(ReportPath);
                var crSections = myReport.ReportDefinition.Sections;

                if (myReport.Subreports.Count > 0)
                {
                    foreach (CrystalDecisions.CrystalReports.Engine.Section crSection in crSections)
                    {
                        //The subreports
                        var crReportObjects = crSection.ReportObjects;
                        foreach (CrystalDecisions.CrystalReports.Engine.ReportObject crReportObject in crReportObjects)
                        {
                            if (crReportObject.Kind == CrystalDecisions.Shared.ReportObjectKind.SubreportObject)
                            {
                                var crSubreportObject = (CrystalDecisions.CrystalReports.Engine.SubreportObject)crReportObject;
                                var crSubreportDocument = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);
                                var crTables = crSubreportDocument.Database.Tables;

                                foreach (CrystalDecisions.CrystalReports.Engine.Table aTable in crTables)
                                {
                                    //The report header subreport.
                                    if (aTable.Name == "Report_Header")
                                        crSubreportDocument.SetDataSource(Header);
                                    else if (aTable.Name == "LoanPaymentDetails")
                                        crSubreportDocument.SetDataSource(LoanPaymentDetails);
                                }
                            }
                        }
                    }
                }


                foreach (
                    CrystalDecisions.CrystalReports.Engine.FormulaFieldDefinition ff in
                        myReport.DataDefinition.FormulaFields)
                {
                    //The formula fields
                    if (ff.Kind == CrystalDecisions.Shared.FieldKind.FormulaField)
                    {
                        if (ff.Name.Contains("Report_Title"))
                        {
                            ff.Text = "'" + ReportTitle + "'";
                        }
                        else if (ff.Name.Contains("cCriteria1"))
                        {
                            ff.Text = "'" + Criteria1 + "'";
                        }
                        else if (ff.Name.Contains("cCriteria2"))
                        {
                            ff.Text = "'" + Criteria2 + "'";
                        }
                        else if (ff.Name.Contains("cCriteria3"))
                        {
                            ff.Text = "'" + Criteria3 + "'";
                        }
                    }
                }

                myReport.SetDataSource(Details);
                crystalReportViewer.ViewerCore.ReportSource = myReport;
                return ReportWindowForm.AddControl(crystalReportViewer);
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        public System.Data.DataTable LoanPaymentDetails
        { get; set; }
        }
    }
