using System;

namespace Demo.Domain.Infrastructure.Riak
{
    public static class Settings
    {
        public static string Bucket { get; set; }
        public static Func<Type, string, string> KeyGenerator { get; set; }

    }
}
