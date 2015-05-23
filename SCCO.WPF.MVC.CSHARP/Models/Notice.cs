using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCCO.WPF.MVC.CS.Models
{
    public class Notice
    {
        public int NoticeId { get; set; }
        public int LoanDetailsId { get; set; }
        public DateTime? NoticeDate { get; set; }
        public string Comment { get; set; }
    }
}
