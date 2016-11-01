using Demo.Library.Queries;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Queries
{
    public interface IDetails : IQuery
    {
        string UserAuthId { get; set; }
    }
}
