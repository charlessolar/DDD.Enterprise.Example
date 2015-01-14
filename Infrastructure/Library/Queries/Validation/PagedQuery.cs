namespace Demo.Library.Queries.Validation
{
    using FluentValidation;

    public class PagedQueryValidator<T> : BasicQueryValidator<T> where T : PagedQuery
    {
        public PagedQueryValidator()
            : base()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }
}