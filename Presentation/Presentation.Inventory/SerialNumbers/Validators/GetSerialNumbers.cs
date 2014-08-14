
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.SerialNumbers.Validators
{
    public class GetSerialNumbers : AbstractValidator<Models.GetSerialNumber>
    {
        public GetSerialNumbers()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
