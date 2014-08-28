<%@ Import Namespace="System.Data" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="frmSearchDebtor" class="divForm">
    <table>
    <tr>
        <td>
            Select Entity:
        </td>
        <td>
            <select id="selDebtorAudit">
                <option value="company">Company</option>
                <option value="individual">Individual</option>
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
            <input type="text" id="txtInputFrom" />
        </td>
        <td>
            To:
        </td>
        <td>
            <input type="text" id="txtInputTo" />
        </td>
    
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>
            <input type="button" id="btnSearch" value="Search"/>
        </td>
        <td>&nbsp;</td>
    </tr>
    
    </table>
     
</div>
<div id="divAuditDebtor" class="divSyncTableResult">
        <% Html.RenderPartial("AuditListTable", new DataTable()); %>
</div>
