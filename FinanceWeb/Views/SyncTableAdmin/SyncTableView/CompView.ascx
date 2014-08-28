<%@ Import Namespace="System.Data" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="divFormCompany" class="divForm">
   <input type="text" id="txtCompanyId" value="Company ID" />
   <select id="slctInsertUpdateCompany">
        <option selected="selected" value="I">Insert</option>
        <option value="U">Update</option>
   </select><br />
   <input type="submit" id="btnSubmitCompany" value="Submit" style="margin-top:5px;" />
</div>

<div id="divSyncTableCompany" class="divSyncTableResult">
        <% Html.RenderPartial("../DataList", (DataTable)ViewData["resultTableCompany"]); %>
</div>



