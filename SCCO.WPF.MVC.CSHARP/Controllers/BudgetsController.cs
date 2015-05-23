using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Controllers
{
    public static class BudgetsController
    {
        private const string TABLE_NAME = "budgets";

        public static BudgetCollection GetObservableCollection()
        {
            var collection = new BudgetCollection();
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM");
            sqlBuilder.AppendLine(TABLE_NAME);
            var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                var budget = new Budget();
                budget.SetPropertiesFromDataRow(dataRow);
                collection.Add(budget);
            }
            return collection;
        }

        public static Budget Find(int id)
        {
            var item = new Budget();
            var dataTable = DatabaseController.FindRecord(TABLE_NAME, id);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                item.SetPropertiesFromDataRow(dataRow);
                break;
            }
            return item;
        }

        public static Result Delete(int id)
        {
            try
            {
                DatabaseController.DeleteRecord(TABLE_NAME, id);
                return new Result(true, "Budget deleted successfully.");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }

        public static Result Save(Budget model)
        {
            if(model.ID == 0)
            {
                try
                {
                    var parameters = ExtractParameters(model);
                    model.ID = DatabaseController.CreateRecord(TABLE_NAME, parameters);
                    return new Result(true, "Budget created successfully.");
                }
                catch (Exception exception)
                {
                    return new Result(false, exception.Message);
                }
            }
            try
            {
                var parameters = ExtractParameters(model);
                var paramKey = new SqlParameter("?id", model.ID);
                DatabaseController.UpdateRecord(TABLE_NAME, paramKey, parameters);
                return new Result(true, "Budget updated successfully.");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }

        private static List<SqlParameter> ExtractParameters(Budget model)
        {
            // id not included
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("?account_code", model.AccountCode));
            parameters.Add(new SqlParameter("?account_title", model.AccountTitle));
            parameters.Add(new SqlParameter("?amount", model.Amount));
            parameters.Add(new SqlParameter("?year", model.Year));
            return parameters;
        }

        public static void CreateBudgetReport(DateTime asOf)
        {
            //var dtBudgetReport = new DataTable("budgets");
            //dtBudgetReport.Columns.Add();

            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("SELECT account_code as group_code, amount, year ");
            queryBuilder.AppendFormat("FROM budgets WHERE year = {0}", asOf.Year);
            var dtBudgetsAllocated = DatabaseController.ExecuteSelectQuery(queryBuilder);

            var priorMonth = (new DateTime(asOf.Year, asOf.Month, 1)).AddDays(-1);

            const string spGeneralLedger = "sp_general_ledger";
            var paramPriorMonth = new SqlParameter("?td_as_of", priorMonth);

            var dtPriorMonth = DatabaseController.ExecuteStoredProcedure(spGeneralLedger, paramPriorMonth);

            var dtJoined = DataTableHelper.JoinTwoDataTablesOnOneColumn(
                    dtBudgetsAllocated, 
                    dtPriorMonth,
                    "group_code",
                    DataTableHelper.JoinType.Left);
            ;
        }
    }
}