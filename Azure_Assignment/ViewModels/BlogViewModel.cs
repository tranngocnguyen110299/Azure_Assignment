using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.ViewModels
{
    public class BlogViewModel
    {
        public int BlogID { get; set; }
        public string BlogName { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public Nullable<int> BlogCategoryID { get; set; }
        public Nullable<System.DateTime> WritingDate { get; set; }
        public string Thumbnail { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}