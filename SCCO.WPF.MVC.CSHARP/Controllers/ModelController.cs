using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SCCO.WPF.MVC.CS.Database;
using System;

namespace SCCO.WPF.MVC.CS.Controllers
{
    public class ModelController
    {
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public enum DataManipulationType
        {
            Create,
            Read,
            Update,
            Delete
        }

        public static class Releases
        {
            public static int MaxReleaseNumber()
            {
                var dataTable = DatabaseController.ExecuteStoredProcedure("sp_max_release_no");
                return Utilities.DataConverter.ToInteger(dataTable.Rows[0][0]);
            }
        }

        public static void AddParameter(List<SqlParameter> sqlParameters, string key, object value)
        {
            if (sqlParameters == null) sqlParameters = new List<SqlParameter>();
            //if (Utilities.Converter.ToNullIfDefault(value) == null)
            //    value = null;
            sqlParameters.Add(new SqlParameter(key, Utilities.Converter.ToNullIfDefault(value)));
        }


        public static void GetAccountMonthEndBalance()
        {
            var reportNo = 1;

            var sqlBuilder = new System.Text.StringBuilder();
            sqlBuilder.AppendLine("SELECT CONCAT(LPAD(MEM_CODE,10,'0'),'-',RPAD(ACC_CODE,10,'0'),'-',LPAD(CERT_NO, 10, '0')) AS REFERENCE_ID,");
            sqlBuilder.AppendLine("MEM_CODE, ACC_CODE, CERT_NO,");
            sqlBuilder.AppendLine("IF(chart.NATURE='D', IFNULL(SUM(DEBIT),0) - IFNULL(SUM(CREDIT),0), IFNULL(SUM(CREDIT),0) - IFNULL(SUM(DEBIT),0)) AS BEG");
            sqlBuilder.AppendLine("FROM `slbal`");
            sqlBuilder.AppendLine("INNER JOIN chart");
            sqlBuilder.AppendLine("ON chart.`CODE` = slbal.ACC_CODE");
            sqlBuilder.AppendLine("WHERE chart.SCODE = ?SCODE");
            sqlBuilder.AppendLine("GROUP BY MEM_CODE, ACC_CODE, CERT_NO");

            var sqlParameter = new SqlParameter("?SCODE", reportNo);

            System.Console.WriteLine(DateTime.Now);
            var result = DatabaseController.ExecuteSelectQuery(sqlBuilder, sqlParameter);
            System.Console.WriteLine(DateTime.Now);

            Console.WriteLine(result);

        }

    }
}
