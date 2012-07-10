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

        /// GET api/default
        /// Must have Queryable attribute or OData does not work
        [Queryable]
        [HttpGet]
        [ActionName("SDataCollection")]
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
        [HttpGet]
        [ActionName("SDataSingleResourceKind")]
        public virtual T GetSingle(String selector)
        {
            return GetCollection("").FirstOrDefault(y => y.Id == selector);
        }

        // GET api/product/5
        [HttpGet]
        [ActionName("SDataSingleResourceKind")]
        public virtual T GetSingle(String selector, String select)
        {
            return GetCollection(select).FirstOrDefault(y => y.Id == selector);
        }

        [HttpGet]
        [ActionName("GetTemplate")]
        virtual public T GetTemplate()
        {
            return respository.GetTemplate();
        }

        // PUT api/customers/5
        [HttpPut]
        [ActionName("SDataSingleResourceKind")]
        public HttpResponseMessage Put(String selector, String select, T value)
        {
            T resourceToModify = GetSingle(selector, select);
            
            T resourceModified;

            if (resourceToModify == null)
            {
                resourceModified = resourceToModify;
            }
            else
            {
                resourceModified = respository.Put(selector, value);
            }

            HttpStatusCode retStatus;
            if(resourceModified == null)
            {
                retStatus = HttpStatusCode.NotFound;
            }
            else
            {
                retStatus = HttpStatusCode.OK;
            }

            var response = Request.CreateResponse<T>(retStatus, resourceModified);

            return response;
        }

        // POST single resource post; client sent single resource
        [HttpPost]
        [ActionName("SDataCollection")]
        public HttpResponseMessage Post(T value)
        {
            T addedResource = respository.Post(value);

            var response = Request.CreateResponse<T>(HttpStatusCode.Created, addedResource);
            return response;
        }

        /* POST Batch post; client sent an array
        [HttpPost]
        [ActionName("SDataCollection")]
        public T PostArray(T[] value)
        {
            if(value != null)
            {
                int valLen = value.Length;

                for (int x = 0; x < valLen; x++ )
                {
                    PostSingle(value[x]);
                }
            }
            return respository.GetTemplate();
        }*/

        // DELETE api/customers/5
        public void Delete(String selector)
        {
        }

    }
}
