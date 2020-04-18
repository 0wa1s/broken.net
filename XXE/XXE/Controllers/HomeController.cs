using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace XXE.Controllers
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

        // Ignore this one
        public ActionResult MyXmlParser(FormCollection form)
        {
            try
            {
                string xml = form["data"].ToString();
                StreamReader stream = new StreamReader(xml);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Parse;
                XmlReader xmlReader = XmlReader.Create(stream, settings);

                return View();
            }


            catch (Exception ex)
            {
                ViewBag.Message = "ERROR:" + ex.Message.ToString();
                return View();
            }

        }


        // XXE Example
        /*
         * <?xml version="1.0" encoding="utf-8"?>
            <!DOCTYPE foo [
            <!ELEMENT foo ANY >
            <!ENTITY xxe SYSTEM "file://c://windows//system32//drivers//etc//hosts" >
            ]>
            <process>
              <userinput>&xxe;</userinput>
            </process>
         * 
        */

        public void doPost()
        {
            string magic_code = "foobar";
           
            string result = "";

            if (Request.RequestType == "POST")
            {
                try
                {

                    StreamReader reader = new StreamReader(Request.InputStream);
                    String xmlData = reader.ReadToEnd();
                    var doc = new XmlDocument();
                    //fix xxe 
                    //doc.XmlResolver = null;

                    // exploit

                    doc.LoadXml(xmlData);
                    XmlElement xRoot = doc.DocumentElement;

                    XmlNode uNode = xRoot.GetElementsByTagName("userinput")[0];
                    

                    string userinput = uNode.InnerText;
                    

                    if (userinput.Equals(magic_code))
                    {
                        result = String.Format("<result><iscorrect>1</iscorrect><msg>You guessed it!</msg></result>");
                    }
                    else
                    {
                        // no data will be displayed but is still vulnerable
                        // try OOB XXE payloads
                        //result = String.Format("<result><iscorrect>0</iscorrect><msg>Sorry! Try again....{0}</msg></result>");

                        // verbose data disclosure example
                        // try normal XXE payloads
                        result = String.Format("<result><iscorrect>0</iscorrect><msg>Sorry! Try again....{0}</msg></result>", userinput);
                    }
                }
                catch (ArgumentException e1)
                {
                    result = String.Format("<result><iscorrect>3</iscorrect><msg>{0}</msg></result>", e1);
                }
                catch (XmlException e2)
                {
                    result = String.Format("<result><iscorrect>3</iscorrect><msg>{0}</msg></result>", e2);
                }
                finally
                {
                    Response.ContentType = "text/xml; charset=utf-8";
                    Response.Write(result);
                }

            }
        }

    }
}