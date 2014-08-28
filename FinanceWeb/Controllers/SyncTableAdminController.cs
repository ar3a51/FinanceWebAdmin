using AppDomain;
using AppDomain.AuditDomain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinanceWeb.Models;
using FinanceWeb.Models.Tools;
using System.Configuration;
using System.Web.Security;
using FinanceWeb.Security;
using Newtonsoft.Json;


namespace FinanceWeb.Controllers
{
    
    public class SyncTableAdminController : Controller
    {
        //
        // GET: /SyncTableAdmin/
        private IndividualSyncModel _syncModel;
        private CompanySyncModel _compSyncModel;


         [Authenticate()]
        public ActionResult Index()
        {
            
            Session["ConnectionString"] = ConfigurationManager.ConnectionStrings["Onyx"].ConnectionString;
            Session["AuditConnString"] = ConfigurationManager.ConnectionStrings["auditEntities"].ConnectionString;

             
            try
            {
                using (this._syncModel = new IndividualSyncModel(Session["ConnectionString"].ToString()))
                {
                    ViewData["resultTable"] = this._syncModel.getAllIndividualSyncList();



                    Session["resultTable"] = ViewData["resultTable"];
                }

                using (this._compSyncModel = new CompanySyncModel(Session["ConnectionString"].ToString(), Session["AuditConnString"].ToString()))
                {
                    ViewData["resultTableCompany"] = this._compSyncModel.getAllCompanySyncList();
                    Session["resultTableCompany"] = ViewData["resultTableCompany"];
                }
            }
            catch (Exception exception)
            {
                Tool.sendException(exception);
                Response.Redirect("~/Error/exceptionError");
                return null;
            }
            
           
            return View();
        }

       
        [HttpPost, SessionExpireFilter]
        public PartialViewResult submitData(indivSync indData, AuditDebtorSync auditData)
        {
            auditData.updateDate = DateTime.Now;
            auditData.modifiedBy = Environment.UserDomainName + @"\" + Environment.UserName;

            try
            {
                using (this._syncModel = new IndividualSyncModel(Session["ConnectionString"].ToString(), Session["AuditConnString"].ToString()))
                {
                    if (!this._syncModel.checkIndividualId(indData.individual_id))
                    {
                        Response.Redirect("~/Error/returnJsonIdNotFound");
                        return null;
                    }

                    ViewData["resultTable"] = this._syncModel.addData(indData, (DataTable)Session["resultTable"], auditData);
                    Session["resultTable"] = ViewData["resultTable"];
                }
            }
            catch (Exception exception)
            { 
                Tool.sendException(exception);
                Response.Redirect(Url.Content("~/Error/returnJsonExceptionError"));
                return null;
            }

            return PartialView("../DataList", ViewData["resultTable"]);
        }

        [HttpPost, SessionExpireFilter]
        public PartialViewResult submitCompanyData(indivSync indData, AuditDebtorSync auditData)
        {
            auditData.updateDate = DateTime.Now;
            auditData.modifiedBy = Environment.UserDomainName + @"\" + Environment.UserName;

            try
            {
                using (this._compSyncModel = new CompanySyncModel(Session["ConnectionString"].ToString(), Session["AuditConnString"].ToString()))
                {
                    if (!this._compSyncModel.checkCompanyId(indData.individual_id))
                    {
                        Response.Redirect("~/Error/returnJsonIdNotFound");
                        return null;
                    }

                    ViewData["resultTableCompany"] = this._compSyncModel.addData(indData, (DataTable)Session["resultTableCompany"], auditData);
                    Session["resultTableCompany"] = ViewData["resultTableCompany"];
                }
            }
            catch (Exception exception)
            {
                Tool.sendException(exception);
                Response.Redirect("~/Error/returnJsonExceptionError");
                return null;
            }

            return PartialView("../DataList", ViewData["resultTableCompany"]);

        }

        public PartialViewResult getCompanySyncList()
        {
            try
            {
                using (this._compSyncModel = new CompanySyncModel(Session["ConnectionString"].ToString(), Session["AuditConnString"].ToString()))
                {
                    ViewData["resultTableCompany"] = this._compSyncModel.getAllCompanySyncList();

                }
            }
            catch (Exception exception)
            {
                Tool.sendException(exception);
                Response.Redirect("~/Error/returnJsonExceptionError");
                return null;
            }

            return PartialView("../DataList", ViewData["resultTableCompany"]);
        }

        public PartialViewResult getIndividualSyncList()
        {
            try
            {
                using (this._syncModel = new IndividualSyncModel(Session["ConnectionString"].ToString()))
                {
                    ViewData["resultTable"] = this._syncModel.getAllIndividualSyncList();
                    // Session["resultTable"] = ViewData["resultTable"];
                }
            }
            catch (Exception exception)
            {
                Tool.sendException(exception);
                Response.Redirect("~/Error/returnJsonExceptionError");
                return null;
            }

            return PartialView("../DataList", ViewData["resultTable"]);
        }

       
       
    }
}
