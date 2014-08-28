<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div  class="leftNav">
        <!--<ul>
            <li></li>
            <li></li>
        </ul>-->
        <div id="radios">
            <input type ="radio" id="btnTrans" name="btnTrans" value="<%=Url.Content("~/Trans") %>" /><label for="btnTrans">Trans Sync</label>
            <input type ="radio" id="btnDebtor" name="btnDebtor" value="<%=Url.Content("~/Sync") %>" /><label for="btnDebtor">Debtor Sync</label>
            <input type ="radio" id="btnAudit" name="btnAudit" value="<%=Url.Content("~/Audit") %>" /><label for="btnAudit">Audit</label>
            
        </div>
  </div>
