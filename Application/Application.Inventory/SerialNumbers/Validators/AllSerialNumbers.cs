using Demo.Library.Queries.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Inventory.SerialNumbers.Validators
{
    public class AllSerialNumbers : BasicQueryValidator<Queries.AllSerialNumbers>
    {
        public AllSerialNumbers()
            : base()
        {
        }
    }
}