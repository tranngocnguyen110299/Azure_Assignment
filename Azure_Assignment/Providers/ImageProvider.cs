using Azure_Assignment.Areas.Admin.Controllers;
using System;
using System.IO;
using System.Web;

namespace Azure_Assignment.Providers
{
    public class ImageProvider : BaseController
    {
        public string Validate(HttpPostedFileBase file)
        {
            string fileName = Path.GetFileNameWithoutExtension( file.FileName );
            string extension = Path.GetExtension( file.FileName );
            if ((extension == ".png" || extension == ".jpg" || extension == ".jpeg") == false)
            {
                return string.Format("The File, which extension is {0}, hasn't accepted. Please try again!", extension);
            }

            long fileSize = ((file.ContentLength) / 1024);
            if (fileSize > 5120)
            {
                return "The File, which size greater than 5MB, hasn't accepted. Please try again!";
            }

            return null;
        }

        public string LoadImage(string fileName, string childNode)
        {
            return "~/public/uploadedFiles/"+ childNode + "/" + fileName;
        }
    }
}