using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FinalProject.LuceneSearch;

namespace FinalProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ApplicationDbContext db = new ApplicationDbContext();
            PostcardSearcher.ClearIndex();
            PostcardSearcher.AddUpdateLuceneIndex(db.Postcards);
            HashTagSearcher.ClearIndex();
            HashTagSearcher.AddUpdateLuceneIndex(db.HashTags);
        }
    }
}
