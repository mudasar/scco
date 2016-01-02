using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    internal class ReportSchedule
    {
        private const string TableName = "ReportSchedules";

        public ReportSchedule(int id)
        {
            ReportScheduleId = id;
            Read();
        }

        public ReportSchedule()
        {
        }

        #region --- PROPERTIES ---

        public string Description { get; set; }

        public int ReportScheduleId { get; set; }

        #endregion

        #region --- CRUD ---

        public Result Create()
        {
            try
            {
                string sqlCommandText = string.Format("INSERT INTO {0} (Description) VALUES (?Description)", TableName);
                ReportScheduleId = DatabaseController.ExecuteInsertQuery(sqlCommandText,
                                                               new SqlParameter("?Description", Description));
                return new Result(true, "Create successful");
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(this, exception);
                return new Result(false, exception.Message);
            }
        }

        public Result Delete()
        {
            try
            {
                string sqlCommandText = string.Format("DELETE FROM {0} WHERE ReportScheduleId = ?ReportScheduleId",
                                                      TableName);
                Database.DatabaseController.ExecuteNonQuery(sqlCommandText, new SqlParameter("?ReportScheduleId", ReportScheduleId));
                return new Result(true, "Delete successful");
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(this, exception);
                return new Result(false, exception.Message);
            }
        }

        public Result Read()
        {
            try
            {
                string sqlCommandText = string.Format("SELECT * FROM {0} WHERE ReportScheduleId = ?ReportScheduleId",
                                                      TableName);
                DataTable dataTable = Database.DatabaseController.ExecuteSelectQuery(sqlCommandText,
                                                                  new SqlParameter("?ReportScheduleId", ReportScheduleId));
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Description = (string)row["Description"];
                    }
                }
                return new Result(true, "Read successful");
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(this, exception);
                return new Result(false, exception.Message);
            }
        }

        public Result Update()
        {
            try
            {
                string sqlCommandText =
                    string.Format(
                        "UPDATE {0} SET Description = ?Description WHERE ReportScheduleId = ?ReportScheduleId",
                        TableName);
                var sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("?ReportScheduleId", ReportScheduleId),
                        new SqlParameter("?Description", Description),
                    };
                DatabaseController.ExecuteNonQuery(sqlCommandText, sqlParameters.ToArray());
                return new Result(true, "Upudate successful");
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(this, exception);
                return new Result(false, exception.Message);
            }
        }

        #endregion

        #region --- STATIC METHODS ---

        public static ReportSchedule GetScheduleDescription(Int32 reportScheduleId)
        {
            var reportSchedule = new ReportSchedule(reportScheduleId);
            reportSchedule.Read();
            return reportSchedule;
        }

        public static List<ReportSchedule> GetList()
        {
            string sqlCommandText = string.Format("SELECT * FROM {0}", TableName);
            DataTable dataTable = Database.DatabaseController.ExecuteSelectQuery(sqlCommandText);
            return (from DataRow row in dataTable.Rows
                    select new ReportSchedule
                        {
                            ReportScheduleId = (int) row["ReportScheduleId"],
                            Description = (string) row["Description"],
                        }).ToList();
        }

        #endregion
    }
}
