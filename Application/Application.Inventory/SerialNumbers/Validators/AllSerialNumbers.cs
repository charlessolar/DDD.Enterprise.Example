using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.SerialNumbers.Validators
{
    public class AllSerialNumbers : AbstractValidator<Queries.AllSerialNumbers>
    {
        public AllSerialNumbers()
        {
        }
    }
}