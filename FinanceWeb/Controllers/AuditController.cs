using AppDomain.AuditDomain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinanceWeb.Models;
using FinanceWeb.Models.Tools;
using FinanceWeb.Security;
using Newtonsoft.Json;



namespace FinanceWeb.Controllers
{
    public class AuditController : Controller
    {
        //
        // GET: /Audit/
        [Authenticate()]
        public ActionResult Index()
        {
            Session["auditConnString"] = ConfigurationManager.ConnectionStrings["auditEntities"].ConnectionString;
            
            return View();
        }

        [SessionExpireFilter]
        public JsonResult getDebtorAuditList(String entityType, DateTime fromDate, DateTime? toDate) 
        {
            
            List<AuditDebtorSync> listAudit;
            String[][] responseString;
            int counter = 0;

            try
            {
               

                using (AuditModel audit = new AuditModel(Session["auditConnString"].ToString()))
                {
                    listAudit = audit.getAuditDebtorSyncParam(entityType, fromDate,  Tool.getDate(fromDate, toDate));

                    responseString = new String[listAudit.Count][];

                    foreach (AuditDebtorSync auditEntity in listAudit)
                    {
                        responseString[counter] = new String[]{
                        auditEntity.auditId.ToString(),
                        auditEntity.MemberId.ToString(),
                        auditEntity.entityType.ToString(),
                        auditEntity.action,
                        auditEntity.reason,
                        auditEntity.modifiedBy,
                        auditEntity.updateDate.ToString("MMM d, yyyy hh:mm tt")
                                                            };

                        counter++;
                    }

                }

                return Json(responseString);
            }
            catch (Exception exception)
            {
                Tool.sendException(exception);
                Response.Redirect("~/Error/returnJsonExceptionError");
                return null;
            }
          
        }

         [SessionExpireFilter]
        public JsonResult getTransAuditList(String entityType, DateTime fromDate, DateTime? toDate)
        {

            List<AuditTransSync> listAudit;
            String[][] responseString;
            int counter = 0;
            try
            {
               

                using (AuditModel audit = new AuditModel(Session["auditConnString"].ToString()))
                {
                    listAudit = audit.getAuditTransSyncParam(entityType, fromDate,  Tool.getDate(fromDate, toDate));

                    responseString = new String[listAudit.Count][];

                    foreach (AuditTransSync auditEntity in listAudit)
                    {
                        responseString[counter] = new String[]{
                        auditEntity.auditId.ToString(),
                        auditEntity.documentId.ToString(),
                        auditEntity.system.ToString(),
                        auditEntity.action,
                        auditEntity.reason,
                        auditEntity.modifiedBy,
                        auditEntity.updateDate.ToString("MMM d, yyyy hh:mm tt")
                                                            };

                        counter++;
                    }

                }

                return Json(responseString,JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Tool.sendException(exception);
                Response.Redirect("~/Error/returnJsonExceptionError");
                return null;
            }
           

        }

       

      

    }
}
