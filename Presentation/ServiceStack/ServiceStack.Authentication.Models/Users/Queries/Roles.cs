using Demo.Library.Queries;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Queries
{
    public interface IRoles : IQuery
    {
        string UserAuthId { get; set; }
    }
}
