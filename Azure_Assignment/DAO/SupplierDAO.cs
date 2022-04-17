using Azure_Assignment.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.DAO
{
    public class SupplierDAO
    {
        DataPalkia db = new DataPalkia();
        public List<Suppliers> Get()
        {
            return db.Suppliers.ToList();
        }
    }
}