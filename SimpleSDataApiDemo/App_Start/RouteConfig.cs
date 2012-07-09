﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimpleSDataApiDemo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute("SDataTemplate", "sdata/-/-/-/{controller}/$template",
                            new { action = "GetTemplate" },
                            new { httpMethod = new HttpMethodConstraint(new string[] { "GET" }) });

            routes.MapHttpRoute(name: "SDataSingleResourceKindFull",
                 routeTemplate: "sdata/-/-/-/{controller}('{selector}')/{query}",
                 defaults: new { selector = RouteParameter.Optional,
                                 query = RouteParameter.Optional });

            routes.MapHttpRoute(name: "SDataCollectionFull",
                routeTemplate: "sdata/-/-/-/{controller}/{query}",
                defaults: new { action = "GetCollection", query = RouteParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}