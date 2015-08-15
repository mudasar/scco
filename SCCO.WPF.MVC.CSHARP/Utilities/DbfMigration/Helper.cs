using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Utilities.DbfMigration
{
    public static class MigrationHelper
    {
        public static DataTable OpenFile(string fileName)
        {
            var con = new OleDbConnection("Provider=VFPOLEDB.1;Data Source=" + fileName);
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                var da = new OleDbDataAdapter("select * from " + Path.GetFileName(fileName), con);
                var ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds.Tables[0];
            }
            catch
            {
                return null;
            }
        }

        public static object GetDefaultFieldValue(object p, Type type)
        {
            if (type == typeof(String))
            {
                var stringValue = (string)p;
                if (string.IsNullOrEmpty(stringValue.Trim())) return null;
                return stringValue;
            }
            if (type == typeof(DateTime))
            {
                var dateTimeValue = (DateTime)p;
                DateTime minDateTime = Convert.ToDateTime("12/30/1899 12:00:00 AM");
                if (dateTimeValue <= minDateTime) return null;
                return dateTimeValue;
            }
            if (type == typeof(Decimal))
            {
                var numericValue = (Decimal)p;
                if (numericValue == 0) return null;
                return numericValue;
            }
            if (type == typeof(Boolean))
            {
                var booleanValue = (Boolean)p;
                if (booleanValue == false) return null;
                return true;
            }

            return null;
        }

        public static bool TruncateTable(string table)
        {
            var truncateTable = string.Format("TRUNCATE TABLE `{0}`", table);
            return DatabaseController.ExecuteNonQuery(truncateTable) >= 0;
        }
    }
}
