using Demo.Library.Queries.ServiceStack.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Items.Validators
{
    public class FindItemsValidator : PagedQueryValidator<FindItems>
    {
        public FindItemsValidator()
            : base()
        {
        }
    }
}
