using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebHeaders
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Headers.Remove("X-Powered-By");
           // HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
          //  HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
           // HttpContext.Current.Response.Headers.Remove("Server");
            // HttpContext.Current.Response.Headers.Remove("X - SourceFiles");
          //  HttpContext.Current.Response.AddHeader("Foo", "Bar");

        }
    }
}
