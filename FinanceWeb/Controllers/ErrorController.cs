using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace FinanceWeb.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnauthorizedEntry()
        {
            return View();
        }

        public JsonResult returnJsonIdNotFound()
        {
            JsonResult result;
            
            result = new JsonResult { Data = new { Error = "ID not found.  Please re-type the ID." }, JsonRequestBehavior=JsonRequestBehavior.AllowGet };

            return result;
        }

        public JsonResult returnJsonSessionTimeOut()
        {
            JsonResult result;

            result = new JsonResult { Data = new { timeout = "Session Time out.  Page will be refreshed" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            return result;


        }

        public JsonResult returnJsonExceptionError()
        {
            JsonResult result;

            result = new JsonResult { Data = new { Error = "An error occured.  Please notify IT team." }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            return result;
        }

        public ActionResult exceptionError()
        {
            return View();
        }

    }
}
