
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Queries.Validation
{
    using FluentValidation;

    public class BasicQueryValidator<T> : AbstractValidator<T> where T : BasicQuery
    {
        public BasicQueryValidator()
        {
        }
    }
}
