using Azure_Assignment.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.DAO
{
    public class OrderDetailDAO
    {
        DataPalkia db = new DataPalkia();

        public bool Insert(OrderDetails detail)
        {
            db.OrderDetails.Add(detail);
            if (db.SaveChanges() > 0)
                return true;
            else
                return false;
        }
    }
}