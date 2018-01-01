using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common;

namespace ErrorLogger_Amrit
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Amrit
            routes.MapRoute(
                name: "Application",
                url: "Details/{userId}/{controller}/{action}/{appId}",
                defaults: new
                {
                    controller = WebConstants.APPLICATION_CONTROLLER,
                    action = "Index",
                    assignmentId = UrlParameter.Optional
                }
            );
            // /Amrit


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
