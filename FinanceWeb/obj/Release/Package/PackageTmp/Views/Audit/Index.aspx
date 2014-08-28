<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Finance Integration Portal</title>
    <link type="text/css" href="<%=Url.Content("~/theme/jquery-ui-1.8.20.custom.css") %>" rel="Stylesheet" />
    <link type="text/css" href="<%=Url.Content("~/Content/Site.css") %>" rel="Stylesheet" />
    <link type="text/css" href="<%=Url.Content("~/Content/div.css") %>" rel="Stylesheet" />
    <link type="text/css" href="<%=Url.Content("~/DataTables/css/jquery.dataTables.css") %>" rel="Stylesheet" />
    <link type="text/css" href="<%=Url.Content("~/DataTables/css/jquery.dataTables_themeroller.css") %>" rel="Stylesheet" />
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js") %>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-ui-1.8.20.custom.min.js") %>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery.dataTables.min.js") %>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/ajaxUtil.js") %>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/Dictionary.js") %>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/Audit.js") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            initPage();

            $("input[type=radio]").click(function () {
                window.location = $(this).attr("value");
            });

            $("#btnSearch, #btnSearchTrans").click(function () {
                if ($(this).attr("id") == "btnSearch") {
                    sendRequest("debtor",
                                $("#selDebtorAudit").val(),
                                $("#txtInputFrom").val(),
                                $("#txtInputTo").val())

                }
                else {

                    sendRequest("trans",
                                $("#selTransAudit").val(),
                                $("#txtInputFromTrans").val(),
                                $("#txtInputToTrans").val())
                }

            });

        });
    </script>
</head>
<body>
   <div id="mainBody" class="main">
          <% Html.RenderPartial("../Header"); %>
          <% Html.RenderPartial("../leftNav"); %>
      <div id="syncTabs" class="syncTabs">
            <ul>
                <li><a href="#TransSync">Trans Sync</a></li>
                <li><a href="#DebtorSync">Debtor Sync</a></li>
            </ul>
            <div id="TransSync">
            
                    <%Html.RenderPartial("AuditView/AuditTrans"); %>
           
            </div>
            <div id="DebtorSync">
                     <%Html.RenderPartial("AuditView/AuditDebtor"); %>
            </div>
      </div>
       <%Html.RenderPartial("../alertBox"); %>
   </div>
</body>
</html>
