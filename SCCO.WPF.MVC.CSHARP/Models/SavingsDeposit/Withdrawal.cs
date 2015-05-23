using System.ComponentModel;
using System.Data;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models.SavingsDeposit
{
    public class Withdrawal : INotifyPropertyChanged
    {
        private const string TableName = "cv";
        private AccountInformation _accountInformation;
        private decimal _withdrawalAmount;
        private DailyWithdrawalSettings _withdrawalSettings;
        private int _withdrawalSlipNo;

        public event PropertyChangedEventHandler PropertyChanged;

        public AccountInformation AccountInfo
        {
            get { return _accountInformation; }
            set { _accountInformation = value; OnPropertyChanged("AccountInformation"); }
        }

        public decimal WithdrawalAmount
        {
            get { return _withdrawalAmount; }
            set { _withdrawalAmount = value; OnPropertyChanged("WithdrawalAmount"); }
        }

        public DailyWithdrawalSettings WithdrawalSettings
        {
            get { return _withdrawalSettings; }
            set { _withdrawalSettings = value; OnPropertyChanged("WithdrawalSettings"); }
        }

        public int WithdrawalSlipNo
        {
            get { return _withdrawalSlipNo; }
            set { _withdrawalSlipNo = value; OnPropertyChanged("WithdrawalSlipNo"); }
        }

        public static bool IsExists(int withdrawalSlipNo)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT WS_NO FROM {0} WHERE WS_NO = ?WS_NO", TableName);
            var sqlParam = new SqlParameter("?WS_NO", withdrawalSlipNo);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, sqlParam);
            if (dataTable == null) return false;

            return dataTable.Rows.Count > 0;
        }

        public static int LastWithdrawalSlipNo()
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT IFNULL(MAX(WS_NO),0) AS WS_NO FROM {0}", TableName);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder);
            return Utilities.DataConverter.ToInteger(dataTable.Rows[0]["WS_NO"]);
        }

        public static decimal TotalWithdrawals(int voucherNo)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT IFNULL(SUM(DEBIT),0) AS TOTAL_DEBIT FROM {0} WHERE DOC_NUM = ?DOC_NUM", TableName);
            var sqlParam = new SqlParameter("?DOC_NUM", voucherNo);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, sqlParam);
            return Utilities.DataConverter.ToInteger(dataTable.Rows[0]["TOTAL_DEBIT"]);
        }

        public static Result ReBalanceWithdrawals(Voucher voucher, decimal totalWithdrawals)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("SELECT *");
            sqlBuilder.AppendLine("FROM cv");
            sqlBuilder.AppendLine("WHERE DOC_NUM = ?DOC_NUM");
            sqlBuilder.AppendLine("AND CREDIT > 0");
            sqlBuilder.AppendLine("LIMIT 1");
            var sqlParam = new SqlParameter("?DOC_NUM", voucher.VoucherNo);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, sqlParam);

            var cv = new CashVoucher();

            foreach (DataRow dataRow in dataTable.Rows)
                cv.SetPropertiesFromDataRow(dataRow);

            cv.Credit = totalWithdrawals;
            cv.Amount = totalWithdrawals;
            cv.AmountInWords = Utilities.Converter.AmountToWords(totalWithdrawals);

            if (cv.ID == 0)
            {
                var company = Nfmb.FindByCode(GlobalSettings.CodeOfCompany);
                cv.MemberCode = company.MemberCode;
                cv.MemberName = company.MemberName;

                var coh = Account.FindByCode(GlobalSettings.CodeOfCashOnHand);
                cv.AccountCode = coh.AccountCode;
                cv.AccountTitle = coh.AccountTitle;

                cv.VoucherNo = voucher.VoucherNo;
                cv.VoucherDate = voucher.VoucherDate;
                cv.VoucherType = cv.VoucherType;

                cv.Explanation = "Daily partial withdrawal from Savings Deposit";
               return cv.Create();
            }

            return cv.Update();
        }

        public void InitializeProperties()
        {
            WithdrawalSettings = new DailyWithdrawalSettings();
            WithdrawalSettings.InitializeProperties();

            AccountInfo = new AccountInformation();
            WithdrawalSlipNo = LastWithdrawalSlipNo() + 1;
        }

        public Result Validate()
        {
            //alert user if there is no withdrawal amount entered
            if (WithdrawalAmount <= 0)
            {
                return new Result(false, "Withdrawal amount in not valid.");
            }

            //1. must not be more than withdrawable amount
            if (WithdrawalSettings.WithdrawableAmount < WithdrawalAmount)
            {
                return new Result(false,
                                  string.Format("Withdrawal amount must not be more than P{0:N}!", WithdrawalSettings.WithdrawableAmount));
            }

            //2. must not exceed total withdrawals
            if (WithdrawalSettings.MaximumDailyWithdrawals < TotalWithdrawals(WithdrawalSettings.WithdrawalVoucherNo) + WithdrawalAmount)
            {
                return new Result(false,
                                  string.Format("Total withdrawals must not be more than P{0:N}!",
                                                WithdrawalSettings.MaximumDailyWithdrawals));
            }

            //3. must meet maintaining balance
            if (WithdrawalSettings.MaintainingBalance > AccountInfo.CurrentBalance - WithdrawalAmount)
            {
                return new Result(false, string.Format("Maintaining balance must not be less than P{0:N}!",
                                                       WithdrawalSettings.MaintainingBalance));
            }

            //4. administrator transaction date must be the same as daily withdrawal transaction date
            if (GlobalSettings.DateOfOpenTransaction != WithdrawalSettings.TransactionDate)
            {
                return new Result(false, string.Format("Withdrawal Date or Transaction Date is not set!"));
            }

            if (WithdrawalSlipNo <= 0)
            {
                return new Result(false, "Invalid Withdrawal Slip Number!");
            }

            if (IsExists(WithdrawalSlipNo))
            {
                return new Result(false, "Withdrawal Slip Number already in use!");
            }

            return new Result(true, "All entries are valid!");
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public class AccountInformation : INotifyPropertyChanged
        {
            private string _accountCode;
            private string _accountTitle;
            private decimal _currentBalance;
            private string _memberCode;
            private string _memberName;
            public event PropertyChangedEventHandler PropertyChanged;

            public string AccountCode
            {
                get { return _accountCode; }
                set { _accountCode = value; OnPropertyChanged("AccountCode"); }
            }

            public string AccountTitle
            {
                get { return _accountTitle; }
                set { _accountTitle = value; OnPropertyChanged("AccountTitle"); }
            }

            public decimal CurrentBalance
            {
                get { return _currentBalance; }
                set { _currentBalance = value; OnPropertyChanged("CurrentBalance"); }
            }

            public string MemberCode
            {
                get { return _memberCode; }
                set { _memberCode = value; OnPropertyChanged("MemberCode"); }
            }

            public string MemberName
            {
                get { return _memberName; }
                set { _memberName = value; OnPropertyChanged("MemberName"); }
            }

            private void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}