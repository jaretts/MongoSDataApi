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
using MongoDB.Bson;
using MongoDB.Driver;


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
            myConventions.SetIgnoreExtraElementsConvention(new AlwaysIgnoreExtraElementsConvention());
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
            unity.RegisterType<SalesDocumentController>();

            unity.RegisterType<IRepository<SalesDocument>, MongoRepository<SalesDocument>>(
                                new HierarchicalLifetimeManager(),
                                new InjectionConstructor("salesDocument"));
       
            //DraftSalesDocument controller
            unity.RegisterType<draftSalesDocumentController>();
            Dictionary<String, String> tmpQueryDraft = new Dictionary<string, string>();
            tmpQueryDraft.Add("Status", "D");

            unity.RegisterType<IRepository<draftSalesDocument>, MongoRepository<draftSalesDocument>>(
                                new HierarchicalLifetimeManager(),
                                new InjectionConstructor("salesDocument", tmpQueryDraft));
           
            //QuoteRequest controller
            unity.RegisterType<quoteRequestController>();
            Dictionary<String, String> tmpQueryQR = new Dictionary<string, string>();
            tmpQueryQR.Add("Status", "R");

        

            unity.RegisterType<IRepository<quoteRequest>, MongoRepository<quoteRequest>>(
                                new HierarchicalLifetimeManager(),
                                new InjectionConstructor("salesDocument", tmpQueryQR));

            //PendingQuote controller
            unity.RegisterType<pendingQuoteController>();
            Dictionary<String, String> tmpQueryPQ = new Dictionary<string, string>();
            tmpQueryPQ.Add("Status", "P");

            unity.RegisterType<IRepository<pendingQuote>, MongoRepository<pendingQuote>>(
                                new HierarchicalLifetimeManager(),
                                new InjectionConstructor("salesDocument", tmpQueryPQ));


            //SalesQuote controller
            unity.RegisterType<salesQuoteController>();
            Dictionary<String, String> tmpQuerySQ = new Dictionary<string, string>();
            tmpQuerySQ.Add("Status", "Q");

            unity.RegisterType<IRepository<salesQuote>, MongoRepository<salesQuote>>(
                                new HierarchicalLifetimeManager(),
                                new InjectionConstructor("salesDocument", tmpQuerySQ));
            
            
            GlobalConfiguration.Configuration.DependencyResolver = new IoCContainer(unity);
        }

    }


}