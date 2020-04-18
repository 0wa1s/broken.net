using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


namespace OS_CommandInjection.Controllers
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
        public ActionResult Ping()
        {
            return View();
        }
        public string Exec(FormCollection form)
        {
            string ip = Request.Form["ip"];

            // fix
            //string whitelist = "[^a-zA-Z0-9.-]";
            //ip = Regex.Replace(ip, whitelist, string.Empty, RegexOptions.IgnoreCase);

            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = "C:\\Windows\\System32\\cmd.exe";
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.Arguments = "/c ping " + ip;
            Process p = Process.Start(process);
            string strOutput = p.StandardOutput.ReadToEnd();

            ViewBag.Message = strOutput;

            return strOutput;


        }

    }
}