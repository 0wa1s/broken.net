using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenRedirect.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Redirect()
        {
            ViewBag.Message = "Redirect Test Page";

            string Page = Request.Params["Page"];
            // Bad bad bad practice !! Use Url.IsLocalUrl
            if (Page!=null)
            return Redirect(Page);

            // Simple Fix !!
            /*
             if(Url.IsLocalUrl(Page))
             return Redirect(Page);
             */

            return View();
        }

        public ActionResult Redirected()
        {
            ViewBag.Message = "Redirected Test Page";

            return View();
        }


    }
}
 
 