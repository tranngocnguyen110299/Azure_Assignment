using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Azure_Assignment.ViewModels
{
    public class BlogCommentViewModel
    {
        public int BlogCommentID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> CommentingDate { get; set; }
        public Nullable<int> BlogID { get; set; }
    }
}