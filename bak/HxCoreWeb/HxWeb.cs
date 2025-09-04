using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using System;
using System.Linq;

using HxCore;
using Microsoft.AspNetCore.Builder;
using System.IO;

namespace HxCore.Web
{
    public class HxWeb : HxUtils
    {
        public HttpContextAccessor ContextAccessor { get; protected set; }
        public HttpContext Context { get; protected set; }
        public HttpRequest Request { get; protected set; }


        #region Static Intance
        private static HxWeb _instance = null;
        public static new HxWeb Instance
        {
            get { return _instance; }
        }

        public static void CreateInstance(HttpContext context)
        {
            _instance = new HxWeb(context);
        }

        public static HxWeb Create(HttpContext context)
        {
            return new HxWeb(context);
        }
        #endregion

        public HxWeb(HttpContextAccessor contextAccessor)
        {
            this.ContextAccessor = contextAccessor;
            if(this.ContextAccessor != null)
            {
                this.Context = this.ContextAccessor?.HttpContext;
                this.Request = this.Context?.Request;
            }
        }

        public HxWeb(HttpContext context)
        {
            if (context != null)
            {
                this.Context = context;
               
            }
            else
            {
                this.Context = Request.HttpContext;
            }
            this.Request = this.Context?.Request;
        }

        public HxWeb(HttpRequest request)
        {
            if(request != null)
            {
                this.Request = request;
            }
            this.Context = this.Request?.HttpContext;
        }

        public string GetRemoteIP(bool tryUseXForwardHeader = true)
        {
            return HxWebUtils.GetRemoteIP(this.Request, tryUseXForwardHeader);
        }

        public string GetHeaderValue(string headerName)
        {
            return this.GetHeaderValueAs<string>(headerName);
        }


        public T GetHeaderValueAs<T>(string headerName)
        {
            //https://stackoverflow.com/questions/28664686/how-do-i-get-client-ip-address-in-asp-net-core
            //StringValues values;

            //if (this.Context?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            //{
            //    string rawValues = values.ToString();   // writes out as Csv when there are multiple.

            //    if (!rawValues.IsNullOrWhiteSpaceEx())
            //    {
            //        return (T)Convert.ChangeType(values.ToString(), typeof(T));
            //    }
            //}
            //return default(T);
            return HxWebUtils.GetHeaderValue<T>(this.Request, headerName);
        }


        public string GetRequestValue(string varName)
        {
            return HxWebUtils.GetRequestValue(this.Request, varName, true);
        }

        [Obsolete("미구현!, 필요여부는 나중에")]
        private void Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder app)
        {
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-2.1&tabs=aspnetcore2x#fileextensioncontenttypeprovider
            // Set up custom content types - associating file extension to MIME type
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".myapp"] = "application/x-msdownload";
            provider.Mappings[".htm3"] = "text/html";
            provider.Mappings[".image"] = "image/png";
            // Replace an existing mapping
            provider.Mappings[".rtf"] = "application/x-msdownload";
            // Remove MP4 videos.
            provider.Mappings.Remove(".mp4");

            app.UseStaticFiles(new Microsoft.AspNetCore.Builder.StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/MyImages",
                ContentTypeProvider = provider
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/MyImages"
            });
        }

    }
}
