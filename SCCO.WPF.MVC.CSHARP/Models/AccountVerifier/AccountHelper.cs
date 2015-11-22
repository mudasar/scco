using System;

namespace SCCO.WPF.MVC.CS.Models.AccountVerifier
{
    static class AccountHelper
    {
        internal static bool IsOpeningYear(DateTime asOf)
        {
            if (asOf.Year == GlobalSettings.DateOfOpenTransaction.Year)
            {
                if (asOf.Month == 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
