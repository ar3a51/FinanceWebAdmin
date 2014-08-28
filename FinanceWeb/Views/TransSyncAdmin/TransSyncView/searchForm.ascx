<%@ Import Namespace="FinanceWeb.Models.pageVariables" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PageVar>" %>
<div id="searchForm" class="divForm">
  Select Status:  <select id="<%=Model.btnSelectId %>">
       <!-- <option value="NEW">NEW</option>
        <option value="IMPORTED">IMPORTED</option>
        <option value="HOLD">HOLD</option>
        <option selected="selected" value="ERROR">ERROR</option>
        <option value="READY">READY</option>
        <option value="INPROGRESS">INPROGRESS</option>
        <option value="COMPLETE">COMPLETE</option>
        <option value="DOCERROR">DOCERROR</option>
        <option value="APPERROR">APPERROR</option>-->
        <%foreach (SelectView data in Model.selectViews)
          {%>
          <option value="<%=data.key %>"><%=data.value %></option>
        <%} %>
    </select><br />
    
    <table>
        <tr>
            <th>Date Range:</th>
        </tr>
        <tr>
            <td>From:</td>
            <td><input type="text" id="<%=Model.inputFromId %>" /></td>
            <td>To:</td>
            <td><input type="text" id="<%=Model.inputToId %>" /></td>
        </tr>
        <tr>
            <td><input type="button" id="<%=Model.btnSubmitId %>" value="Search" style="position:relative; margin-left:auto; margin-right:auto;" /></td>
            <td> <input type="button" id="<%=Model.btnResetId %>" value="Reset" style="position:relative; margin-left:auto; margin-right:auto;" /></td>
        </tr>
    </table>
    
     
    
       

</div>
