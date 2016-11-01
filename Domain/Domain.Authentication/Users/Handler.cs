using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace Demo.Domain.Authentication.Users
{
    public class Handler :
        IHandleMessages<Commands.Login>,
        IHandleMessages<Commands.Logout>,
        IHandleMessages<Commands.ChangeAvatar>,
        IHandleMessages<Commands.ChangeEmail>,
        IHandleMessages<Commands.ChangeName>,
        IHandleMessages<Commands.ChangeTimezone>
    {
        private readonly IUnitOfWork _uow;
        

        public Handler(IUnitOfWork uow)
        {
            _uow = uow;
           
        }

        public async Task Handle(Commands.Login command, IMessageHandlerContext ctx)
        {
            var user = await _uow.For<User>().Get(command.CurrentUserId);

            if (user == null)
                user = await _uow.For<User>().New(command.CurrentUserId);

            user.Login(command.Name, command.Email, command.NickName, command.ImageType, command.ImageData);
        }

        public async Task Handle(Commands.Logout command, IMessageHandlerContext ctx)
        {
            var user = await _uow.For<User>().Get(command.CurrentUserId);
            user.Logout();
        }

        public async Task Handle(Commands.ChangeAvatar command, IMessageHandlerContext ctx)
        {
            var user = await _uow.For<User>().Get(command.CurrentUserId);
            user.ChangeAvatar(command.ImageType, command.ImageData);
        }

        public async Task Handle(Commands.ChangeEmail command, IMessageHandlerContext ctx)
        {
            var user = await _uow.For<User>().Get(command.CurrentUserId);
            user.ChangeEmail(command.Email);
        }

        public async Task Handle(Commands.ChangeName command, IMessageHandlerContext ctx)
        {
            var user = await _uow.For<User>().Get(command.CurrentUserId);
            user.ChangeName(command.Name);
        }

        public async Task Handle(Commands.ChangeTimezone command, IMessageHandlerContext ctx)
        {
            var user = await _uow.For<User>().Get(command.CurrentUserId);
            user.ChangeTimezone(command.Timezone);
        }
    }
}