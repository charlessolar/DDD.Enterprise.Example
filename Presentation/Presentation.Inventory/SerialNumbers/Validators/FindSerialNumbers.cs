using Demo.Library.Queries.ServiceStack.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.SerialNumbers.Validators
{
    public class FindSerialNumbersValidator : PagedQueryValidator<FindSerialNumbers>
    {
        public FindSerialNumbersValidator() : base() { }
    }
}
