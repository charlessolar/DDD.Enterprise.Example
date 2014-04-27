using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.Items.Validators
{
    public class FindItems : AbstractValidator<Queries.FindItems>
    {
        public FindItems()
        {
        }
    }
}