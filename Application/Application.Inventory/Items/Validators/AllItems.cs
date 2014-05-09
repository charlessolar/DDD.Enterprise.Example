using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Inventory.Items.Validators
{
    public class AllItems : AbstractValidator<Queries.AllItems>
    {
        public AllItems()
        {
        }
    }
}