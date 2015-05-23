using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.Models
{
    public abstract class AMappingDetail
    {
        public int  MappingDetailId { get; set; }
        public int  LoanDetailId { get; set; }
        public int  TimeDepositDetailId { get; set; }

        public abstract Result Find(int id);
        public abstract Result Create();

    }
}
