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
    public class Customer : MobileModelEntity
    {
        override public void InitializeDefaults()
        {
            city = "Irvine";
            state = "CA";
            zipcode = "92614";
        }

        [DataMember]
        public String customername { get; set; }

        [DataMember]
        public String ardivisionno { get; set; }

        [DataMember]
        public String customerno { get; set; }

        [DataMember]
        public String addressline1 { get; set; }

        [DataMember]
        public String city { get; set; }

        [DataMember]
        public String state { get; set; }

        [DataMember]
        public String zipcode { get; set; }

        [DataMember]
        public String telephoneno { get; set; }

        [DataMember]
        public double openorderamt { get; set; }

    }
}