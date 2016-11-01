using ServiceStack;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/subscriptions", "DELETE")]
    public class DropForSession  : Infrastructure.Commands.ServiceCommand
    {
    }
}
