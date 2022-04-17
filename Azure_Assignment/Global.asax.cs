using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Azure_Assignment
{
    public class MvcApplication : System.Web.HttpApplication
    {
        

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                if (IsMaxRequestExceededException(this.Server.GetLastError()))
                {
                    this.Server.ClearError();
                    HttpContext.Current.ClearError();
                    /*  NOTE: Because of the special context we're in, Server.Execute and Server.Transfer
                        *  won't work on the next line. */
                    Response.Redirect("~/Home/UploadTooBig");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        const int TimedOutExceptionCode = -2147467259;
        private static bool IsMaxRequestExceededException(Exception e)
        {
            // unhandled errors = caught at global.ascx level
            // http exception = caught at page level

            Exception main;
            var unhandled = e as HttpUnhandledException;

            if (unhandled != null && unhandled.ErrorCode == TimedOutExceptionCode)
            {
                main = unhandled.InnerException;
            }
            else
            {
                main = e;
            }


            var http = main as HttpException;

            if (http != null && http.ErrorCode == TimedOutExceptionCode)
            {
                // hack: no real method of identifying if the error is max request exceeded as 
                // it is treated as a timeout exception
                if (http.StackTrace.Contains("GetEntireRawContent"))
                {
                    // MAX REQUEST HAS BEEN EXCEEDED
                    return true;
                }
            }

            return false;
        }

        
    }
}
