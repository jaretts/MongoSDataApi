using System;
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
        public static string HTTP_VERB_POST = "POST";
        public static string HTTP_VERB_PUT = "PUT";
        public static string HTTP_VERB_GET = "GET";
        public static string HTTP_VERB_DELETE = "DELETE";

        public static HttpMethodConstraint HTTP_METHOD_CRUD = new HttpMethodConstraint(new string[] { HTTP_VERB_POST, HTTP_VERB_GET, HTTP_VERB_PUT, HTTP_VERB_DELETE });
        public static HttpMethodConstraint HTTP_METHOD_POST = new HttpMethodConstraint(new string[] { HTTP_VERB_POST });
        public static HttpMethodConstraint HTTP_METHOD_GET = new HttpMethodConstraint(new string[] { HTTP_VERB_GET });
        public static HttpMethodConstraint HTTP_METHOD_PUT = new HttpMethodConstraint(new string[] { HTTP_VERB_PUT });
        public static HttpMethodConstraint HTTP_METHOD_DELETE = new HttpMethodConstraint(new string[] { HTTP_VERB_DELETE });

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(name: "SDataSingleResourceKind",
                 routeTemplate: "sdata/-/-/{dataset}/{controller}('{selector}')/{query}",
                 defaults: new
                 {
                     query = RouteParameter.Optional,
                     action = "SDataSingleResourceKind"
                 });

            routes.MapHttpRoute(name: "SDataTemplate",
               routeTemplate: "sdata/-/-/{dataset}/{controller}/$template",
               defaults: new
               {
                   action = "GetTemplate",
                   httpMethod = HTTP_METHOD_GET
               }
              );
            
            routes.MapHttpRoute(name: "SDataCollection",
                 routeTemplate: "sdata/-/-/{dataset}/{controller}/{query}",
                 defaults: new 
                 {
                     query = RouteParameter.Optional,
                     action = "SDataCollection"
                 });

             routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}