<%@ Import Namespace="System.Data" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DataTable>" %>
<table style="width:100%;" border="1" cellpadding="0" cellspacing="0" class="tblDatalist">
    <thead>
        <tr>
            <th>Audit ID</th>
            <th>Doc Ref ID</th>
            <th>Entity/System</th>
            <th>Act</th>
            <th>Reason</th>
            <th>Modified By</th>
            <th>Update Date</th>
          
        </tr>
    </thead>
    <tbody>
    <%foreach (DataRow row in Model.Rows)
      {%>
      <tr>
        <td style="width:10px;">
            <%=row[0].ToString() %>
        </td>
        <td style="width:10px;">
            <%=row[1].ToString() %>
        </td>
        <td>
            <%=row[2].ToString() %>
        </td>
        <td style="width:5px;">
            <%=row[3].ToString() %>
        </td>
        <td style="width:60px;">
            <%=row[4].ToString() %>
        </td>
        <td>
            <%=row[5].ToString() %>
        </td>
        <td>
            <%=row[6].ToString() %>
        </td>
      </tr>
    <%} %>
    </tbody>
</table>
