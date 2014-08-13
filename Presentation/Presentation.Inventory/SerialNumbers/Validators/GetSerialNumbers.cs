using Demo.Library.Queries.ServiceStack.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.SerialNumbers.Validators
{
    public class GetSerialNumbers : BasicQueryValidator<Models.GetSerialNumber>
    {
        public GetSerialNumbers() : base() { }
    }
}
