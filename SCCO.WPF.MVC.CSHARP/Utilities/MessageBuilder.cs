using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Utilities
{
    internal class MessageBuilder
    {
        internal static string TransactionPosted(Voucher voucher)
        {
            return string.Format("Transaction Posted! Please check {0}#{1}.", voucher.VoucherType, voucher.VoucherNo);
        }
    }
}