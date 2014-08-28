<%@ Import Namespace="System.Data" %>
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
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/Content.js") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            initPage();


            $("input[type=radio]").click(function () {

                //$("input[type=radio]").attr("checked", "");
                window.location = $(this).attr("value");
                //$(this).attr("checked", "checked");


            });



            $("input[type=submit]").click(function () {

                if ($(this).attr("id") == "btnSubmit")
                    submitPage("individual");
                else
                    submitPage("company");

            });

            $("#txtMemberId, #txtCompanyId").focus(function () {
                //if ($(this).val() == "Member ID") ||
                $(this).val("");
            });

            $("#txtMemberId, #txtCompanyId").blur(function () {
                if ($(this).attr("id") == "txtMemberId") {

                    if ($(this).val() == "")
                        $(this).val("Individual ID");
                }
                else {

                    if ($(this).val() == "")
                        $(this).val("Company ID");
                }
            });

            //alert($("#divSyncTableInd .tblDatalist").html());
            oTable = $(".tblDatalist").dataTable({ "bDestroy": true });
        });
    </script>
</head>
<body>
<div id="mainBody" class="main">
      <% Html.RenderPartial("../Header"); %>
      <% Html.RenderPartial("../leftNav"); %>
      <div id="syncTabsi" class="syncTabs">
            <ul>
                <li><a href="#indSync">Individual Sync</a></li>
                <li><a href="#compSync">Company Sync</a></li>
            </ul>
            <div id="indSync">
            
                    <% Html.RenderPartial("SyncTableView/IndivView"); %>
           
            </div>
            <div id="compSync">
                <% Html.RenderPartial("SyncTableView/CompView"); %>
            </div>
      </div>
      <%Html.RenderPartial("../alertBox"); %>
      <div title="Confirm submission" id="dialogConfirm" class="divConfirmSubmission">
            <textarea id="txtConfirm" rows="10" cols="50" >reason</textarea><br />
      </div>
</div>  
</body>
</html>
