using System;

namespace Azure_Assignment.ViewModels
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> UnitPrice { get; set; }
        public Nullable<int> OldUnitPrice { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Thumbnail { get; set; }
        public int SaleID { get; set; }
        public string SaleName { get; set; }
    }
}