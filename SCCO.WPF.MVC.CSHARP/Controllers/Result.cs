namespace SCCO.WPF.MVC.CS.Controllers
{
    public class Result
    {
        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public Result(bool success, string message, object classname, string methodname)
        {
            Success = success;
            Message = message;

            //System.Diagnostics.Debug.WriteLine(@">>> Result: {0} --> {1}.{2} --> {3}", success, classname, methodname,
            //                                   message);
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
