using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_Assignment.EF;

namespace Azure_Assignment.DAO
{
    public class OrderDAO
    {
        DataPalkia db = new DataPalkia();
        public long Insert(Orders order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.OrderID;
        }

        
    }
}