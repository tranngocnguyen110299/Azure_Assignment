using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Azure_Assignment.ViewModels
{
    public class ProductDetailViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ImgFileName { get; set; }
        public int ImgID { get; internal set; }
    }
}