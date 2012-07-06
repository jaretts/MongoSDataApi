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
            MongoCollection<T> pcollect = db.GetCollection<T>(collectionName);
            return pcollect.FindAllAs<T>().AsQueryable<T>();
        }

        public T GetTemplate()
        {
            try
            {
                return (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
            }
            catch
            {
                return default(T);
            }
        }

        public void Post(String selector, T value)
        {
            Put(selector, value);
        }

        public void Put(String selector, T value)
        {
            // not needed because have an instance variable
            //var server = MongoServer.Create(connectionString);
            //MongoDatabase db = server.GetDatabase("test");

            MongoCollection<T> pcollect = db.GetCollection<T>(collectionName);

            var query = Query.EQ("_id", value._id);

            BsonElement bsonElem = new BsonElement("_id", value._id);

            var wrapper = BsonDocumentWrapper.Create(value);
            var doc = wrapper.ToBsonDocument();
            doc.RemoveElement(bsonElem);

            var update = new UpdateDocument
            {
                { "$set", doc }
            };

            pcollect.Update(Query.EQ("_id", value._id), update);

        }
    }

}
