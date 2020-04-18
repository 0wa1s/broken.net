using SQLInjection.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SQLInjection.Models.Search;

namespace SQLInjection.Controllers
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

        public ActionResult ViewProduct()
        {
            return View("ViewProduct", new SearchByID());
        }

        [HttpPost]
        public ActionResult ViewProduct(SearchByID model)
        {
            if (ModelState.IsValid)
            {
                model.ErrorMessage = null;
                model.Details = null;

                try
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        // unsafe example
                        cmd.CommandText = @"SELECT Id, ProdName, Cost from Products " + "WHERE Id = " + model.Id;
                        /*
                        // safe example
                        cmd.CommandText = @"SELECT Id, ProdName, Cost from Products " + "WHERE Id = @id";
                        SqlParameter parm = new SqlParameter("@Id", SqlDbType.Int);
                        parm.Value = model.Id;
                        cmd.Parameters.Add(parm);
                        */

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            model.Details = new Details
                            {
                                Id = reader.GetInt32(0),
                                ProdName = reader.GetString(1),
                                Cost = reader.GetString(2),
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
            }
            return View("ViewProduct", model);
        }



    }
}