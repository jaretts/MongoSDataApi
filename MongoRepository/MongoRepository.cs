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

        public MongoRepository(String init_CollectionName)
        {
            var server = MongoServer.Create(connectionString);
            db = server.GetDatabase("test");
            collectionName = init_CollectionName;
        }

        public IQueryable<T> GetAll()
        {
            MongoCollection<T> pcollect = db.GetCollection<T>(collectionName);
            return pcollect.FindAllAs<T>().AsQueryable<T>();
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

            MongoCollection<T> pcollect = db.GetCollection<T>(collectionName);
            pcollect.Save(value);
            return GetAll().FirstOrDefault(y => y.Id == value.Id);
        }

        public T Put(String selector, T value)
        {
            // not needed because have an instance variable
            //var server = MongoServer.Create(connectionString);
            //MongoDatabase db = server.GetDatabase("test");

            MongoCollection<T> pcollect = db.GetCollection<T>(collectionName);

            var query = Query.EQ("_id", value.Id);

            BsonElement bsonElem = new BsonElement("_id", value.Id);

            var wrapper = BsonDocumentWrapper.Create(value);
            var doc = wrapper.ToBsonDocument();
            doc.RemoveElement(bsonElem);

            var update = new UpdateDocument
            {
                { "$set", doc }
            };

            pcollect.Update(Query.EQ("_id", value.Id), update);
            return GetAll().FirstOrDefault(y => y.Id == value.Id);
        }
    }

}
