using System.ComponentModel;

namespace SCCO.WPF.MVC.CS.Models
{
    public class CoMaker : INotifyPropertyChanged
    {
        public CoMaker()
        { }

        public CoMaker(string memberCode, string memberName)
        {
            MemberCode = memberCode;
            MemberName = memberName;
        }
        private string _memberCode;
        private string _memberName;

        #region --PROPERTIES--

        public string MemberCode
        {
            get { return _memberCode; }
            set { _memberCode = value; OnPropertyChanged("MemberCode"); }
        }

        public string MemberName
        {
            get { return _memberName; }
            set { _memberName = value; OnPropertyChanged("MemberName"); }
        }

        #endregion

        public void ResetProperties()
        {
            MemberCode = string.Empty;
            MemberName = string.Empty;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", MemberCode, MemberName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
