using Login.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Web.Security;

namespace Login.Controllers
{
    public class UserController : Controller
    {
        //Dashboard
        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }
        //Login 
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = @"SELECT * from Users WHERE EmailId = @EmailId and Password = @Password";

                    SqlParameter EmailId = new SqlParameter("@EmailId", SqlDbType.VarChar, 50);
                    SqlParameter Password = new SqlParameter("@Password", SqlDbType.VarChar, 200);

                    EmailId.Value = login.EmailID;
                    Password.Value = Crypto.SHA256(login.Password);

                    cmd.Parameters.Add(EmailId);
                    cmd.Parameters.Add(Password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        
                        Session.Add("EmailId", reader.GetString(3));

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Dashboard", "User");
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
            }
            catch
            {
                message = "Invalid credential provided";
            }
                     ViewBag.Message = message;
                     return View();
        }

        //Logout
        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear();
            // Fix using same cookies again
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            return RedirectToAction("Login", "User");
        }

        //View Profile
        [HttpGet]
        public ActionResult ViewProfile()
        {
            if (Session["EmailId"] != null)
            {
                return View("ViewProfile", new SearchByID());

            }
            else
                return View("Login");

        }

        //View Profile
        [HttpPost]
        public ActionResult ViewProfile(SearchByID model)
        {
            if (ModelState.IsValid)
            {
                model.ErrorMessage = null;
                model.Details = null;
                /*
//              Fix for IDOR and Broken authentication by checking if the requestor 
                is providing it's own email id or not by comparing it with email id saved in sessions variable
                if (Session["EmailId"].ToString() != model.EmailId.ToString())
                {
                    return View("Error");
                } */               
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = @"SELECT FirstName, LastName, EmailID, Secret from Users " + "WHERE EmailId = @EmailId";
                        SqlParameter parm = new SqlParameter("@EmailId", SqlDbType.VarChar);
                        parm.Value = model.EmailId;
                        cmd.Parameters.Add(parm);
                        SqlDataReader reader = cmd.ExecuteReader();
                  
                        if (reader.Read())
                        {
                            model.Details = new Details
                            {
                                FirstName = reader.GetString(0),
                                LastName = reader.GetString(1),
                                EmailID = reader.GetString(2),
                                Secret = reader.GetString(3)
                            };
                        }
                        else
                        {
                            model.ErrorMessage = "Something went wrong";
                        }
                    }
                }
                catch (Exception e)
                {
                    model.ErrorMessage = e.Message;
                }
                return View("ViewProfile", model);
            }
            return View("ViewProfile", model);
        }


    }
}