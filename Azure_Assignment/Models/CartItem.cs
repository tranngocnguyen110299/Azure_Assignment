using Azure_Assignment.EF;
using Azure_Assignment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.Models
{
    [Serializable]
    public class CartItem
    {
        public Products Product { get; set; }
        public int Quantity { get; set; }

    }
}