using System;
using System.IO.Compression;
using System.Web;

namespace Demo.Presentation.ServiceStack.Module
{
    // Credit to http://stackoverflow.com/a/28159849/795339
    // http://www.runningcode.net/2015/01/30/request-compression/
    public class GZipRequest : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) =>
            {
                var request = (sender as HttpApplication).Request;

                string contentEncoding = request.Headers["Content-Encoding"];

                if (string.Equals(contentEncoding, "gzip",
                    StringComparison.OrdinalIgnoreCase))
                {
                    request.Filter = new GZipStream(request.Filter,
                        CompressionMode.Decompress);
                    //request.Headers.Remove("Content-Encoding");
                }
            };
        }
        public void Dispose()
        {
        }
    }

}