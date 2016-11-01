using Demo.Library.Queries;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Queries
{
    public interface IGet : IQuery
    {
        string UserAuthId { get; set; }
    }
}
