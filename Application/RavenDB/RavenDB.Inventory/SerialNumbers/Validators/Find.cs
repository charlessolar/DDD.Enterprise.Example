using Demo.Library.Queries.Validation;

namespace Demo.Application.RavenDB.Inventory.SerialNumbers.Validators
{
    public class Find : BasicQueryValidator<Queries.Find>
    {
        public Find()
            : base()
        {
        }
    }
}