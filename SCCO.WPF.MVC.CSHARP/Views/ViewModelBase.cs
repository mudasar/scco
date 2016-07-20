using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views
{
    public class ViewModelBase : INotifyPropertyChanged
    {
      public event PropertyChangedEventHandler PropertyChanged;

      protected virtual void OnPropertyChanged(string propertyName)
      {
          PropertyChangedEventHandler handler = PropertyChanged;
          if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
      }

      protected static IList<MemberAccountMontlyEndBalance> GetMontlyEndBalance(int year, string accountCode)
      {
          var database = string.Format("{0}_{1}_{2}", Properties.Settings.Default.BranchName, year,
                                       Properties.Settings.Default.DatabaseEnvironment);
          var parameters = new List<SqlParameter> { new SqlParameter("tc_account_code", accountCode) };
          DataTable dataTable = DatabaseController.ExecuteStoredProcedure(
              "sp_account_monthly_ending_balance_by_code", database, parameters.ToArray());
          return (from DataRow row in dataTable.Rows select new MemberAccountMontlyEndBalance(row)).ToList();
      }
    }
}
