<%@ Import Namespace="System.Data" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="frmSearchTrans" class="divForm">
    <table>
    <tr>
        <td>
            Select Entity:
        </td>
        <td>
            <select id="selTransAudit">
                <option value="inca">InCA</option>
                <option value="topclass">Top Class</option>
            </select>
        </td>
    </tr>
    <tr>
        <th>
            Date Range:
        </th>            
    </tr>
    <tr>
        <td>
            From:
        </td>
        <td>
            <input type="text" id="txtInputFromTrans" />
        </td>
        <td>
            To:
        </td>
        <td>
            <input type="text" id="txtInputToTrans" />
        </td>
    
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>
            <input type="button" id="btnSearchTrans" value="Search" />
        </td>
        <td>&nbsp;</td>
    </tr>
    
    </table>
     
</div>
<div id="divAuditTrans" class="divSyncTableResult">
        <% Html.RenderPartial("AuditListTable", new DataTable()); %>
</div>

