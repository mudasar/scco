using System;
using System.Data;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Utilities;
using SCCO.WPF.MVC.CS.Views.AdministratorModule;

namespace SCCO.WPF.MVC.CS.Controllers
{
    internal class SavingsDepositController
    {
        public static void InitializeModel(InterestOnSavingsDepositViewModel viewModel)
        {
            viewModel.Collection = new VoucherCollection();
            viewModel.InterestRate = GlobalSettings.RateOfInterestOnSavingsDeposit;
            viewModel.RequiredBalance = GlobalSettings.AmountOfInterestOnSavingsDepositRequiredBalance;

            viewModel.InterestExpenseOnSavingsDepositAccount =
                Account.FindByCode(GlobalSettings.CodeOfInterestExpenseOnSavingsDeposit);
            viewModel.SavingsDepositAccount =
                Account.FindByCode(GlobalSettings.CodeOfSavingsDeposit);
        }

        public static void ProcessInterestOnSavingsDeposit(InterestOnSavingsDepositViewModel viewModel)
        {
            // settings
            decimal interestRate = viewModel.InterestRate;
            var multiplier = interestRate / 4;
            decimal minimumBalance = viewModel.RequiredBalance;
            int quarter = viewModel.Quarter;

            // database
            const string spName = "sp_account_monthly_ending_balance_by_code";
            var param = new SqlParameter("tc_account_code", viewModel.SavingsDepositAccount.AccountCode);

            var dataTable = DatabaseController.ExecuteStoredProcedure(spName, param);
            var voucherCollection = new VoucherCollection();

            foreach (DataRow datarow in dataTable.Rows)
            {
                decimal average = 0;

                #region --- Get average per quarter ---

                decimal month1;
                decimal month2;
                decimal month3;

                switch (quarter)
                {
                    case 1:
                        month1 = DataConverter.ToDecimal(datarow["january"]);
                        month2 = DataConverter.ToDecimal(datarow["february"]);
                        month3 = DataConverter.ToDecimal(datarow["march"]);
                        average = (month1 + month2 + month3) / 3;
                        break;

                    case 2:
                        month1 = DataConverter.ToDecimal(datarow["april"]);
                        month2 = DataConverter.ToDecimal(datarow["may"]);
                        month3 = DataConverter.ToDecimal(datarow["june"]);
                        average = (month1 + month2 + month3) / 3;
                        break;
                    case 3:
                        month1 = DataConverter.ToDecimal(datarow["july"]);
                        month2 = DataConverter.ToDecimal(datarow["august"]);
                        month3 = DataConverter.ToDecimal(datarow["september"]);
                        average = (month1 + month2 + month3) / 3;
                        break;
                    case 4:
                        month1 = DataConverter.ToDecimal(datarow["october"]);
                        month2 = DataConverter.ToDecimal(datarow["november"]);
                        month3 = DataConverter.ToDecimal(datarow["december"]);
                        average = (month1 + month2 + month3) / 3;
                        break;
                }

                #endregion

                if (average < minimumBalance) continue;

                var voucher = new Voucher();
                voucher.MemberCode = DataConverter.ToString(datarow["member_code"]);
                voucher.MemberName = DataConverter.ToString(datarow["member_name"]);
                voucher.AccountCode = DataConverter.ToString(datarow["account_code"]);
                voucher.AccountTitle = DataConverter.ToString(datarow["account_title"]);
                voucher.Credit = Math.Round((average * multiplier), 2);
                voucherCollection.Add(voucher);
            }

            viewModel.Collection = voucherCollection;
        }


    }
}
