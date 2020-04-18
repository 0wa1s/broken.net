using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileUpload_PathTraversal.Controllers
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

        // File upload isssue
        public ActionResult Upload(HttpPostedFileBase file)
        {


            // file upload with no validation at all

            try
            {
                if (file.ContentLength > 0)
                {
                    // string _FileName = Path.GetFileName(file.FileName);
                    string _FileName = (file.FileName);
                    // string _path = Path.Combine(Server.MapPath("~/Documents"), _FileName);
                    string _path = base.Server.MapPath("/Documents/" + _FileName);
                    //string _path = Path.Combine(Server.MapPath("~/Documents"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "File uploaded successfully to following directory: /Documents";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed";
                return View();
            }

            /*/
            // file upload that forbids only .aspx file this check will fail if extention is .aspX
            /*
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    if (_FileName.Contains(".aspx"))
                    {
                        ViewBag.Message = "File upload failed";
                        return View();
                    }
                    string _path = Path.Combine(Server.MapPath("~/Documents"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "File uploaded successfully to following directory: /Documents";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed";
                return View();
            }

            */
            // black list approach block any aspx asp or cer file extension
            // file upload that converts filename to lowercase then check for .aspx / .asp / .cer file extensions
            /*
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    if ((_FileName.ToLower()).Contains(".aspx") || (_FileName.ToLower()).Contains(".asp") || (_FileName.ToLower()).Contains(".cer"))
                    {
                        ViewBag.Message = "File upload failed";
                        return View();
                    }
                    string _path = Path.Combine(Server.MapPath("~/Documents"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "File uploaded Successfully to following directory: /Documents";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed";
                return View();
            }
            
            /*
            // file upload that converts filename to lowercase then check for .pdf extension and mime type as well also limit file size to 5MB

            try
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _FileExt = Path.GetExtension(file.FileName);
                string _MimeType = MimeMapping.GetMimeMapping(file.FileName);


                if (file.ContentLength > 0 && file.ContentLength<= 5242880)
                {
                    // check for file ext || check for mime type || check for file header or magic number
                    if (_FileExt != ".pdf" || _MimeType != "application/pdf")
                    {
                        ViewBag.Message = "File upload failed\r\nFile Type: " + Path.GetExtension(_FileName) + "\r\nMime Type: " + _MimeType;
                        return View();
                    }
                    string _path = Path.Combine(Server.MapPath("~/Documents"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "File uploaded to following directory: /Documents\r\nFile Type: " + Path.GetExtension(file.FileName) + "\r\n Mime Type: " + _MimeType;
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed";
                return View();
            }
            
            // ========== FIX ============= 

            /*
            
                try
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _FileExt = Path.GetExtension(file.FileName);
                    string _MimeType = MimeMapping.GetMimeMapping(file.FileName);
                    string _SaveName = "";

                    if (_MimeType=="application/pdf" && file.ContentLength > 0 && file.ContentLength<= 5242880)
                    {
                        _SaveName = Guid.NewGuid().ToString() + ".pdf";
                        string path = Path.Combine(Server.MapPath("~/Documents"), _SaveName);
                        file.SaveAs(path);
                        ViewBag.Message = "File uploaded successfully FileType: " + _MimeType;
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Sorry file type not allowed:";
                        return View();
                    }
                    
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    return View();
                }
           
            */





        }

        public ActionResult ListDocuments()
        {

            // Get the file names from the persistent store

            DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~/Documents"));
            var files = directory.GetFiles().ToList();
            //return files;
            return View(files);
        }

        // Path traversal
        // exploit => xyz.pdf/../../web.config
        //exploit => 1549428663.jpg/.....///.....///web.config
        public ActionResult DownloadDocument(string document)
        {
            string _document = null;
            _document = document;
            /*
            // incorrect approach to strip ./
            if (_document.Contains("./"))
            {
                _document = _document.Replace("./", "");
            }
            // incorrect approach to strip ../
            if (_document.Contains("../"))
            {
                _document = _document.Replace("../", "");
            }
            // ===========FIX==========
            //  correct approach check for file name and file path using Path.GetFileName and see if file name is equal to Path.GetFileName
            if (string.IsNullOrEmpty(document) || Path.GetFileName(document) != document)
            {
                return null;
            }
            */
            string documentPath = Path.Combine(Server.MapPath("~/Documents"), _document);
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~/Documents"));
            var files = directory.GetFiles().ToList();
            var loc = directory.ToString();

            if (!System.IO.File.Exists(documentPath))
            {
                return null;
            }

            return File(documentPath, "application/pdf", _document);
        }



    }
}