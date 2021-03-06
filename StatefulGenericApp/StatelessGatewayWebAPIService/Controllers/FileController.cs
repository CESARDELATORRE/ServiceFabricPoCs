﻿

namespace StatelessGatewayWebAPIService.WordCountWeb
{
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// Controller that serves up files from the wwwroot directory that are included as embedded assembly resources.
    /// You can also use the FileSystem and StaticFile middleware for OWIN to render script files,
    /// or wait for ASP.NET vNext when the full MVC stack will be available for self-hosting.
    /// </summary>
    public class FileController : ApiController
    {
        /// <summary>
        /// Renders files in the wwwroot directory.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get(string name)
        {
            return this.View("StatelessGatewayWebAPIService.wwwroot." + name, ContentType(name));
        }

        private string ContentType(string file)
        {
            if (file.EndsWith(".js"))
            {
                return "application/javascript";
            }

            if (file.EndsWith(".css"))
            {
                return "text/css";
            }

            return "text/plain";
        }
    }
}
