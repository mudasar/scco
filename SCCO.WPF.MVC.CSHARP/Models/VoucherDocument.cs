using System;

namespace SCCO.WPF.MVC.CS.Models
{
    public struct VoucherDocument
    {
        public VoucherTypes Type;
        public int Number;
        public DateTime Date;

        public VoucherDocument(VoucherTypes type, int number, DateTime date)
        {
            Type = type;
            Number = number;
            Date = date;
        }
    }
}
