namespace SCCO.WPF.MVC.CS.Models.Loan
{
    public class MemberLoanDepositSummary
    {
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string AreaCode { get; set; }
        public decimal TotalLoanBalance { get; set; }
        public decimal TotalDepositBalance { get; set; }
    }
}
