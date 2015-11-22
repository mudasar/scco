namespace SCCO.WPF.MVC.CS.Database
{
    public class SqlParameter
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public SqlParameter(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("`{0}` = {1}", Key.Replace("?", ""), Key);
        }
    }
}
