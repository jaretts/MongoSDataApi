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
        public IRepository<T> repository { get; set; }

        public DefaultController(IRepository<T> respository)
        {
            this.repository = respository;
        }


        private string GetUserTokenFromRequest()
        {
            IEnumerable<string> values;
            if (Request.Headers.TryGetValues("X-User-Token", out values))
            {
                foreach (string val in values)
                {
                    return val;
                }
            }
            return @"";
        }
        /// GET api/default
        /// Must have Queryable attribute or OData does not work
        [Queryable]
        [HttpGet]
        [ActionName("SDataCollection")]
        public virtual IQueryable<T> GetCollection(string select, string dataset)
        {
            IQueryable<T> retVal;

            var userToken = GetUserTokenFromRequest();
            repository.SetTenantDataSet(dataset, userToken);
            if(string.IsNullOrEmpty(select))
            {
                retVal = repository.GetAll();
            }
            else
            {
                retVal = repository.GetAll(select);
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
        public virtual T GetSingle(String selector, String dataset)
        {
            return GetCollection("", dataset).FirstOrDefault(y => y.Id == selector);
        }

        // GET api/product/5
        [HttpGet]
        [ActionName("SDataSingleResourceKind")]
        public virtual T GetSingle(String selector, String select, String dataset)
        {
            return GetCollection(select, dataset).FirstOrDefault(y => y.Id == selector);
        }

        [HttpGet]
        [ActionName("GetTemplate")]
        virtual public T GetTemplate(String dataset)
        {
            var userToken = GetUserTokenFromRequest();
            repository.SetTenantDataSet(dataset, userToken);
            return repository.GetTemplate();
        }

        // PUT api/customers/5
        [HttpPut]
        [ActionName("SDataSingleResourceKind")]
        public HttpResponseMessage Put(String selector, String select, T value, String dataset)
        {
            T resourceToModify = GetSingle(selector, select, dataset);
            
            T resourceModified;

            if (resourceToModify == null)
            {
                resourceModified = resourceToModify;
            }
            else
            {
                var userToken = GetUserTokenFromRequest();
                repository.SetTenantDataSet(dataset, userToken);
                resourceModified = repository.Put(selector, value);
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

            // check for any errors from repository and reset status code and reason phrase
            String tmpReason = repository.GetErrorText();
            if (tmpReason != null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ReasonPhrase = tmpReason;
                response.Content = new StringContent(tmpReason);
            }
            return response;
        }

        // POST single resource post; client sent single resource
        [HttpPost]
        [ActionName("SDataCollection")]
        public HttpResponseMessage Post(T value, String dataset)
        {
            var userToken = GetUserTokenFromRequest();
            repository.SetTenantDataSet(dataset, userToken);
            T addedResource = repository.Post(value);
            HttpStatusCode retStatus;

            if (addedResource == null)
            {
                retStatus = HttpStatusCode.BadRequest;
            }
            else
            {
                retStatus = HttpStatusCode.Created;
            }


            var response = Request.CreateResponse<T>(retStatus, addedResource);
            
            // check for any errors from repository and reset status code and reason phrase
            String tmpReason = repository.GetErrorText();
            if (tmpReason != null)
            {
                response.ReasonPhrase = tmpReason; //this doesn't appear to do anything at least in firefox
                response.Content = new StringContent(tmpReason);
            }            

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
        public void Delete(String selector, String dataset)
        {
            //TO-DO delete code
        }

    }
}
