using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_Assignment.Models
{
    public class UserLogin
    {
        [Key]
        public string Username { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public Nullable<bool> Gender { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Picture { get; set; }
        public Nullable<int> Role { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}