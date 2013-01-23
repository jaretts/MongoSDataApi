using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Mobile.Models;

namespace SimpleSDataApiDemo.Controllers
{
    public class SalesDocumentController : DefaultController<SalesDocument>
    {
        public SalesDocumentController(IRepository<SalesDocument> repo) : base(repo) {  }
    }

    //swm not digging that I also need to add a new POCO
    // establish controller for state transition API's
    public class draftSalesDocumentController : DefaultController<draftSalesDocument>
    {
        public draftSalesDocumentController(IRepository<draftSalesDocument> repo) : base(repo) {  }
    }

    public class quoteRequestController : DefaultController<quoteRequest>
    {
        public quoteRequestController(IRepository<quoteRequest> repo) : base(repo) { }
    }

    public class pendingQuoteController : DefaultController<pendingQuote>
    {
        public pendingQuoteController(IRepository<pendingQuote> repo) : base(repo) { }
    }

    public class salesQuoteController : DefaultController<salesQuote>
    {
        public salesQuoteController(IRepository<salesQuote> repo) : base(repo) { }
    }
}