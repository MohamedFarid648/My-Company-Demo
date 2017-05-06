using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCDemo1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            /*
             To Solve error:
             * The model backing the 'Context Class' context has changed since the database was created. 
             * Consider using Code First Migrations to update the database
         
             */
            Database.SetInitializer<MVCDemo1.Models.MyContextClass>(null);
            /*End*/

            /*Add engine*/
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());


            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
