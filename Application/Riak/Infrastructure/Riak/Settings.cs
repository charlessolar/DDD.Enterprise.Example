using RiakClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Riak.Infrastructure.Riak
{
    public static class Settings
    {
        public static String Bucket { get; set; }
        public static Func<Type, String, String> KeyGenerator { get; set; }
        
    }
}
