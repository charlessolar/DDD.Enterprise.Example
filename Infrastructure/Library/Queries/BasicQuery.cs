using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Queries
{
    public class BasicQuery : ICommand
    {
        public String QueryId { get; set; }
    }
}