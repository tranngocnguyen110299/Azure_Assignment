using Azure_Assignment.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.DAO
{
    public class PaymentDAO
    {
        DataPalkia db = new DataPalkia();
        public List<Payments> GetAll()
        {
            return db.Payments.ToList();
        }
    }
}