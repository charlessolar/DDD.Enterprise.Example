using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(String message)
            : base(message)
        {
        }
    }
}