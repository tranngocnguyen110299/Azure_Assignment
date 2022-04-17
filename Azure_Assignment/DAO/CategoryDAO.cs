using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using Azure_Assignment.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Azure_Assignment.DAO
{
    public class CategoryDAO
    {
        DataPalkia db = new DataPalkia();
        FTPServerProvider ftp = new FTPServerProvider();
        ImageProvider imageProvider = new ImageProvider();
        string ftpChild = "imgCategories";

        public List<CategoryViewModel> Get()
        {
            var list = (from cate in db.Categories
                             select new CategoryViewModel
                             {
                                 CategoryID = cate.CategoryID,
                                 CategoryName = cate.CategoryName,
                                 Description = cate.Description,
                                 Picture = cate.Picture
                             }).ToList();
            foreach (var item in list)
            {
                item.Picture = imageProvider.LoadImage(item.Picture, ftpChild);
            }
            return list;
        }

        public int Count(int? cateID)
        {
            var num = (from cate in db.Categories
                        where cate.CategoryID == cateID
                        select new CategoryViewModel
                        {
                            CategoryID = cate.CategoryID,
                            CategoryName = cate.CategoryName,
                            Description = cate.Description,
                            Picture = cate.Picture
                        }).Count();
            
            return num;
        }


        public List<CategoryViewModel> GetNewCategories()
        {
            var list = (from cate in db.Categories
                             orderby cate.CategoryID descending
                             select new CategoryViewModel
                             {
                                 CategoryID = cate.CategoryID,
                                 CategoryName = cate.CategoryName,
                                 Description = cate.Description,
                                 Picture = cate.Picture
                             }).ToList();
            
            
            

            foreach (var item in list)
            {
                item.Picture = imageProvider.LoadImage(item.Picture, ftpChild);
            }
            return list;
        }
    }
}