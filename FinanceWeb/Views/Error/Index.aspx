<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Error</title>
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
