using System;

namespace Demo.Application.RavenDB.Authentication.Users
{
    public class User
    {
        public String Id { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }

        public String NickName { get; set; }

        public String ImageType { get; set; }

        public String ImageData { get; set; }
    }
}