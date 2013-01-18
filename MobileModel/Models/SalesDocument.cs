using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Models
{

    [DataContract]
    public class SalesDocument : MobileModelEntity
    {
        [DataMember]
        public String CustomerKey { get; set; }
        [DataMember]
        public String BillToName { get; set; }
        [DataMember]
        public String ShipToName { get; set; }
        [DataMember]
        public Double FreightAmount { get; set; }
        [DataMember]
        public Double DiscountAmount { get; set; }
        [DataMember]
        public Double SalesTaxAmount { get; set; }
        [DataMember]
        public AddressBlock BillToAddress { get; set; }
        [DataMember]
        public AddressBlock ShipToAddress { get; set; }
        [DataMember]
        public String ConfirmToEmail { get; set; }
        [DataMember]
        public String Status { get; set; }
        [DataMember]
        public List<SalesLines> Details { get; set; }

        public SalesDocument()
        {
            this.Details = new List<SalesLines>();
        }
    }

    public class SalesLines : MobileModelEntity
    {
        [DataMember]
        public String ItemID { get; set; }
        [DataMember]
        public Double Quantity { get; set; }
        [DataMember]
        public Double UnitPrice { get; set; }
        [DataMember]
        public Double ExtendedAmount { get; set; }
        [DataMember]
        public String Description { get; set; }
    }

    [DataContract]
    public class draftSalesDocument : SalesDocument
    {
    }

    public class quoteRequest : SalesDocument
    {
    }

    public class pendingQuote : SalesDocument
    {
    }

    public class salesQuote : SalesDocument
    {
    }


}

