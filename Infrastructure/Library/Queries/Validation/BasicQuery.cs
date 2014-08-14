
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
            RuleFor(x => x.Fields).NotEmpty();
        }
    }
}

namespace Demo.Library.Queries.ServiceStack.Validation
{
    using global::ServiceStack.FluentValidation;

    public class BasicQueryValidator<T> : AbstractValidator<T> where T : BasicQuery
    {
        public BasicQueryValidator()
        {
            // Servicestack side will use default Field list if this is empty
            // RuleFor(x => x.Fields).NotEmpty();
        }
    }
}