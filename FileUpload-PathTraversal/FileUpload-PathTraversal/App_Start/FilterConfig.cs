﻿using System.Web;
using System.Web.Mvc;

namespace FileUpload_PathTraversal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
