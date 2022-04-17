using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using Azure_Assignment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.DAO
{
    public class FeedbackDAO
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();

        public List<FeedbackViewModel> geAllFeedbackOfProduct(int? id)
        {
            var feedbackOfProduct = (from pro in db.Products
                                     join feed in db.Feedbacks on pro.ProductID equals feed.ProductID
                                     where pro.UnitsInStock > 0 && feed.ProductID == id
                                     orderby feed.FeedBackDate descending
                                     select new FeedbackViewModel()
                                     {
                                         FeedbackID = feed.FeedbackID,
                                         FeedbackName = feed.FeedbackName,
                                         Email = feed.Email,
                                         Content = feed.Content,
                                         FeedBackDate = feed.FeedBackDate,
                                         ProductID = feed.ProductID
                                     }).ToList();
            return feedbackOfProduct;
        }


        public bool Insert(Feedbacks feedback)
        {
            feedback.FeedBackDate = DateTime.Now;
            db.Feedbacks.Add(feedback);
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }
    }
}