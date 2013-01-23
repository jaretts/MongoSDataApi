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
        public String CreatedBy { get; set; }
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
        //documents save from a mobile device prior to being submitted
        //valid state transitions are to quoteRequest, orderRequest, saleRequest
        //
        //Actor permissions: Mobile User - full CRUD (GET/POST/PUT/DELETE) for documents created by user (SageID/UserID filtering)
    }

    public class quoteRequest : SalesDocument
    {
        //submitted to the cloud ready for on-premises system to pick up, price out and write back as salesQuote
        //valid state transitions are to pendingQuote
        //
        //Actor permissions: Mobile User - POST for documents created by user
        //                   On-Premises Connector (or identified subscriber) - GET
        //                   (NOTE: no DELETE or PUT, can only be changed by a POST to pendingQuote)
    }

    public class pendingQuote : MobileModelEntity
    {
        //updated by on-premises connector to indicate successful GET and in-process status of pricing a quote on-premises
        //valid state transitions are to salesQuote or errorQuote and only by on-premises connector subscriber API key
        //NOTE: inherits MobileModelEntity for Id guid process and only other required element in payload is the status
        //      may not need this here as will be set automatically
        //
        //Actor permissions: Mobile User - NONE
        //                   On-premises Connector (or identified subscriber) - POST
        //                   (NOTE: no DELETE of PUT, can only be changed by a POST to salesQuote or errorQuote)
        [DataMember]
        public String Status { get; set; }
    }

    public class salesQuote : SalesDocument
    {
        //document has been priced and PUT back to this state by on-premises connector based on successful "sync"
        //valid state transitions are back to draftQuote (if user changes anything but DiscountPercentage) or orderRequest
        //
        //Actor permissions: Mobile User - GET/PUT/DELETE (no POST) - PUT should call PUT to draftSalesDocument if anything other than DiscountPercentage is changed (or does the mobile app do PUT to draftSalesDocument)
        //                   On-premises Connector - POST (after quote has been priced out and then deleted on-premises - to avoid maintenance in multiple places)
        //              OPEN: If discount is over threshold, UI is suppose to change Buy Now button to Approval Required and kick off the process
    }

    public class orderRequest : SalesDocument
    {
        //submitted to cloud for on-premises system to pick up, and save as an order - connector must take pricing, salestax, etc. as actual (i.e. not re-price) will write back as a salesOrder
        //valid state transitions are to pendingOrder
        //
        //Actor permissions: Mobile User - POST
        //                   On-premises connector - GET
    }

    public class pendingOrder : MobileModelEntity
    {
        //updated by on-premises connector to indicate successful GET and in-process status of creating a sales order on-premises
        //valid state transitions are to salesOrder or errorOrder and only by on-premises connector subscriber API key
        //NOTE: inherits MobileModelEntity for Id guid process and only other required element in payload is the status
        //      may not need this here as will be set automatically
        //
        //NOTE2: May only need a single errorXxxxx type status to handle any of the pendingXxxxx requests when on-premises fails for whatever reason
        //
        //Actor permissions: Mobile User - NONE
        //                   On-premises connector - POST
        [DataMember]
        public String Status { get; set; }
    }

    public class salesOrder : SalesDocument
    {
        //document has been priced and PUT back to this state by on-premises connector based on successful "sync"
        //valid state transitions are NONE - only data management worker role can remove this based on data retention policy
        //
        //Actor permissions: Mobile User - GET (OPEN: only their orders or ALL orders?)
        //                   On-premises Connector POST/PUT
        //                   OPEN: when do they get deleted?  Worker role - data retention policy
    }
    public class saleRequest : SalesDocument
    {
        //submitted to cloud for on-premises system to pick up and save as a cash sales based on the Buy Now button when coming from a Draft status
        //these could be handled as One-step invoices OR a 100% deposit of a Sales Order based on the preference of the individual on-premises product teams
        //valid state transitions are to pendingSales
        //
        //Actor permissions: Mobile User - POST
        //                   On-premises connector - GET
    }

    public class pendingSale : MobileModelEntity
    {
        //updated by on-premises connector to indicate successful GET and in-process status of creating a sales order on-premises
        //valid state transitions are to salesOrder, salesInvoice or errorOrder and only by on-premises connector subscriber API key
        //NOTE: inherits MobileModelEntity for Id guid process and only other required element in payload is the status
        //      may not need this here as will be set automatically
        //
        //NOTE2: May only need a single errorXxxxx type status to handle any of the pendingXxxxx requests when on-premises fails for whatever reason
        //
        //NOTE3: What becomes of sale, does it come back to Azure as an invoice
        //
        //Actor permissions: Mobile User - NONE
        //                   On-premises connector POST - (possibly DELETE to indicate completion to remove pending status - end of the line)
        [DataMember]
        public String Status { get; set; }
    }

}

