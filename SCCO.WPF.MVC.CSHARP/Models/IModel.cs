using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.Models {

    public interface IModel
    {
        Result Create();

        Result Update();

        Result Destroy();

        Result Find(int id);

        void ResetProperties();

        void SetPropertiesFromDataRow(System.Data.DataRow dataRow);

    }
}
