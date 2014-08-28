using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Security.Principal;
using System.Configuration;



namespace FinanceWeb.Security
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,Inherited=true,AllowMultiple=true)]
    public class Authenticate:AuthorizeAttribute
    {
        private String writePermission;
        private String readOnlyPermission;
        public Authenticate()
        {
            writePermission = ConfigurationManager.AppSettings["RWPermission"].ToString();
            readOnlyPermission = ConfigurationManager.AppSettings["ROPermission"].ToString();
        }
       protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Error = "You are not authorized to make any changes."
                    }
                   
                };
            }
            else
                filterContext.Result = new RedirectResult("~/Error/UnauthorizedEntry/");

            //base.HandleUnauthorizedRequest(filterContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            
            //filterContext.HttpContext.Request.
            Boolean isGivenAccess = true;

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                isGivenAccess = false;

            if (filterContext.HttpContext.Request.CurrentExecutionFilePath.Equals(UrlHelper.GenerateContentUrl("~/trans/submit", filterContext.HttpContext)))
            {
                if (!filterContext.HttpContext.User.IsInRole(this.writePermission))
                   
                    isGivenAccess = false;
              
            }
            else {
                if (!filterContext.HttpContext.User.IsInRole(this.readOnlyPermission))
                    isGivenAccess = false;
            }

            if (isGivenAccess)
            {
                SetCachePolicy(filterContext);
                base.OnAuthorization(filterContext);
            }
            else
                HandleUnauthorizedRequest(filterContext);
        }

        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext.User.IsInRole(this.Roles))
               return HttpValidationStatus.Invalid;
            else
            {
                return HttpValidationStatus.Valid;
            }
               // base.OnAuthorization(filterContext);
            //return base.OnCacheAuthorization(httpContext);
        }

        protected void SetCachePolicy(AuthorizationContext filterContext)
        {
            // ** IMPORTANT ** 
            // Since we're performing authorization at the action level, the authorization code runs 
            // after the output caching module. In the worst case this could allow an authorized user 
            // to cause the page to be cached, then an unauthorized user would later be served the 
            // cached page. We work around this by telling proxies not to cache the sensitive page, 
            // then we hook our custom authorization code into the caching mechanism so that we have 
            // the final say on whether a page should be served from the cache. 
            HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
            cachePolicy.SetProxyMaxAge(new TimeSpan(0));
            cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        } 


      

    }
}