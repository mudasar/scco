using System;

namespace SCCO.WPF.MVC.CS.Views
{
    interface IListDetailView
    {
        void Add();
        void Edit();
        void Delete();
        void Search();
        void RefreshDisplay();
    }
}
