using System;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Controllers
{
    public class ActionController
    {
        public static Result InvokeAction(Action action)
        {
            try
            {
              action.Invoke();
              return new Result(true, "Successful: " + action.Method);
            }
            catch (Exception exception)
            {
                Utilities.Logger.ExceptionLogger(action, exception);
                return new Result(false, exception.Message);
            }
        }

    }
}
