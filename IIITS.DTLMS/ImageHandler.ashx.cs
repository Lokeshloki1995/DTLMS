using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace IIITS.DTLMS
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string sFTPPath = string.Empty;
            if (HttpContext.Current.Request["FTPImagePath"] != null && HttpContext.Current.Request["FTPImagePath"] != "")
            {
                sFTPPath = Convert.ToString(HttpContext.Current.Request["FTPImagePath"]);
            }

            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData("ftp://server/image.png");

            context.Response.Buffer = true;
            context.Response.Charset = "";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = "image/png";
            context.Response.AddHeader("content-disposition", "attachment;filename=Image.png");
            context.Response.BinaryWrite(imageBytes);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}