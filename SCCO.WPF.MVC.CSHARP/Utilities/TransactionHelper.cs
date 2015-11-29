using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Utilities
{
    public static class TransactionHelper
    {
        public static bool IsPostingAllowed()
        {
            return MainController.LoggedUser.TransactionDate == GlobalSettings.DateOfOpenTransaction;
        }


        public static bool IsVoucherNumberUsed(VoucherTypes voucherType, int documentNo)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT * FROM `{0}` WHERE DOC_NUM = ?DOC_NUM", voucherType.ToString().ToLower());
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), new SqlParameter("?DOC_NUM", documentNo));
            return dataTable.Rows.Count > 0;
        }
    }
}
