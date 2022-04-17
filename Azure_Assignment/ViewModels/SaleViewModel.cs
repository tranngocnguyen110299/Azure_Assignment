using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.ViewModels
{
    public class SaleViewModel
    {
        public int SaleID { get; set; }
        public string SaleName { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Picture { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> Discount { get; set; }
    }
}