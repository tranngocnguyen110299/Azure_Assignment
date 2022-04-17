using Azure_Assignment.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.ViewModels
{
    public class FeedbackViewModel
    {
        public int FeedbackID { get; set; }
        public string FeedbackName { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> FeedBackDate { get; set; }
        public Nullable<int> ProductID { get; set; }

        public virtual Products Products { get; set; }
    }
}