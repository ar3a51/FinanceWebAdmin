<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="FinanceWeb.Models.pageVariables" %>
<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Finance Integration Portal</title>
    <link type="text/css" href="<%= Url.Content("~/theme/jquery-ui-1.8.20.custom.css") %>" rel="Stylesheet" />
    <link type="text/css" href="<%= Url.Content("~/Content/Site.css") %>" rel="Stylesheet" />
    <link type="text/css" href="<%= Url.Content("~/Content/div.css") %>" rel="Stylesheet" />
    <link type="text/css" href="<%= Url.Content("~/DataTables/css/jquery.dataTables.css") %>" rel="Stylesheet" />
    <link type="text/css" href="<%= Url.Content("~/DataTables/css/jquery.dataTables_themeroller.css") %>" rel="Stylesheet" />
    <script type="text/javascript">
        var globalSearch = "<%=Url.Content("~/Trans/search") %>";
        var globalDetail = "<%=Url.Content("~/trans/details") %>";
    </script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery-1.7.2.min.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery-ui-1.8.20.custom.min.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.dataTables.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/ajaxUtil.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/Dictionary.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/trans.js") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {


            

            /*test*/
           /* getData($("#selBizTransStatus").val(),
                "01/08/2012",
                "30/08/2012",
                 "#divTransInCA", "inca", ".bizStagingList");*/
            /*end test*/

            initElement();

            $("#selBizTransStatus").change(function () {

                if ($(this).val() == "IMPORTED")
                    $("#btnBizTransSubmit").button("option", "disabled", true);
                else
                    $("#btnBizTransSubmit").button("option", "disabled", false);

            });

            $("#selTopClassStatus").change(function () {

                if ($(this).val() == "COMPLETE")
                    $("#btnTopClassReset").button("option", "disabled", true);
                else
                    $("#btnTopClassReset").button("option", "disabled", false);

            });




            $("#syncTabs").show();

            $("#btnBizSearch").click(function () {


                getData(globalSearch, $("#selBizTransStatus").val(),
                $("#dtInputFromDate").val(),
                $("#dtInputToDate").val(),
                 "#divTransInCA", "inca", ".bizStagingList");

            });

            $("#btnTopClassSubmit").click(function () {
            
                getData(globalSearch,$("#selTopClassStatus").val(),
                 $("#dtInputFromDate2").val(),
                $("#dtInputToDate2").val(),
                "#divTransTopClass", "topclass", ".topclasslist");

            });

            $("#btnBizTransSubmit").click(function () {

                //retrieveData("#divTransInCA Table input:checked");
                ShowDialog("biztalk", "#divTransInCA Table input:checked", $("#selBizTransStatus").val());
            });

            $("#btnTopClassReset").click(function () {

                ShowDialog("topclass", "#divTransTopClass Table input:checked", $("#selTopClassStatus").val());

            });

            

        });
        
    
    </script>
</head>
<body>
<div id="divMain" class="main">
    <% Html.RenderPartial("../Header"); %>
    <% Html.RenderPartial("../leftNav"); %>
    <div id="syncTabs" class="syncTabs">
        <ul>
            <li><a href="#inca">inCA Transaction</a></li>
            <li><a href="#topclass">TopClass Transaction</a></li>
        </ul>
        <div id="inca">
            
                <% Html.RenderPartial("TransSyncView/incaView"); %>
                 
        </div>
        <div id="topclass">
            <% Html.RenderPartial("TransSyncView/topClassView"); %>
        </div>
   </div>
   <%Html.RenderPartial("../alertBox"); %>
   <%Html.RenderPartial("TransSyncView/ConfirmForm"); %>
</div>
</body>
</html>
