using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Castalia.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,
                 "",
                 new
                 {
                     controller = "Home",
                     action = "SelectionByTopic",
                     sortingParam = (string)null,
                     page = 1
                 }
             );

            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Home", action = "SelectionByTopic", sortingParam = (string)null },
                constraints: new { page = @"\d+" }
            );

            routes.MapRoute(null,
                "{topic}",
                new { controller = "Home", action = "SelectionByTopic", page = 1 }
            );

            routes.MapRoute(null,
                "{topic}/Page{page}",
                new { controller = "Home", action = "SelectionByTopic" },
                new { page = @"\d+" }
            );




            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
