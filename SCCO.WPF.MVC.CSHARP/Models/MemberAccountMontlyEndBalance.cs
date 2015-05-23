using System.Linq;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class MemberAccountMontlyEndBalance
    {
        public MemberAccountMontlyEndBalance(System.Data.DataRow row)
        {
            MemberCode = DataConverter.ToString(row["member_code"]);
            MemberName = DataConverter.ToString(row["member_name"]);
            AccountCode = DataConverter.ToString(row["account_code"]);
            AccountTitle = DataConverter.ToString(row["account_title"]);
            CertificateNo = DataConverter.ToString(row["certificate_no"]);

            Beginning = DataConverter.ToDecimal(row["beginning"]);
            January = DataConverter.ToDecimal(row["january"]);
            February = DataConverter.ToDecimal(row["february"]);
            March = DataConverter.ToDecimal(row["march"]);
            April = DataConverter.ToDecimal(row["april"]);
            May = DataConverter.ToDecimal(row["may"]);
            June = DataConverter.ToDecimal(row["june"]);
            July = DataConverter.ToDecimal(row["july"]);
            August = DataConverter.ToDecimal(row["august"]);
            September = DataConverter.ToDecimal(row["september"]);
            October = DataConverter.ToDecimal(row["october"]);
            November = DataConverter.ToDecimal(row["november"]);
            December = DataConverter.ToDecimal(row["december"]);
        }

        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string AccountCode { get; set; }
        public string AccountTitle { get; set; }
        public string CertificateNo { get; set; }

        public decimal Beginning { get; set; }

        public decimal January { get; set; }

        public decimal February { get; set; }

        public decimal March { get; set; }

        public decimal April { get; set; }

        public decimal May { get; set; }

        public decimal June { get; set; }

        public decimal July { get; set; }

        public decimal August { get; set; }

        public decimal September { get; set; }

        public decimal October { get; set; }

        public decimal November { get; set; }

        public decimal December { get; set; }

        public decimal Average {
            get
            {
                var endBalances = new decimal[12];
                endBalances[0] = January;
                endBalances[1] = February;
                endBalances[2] = March;
                endBalances[3] = April;
                endBalances[4] = May;
                endBalances[5] = June;
                endBalances[6] = July;
                endBalances[7] = August;
                endBalances[8] = September;
                endBalances[9] = October;
                endBalances[10] = November;
                endBalances[11] = December;
                return endBalances.Average();
            }
        }

        public decimal End { get { return December; } }

        public decimal InterestEarned { get; set; }
    }
}
