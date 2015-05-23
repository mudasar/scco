using System;
using System.Linq;
using System.Text;
using System.Data;
using SCCO.WPF.MVC.CS.Database;


namespace SCCO.WPF.MVC.CS.Models.ShareCapital
{
    public class ShareCapital
    {
        public string Filename { get; set; }
        private string _memberCode = "";
        private decimal _shareCapitalAmount = 0;

        public  Controllers.Result UpdateShareCapital()
        {
            try
            {
                DataTable dtShareCapital = Filename.ImportCsvFile();
                DataTable dtDistinctResult = dtShareCapital.DefaultView.ToTable(true, "MemberCode");

                foreach (DataRow row in dtDistinctResult.Rows)
                {
                    _memberCode = row["MemberCode"].ToString();
                    _shareCapitalAmount  = dtShareCapital.Select().Where(o => (string)o["MemberCode"] == _memberCode).Sum(o => Convert.ToDecimal(o["Amount"]));

                    #region Update the Share Capital of Member

                    var sqlStatement =
                        new StringBuilder();
                    sqlStatement.Append(
                        "Update members SET " +
                        "ShareCapitalAmount = ?ShareCapitalAmount " +
                        "WHERE MemberCode = ?MemberCode");
                    Database.DatabaseController.ExecuteNonQuery(
                        sqlStatement.ToString(),
                        new SqlParameter("?ShareCapitalAmount",_shareCapitalAmount),
                        new SqlParameter("?MemberCode",_memberCode));

                    #endregion
                }

                return new Controllers.Result(true, "Successfully Updated!");
            }
            catch (Exception ex)
            {
                return new Controllers.Result(false, ex.Message);
            }
        }
    }
}
