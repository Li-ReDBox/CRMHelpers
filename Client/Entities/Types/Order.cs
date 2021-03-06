﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Client.Types
{
    /// <summary>
    /// Base of Order for serialization
    /// </summary>
    [DataContract]
    public class OrderBase
    {
        public readonly static string URI = "salesorders";

        [DataMember(Name = "name")]
        public string Name { set; get; }

        [DataMember(Name = "new_orderid", EmitDefaultValue = false)]
        public string OrderID { set; get; }

        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }
    }

    [DataContract]
    public class Order : OrderBase
    {
        [DataMember(Name = "salesorderid")]
        public string ID { set; get; }
        
        [DataMember(Name = "statuscode", EmitDefaultValue = false)]
        public int Status { get; private set; }

        [DataMember(Name = "role", EmitDefaultValue = false)]
        public string Role { set; get; }
    }

    /// <summary>
    /// Data contractor for creating JSON object for creating in Dynamics
    /// This is only used in serialization
    /// </summary>
    [DataContract]
    public class Order4Creation : OrderBase
    {
        private Guid customerID;
        private Guid priceLevelID;

        [DataMember(Name = "customerid_account@odata.bind")]
        public string CustomerID {
            set { customerID = new Guid(value); }
            get { return $"/accounts({customerID.ToString()})"; }
        }

        [DataMember(Name = "pricelevelid@odata.bind")]
        public string PriceLevelID
        {
            set { priceLevelID = new Guid(value); }
            get { return $"/pricelevels({priceLevelID.ToString()})"; }
        }

        [DataMember(Name = "order_details")]
        public List<OrderDetail>  OrderedProducts { set; get; }
    }
}
