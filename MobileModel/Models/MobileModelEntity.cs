using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Mobile.Models
{
    [DataContract]
    [BsonIgnoreExtraElements]
    public abstract class  MobileModelEntity
    {
        [DataMember(Name = "$url")]
        public String relativeUrl
        {
            get
            {
                return this.GetType().Name + "(" + this._id + ")";
            }
        }

        [DataMember(Name = "$key")]
        [Key]
        public String _id { get; set; }

    }
}
