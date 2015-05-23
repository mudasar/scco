using System;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    public class ScheduledPayment
    {
        public int PaymentNo { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal Balance { get; set; }
        public decimal CapitalBuildUp { get; set; }
    }


}
