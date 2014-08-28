<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="FinanceWeb.Models.pageVariables" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% ViewData["tablename"] = "bizStagingList"; %>
<% Html.RenderPartial("TransSyncView/searchForm", new PageVar { 
    btnResetId = "btnBizTransSubmit",
    btnSubmitId = "btnBizSearch",
    btnSelectId = "selBizTransStatus",
    inputFromId = "dtInputFromDate",
    inputToId = "dtInputToDate",
    selectViews = new SelectView[] {
                       new SelectView{ key="ERROR", value= "ERROR"},
                       new SelectView{ key="IMPORTED", value="IMPORTED"},
                      new SelectView{ key="INPROGRESS", value= "IN PROGRESS"},
                       new SelectView{ key="NEW", value= "NEW"},
                       new SelectView{ key="HOLD", value= "HOLD"},
                       new SelectView{ key="READY", value= "READY"}
                       
                     }
   }); %>
<div id="divTransInCA" class="divSyncTableResult">
        <% Html.RenderPartial("../DataListTransaction", new DataTable()); %>
</div>
<div id="divForm" class="divForm">
    
</div>