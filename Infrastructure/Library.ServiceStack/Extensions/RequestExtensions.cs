using Demo.Library.Authentication;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Web;
using System;

namespace Demo.Library.Extensions
{
    public static class RequestExtensions
    {
        public static Auth0Profile RetreiveUserProfile(this IRequest request)
        {
            var appSettings = new AppSettings();
            var appSecret = appSettings.GetString("oauth.auth0.AppSecret").Replace('-', '+').Replace('_', '/');

            var header = request.Headers["Authorization"];

            if (header.IsNullOrEmpty())
                return null;
            try
            {
                var token = header.Split(' ');

                if (token[0].ToUpper() != "FORTEAUTH")
                    return null;

                var profile = JWT.JsonWebToken.Decode(token[1], Convert.FromBase64String(appSecret), verify: true);
                if (profile.IsNullOrEmpty())
                    return null;

                var auth0Profile = profile.FromJson<Auth0Profile>();

                return auth0Profile;
            }
            catch
            {
                return null;
            }
        }
    }
}