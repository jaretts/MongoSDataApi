using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Mobile.Models;

namespace SimpleSDataApiDemo.Controllers
{
    public class CustomerController : DefaultController<Customer>
    {
        public CustomerController(IRepository<Customer> repo) : base(repo) { }

    }

}
