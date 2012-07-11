using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;


namespace Mobile.Models
{
    [DataContract]
    public class Client : MobileModelEntity


    {
        [DataMember]
        public String CustomerKey { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public AddressBlock Address { get; set; }
        [DataMember]
        public List<OpenInvoices> InvoicesDue { get; set; }

        public Client()
        {
            this.InvoicesDue = new List<OpenInvoices>();
        }
    }

    public class AddressBlock
    {
        [DataMember]
        public String Address1 { get; set; }
        [DataMember]
        public String Address2 { get; set; }
        [DataMember]
        public String City { get; set; }
        [DataMember]
        public String State { get; set; }
        [DataMember]
        public String PostalCode { get; set; }
    }

    public class OpenInvoices : MobileModelEntity
    {
        [DataMember]
        public String InvoiceNo { get; set; }
        //public DateTime InvoiceDate { get; set; }
        //public DateTime DueDate { get; set; }
        [DataMember]
        public Double AmountDue { get; set; }
        [DataMember]
        public String Status { get; set; }
    }

}