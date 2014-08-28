<%@ Import Namespace="System.Data" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DataTable>" %>
<table style="width:100%;" border="1" cellpadding="0" cellspacing="0" class="<%=ViewData["tableName"].ToString() %>">
    <thead>
        <tr>
            <th>Doc Ref ID</th>
            <th>Status</th>
            <th>Document type</th>
            <th>Member ID</th>
            <th>Date Created</th>
            <th>Amount</th>
            <th>Reset</th>
          
        </tr>
    </thead>
    <tbody>
    <%foreach (DataRow row in Model.Rows)
      {%>
      <tr>
        <td>
            <%=row[1].ToString() %>
        </td>
        <td>
            <%=row[2].ToString() %>
        </td>
        <td>
            <%=row[3].ToString() %>
        </td>
        <td>
            <%=row[4].ToString() %>
        </td>
        <td>
            <%=row[5].ToString() %>
        </td>
         <td>
            <%=row[6].ToString() %>
        </td>
        <td>
            <input type="checkbox"  value="<%=row[0].ToString() %> "/>
        </td>
      </tr>
    <%} %>
    </tbody>
</table>
