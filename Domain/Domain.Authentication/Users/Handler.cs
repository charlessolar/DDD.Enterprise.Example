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
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public void Handle(Commands.Login command)
        {
            var user = _uow.R<User>().Get(command.UserId);

            if (user == null)
                user = _uow.R<User>().New(command.UserId);

            user.Login(command.Name, command.Email, command.NickName, command.ImageType, command.ImageData);
        }

        public void Handle(Commands.Logout command)
        {
            var user = _uow.R<User>().Get(command.UserId);
            user.Logout();
        }

        public void Handle(Commands.ChangeAvatar command)
        {
            var user = _uow.R<User>().Get(command.UserId);
            user.ChangeAvatar(command.ImageType, command.ImageData);
        }

        public void Handle(Commands.ChangeEmail command)
        {
            var user = _uow.R<User>().Get(command.UserId);
            user.ChangeEmail(command.Email);
        }

        public void Handle(Commands.ChangeName command)
        {
            var user = _uow.R<User>().Get(command.UserId);
            user.ChangeName(command.Name);
        }

        public void Handle(Commands.ChangeTimezone command)
        {
            var user = _uow.R<User>().Get(command.UserId);
            user.ChangeTimezone(command.Timezone);
        }
    }
}