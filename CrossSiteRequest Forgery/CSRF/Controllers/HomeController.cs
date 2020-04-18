using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrossSiteScripting.Controllers
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

        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create

        // remove this to avoid xss attacks by default
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                ViewData["Name"] = collection[1];
                ViewData["City"] = collection[2];
                ViewData["Address"] = collection[3];
                
                return View("ViewProfile");


            }
            catch
            {
                return View();
            }
        }

        public ActionResult ViewProfile()
        {
            return View();
        }



    }
}