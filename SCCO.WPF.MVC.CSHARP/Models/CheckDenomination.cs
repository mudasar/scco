using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCCO.WPF.MVC.CS.Models
{
    public class CheckDenomination
    {

        public int CheckDenominationId { get; set; }
        public int TransactionHeaderId { get; set; }
        public string BankName { get; set; }
        public DateTime? CheckDate { get; set; }
        public string CheckNo { get; set; }
        public decimal Amount { get; set; }

    }
}
