<%@ Import Namespace="System.Web.Mvc" %>
<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Forbidden</title>
     <link type="text/css" href="~/theme/jquery-ui-1.8.20.custom.css" rel="Stylesheet" />
    <link type="text/css" href="~/Content/Site.css" rel="Stylesheet" />
    <link type="text/css" href="~/Content/div.css" rel="Stylesheet" />
     <link type="text/css" href="~/DataTables/css/jquery.dataTables.css" rel="Stylesheet" />
    <link type="text/css" href="~/DataTables/css/jquery.dataTables_themeroller.css" rel="Stylesheet" />
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js") %>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-ui-1.8.20.custom.min.js")%>"></script>
     <script type="text/javascript">
         $(document).ready(function () {
             
             $("#radios").buttonset();

             $("input[type=radio]").click(function () {
                 window.location = $(this).attr("value");
             });
         });
     
     </script>
</head>
<body>
     <div id="mainBody" class="main">
          <% Html.RenderPartial("../Header"); %>
          <% Html.RenderPartial("../leftNav"); %>
      <div id="syncTabs" class="syncTabs">
           <h1>You are not authorized to use this system.  Please contact your manager.</h1>
      </div>
      
   </div>
</body>
</html>
