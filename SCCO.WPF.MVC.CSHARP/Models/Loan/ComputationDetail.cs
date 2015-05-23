using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    public struct ComputationDetail
    {
        public ComputationDetail(string accountCode, string accountTitle, decimal amount)
            : this()
        {
            AccountCode = accountCode;
            AccountTitle = accountTitle;
            Amount = amount;
        }

        public string AccountCode { get; set; }
        public string AccountTitle { get; set; }
        public decimal Amount { get; set; }
    }
}
