using NES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Identity
{
    public class IdentityAggregateRoot<T> : AggregateBase where T : IIdentity
    {
        protected T _identity { get; set; }
    }
}