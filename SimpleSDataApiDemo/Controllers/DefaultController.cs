using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Mobile.Models;

namespace SimpleSDataApiDemo.Controllers
{
    public class DefaultController<T> : ApiController where T : MobileModelEntity // class
    {
        public IRepository<T> respository { get; set; }

        public DefaultController(IRepository<T> respository)
        {
            this.respository = respository;
        }

        virtual public T GetTemplate()
        {
            return respository.GetTemplate();
        }

        /// GET api/default
        /// Must have Queryable attribute or OData does not work
        [Queryable]
        public virtual IQueryable<T> GetCollection(string select)
        {
            IQueryable<T> retVal;
            
            if(string.IsNullOrEmpty(select))
            {
                retVal = respository.GetAll();
            }
            else
            {
                retVal = respository.GetAll(select);
            }
            

            if (retVal == null)
            {
                // should have something now 
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Resource not found"),
                    ReasonPhrase = "Resource not found"
                });
            }

            return retVal;
        }

        // GET api/product/5
        public virtual T GetSingle(String selector, String select)
        {
            return GetCollection(select).FirstOrDefault(y => y.Id == selector);
        }

        // POST api/customers/5
        public T Post(T value)
        {
            return respository.Post(value);
        }

        // PUT api/customers/5
        public T Put(String selector, T value)
        {
            respository.Put(selector, value);

            return GetSingle(selector, "");
        }

        // DELETE api/customers/5
        public void Delete(String selector)
        {
        }

    }
}
