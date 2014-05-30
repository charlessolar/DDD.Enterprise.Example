using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Requests
{
    public class Default : IRequest
    {
        public Tuple<String, String> Who { get; private set; }
        public Tuple<String, String> What { get; private set; }
        public Tuple<String, String> How { get; private set; }

        public void SetHow(String Name, String Value)
        {
            How = new Tuple<string, string>(Name, Value);
        }
        public void SetWhat(String Name, String Value)
        {
            What = new Tuple<string, string>(Name, Value);
        }
        public void SetWho(String Name, String Value)
        {
            Who = new Tuple<string, string>(Name, Value);
        }
    }
}