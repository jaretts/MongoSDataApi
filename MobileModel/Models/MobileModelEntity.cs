using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Mobile.Models
{
    [DataContract]
    [BsonIgnoreExtraElements]
    public abstract class  MobileModelEntity
    {
        /*
        [DataMember(Name = "$url")]
        public String relativeUrl
        {
            get
            {
                return this.GetType().Name + "('" + this._id + "')";
            }
        }
         */

        [DataMember(Name = "$key")]
        [Key]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public String Id { get; set; }

        virtual public void InitializeDefaults() { }

    }
}
