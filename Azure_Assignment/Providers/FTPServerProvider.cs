using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;


namespace Azure_Assignment.Providers
{
    public class FTPServerProvider 
    {
        string ftp = "ftp://156.67.222.163:21/";
        string ftpParent = "NhomHoangTam/";
        string username = "u657022003.ftpuser";
        string password = "123456789-Aa";

        public string Get(string fileName, string ftpChild)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpParent + ftpChild + "/" + fileName);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.EnableSsl = false;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                using (MemoryStream stream = new MemoryStream())
                {
                    response.GetResponseStream().CopyTo(stream);
                    string base64String = Convert.ToBase64String(stream.ToArray(), 0, stream.ToArray().Length);
                    return "data:image/png;base64," + base64String;
                }
            }
            catch (WebException)
            {
                return null;
            }
        }

        public void Add(string fileName, string ftpChild, HttpPostedFileBase file)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpParent + ftpChild + "/" + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential( username, password );

            byte[] fileContents;
            using (Stream inputStream = file.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                fileContents = memoryStream.ToArray();
            }

            request.ContentLength = fileContents.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            }
        }

        public void Delete(string fileName, string ftpChild)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpParent + ftpChild + "/" + fileName);
            request.Credentials = new NetworkCredential( username, password );
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("Delete status: {0}", response.StatusDescription);
            response.Close();
        }

        public void Update(string fileName, string ftpChild, HttpPostedFileBase file, string oldFileName)
        {
            Delete(oldFileName,ftpChild);
            Add(fileName,ftpChild,file);
        }


    }
}