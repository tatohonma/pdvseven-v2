using System;

namespace a7D.PDV.BLL.Utils
{
    public class ExceptionHelper
    {
        public static string InnerExceptionMessageLoop(Exception ex)
        {
            if (ex != null)
            {
                return ex.Message + " " + InnerExceptionMessageLoop(ex.InnerException);
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
