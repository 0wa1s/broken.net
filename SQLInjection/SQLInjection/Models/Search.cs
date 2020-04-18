using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SQLInjection.Models
{
        public class Search
        {
        public string ErrorMessage { get; set; }
        }

        public class SearchByID : Search
        {

        public string Id { get; set; }
        public Details Details { get; set; }

        }
        public class Details
        {
            [Display(Name = "Id")]
            public int Id { get; set; }

            [Display(Name = "ProdName")]
            public string ProdName { get; set; }

            [Display(Name = "Cost")]
            public string Cost { get; set; }


        }

}