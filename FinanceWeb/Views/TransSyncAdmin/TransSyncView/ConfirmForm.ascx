<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="frmConfirm" class="divConfirmSubmission">
   
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>reason: <span id="notify" class="notification"></span></td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td><textarea id="txtConfirm" rows="10" cols="50" ></textarea></td>
        </tr>
   
    </table> <br /><br />
    Reset Status: <select id="selResetStatus"> </select>  
</div>
