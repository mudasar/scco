using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCCO.WPF.MVC.CS.Models.ShareCapital
{
    public static class ExtensionMethod
    {

        public static DataTable ImportCsvFile(this string filePath)
        {
            try
            {
                var file = new FileInfo(filePath);
                string connectionString = "Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + file.DirectoryName + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\"";
                DataTable tbl;
                using (var con = new OleDbConnection(connectionString))
                {
                    string cmdText = string.Format("SELECT * FROM [{0}]", file.Name);
                    using (var cmd = new OleDbCommand(cmdText, con))
                    {
                        con.Open();
                        using (var adp = new OleDbDataAdapter(cmd))
                        {
                            tbl = new DataTable("Result");
                            adp.Fill(tbl);
                        }
                    }
                }
                return tbl;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Boolean WriteToCsvFile(this DataTable dataTable, string filePath)
        {
            try
            {
                var fileContent = new StringBuilder();

                foreach (var col in dataTable.Columns)
                    fileContent.Append(col.ToString() + ",");

                fileContent.Replace(",", Environment.NewLine, fileContent.Length - 1, 1);

                var parallel = Parallel.ForEach(dataTable.AsEnumerable(), dr =>
                                                               {
                                                                   foreach (var column in dr.ItemArray)
                                                                   {
                                                                       fileContent.Append("\"" + column.ToString() +
                                                                                          "\",");
                                                                   }
                                                                   fileContent.Replace(",", Environment.NewLine,
                                                                                       fileContent.Length - 1, 1);
                                                               });
                if (parallel.IsCompleted)
                    File.WriteAllText(filePath, fileContent.ToString());

                return true;
            }
            catch (Exception)
            {
                return false;
            }

           

        }
    }
}
