using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RazorEngine;
using RazorEngine.Templating;

/*
// The vulnearble code was shamelessly copied from 
// https://clement.notin.org/blog/2020/04/15/Server-Side-Template-Injection-(SSTI)-in-ASP.NET-Razor/
// https://github.com/cnotin/RazorVulnerableApp
*/

namespace SSTI.Controllers
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
        public ActionResult Ssti()
        {
            ViewBag.Message = "SSTI Test Page.";
            
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Ssti(FormCollection collection)
        {
            string name = collection["name"];
            
            /*
             // Input validation
            name = name.Replace("@", "%40");
            name = name.Replace("{", "\\{");
            */
            
            ViewBag.RenderedTemplate = Razor.Parse(name);
            ViewBag.Template = name;
            return View();
        }
        


    }
}