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