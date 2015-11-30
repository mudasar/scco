using System;
using System.Data;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public enum GlobalKeys
    {
        CodeOfCapitalBuildUp,
        CodeOfCashOnHand,
        CodeOfCompany,
        CodeOfInterestExpenseOnSavingsDeposit,
        CodeOfInterestExpenseOnTimeDeposit,
        CodeOfInterestIncomeFromLoans,
        CodeOfLoanReceivables,
        CodeOfMiscellaneousIncome,
        CodeOfSalaryAdvance,
        CodeOfSavingsDeposit,
        CodeOfTimeDeposit,
        CodeOfUnearnedIncome,
        CodeOfServiceFee,
        DateOfOpenTransaction,
        NumberOfSavingsDepositWithdrawalCashVoucher,
        AmountOfSavingsDepositMaximumDailyWithdrawals,
        AmountOfSavingsDepositMaintainingBalance,
        AmountOfSavingsDepositWithdrawable,
        AmountOfInterestOnSavingsDepositRequiredBalance,
        AmountOfMinimumTimeDepositServiceFee,
        RateOfFines,
        RateOfInterestOnSavingsDeposit,
        RateOfTimeDepositServiceFee,
        DateOfSavingsDepositWithdrawalCashVoucher,
    }

    public static class GlobalSettings
    {
        public static string CodeOfCapitalBuildUp
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfCapitalBuildUp;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static string CodeOfCashOnHand
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfCashOnHand;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static string CodeOfCompany
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfCompany;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static string CodeOfInterestIncomeFromLoans
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfInterestIncomeFromLoans;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static string CodeOfLoanReceivables
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfLoanReceivables;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static string CodeOfMiscellaneousIncome
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfMiscellaneousIncome;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static string CodeOfSalaryAdvance
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfSalaryAdvance;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static string CodeOfSavingsDeposit
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfSavingsDeposit;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static string CodeOfTimeDeposit
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfTimeDeposit;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static string CodeOfInterestExpenseOnTimeDeposit
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfInterestExpenseOnTimeDeposit;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static string CodeOfServiceFee
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfServiceFee;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static string CodeOfUnearnedIncome
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfUnearnedIncome;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static DateTime DateOfOpenTransaction
        {
            get
            {
                const GlobalKeys key = GlobalKeys.DateOfOpenTransaction;
                return DataConverter.ToDateTime(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static decimal RateOfFines
        {
            get
            {
                const GlobalKeys key = GlobalKeys.RateOfFines;
                return DataConverter.ToDecimal(SearchDatabase(key)["CurrentValue"]);
            }
        }

        //AmountOfMinimumTimeDepositServiceFee

        public static decimal AmountOfMinimumTimeDepositServiceFee
        {
            get
            {
                const GlobalKeys key = GlobalKeys.AmountOfMinimumTimeDepositServiceFee;
                return DataConverter.ToDecimal(SearchDatabase(key)["CurrentValue"]);
            }
        }

        #region --- Savings Deposit Withdrawal Setup ---

        public static decimal AmountOfSavingsDepositMaintainingBalance
        {
            get
            {
                const GlobalKeys key = GlobalKeys.AmountOfSavingsDepositMaintainingBalance;
                return DataConverter.ToDecimal(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static decimal AmountOfSavingsDepositMaximumDailyWithdrawals
        {
            get
            {
                const GlobalKeys key = GlobalKeys.AmountOfSavingsDepositMaximumDailyWithdrawals;
                return DataConverter.ToDecimal(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static decimal AmountOfSavingsDepositWithdrawable
        {
            get
            {
                const GlobalKeys key = GlobalKeys.AmountOfSavingsDepositWithdrawable;
                return DataConverter.ToDecimal(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static DateTime DateOfSavingsDepositWithdrawalCashVoucher
        {
            get
            {
                const GlobalKeys key = GlobalKeys.DateOfSavingsDepositWithdrawalCashVoucher;
                return DataConverter.ToDateTime(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static int NumberOfSavingsDepositWithdrawalCashVoucher
        {
            get
            {
                const GlobalKeys key = GlobalKeys.NumberOfSavingsDepositWithdrawalCashVoucher;
                return DataConverter.ToInteger(SearchDatabase(key)["CurrentValue"]);
            }
        }

        #endregion --- Savings Deposit Withdrawal Setup ---

        #region --- Interest on Savings Deposit Posting Setup ---

        public static decimal AmountOfInterestOnSavingsDepositRequiredBalance
        {
            get
            {
                const GlobalKeys key = GlobalKeys.AmountOfInterestOnSavingsDepositRequiredBalance;
                return DataConverter.ToDecimal(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static string CodeOfInterestExpenseOnSavingsDeposit
        {
            get
            {
                const GlobalKeys key = GlobalKeys.CodeOfInterestExpenseOnSavingsDeposit;
                return DataConverter.ToString(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static decimal RateOfInterestOnSavingsDeposit
        {
            get
            {
                const GlobalKeys key = GlobalKeys.RateOfInterestOnSavingsDeposit;
                return DataConverter.ToDecimal(SearchDatabase(key)["CurrentValue"]);
            }
        }

        public static decimal RateOfTimeDepositServiceFee
        {
            get
            {
                const GlobalKeys key = GlobalKeys.RateOfTimeDepositServiceFee;
                return DataConverter.ToDecimal(SearchDatabase(key)["CurrentValue"]);
            }
        }

        #endregion --- Interest on Savings Deposit Posting Setup ---

        public static object GetSettingsFromKey(Enum key)
        {
            var row = SearchDatabase(key);

            var keyword = GetKeywordFromEnum(key);
            if (keyword.ToLower().Contains("amountof") || keyword.ToLower().Contains("rateof"))
            {
                return DataConverter.ToDecimal(row);
            }
            if (keyword.ToLower().Contains("codeof"))
            {
                return DataConverter.ToString(row);
            }
            if (keyword.ToLower().Contains("numberof"))
            {
                return DataConverter.ToInteger(row);
            }
            if (keyword.ToLower().Contains("dateof"))
            {
                return DataConverter.ToDateTime(row);
            }
            return null;
        }

        public static string ToKeyword(this Enum keys)
        {
            return GetKeywordFromEnum(keys);
        }

        internal static void Update(string keyword, object value)
        {
            try
            {
                var globalVariable = GlobalVariable.FindByKeyword(keyword);

                if (keyword.ToLower().Contains("amountof") || keyword.ToLower().Contains("rateof"))
                {
                    globalVariable.CurrentValue = string.Format("{0:N}", value);
                }
                if (keyword.ToLower().Contains("codeof"))
                {
                    globalVariable.CurrentValue = string.Format("{0}", value);
                }
                if (keyword.ToLower().Contains("numberof"))
                {
                    globalVariable.CurrentValue = string.Format("{0}", value);
                }
                if (keyword.ToLower().Contains("dateof"))
                {
                    globalVariable.CurrentValue = string.Format("{0:d}", value);
                }

                if (globalVariable.ID > 0)
                    globalVariable.Update();
                else
                    globalVariable.Create();
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(typeof(GlobalSettings), exception);
            }
        }

        private static string GetKeywordFromEnum(Enum key)
        {
            return Enum.GetName(typeof(GlobalKeys), key);
        }

        private static DataRow SearchDatabase(Enum key)
        {
            try
            {
                var keyword = GetKeywordFromEnum(key);

                const string sqlCommandText =
                    "SELECT CurrentValue FROM global_variables WHERE Keyword = ?Keyword LIMIT 1";
                var sqlParameter = new SqlParameter("?Keyword", keyword);
                var dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText, sqlParameter);

                if (dataTable.Rows.Count > 0)
                    return dataTable.Rows[0];

                // save settings if not yet existing in the database
                var value = string.Empty;
                if (keyword.ToLower().Contains("amountof") || keyword.ToLower().Contains("rateof"))
                {
                    value = "0.00";
                }
                if (keyword.ToLower().Contains("codeof"))
                {
                    value = string.Empty;
                }
                if (keyword.ToLower().Contains("numberof"))
                {
                    value = "0";
                }
                if (keyword.ToLower().Contains("dateof"))
                {
                    value = string.Format("{0:d}", DateTime.Now);
                }

                Update(keyword, value);

                return SearchDatabase(key);
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(typeof(GlobalSettings), exception);
                return null;
            }
        }
    }
}