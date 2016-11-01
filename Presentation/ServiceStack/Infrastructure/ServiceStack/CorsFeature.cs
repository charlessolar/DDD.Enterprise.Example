using System;
using System.Collections.Generic;
using ServiceStack.Host.Handlers;
using ServiceStack.Web;
using ServiceStack;

namespace Demo.Presentation.ServiceStack.Infrastructure.ServiceStack
{
    /// <summary>
    /// Plugin adds support for Cross-origin resource sharing (CORS, see http://www.w3.org/TR/access-control/). 
    /// CORS allows to access resources from different domain which usually forbidden by origin policy. 
    /// </summary>
    public class CorsFeature : IPlugin
    {
        internal const string DefaultMethods = "GET, POST, PUT, DELETE, OPTIONS";
        internal const string DefaultHeaders = "Content-Type";

        private readonly string _allowedOrigins;
        private readonly string _allowedMethods;
        private readonly string _allowedHeaders;
        private readonly string _exposeHeaders;
        private readonly int? _maxAge;

        private readonly bool _allowCredentials;

        private readonly ICollection<string> _allowOriginWhitelist;

        public ICollection<string> AllowOriginWhitelist => _allowOriginWhitelist;

        public bool AutoHandleOptionsRequests { get; set; }

        /// <summary>
        /// Represents a default constructor with Allow Origin equals to "*", Allowed GET, POST, PUT, DELETE, OPTIONS request and allowed "Content-Type" header.
        /// </summary>
        public CorsFeature(string allowedOrigins = "*", string allowedMethods = DefaultMethods, string allowedHeaders = DefaultHeaders, bool allowCredentials = false,
            string exposeHeaders = null, int? maxAge = null)
        {
            this._allowedOrigins = allowedOrigins;
            this._allowedMethods = allowedMethods;
            this._allowedHeaders = allowedHeaders;
            this._allowCredentials = allowCredentials;
            this.AutoHandleOptionsRequests = true;
            this._exposeHeaders = exposeHeaders;
            this._maxAge = maxAge;
        }

        public CorsFeature(ICollection<string> allowOriginWhitelist, string allowedMethods = DefaultMethods, string allowedHeaders = DefaultHeaders, bool allowCredentials = false,
            string exposeHeaders = null, int? maxAge = null)
        {
            this._allowedMethods = allowedMethods;
            this._allowedHeaders = allowedHeaders;
            this._allowCredentials = allowCredentials;
            this._allowOriginWhitelist = allowOriginWhitelist;
            this.AutoHandleOptionsRequests = true;
            this._exposeHeaders = exposeHeaders;
            this._maxAge = maxAge;
        }

        public void Register(IAppHost appHost)
        {
            if (appHost.HasMultiplePlugins<CorsFeature>())
                throw new NotSupportedException("CorsFeature has already been registered");

            if (!string.IsNullOrEmpty(_allowedOrigins) && _allowOriginWhitelist == null)
                appHost.Config.GlobalResponseHeaders.Add(HttpHeaders.AllowOrigin, _allowedOrigins);
            if (!string.IsNullOrEmpty(_allowedMethods))
                appHost.Config.GlobalResponseHeaders.Add(HttpHeaders.AllowMethods, _allowedMethods);
            if (!string.IsNullOrEmpty(_allowedHeaders))
                appHost.Config.GlobalResponseHeaders.Add(HttpHeaders.AllowHeaders, _allowedHeaders);
            if (_allowCredentials)
                appHost.Config.GlobalResponseHeaders.Add(HttpHeaders.AllowCredentials, "true");
            if (_exposeHeaders != null)
                appHost.Config.GlobalResponseHeaders.Add(HttpHeaders.ExposeHeaders, _exposeHeaders);
            if (_maxAge != null)
                appHost.Config.GlobalResponseHeaders.Add(HttpHeaders.AccessControlMaxAge, _maxAge.Value.ToString());

            Action<IRequest, IResponse> allowOriginFilter = null;

            if (_allowOriginWhitelist != null)
            {
                allowOriginFilter = (httpReq, httpRes) => {
                    var origin = httpReq.Headers.Get(HttpHeaders.Origin);
                    if (_allowOriginWhitelist.Contains(origin))
                    {
                        httpRes.AddHeader(HttpHeaders.AllowOrigin, origin);
                    }
                };

                appHost.PreRequestFilters.Add(allowOriginFilter);
            }

            if (AutoHandleOptionsRequests)
            {
                //Handles Request and closes Response after emitting global HTTP Headers
                var emitGlobalHeadersHandler = new CustomActionHandler(
                    (httpReq, httpRes) => {
                        httpRes.EndRequest(); //PreRequestFilters already written in CustomActionHandler
                    });

                appHost.RawHttpHandlers.Add(httpReq =>
                    httpReq.HttpMethod == HttpMethods.Options
                        ? emitGlobalHeadersHandler
                        : null);
            }
        }
    }
}