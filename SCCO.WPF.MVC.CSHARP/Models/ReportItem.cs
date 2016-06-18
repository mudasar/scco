using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Windows.Media.Imaging;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SAPBusinessObjects.WPF.Viewer;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.CrystalReportViewer;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class ReportItem : INotifyPropertyChanged, IModel
    {
        private const string TableName = "report_items";
        private BitmapImage _bitmapImage;
        private string _category;
        private string _description;
        private int _id;
        private byte[] _image;

        private string _reportFile;
        private string _storedProcedure;
        private string _title;
        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapImage BitmapImage
        {
            get { return _bitmapImage; }
            set
            {
                _bitmapImage = value;
                OnPropertyChanged("BitmapImage");
            }
        }

        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged("Category");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        public byte[] Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged("Image");

                if (Image == null)
                {
                    BitmapImage = null;
                    return;
                }
                try
                {
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = new MemoryStream(Image);
                    imageSource.EndInit();
                    BitmapImage = imageSource;
                }
                catch (Exception exception)
                {
                    Logger.ExceptionLogger(new ImageTool(), exception);
                }
            }
        }

        public string ReportFile
        {
            get { return _reportFile; }
            set
            {
                _reportFile = value;
                OnPropertyChanged("ReportFile");
            }
        }

        public string StoredProcedure
        {
            get { return _storedProcedure; }
            set
            {
                _storedProcedure = value;
                OnPropertyChanged("StoredProcedure");
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        // custom attribute to hold report explanation
        public string Explanation { get; set; }

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?Title", Title);
                ModelController.AddParameter(sqlParameters, "?Description", Description);
                ModelController.AddParameter(sqlParameters, "?Image", Image);
                ModelController.AddParameter(sqlParameters, "?Category", Category);
                ModelController.AddParameter(sqlParameters, "?ReportFile", ReportFile);
                ModelController.AddParameter(sqlParameters, "?StoredProcedure", StoredProcedure);
                return sqlParameters;
            }
        }

        private SqlParameter ParamKey
        {
            get { return new SqlParameter("?ID", ID); }
        }

        #region Implementation of IModel

        public Result Create()
        {
            Action createRecord = () =>
                {
                    var sql = DatabaseController.GenerateInsertStatement(TableName, Parameters);
                    ID = DatabaseController.ExecuteInsertQuery(sql, Parameters.ToArray());
                };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
                {
                    var key = ParamKey;

                    string sql = DatabaseController.GenerateDeleteStatement(TableName, key);

                    DatabaseController.ExecuteNonQuery(sql, key);
                };

            return ActionController.InvokeAction(deleteRecord);
        }

        public Result Find(int id)
        {
            Action findRecord = () =>
                {
                    ResetProperties();
                    ID = id;

                    var key = ParamKey;
                    string sql = DatabaseController.GenerateSelectStatement(TableName, key);

                    DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        SetPropertiesFromDataRow(dataRow);
                    }
                };

            return ActionController.InvokeAction(findRecord);
        }

        public void ResetProperties()
        {
            ID = 0;
            Title = "";
            Description = "";
            Image = null;
            Category = "";
            ReportFile = "";
            StoredProcedure = "";
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            Title = DataConverter.ToString(dataRow["Title"]);
            Description = DataConverter.ToString(dataRow["Description"]);
            Image = DataConverter.ToByteArray(dataRow["Image"]);
            Category = DataConverter.ToString(dataRow["Category"]);
            ReportFile = DataConverter.ToString(dataRow["ReportFile"]);
            StoredProcedure = DataConverter.ToString(dataRow["StoredProcedure"]);
        }

        public Result Update()
        {
            Action updateRecord = () =>
                {
                    var key = ParamKey;

                    List<SqlParameter> sqlParameters = Parameters;
                    string sql = DatabaseController.GenerateUpdateStatement(TableName,
                                                                            sqlParameters, key);

                    sqlParameters.Add(key);
                    DatabaseController.ExecuteNonQuery(sql, sqlParameters.ToArray());
                };

            return ActionController.InvokeAction(updateRecord);
        }

        #endregion

        public static ReportItem WhereTitleIs(string title)
        {
            var sqlBuilder = new System.Text.StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM");
            sqlBuilder.AppendLine(TableName);
            sqlBuilder.AppendLine("WHERE Title = ?Title");

            var param = new SqlParameter("?Title", title);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, param);

            if (dataTable.Rows.Count == 0) return null;

            var item = new ReportItem();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                item = new ReportItem();
                item.SetPropertiesFromDataRow(dataRow);
            }
            return item;
        }

        internal static ReportItemCollection CollectAll()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT * FROM " + TableName);
            var collection = new ReportItemCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new ReportItem();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }

        internal static ReportItemCollection GetListByCategory(string category)
        {
            var sqlBuilder = new System.Text.StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM");
            sqlBuilder.AppendLine(TableName);
            sqlBuilder.AppendLine("WHERE Category = ?Category");

            var param = new SqlParameter("?Category", category);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, param);

            var collection = new ReportItemCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new ReportItem();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #region --- Report Generating Procedure ---

        public DataSet DataSource { get; set; }

        public bool HasStoredProcedureParameter { get { return StoredProcedureParameters != null && StoredProcedureParameters.Length > 0; } }

        public static Result Load(DataTable dataTable, string reportFileName)
        {
            // is report file specified?
            if (string.IsNullOrEmpty(reportFileName))
                return new Result(false, "No report file specified in the Report Item");

            // is report file exist?
            var fullPathName = Path.Combine(ReportController.ReportFolder, reportFileName);
            if (!File.Exists(fullPathName))
            {
                return new Result(false, "Report File not found.");
            }
            try
            {
                var reportDocument = new ReportDocument();
                reportDocument.Load(fullPathName);
                reportDocument.SetDataSource(dataTable);

                /****************************************
                * Load the CrystalReport using WPF Form
                ****************************************/
                var crystalReportsViewer = new CrystalReportsViewer();
                crystalReportsViewer.ViewerCore.ReportSource = reportDocument;

                var reportWindow = new MainReportWindow();
                reportWindow.AddControl(crystalReportsViewer);
                reportWindow.ShowDialog();

                return new Result(true, "Report loaded successful.");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }

        public Result LoadReport()
        {
            // is report file specified?
            if (string.IsNullOrEmpty(ReportFile))
                return new Result(false,"No report file specified in the Report Item");

            // is report file exist?
            var fullPathName = Path.Combine(ReportController.ReportFolder, ReportFile);
            if (!File.Exists(fullPathName))
                return new Result(false, "Report File not found.");

            try
            {
                // query the database / run stored procedure
                if (!string.IsNullOrEmpty(StoredProcedure))
                {
                    DataTable comp = Company.GetData();
                    comp.TableName = "comp";

                    DataTable detail = HasStoredProcedureParameter
                                           ? DatabaseController.ExecuteStoredProcedure(StoredProcedure, StoredProcedureParameters)
                                           : DatabaseController.ExecuteStoredProcedure(StoredProcedure);

                    detail.TableName = Path.GetFileNameWithoutExtension(ReportFile);

                    // initialize report data
                    var dataSet = new DataSet();
                    dataSet.Tables.Add(comp);
                    dataSet.Tables.Add(detail);
                    
                    DataSource = dataSet;
                }

                /*******************************************************
                * define report document and set report file and data  
                * *****************************************************/
                var reportDocument = new ReportDocument();
                reportDocument.Load(fullPathName);

                /*************************************************************************
                * Must loop through each DataTable to update the ReportDocument.Database
                *************************************************************************/
                if (DataSource != null)
                {
                    foreach (DataTable table in DataSource.Tables)
                    {
                        reportDocument.Database.Tables[table.TableName].SetDataSource(DataSource.Tables[table.TableName]);
                    }
                }
                /******************************************************************
                * Use ReportModel.ReportTitle as the report title of the rpt file
                ******************************************************************/
                foreach (FormulaFieldDefinition ff in reportDocument.DataDefinition.FormulaFields)
                {
                    if (ff.Kind == FieldKind.FormulaField)
                    {
                        if (ff.Name.Contains("ReportTitle"))
                        {
                            ff.Text = "'" + Title + "'";
                            continue;
                        }
                        if (ff.Name.Contains("Explanation"))
                        {
                            ff.Text = "'" + Explanation + "'";
                        }
                    }
                }

                /****************************************
                * Load the CrystalReport using WPF Form
                ****************************************/
                var crystalReportsViewer = new CrystalReportsViewer();
                crystalReportsViewer.ViewerCore.ReportSource = reportDocument;

                var reportWindow = new MainReportWindow();
                reportWindow.AddControl(crystalReportsViewer);
                reportWindow.ShowDialog();

                return new Result(true, "Report loaded successful.");
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(GetType(), exception);
                return new Result(false, exception.Message);
            }
        }

        public Result LoadReport(DataTable data, string reportFile)
        {
            ReportFile = reportFile;
            // is report file specified?
            if (string.IsNullOrEmpty(ReportFile))
                return new Result(false, "No report file specified in the Report Item");

            // is report file exist?
            var fullPathName = Path.Combine(ReportController.ReportFolder, ReportFile);
            if (!File.Exists(fullPathName))
                return new Result(false, string.Format("Report File '{0}' not found.", reportFile));

            try
            {
                // query the database / run stored procedure
                if (!string.IsNullOrEmpty(reportFile))
                {
                    DataTable comp = Company.GetData();
                    comp.TableName = "comp";

                    var detail = data;
                    detail.TableName = Path.GetFileNameWithoutExtension(ReportFile);

                    // initialize report data
                    var dataSet = new DataSet();
                    dataSet.Tables.Add(comp);
                    dataSet.Tables.Add(detail);

                    DataSource = dataSet;
                }

                /*******************************************************
                * define report document and set report file and data  
                * *****************************************************/
                var reportDocument = new ReportDocument();
                reportDocument.Load(fullPathName);

                /*************************************************************************
                * Must loop through each DataTable to update the ReportDocument.Database
                *************************************************************************/
                foreach (DataTable table in DataSource.Tables)
                {
                    reportDocument.Database.Tables[table.TableName].SetDataSource(DataSource.Tables[table.TableName]);
                }

                /******************************************************************
                * Use ReportModel.ReportTitle as the report title of the rpt file
                ******************************************************************/
                foreach (FormulaFieldDefinition ff in reportDocument.DataDefinition.FormulaFields)
                {
                    if (ff.Kind == FieldKind.FormulaField)
                    {
                        if (ff.Name.Contains("ReportTitle"))
                        {
                            ff.Text = "'" + Title + "'";
                            break;
                        }
                    }
                }

                /****************************************
                * Load the CrystalReport using WPF Form
                ****************************************/
                var crystalReportsViewer = new CrystalReportsViewer();
                crystalReportsViewer.ViewerCore.ReportSource = reportDocument;

                var reportWindow = new MainReportWindow();
                reportWindow.AddControl(crystalReportsViewer);
                reportWindow.ShowDialog();

                return new Result(true, "Report loaded successful.");
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(GetType(), exception);
                return new Result(false, exception.Message);
            }
        }

        public Result LoadReport(DataTable[] dataTables, string reportFile)
        {
            ReportFile = reportFile;
            // is report file specified?
            if (string.IsNullOrEmpty(ReportFile))
                return new Result(false, "No report file specified in the Report Item");

            // is report file exist?
            var fullPathName = Path.Combine(ReportController.ReportFolder, ReportFile);
            if (!File.Exists(fullPathName))
                return new Result(false, string.Format("Report File '{0}' not found.", reportFile));

            try
            {
                // query the database / run stored procedure
                if (!string.IsNullOrEmpty(reportFile))
                {
                    DataTable comp = Company.GetData();

                    // initialize report data
                    DataSource = new DataSet();
                    DataSource.Tables.Add(comp);
                    DataSource.Tables[0].TableName = "comp";

                    foreach (DataTable dataTable in dataTables)
                    {
                        DataSource.Tables.Add(dataTable);    
                    }                   
                }

                /*******************************************************
                * define report document and set report file and data  
                * *****************************************************/
                var reportDocument = new ReportDocument();
                reportDocument.Load(fullPathName);

                /*************************************************************************
                * Must loop through each DataTable to update the ReportDocument.Database
                *************************************************************************/
                foreach (DataTable table in DataSource.Tables)
                {
                    reportDocument.Database.Tables[table.TableName].SetDataSource(DataSource.Tables[table.TableName]);
                }

                /******************************************************************
                * Use ReportModel.ReportTitle as the report title of the rpt file
                ******************************************************************/
                foreach (FormulaFieldDefinition ff in reportDocument.DataDefinition.FormulaFields)
                {
                    if (ff.Kind == FieldKind.FormulaField)
                    {
                        if (ff.Name.Contains("ReportTitle"))
                        {
                            ff.Text = "'" + Title + "'";
                            break;
                        }
                    }
                }

                /****************************************
                * Load the CrystalReport using WPF Form
                ****************************************/
                var crystalReportsViewer = new CrystalReportsViewer();
                crystalReportsViewer.ViewerCore.ReportSource = reportDocument;

                var reportWindow = new MainReportWindow();
                reportWindow.AddControl(crystalReportsViewer);
                reportWindow.ShowDialog();

                return new Result(true, "Report loaded successful.");
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(GetType(), exception);
                return new Result(false, exception.Message);
            }
        }
        #endregion

        public SqlParameter[] StoredProcedureParameters { get; set; }

        public Result LoadReport(DateTime asOf, DataTable[] dataTables, string reportFile)
        {
            ReportFile = reportFile;
            // is report file specified?
            if (string.IsNullOrEmpty(ReportFile))
                return new Result(false, "No report file specified in the Report Item");

            // is report file exist?
            var fullPathName = Path.Combine(ReportController.ReportFolder, ReportFile);
            if (!File.Exists(fullPathName))
                return new Result(false, string.Format("Report File '{0}' not found.", reportFile));

            try
            {
                // query the database / run stored procedure
                if (!string.IsNullOrEmpty(reportFile))
                {
                    DataTable comp = Company.GetData();

                    // initialize report data
                    DataSource = new DataSet();
                    DataSource.Tables.Add(comp);
                    DataSource.Tables[0].TableName = "comp";

                    foreach (DataTable dataTable in dataTables)
                    {
                        DataSource.Tables.Add(dataTable);
                    }
                }

                /*******************************************************
                * define report document and set report file and data  
                * *****************************************************/
                var reportDocument = new ReportDocument();
                reportDocument.Load(fullPathName);

                /*************************************************************************
                * Must loop through each DataTable to update the ReportDocument.Database
                *************************************************************************/
                foreach (DataTable table in DataSource.Tables)
                {
                    reportDocument.Database.Tables[table.TableName].SetDataSource(DataSource.Tables[table.TableName]);
                }

                /******************************************************************
                * Use ReportModel.ReportTitle as the report title of the rpt file
                ******************************************************************/
                foreach (FormulaFieldDefinition ff in reportDocument.DataDefinition.FormulaFields)
                {
                    if (ff.Kind == FieldKind.FormulaField)
                    {
                        if (ff.Name.Contains("report_title"))
                        {
                            ff.Text = "'" + Title + "'";
                            continue;
                        }
                        if (ff.Name.Contains("as_of"))
                        {
                            ff.Text = "'" + string.Format("As of {0:MMMM dd, yyyy}",asOf) + "'";
                        }

                    }
                }

                /****************************************
                * Load the CrystalReport using WPF Form
                ****************************************/
                var crystalReportsViewer = new CrystalReportsViewer();
                crystalReportsViewer.ViewerCore.ReportSource = reportDocument;

                var reportWindow = new MainReportWindow();
                reportWindow.AddControl(crystalReportsViewer);
                reportWindow.ShowDialog();

                return new Result(true, "Report loaded successful.");
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(GetType(), exception);
                return new Result(false, exception.Message);
            }
        }
    }

    public class ReportItemCollection : System.Collections.ObjectModel.ObservableCollection<ReportItem>
    {
    }
}