<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="FinanceWeb.Models.pageVariables" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% ViewData["tablename"] = "topclasslist"; %>
<% Html.RenderPartial("TransSyncView/searchForm", new PageVar
   {
       btnSubmitId = "btnTopClassSubmit",
       btnResetId = "btnTopClassReset",
       btnSelectId = "selTopClassStatus",
       inputFromId = "dtInputFromDate2",
       inputToId = "dtInputToDate2",
       selectViews =new SelectView[] {
                      new SelectView{ key="ERROR", value= "ERROR"},
                       new SelectView{ key="DOCERROR", value= "DOCERROR"},
                       new SelectView{ key="APPERROR", value= "APPERROR"},
                      new SelectView{ key="COMPLETE", value="COMPLETE"},
                      new SelectView{ key="IN PROGRESS", value= "IN PROGRESS"},
                       new SelectView{ key="NEW", value= "NEW"},
                       new SelectView{ key="HOLD", value= "HOLD"},
                        new SelectView{ key="READY", value= "READY"}
                       
                     }
   }); %>
<div id="divTransTopClass" class="divSyncTableResult">
        <% Html.RenderPartial("../DataListTransaction", new DataTable()); %>
</div>
