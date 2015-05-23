using System.Data;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.Models.Reports
{
    internal class ReportModule
    {
        static public void ShowReport(ReportItem selectedReport)
        {
            var transactionDate = GlobalSettings.DateOfOpenTransaction;
            //switch (selectedReport.Title)
            {
            //    #region --- MIS Reports ---

            //    case "GetMemberListByCode":
            //        ReportController.GenerateMemberListReport(GetMemberListByCode(), selectedReport.Title);
            //        break;
            //    case "GetMemberListByName":
            //        ReportController.GenerateMemberListReport(GetMemberListByName(), selectedReport.Title);
            //        break;
            //    case "GetMemberListByGender":
            //        ReportController.GenerateMemberListReport(GetMemberListByGender(), selectedReport.Title);
            //        break;
            //    case "GetMemberListByCivilStatus":
            //        ReportController.GenerateMemberListReport(GetMemberListByCivilStatus(), selectedReport.Title);
            //        break;
            //    case "GetMemberListByIsMember":
            //        ReportController.GenerateMemberListReport(GetMemberListByIsMember(), selectedReport.Title);
            //        break;
            //    case "GetMemberListByMEM_TYPE":
            //        ReportController.GenerateMemberListReport(GetMemberListByMembershipType(), selectedReport.Title);
            //        break;
            //    case "GetDamayanMemberList":
            //        ReportController.GenerateMemberListReport(GetDamayanMemberList(), selectedReport.Title);
            //        break;
            //    case "GetMemberListByAge":
            //        ReportController.GenerateMemberListReport(GetMemberListByAge(), selectedReport.Title);
            //        break;

            //    #endregion --- MIS Reports ---

                //    #region 

                //case "GetScheduleReport01":
                //    ReportController.GenerateScheduleOfLoans(transactionDate, "Code");
                //    ReportController.GenerateScheduleOfLoans(transactionDate, "Name");
                //    break;

                //case "GetScheduleReport02":
                //    ReportController.GenerateScheduleOfTimeDeposit(transactionDate, "Code");
                //    ReportController.GenerateScheduleOfTimeDeposit(transactionDate, "Name");
                //    break;
                //case "GetScheduleReport03":
                //    ReportController.GenerateScheduleOfInterestOnLoans(transactionDate, "Code");
                //    ReportController.GenerateScheduleOfInterestOnLoans(transactionDate, "Name");
                //    break;
                //case "GetScheduleReport04":
                //    ReportController.GenerateScheduleOfFines(transactionDate, "Code");
                //    ReportController.GenerateScheduleOfFines(transactionDate, "Name");
                //    break;
                //case "GetScheduleReport05":
                //    ReportController.GenerateScheduleOfSavingsDeposit(transactionDate, "Code");
                //    ReportController.GenerateScheduleOfSavingsDeposit(transactionDate, "Name");
                //    break;
                //case "GetScheduleReport06":
                //    ReportController.GenerateScheduleOfTimeDepositConsolidated(transactionDate, "Code");
                //    ReportController.GenerateScheduleOfTimeDepositConsolidated(transactionDate, "Name");
                //    break;
                //case "GetScheduleReport07":
                //    ReportController.GenerateScheduleOfShareCapital(transactionDate, "Code");
                //    ReportController.GenerateScheduleOfShareCapital(transactionDate, "Name");
                //    break;
                //    //default:
                //    //    ReportController.GenerateScheduleReports(id, selectedReport.Title, transactionDate, "Code");
                //    //    ReportController.GenerateScheduleReports(id, selectedReport.Title, transactionDate, "Name");
                //    //    break;

                //    #endregion

            }
        }

        #region --- MIS REPORT QUERIES ---

        private const string MEMBERS_TABLE = "nfmb";

        private static readonly StringBuilder QueryBuilder = new StringBuilder();
        static DataTable GetMemberListByCode()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine("SELECT MEM_CODE, MEM_NAME, LEFT(MEM_CODE,1) as `Group`, MEM_TYPE as Description FROM");
            QueryBuilder.AppendLine(MEMBERS_TABLE);
            QueryBuilder.AppendLine("ORDER BY MEM_CODE");
            return Database.DatabaseController.ExecuteSelectQuery(QueryBuilder.ToString());
        }

        static DataTable GetMemberListByName()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine("SELECT MEM_CODE, MEM_NAME, LEFT(MEM_NAME,1) as `Group`, MEM_TYPE as Description FROM");
            QueryBuilder.AppendLine(MEMBERS_TABLE);
            QueryBuilder.AppendLine("ORDER BY MEM_NAME");
            return Database.DatabaseController.ExecuteSelectQuery(QueryBuilder.ToString());
        }

        static DataTable GetMemberListByGender()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine("SELECT MEM_CODE, MEM_NAME, SEX as `Group`, MEM_TYPE as Description FROM");
            QueryBuilder.AppendLine(MEMBERS_TABLE);
            QueryBuilder.AppendLine("ORDER BY MEM_NAME");
            return Database.DatabaseController.ExecuteSelectQuery(QueryBuilder.ToString());
        }

        static DataTable GetMemberListByCivilStatus()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine("SELECT MEM_CODE, MEM_NAME, CIVIL as `Group`, MEM_TYPE as Description FROM");
            QueryBuilder.AppendLine(MEMBERS_TABLE);
            QueryBuilder.AppendLine("ORDER BY MEM_NAME");
            return Database.DatabaseController.ExecuteSelectQuery(QueryBuilder.ToString());
        }

        static DataTable GetMemberListByIsMember()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine("SELECT MEM_CODE, MEM_NAME, CASE MEMBER WHEN 0 THEN 'Member' WHEN 1 THEN 'Non-Member' END as `Group`, MEM_TYPE as Description FROM");
            QueryBuilder.AppendLine(MEMBERS_TABLE);
            QueryBuilder.AppendLine("ORDER BY MEM_NAME");
            return Database.DatabaseController.ExecuteSelectQuery(QueryBuilder.ToString());
        }

        static DataTable GetMemberListByMembershipType()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine("SELECT MEM_CODE, MEM_NAME, MEM_TYPE as `Group`, MEM_TYPE as Description FROM");
            QueryBuilder.AppendLine(MEMBERS_TABLE);
            QueryBuilder.AppendLine("ORDER BY MEM_NAME");
            return Database.DatabaseController.ExecuteSelectQuery(QueryBuilder.ToString());
        }

        static DataTable GetDamayanMemberList()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine("SELECT MEM_CODE, MEM_NAME, CAST(Year(D_DATE) AS CHAR) as `Group`, CAST(D_DATE AS CHAR) as Description FROM");
            QueryBuilder.AppendLine(MEMBERS_TABLE);
            QueryBuilder.AppendLine("WHEwwwwRE DAMAYAN = 1");
            QueryBuilder.AppendLine("ORDER BY MEM_NAME");
            return Database.DatabaseController.ExecuteSelectQuery(QueryBuilder.ToString());
        }

        static DataTable GetMemberListByAge()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine("SELECT a.MEM_CODE, a.MEM_NAME, CONCAT(b.AgeGroup,' - ',b.AgeGroup + 9)  AS `Group`, a.Age AS Description FROM");
            QueryBuilder.AppendLine(MEMBERS_TABLE + " AS a");
            QueryBuilder.AppendLine("INNER JOIN (SELECT MEM_CODE, FLOOR(Age/10) * 10 AS AgeGroup FROM " + MEMBERS_TABLE + ") AS b");
            QueryBuilder.AppendLine("ON a.MEM_CODE = b.MEM_CODE");
            QueryBuilder.AppendLine("ORDER BY MEM_NAME");
            return Database.DatabaseController.ExecuteSelectQuery(QueryBuilder.ToString());
        }

        #endregion --- MIS REPORT QUERIES ---
    }
}
