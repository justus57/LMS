using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace LMS
{
    /// <summary>
    /// Summary description for FileUploadHandler
    /// </summary>
    public class FileUploadHandler 
    {
     
        internal static void ProcessRequest(HttpContext context)
        {
            //Check if Request is to Upload the File.
            if (context.Request.Files.Count > 0)
            {
                try
                {
                    //Fetch the Uploaded File.
                    HttpPostedFile postedFile = context.Request.Files[0];

                    //Set the Folder Path.
                    string folderPath = context.Server.MapPath("~/Uploads/");

                    //Set the File Name.
                    string fileName = Path.GetFileName(postedFile.FileName);

                    string filePath = folderPath + fileName;

                    //if exists delete
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    //Save the File in Folder.
                    postedFile.SaveAs(folderPath + fileName);

                    //Send File details in a JSON Response.
                    string json = new JavaScriptSerializer().Serialize(
                        new
                        {
                            name = fileName,
                            path = filePath,
                            uploadspath = folderPath
                        });
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "text/json";
                    context.Response.Write(json);
                    context.Response.End();
                }
                catch (Exception es)
                {
                    Console.Write(es);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        
    }
    internal class Request
    {
    }
}