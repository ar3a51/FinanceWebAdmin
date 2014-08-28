using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinanceWeb.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionExpireFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext currContext = HttpContext.Current;

            if( currContext.Session["ConnectionString"] == null)
            {

                currContext.Session["ConnectionString"] = ConfigurationManager.ConnectionStrings["Onyx"].ConnectionString;
                currContext.Session["auditConnString"] = ConfigurationManager.ConnectionStrings["auditEntities"].ConnectionString;
                currContext.Session["TopClass"] = ConfigurationManager.ConnectionStrings["TopClass"].ConnectionString;

            }


            base.OnActionExecuting(filterContext);
        }
    }
}