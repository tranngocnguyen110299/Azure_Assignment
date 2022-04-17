using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
    }
}