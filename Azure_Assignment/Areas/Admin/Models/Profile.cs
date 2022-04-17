using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_Assignment.Areas.Admin.Models
{
    public class Profile
    {
        [Key]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter firtname")]
        public string FirtName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter lastname")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please choice gender")]
        public Nullable<bool> Gender { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select birthdate")]
        [DataType(DataType.Date, ErrorMessage = "Birthdate must be date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Birthday { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter phone")]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Incorrect Phone Number Format")]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Incorrect Email Format")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter address")]
        public string Address { get; set; }

        public string Picture { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
    }
}