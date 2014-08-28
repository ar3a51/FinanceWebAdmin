using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Web.Mvc;

namespace FinanceWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "AuditTrans",
                "Audit/Trans/{entityType}/{fromDate}/{toDate}",
                new { Controller = "Audit", action = "getTransAuditList",
                      entityType = UrlParameter.Optional,
                      fromDate = UrlParameter.Optional,
                      toDate = UrlParameter.Optional});

            routes.MapRoute(
              "AuditDebtor",
              "Audit/Debtor/{entityType}/{fromDate}/{toDate}",
              new
              {
                  Controller = "Audit",
                  action = "getDebtorAuditList",
                  entityType = UrlParameter.Optional,
                  fromDate = UrlParameter.Optional,
                  toDate = UrlParameter.Optional
              });


            routes.MapRoute(
                    "Submit",
                    "Trans/submit/{datas}/{transStatus}/{fromDate}/{toDate}/{type}/{auditData}",
                    new { Controller = "TransSyncAdmin", action = "submitchange", 
                            datas = UrlParameter.Optional, 
                            transStatus = UrlParameter.Optional, 
                            fromDate = UrlParameter.Optional,
                            toDate = UrlParameter.Optional,
                            type = UrlParameter.Optional,
                            auditData = UrlParameter.Optional }
                );

            /*routes.MapRoute(
                "Search",
                "Trans/search/{transStatus}/{type}/{fromDate}/{toDate}",
                new { Controller = "TransSyncAdmin", action = "getSearchResult", 
                      transStatus = UrlParameter.Optional, 
                      type= UrlParameter.Optional,
                      fromDate = UrlParameter.Optional,
                      toDate = UrlParameter.Optional
                }
            );*/

            routes.MapRoute(
               "Search",
               "Trans/search/",
               new { Controller = "TransSyncAdmin", action = "getSearchResult"}
                    
               
           );

            routes.MapRoute(
                "Details",
                "Trans/Details/",
                new { Controller = "TransSyncAdmin", action = "getTransDetails"}
            );

            routes.MapRoute(
              "Trans", // Route name
              "Trans/{action}/{id}", // URL with parameters
              new { controller = "TransSyncAdmin", action = "Index", id = UrlParameter.Optional } // Parameter defaults
          );

            routes.MapRoute(
               "sync", // Route name
               "sync/{action}/{id}", // URL with parameters
               new { controller = "SyncTableAdmin", action = "Index", id = UrlParameter.Optional } // Parameter defaults
           );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "TransSyncAdmin", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
        }
    }
}