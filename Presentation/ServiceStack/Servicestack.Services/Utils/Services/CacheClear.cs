using ServiceStack;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/cache/clear", "DELETE")]
    public class CacheClear  : Infrastructure.Commands.ServiceCommand
    {
    }
}
