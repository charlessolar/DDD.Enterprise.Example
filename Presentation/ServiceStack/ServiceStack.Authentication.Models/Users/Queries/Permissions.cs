using Demo.Library.Queries;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Queries
{
    public interface IPermissions : IQuery
    {
        string UserAuthId { get; set; }
    }
}
