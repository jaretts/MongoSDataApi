using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Sage.SDataHandler;
using Microsoft.Practices.Unity;
using SimpleSDataApiDemo.Controllers;
using Mobile.Models;
using MongoRepository;
using SimpleSDataApiDemo.DependencyResolvers;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization;


namespace SimpleSDataApiDemo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterDependencyResolver();
            GlobalConfiguration.Configuration.MessageHandlers.Add(new SDataHandler());

            // set up Ignore if null for all controllers - should this be in another class so Mongo 
            // stuff doesn't appear in here? - Added using above and also added reference to MongoDB.Bson
            var myConventions = new ConventionProfile();
            myConventions.SetIgnoreIfNullConvention(new AlwaysIgnoreIfNullConvention());
            BsonClassMap.RegisterConventions(myConventions, t => t.FullName.StartsWith("Mobile."));
        }

        private static void RegisterDependencyResolver()
        {
            UnityContainer unity = new UnityContainer();

            // Register the Customer Controller
            unity.RegisterType<CustomerController>();

            // Register the Repository for the CustomerController, need to provide a constructor with MongoDB Collection Name
            unity.RegisterType<IRepository<Customer>, MongoRepository<Customer>>(
                                new HierarchicalLifetimeManager(),
                                new InjectionConstructor("customers"));

            // Register the SalesQuote Controller
            unity.RegisterType<SalesQuoteController>();

            unity.RegisterType<IRepository<SalesQuote>, MongoRepository<SalesQuote>>(
                                new HierarchicalLifetimeManager(),
                                new InjectionConstructor("salesQuote"));

            GlobalConfiguration.Configuration.DependencyResolver = new IoCContainer(unity);
        }

    }


}