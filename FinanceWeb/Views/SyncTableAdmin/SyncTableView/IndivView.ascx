<%@ Import Namespace="System.Data" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="divForm" class="divForm">
   <input type="text" id="txtMemberId" value="Individual ID" />
   <select id="slctInsertUpdate">
        <option selected="selected" value="I">Insert</option>
        <option value="U">Update</option>
   </select><br />
   <input type="submit" id="btnSubmit" value="Submit" style="margin-top:5px;" />
</div>

<div id="divSyncTableInd" class="divSyncTableResult">
        <% Html.RenderPartial("../DataList", (DataTable)ViewData["resultTable"]); %>
</div>
