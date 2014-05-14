using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException()
            : base("")
        {
        }
        public BusinessLogicException(String message)
            : base(message)
        {
        }
    }
}