using ServiceStack;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/subscriptions/pause", "POST")]
    public class PauseForSession  : Infrastructure.Commands.ServiceCommand
    {
        public bool Pause { get; set; }
    }
}
