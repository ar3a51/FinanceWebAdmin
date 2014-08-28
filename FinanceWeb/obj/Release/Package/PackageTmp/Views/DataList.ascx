<%@ Import Namespace="System.Data" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Data.DataTable>" %>
<table border="0" width="100%" class="tblDatalist" cellpadding="0" cellspacing="0">
    <thead>
    <tr>
    <%foreach (DataColumn aColumn in Model.Columns)
      { %>
        <th><%=aColumn.ColumnName.ToString() %></th>
    <%} %>
    </tr>
    </thead>
    <tbody>
    <%foreach (DataRow aRow in Model.Rows)
      {  %>
      <tr>
        
        <%foreach (DataColumn currColumn in Model.Columns)
          { %>
            
              <td><%=aRow[currColumn.ColumnName.ToString()].ToString() %></td>
            
        <%} %>
      </tr>
    <%} %>
    </tbody>
</table>
