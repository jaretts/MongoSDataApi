using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mobile.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;


namespace MongoRepository
{
    public class MongoRepository<T> : IRepository<T> where T : MobileModelEntity //class
    { 
        String connectionString = "mongodb://localhost";
        MongoDatabase db;
        String collectionName;
        QueryDocument controllerQuery;
        Dictionary<String, String> controllerDict;
        String _userToken;
        //String _dataset;

        private void InitMongo(String init_CollectionName)
        {
            var server = MongoServer.Create(connectionString);
            db = server.GetDatabase("test");
            collectionName = init_CollectionName;
        }

        private void InitQueryDocument()
        {
            // setup controllerQuery based on dictionary of applied where clauses
            if (controllerDict != null)
            {
                controllerQuery = null;
                controllerQuery = new QueryDocument();
                foreach (KeyValuePair<String, String> pair in controllerDict)
                {
                    //controllerQuery = new QueryDocument(pair.Key, pair.Value);
                    controllerQuery.Add(pair.Key, pair.Value);
                }
            }
        }

        public MongoRepository(String init_CollectionName)
        {
            this.InitMongo(init_CollectionName);
        }

        public MongoRepository(String init_CollectionName, Dictionary<String, String> init_ControllerDict)
        {
            this.InitMongo(init_CollectionName);
            controllerDict = init_ControllerDict;
        }

        public void SetTenantDataSet(string dataset, string userToken)
        {
            //_dataset = dataset;

            _userToken = userToken;
            if (controllerDict == null)
            {
                controllerDict = new Dictionary<string, string>();
            }
            if (controllerDict.ContainsKey("tenantID")) {
                controllerDict.Remove("tenantID");
            }
            controllerDict.Add("tenantID", dataset); // what is the scope and is there a potential conflict?

            //for now assume all collections have a CreatedBy column - TO-DO should have meta data for controller whether user filtering is applied
            if (controllerDict.ContainsKey("CreatedBy"))
            {
                controllerDict.Remove("CreatedBy");
            }           
            controllerDict.Add("CreatedBy", _userToken);
            this.InitQueryDocument();
        }

        //public String errorText { get; }\
        private String _errorText;
        public String GetErrorText()
        {
            return _errorText;
        }

        public IQueryable<T> GetAll()
        {
            MongoCollection<T> pcollect = db.GetCollection<T>(collectionName);
            //QueryDocument q = new QueryDocument("state", "CA");
            //return pcollect.FindAllAs<T>().AsQueryable<T>();
            //q.Add("acl", "steve.malmgren@sage.com");
            //return pcollect.FindAs<T>(q).AsQueryable<T>();

            if (controllerQuery != null)
            {
                return pcollect.FindAs<T>(controllerQuery).AsQueryable<T>();
            }
            else
            {
                return pcollect.FindAllAs<T>().AsQueryable<T>();
            }
        }

        public IQueryable<T> GetAll(string select)
        {
            select = select.Trim();
            if (string.IsNullOrEmpty(select))
            {
                return GetAll();
            }

            string[] includeFields = select.Split(',');
            if (includeFields.Length < 1)
            {
                return GetAll();
            }

            MongoCollection<T> pcollect = db.GetCollection<T>(collectionName);

            PropertyInfo[] props = typeof(T).GetProperties();
            string[] fieldsToExclude = new string[props.Length];

            for(int i = 0; i < fieldsToExclude.Length; i++)
            {
                fieldsToExclude[i] = props[i].Name;
            }

            MongoCursor<T> result = pcollect.FindAllAs<T>();

            result.Fields = Fields.Exclude(fieldsToExclude);
            result.Fields = Fields.Include(includeFields);

            return result.AsQueryable<T>();
        }

        public T GetTemplate()
        {
            T retValue;
            try
            {
                retValue = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
                retValue.InitializeDefaults();
            }
            catch
            {
                retValue = default(T);
            }
            return retValue;
        }

        public T Post(T value)
        {

            // 1. Need to check for existence of value.Id, then validate state transition (e.g. from existing Draft to QuoteRequest, OrderRequest; etc)
            //    Then call self.Put(with selector)
            //    Otherwise we need to fail on state transition

            _errorText = null;

            if (value.Id != null)
            {
                // Post to a new resource/state, need to PUT to update status field.  Valid state transition business rules to be enforced in PUT
                return Put(value.Id, value);
            }

            MongoCollection<T> pcollect = db.GetCollection<T>(collectionName);

            //set document state based on resource
            if (controllerDict != null)
            {
                foreach (KeyValuePair<String, String> pair in controllerDict)
                {
                    PropertyInfo tmpPropInfo = typeof(T).GetProperty(pair.Key);
                    if (tmpPropInfo != null)
                    {
                        tmpPropInfo.SetValue(value, pair.Value, null);
                    }
                    else
                    {
                        //
                    }
                }
            }

            pcollect.Save(value);

            
            return GetAll().FirstOrDefault(y => y.Id == value.Id);
        }

        public T Put(String selector, T value)
        {
            // not needed because have an instance variable
            //var server = MongoServer.Create(connectionString);
            //MongoDatabase db = server.GetDatabase("test");

            // 1. need to ensure selector == value.Id or bail out with failure
            // 
            _errorText = null;

            if (selector != value.Id)
            {
                _errorText = @"resource selector/Id mismatch";
                return null;
            }


            MongoCollection<T> pcollect = db.GetCollection<T>(collectionName);

            var query = Query.EQ("_id", value.Id);

            BsonElement bsonElem = new BsonElement("_id", value.Id);

            var wrapper = BsonDocumentWrapper.Create(value);
            var doc = wrapper.ToBsonDocument();
            doc.RemoveElement(bsonElem);

            /*
            BsonElement tmpElement = doc.GetElement("Status");

            if (tmpElement.Value == "S")
            {
                tmpElement.Value = "N";
            }
            */

            // all of this checking should really be done in a biz layer
            if (controllerDict != null)
            {
                foreach (KeyValuePair<String, String> pair in controllerDict)
                {
                    //PropertyInfo tmpPropInfo = typeof(T).GetProperty(pair.Key);
                    //tmpPropInfo.SetValue(value, pair.Value, null);

                    BsonElement tmpElement = doc.GetElement(pair.Key);

                    // here we check current state and see if valid to proceed to this state
                    // hard coded Request cannot go back to Draft
                    if (tmpElement.Value == "R" & pair.Value == "D")
                    {
                        _errorText = @"Invalid state transition Request cannot be put back to Draft status";
                        return null;
                    }

                    tmpElement.Value = pair.Value;

                }
            }

            var update = new UpdateDocument
            {
                { "$set", doc }
            };

            pcollect.Update(Query.EQ("_id", value.Id), update);
            return GetAll().FirstOrDefault(y => y.Id == value.Id);
        }
    }

}
