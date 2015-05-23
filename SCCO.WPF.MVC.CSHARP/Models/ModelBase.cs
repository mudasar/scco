using System.ComponentModel;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID");}
        }

        protected string _tableName;

        protected SqlParameter _paramKey
        {
            get { return new SqlParameter("?ID", ID); }
        }

        protected ModelBase(string tableName)
        {
            _tableName = tableName;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
