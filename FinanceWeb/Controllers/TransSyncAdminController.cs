using AppDomain;
using AppDomain.paramInput;
using AppDomain.AuditDomain;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using System.Configuration.Assemblies;
using FinanceWeb.Models;
using FinanceWeb.Models.Tools;
using FinanceWeb.Models.response;
using FinanceWeb.Security;
using Newtonsoft.Json;

namespace FinanceWeb.Controllers
{
    
    public class TransSyncAdminController : Controller
    {
        //
        // GET: /TransSyncAdmin/


        [Authenticate()]
        public ActionResult Index()
        {
            Session.Clear();
            Session["ConnectionString"] = ConfigurationManager.ConnectionStrings["Onyx"].ConnectionString;
            Session["auditConnString"] = ConfigurationManager.ConnectionStrings["auditEntities"].ConnectionString;
            Session["TopClass"] = ConfigurationManager.ConnectionStrings["TopClass"].ConnectionString;
         

                   
            return View();
        }

        [HttpGet][SessionExpireFilter]
        public JsonResult getSearchResult(/*String transStatus, String type, DateTime fromDate, DateTime? toDate*/)
        {
           Models.response.Response resp;
            DateTime? toDate;
           String[][] resultSet;
           String sEcho = Request.QueryString["sEcho"].ToString();
           int iDisplayLength = int.Parse(Request.QueryString["iDisplayLength"]);
           int iDisplayStart = int.Parse(Request.QueryString["iDisplayStart"]);
           String transStatus = Request.QueryString["transStatus"].ToString();
           String type = Request.QueryString["type"].ToString();
           DateTime fromDate = DateTime.Parse(Request.QueryString["fromDate"].ToString());
           String keyword = (Request.QueryString["sSearch"].Equals("")) ? null : Request.QueryString["sSearch"].ToString();

            if(Request.QueryString["toDate"].Equals(""))
                toDate = Tool.getDate(fromDate, null);
            else
                toDate = DateTime.Parse(Request.QueryString["toDate"].ToString());

            

           if (iDisplayStart == 0)
               iDisplayStart = 1;
          

           //return Json(new Response { sEcho = int.Parse(sEcho), iTotalRecords = "0", iTotalDisplayRecords = "0", aaData = null }, JsonRequestBehavior.AllowGet);
           
            try
            {
                

                if (type.Equals("inca"))
                
                 resp =  incaGetSearchResult(int.Parse(sEcho),iDisplayStart, iDisplayLength, transStatus, fromDate, toDate,keyword, out resultSet);
                    
                
                else
                    topClassGetSearchResult(int.Parse(sEcho),iDisplayStart, iDisplayLength, transStatus, fromDate, toDate,keyword, out resp);


                return Json(resp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                //String displayName;
                Tool.sendException(exception);

                Response.Redirect(Url.Content("~/Error/returnJsonExceptionError"));
                return null;
               
            }

            //return PartialView("../DataListTransaction", (DataTable)ViewData["resultTableBIZTrans"]);
            
        }

       
        [HttpPost,Authenticate(), SessionExpireFilter]
        public void submitchange(TransInput[] datas, String transStatus, DateTime fromDate, DateTime? toDate,String type, AuditTransSync auditData)
        {
            Models.response.Response resp;
           
            DataTable unParsedTable = (DataTable)Session["resultTableBIZTrans"];
            
            auditData.updateDate = DateTime.Now;
            auditData.modifiedBy = Environment.UserDomainName + @"\" + Environment.UserName;
           

           
            try
            {
                

                if (type.Equals("inca"))    
                    incaSubmitChange(datas, transStatus, fromDate, toDate, type, auditData, out resp);         
                else
                    topClassSubmit(datas, transStatus, fromDate, toDate, type, auditData);

                

                //return Json(resp);
            }
            catch (Exception exception)
            {
                Tool.sendException(exception);
                Response.Redirect(Url.Content("~/Error/returnJsonExceptionError"));
                //return null;
            }

          

          
            //return PartialView("../DataListTransaction", (DataTable)ViewData["resultTableBIZTrans"]);
        }

        private String drawInput(Boolean isAuthorized, String transId)
        {
            if (isAuthorized)
                return "<input type='checkbox' value='" + transId + "' />";
            else
                return "Read only";

        }

        private Models.response.Response incaGetSearchResult(int echo,int startRow, int recordPerPage, String transStatus, DateTime fromDate, DateTime? toDate, String keyword, out String [][] resultset)
        {
            bizSync[] list;
            String[] arrayString;
            int counter = 0;
            int? totalRecords = 0;
            //String[][] resultset;
            Models.response.Response resp;

            try
            {
                using (BizStagingModel stagingModel = new BizStagingModel(Session["ConnectionString"].ToString(), Session["auditConnString"].ToString()))
                {
                    DataTable unParsedTable;
                    resp = new Models.response.Response();
                    
                    list = stagingModel.getAllParsedTrans(startRow, recordPerPage, transStatus, fromDate, Tool.getDate(fromDate, toDate), keyword,out unParsedTable,out totalRecords).ToArray<bizSync>();
                    resultset = new String[list.Count()][];
                    resp.aaData = new String[list.Count()][];
                    foreach (bizSync data in list)
                    {
                        arrayString = new String[] {
                                                "<a class='incarows' href='"+ data.ONYX_iTransId.ToString() +"'>" + data.DOC_ID.ToString() + "</a>", 
                                                data.ONYX_StagingStatus.ToString(),
                                                data.BAT_DOC_TYPE,
                                                data.ONYX_iOwnerId.ToString(),
                                                data.ONYX_dtTransactionDate.ToString("MMM d, yyyy hh:mm tt"),
                                                data.LNE_AMT1.ToString(),
                                                "<input type='checkbox' value='"+ data.ONYX_iTransId.ToString() +"' />"
                                                };

                        //resultset[counter] = arrayString;
                        resp.aaData[counter] = arrayString;
                        counter++;
                    }

                    resp.sEcho = echo;
                    resp.iTotalRecords = totalRecords.ToString();
                    resp.iTotalDisplayRecords = totalRecords.ToString();

                    //respValue = resp;

                    Session["resultTableBIZTrans"] = unParsedTable;
                    //Session["SearchResult"] = resultset;

                    return resp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        private void topClassGetSearchResult(int echo, int iStartRow, int recsPerPage, String transStatus, DateTime fromDate, DateTime? toDate,String keyword, out Models.response.Response respValue)
        {
           
            String[] arrayString;
            int counter = 0;
            Models.response.Response resp;

            int totalRecords = 0;

            try
            {
                using (TopClassStagingModel topClass = new TopClassStagingModel(Session["TopClass"].ToString(), Session["auditConnString"].ToString()))
                {
                    TopClassSync[] returnList;

                    resp = new Models.response.Response();
                    returnList = topClass.getAllTransactions(iStartRow, recsPerPage, transStatus, fromDate, Tool.getDate(fromDate, toDate), keyword,out totalRecords).ToArray<TopClassSync>();
                    resp.aaData = new String[returnList.Count()][];

                    foreach (TopClassSync topclass in returnList)
                    {
                        arrayString = new String[] { 
                                "<a class='rows' href='"+ topclass.guid.ToString() +"'>" + topclass.DocumentId + "</a>",
                                topclass.Status,
                                topclass.documentType,
                                topclass.AccountID,
                                topclass.DocumentDate.ToString("MMM d, yyyy hh:mm tt"),
                                topclass.Amount.ToString(),
                                 "<input type='checkbox' value='" + topclass.guid.ToString() + "' />"
                       };

                        resp.aaData[counter] = arrayString;
                        counter++;
                    }

                    resp.sEcho = echo;
                    resp.iTotalRecords = totalRecords.ToString();
                    resp.iTotalDisplayRecords = totalRecords.ToString();

                    respValue = resp;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void incaSubmitChange(TransInput[] datas, String transStatus, DateTime fromDate, DateTime? toDate, String type, AuditTransSync auditData, out Models.response.Response respValue)
        {

            Models.response.Response resp;
            String[] arrayString;
            bizSync[] list;
            respValue = null;

            try
            {
                DataTable unParsedTable = (DataTable)Session["resultTableBIZTrans"];

                using (BizStagingModel stagingModel = new BizStagingModel(Session["ConnectionString"].ToString(), Session["auditConnString"].ToString()))
                {
                    resp = new Models.response.Response();
                    stagingModel.updateStatus(datas, fromDate, Tool.getDate(fromDate, toDate), transStatus, unParsedTable, out unParsedTable, auditData);
                    //Session["resultTableBIZTrans"] = ViewData["resultTableBIZTrans"];
                    Session["resultTableBIZTrans"] = unParsedTable;
                }

                /*resp.aaData = new String[list.Count()][];
                int counter = 0;

                foreach (bizSync data in list)
                {
                    arrayString = new String[] {data.DOC_ID.ToString(), 
                                            data.ONYX_StagingStatus.ToString(),
                                            data.BAT_DOC_TYPE,
                                            data.ONYX_iOwnerId.ToString(),
                                            data.ONYX_dtTransactionDate.ToString("MMM d, yyyy hh:mm tt"),
                                            data.LNE_AMT1.ToString(),
                                            "<input type='checkbox' value='"+ data.ONYX_iTransId.ToString() +"' />"
                                            };

                    resp.aaData[counter] = arrayString;
                    counter++;
                }

                resp.sEcho = 1;
                resp.iTotalRecords = list.Count().ToString();
                resp.iTotalDisplayRecords = list.Count().ToString();

                respValue = resp;*/
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                resp = null;
            }
        }

        private void topClassSubmit(TransInput[] datas, String transStatus, DateTime fromDate, DateTime? toDate, String type, AuditTransSync auditData/*, out Models.response.Response respValue*/)
        {
            TopClassSync[] listTopClass;
            String[] arrayString;

            Models.response.Response resp;
            resp = new Response();

            try
            {
                using (TopClassStagingModel topclassModel = new TopClassStagingModel(Session["TopClass"].ToString(), Session["auditConnString"].ToString()))
                {
                    topclassModel.updateData(datas, transStatus, fromDate, Tool.getDate(fromDate, toDate), auditData);
                    //topclassModeltopclassModel.updateData(datas, transStatus, fromDate, Tool.getDate(fromDate, toDate), auditData);
                }

               /* resp.aaData = new String[listTopClass.Count()][];
                int counter = 0;
                foreach (TopClassSync data in listTopClass)
                {
                    arrayString = new String[] {data.DocumentId.ToString(), 
                                            data.Status.ToString(),
                                            data.documentType,
                                            data.AccountID.ToString(),
                                            data.DocumentDate.ToString("MMM d, yyyy hh:mm tt"),
                                            data.Amount.ToString(),
                                            "<input type='checkbox' value='"+ data.guid.ToString() +"' />"
                                            };

                    resp.aaData[counter] = arrayString;
                    counter++;
                }

                resp.sEcho = 1;
                resp.iTotalRecords = listTopClass.Count().ToString();
                resp.iTotalDisplayRecords = listTopClass.Count().ToString();

                respValue = resp;*/
            }
            catch (Exception ex){
                
                throw ex;
            }
            finally{

            resp = null;
            }
        }


        [HttpGet]
        public ActionResult getTransDetails()
        {
            String type = Request.QueryString["sType"].ToString();
            int transId = int.Parse(Request.QueryString["transId"].ToString());

            if (!type.Equals("inca"))
            {
                TopClassSync details;
                using (TopClassStagingModel stagingModel = new TopClassStagingModel(Session["TopClass"].ToString(), Session["auditConnString"].ToString()))
                {

                    details = stagingModel.getTransDetail(transId);
                }

                return Json(details, JsonRequestBehavior.AllowGet);
            }
            else
            {
                bizSync details;
                using (BizStagingModel stagingModel = new BizStagingModel(Session["ConnectionString"].ToString(), Session["auditConnString"].ToString()))
                {
                    details = stagingModel.getTransDetails(transId);
                }

                return Json(details,JsonRequestBehavior.AllowGet);
                //return null;
            }
        }

        public ActionResult test()
        {
            Models.response.Response resp;
           

            String[][] resultSet = Session["SearchResult"] as String[][];

           

            int iDisplayLength = int.Parse(Request.QueryString["iDisplayLength"]); 
            int iDisplayStart = int.Parse(Request.QueryString["iDisplayStart"]);
            String sEcho = Request.QueryString["sEcho"];

            if (resultSet == null)
            {
                resp = new Response();
                resp.sEcho = int.Parse(sEcho);
                resp.iTotalDisplayRecords = "0";
                resp.iTotalRecords = "0";
                
                return new JsonResult { Data = resp, ContentType = "Application/json", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            int iTotalRecord = resultSet.Count();
            int iUpperBound = iDisplayStart + iDisplayLength;

            /*int iDisplayLength = 10;
            int iDisplayStart = 0;
            int sEcho = 5;
            int iTotalRecord = 0;
            int iUpperBound = 30;
            int iTotalDisplayRecords = 10;*/
            

            resp = new Response();
            resp.iTotalRecords = iTotalRecord.ToString();
            
            resp.sEcho = int.Parse(sEcho);
            

            int counter = 0;




            if (iUpperBound > iTotalRecord)
            {
                iUpperBound = iTotalRecord;
                iDisplayLength = iUpperBound - iDisplayStart;
            }

            
                

            resp.iTotalDisplayRecords = resultSet.Count().ToString();
            resp.aaData = new String[iDisplayLength][];

            
                //return Json(resp,JsonRequestBehavior.AllowGet); 

            for (int i = iDisplayStart; i < iUpperBound; i++)
            {
                //while (counter < resp.aaData.Count()) 
                //{
                    resp.aaData[counter] = resultSet[i];
                    counter++;
                  //  break;
                    
                //}
                /*if (counter == resp.aaData.Count())
                    break;

                counter++;*/
            }

            //return Json(resp,JsonRequestBehavior.AllowGet);
            return new JsonResult { Data = resp, ContentType = "Application/json", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //return test ;
        }

       

              

    }
}
