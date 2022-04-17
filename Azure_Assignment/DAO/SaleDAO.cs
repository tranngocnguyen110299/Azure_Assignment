using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using Azure_Assignment.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Azure_Assignment.DAO
{
    public class SaleDAO
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        private string ftpChild = "imgSales";


        public List<SaleViewModel> Get()
        {
            var list = (from sale in db.Sale
                        orderby sale.SaleID descending
                        where sale.SaleName != "No Sale"
                        select new SaleViewModel
                        {
                            SaleID = sale.SaleID,
                            SaleName = sale.SaleName,
                            Content = sale.Content,
                            StartDate = sale.StartDate,
                            EndDate = sale.EndDate,
                            Code = sale.Code,
                            Discount = sale.Discount,
                            Picture = sale.Picture
                        }).ToList();
            foreach(var item in list)
            {
                item.Picture = ftp.Get(item.Picture, ftpChild);
            }
            return list;
        }
    }
}