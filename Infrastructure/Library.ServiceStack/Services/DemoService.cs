using Demo.Library.Authentication;
using Demo.Library.Extensions;
using ServiceStack;

namespace Demo.Library.Services
{
    public class DemoService : Service
    {
        protected Auth0Profile Profile { get { return Request.RetreiveUserProfile(); } }

        public override bool IsAuthenticated
        {
            get
            {
                return Profile != null;
            }
        }
    }
}