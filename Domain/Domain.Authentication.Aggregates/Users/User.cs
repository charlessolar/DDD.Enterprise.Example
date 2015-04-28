using Demo.Domain.Authentication.Users.Events;
using System;

namespace Demo.Domain.Authentication.Users
{
    public class User : Aggregates.Aggregate<String>, IUser
    {
        private User()
        {
        }

        public void Login(String Name, String Email, String NickName, String ImageType, String ImageData)
        {
            Apply<LoggedIn>(e =>
            {
                e.UserId = Id;
                e.Name = Name;
                e.Email = Email;
                e.NickName = NickName;
                e.ImageType = ImageType;
                e.ImageData = ImageData;
            });
        }

        public void Logout()
        {
            Apply<LoggedOut>(e =>
            {
                e.UserId = Id;
            });
        }

        public void ChangeAvatar(String imageType, String imageData)
        {
            Apply<AvatarChanged>(e =>
                {
                    e.UserId = Id;
                    e.ImageType = imageType;
                    e.ImageData = imageData;
                });
        }

        public void ChangeEmail(String email)
        {
            Apply<EmailChanged>(e =>
                {
                    e.UserId = Id;
                    e.Email = email;
                });
        }

        public void ChangeName(String name)
        {
            Apply<NameChanged>(e =>
            {
                e.UserId = Id;
                e.Name = name;
            });
        }

        public void ChangeTimezone(String timezone)
        {
            Apply<TimezoneChanged>(e =>
            {
                e.UserId = Id;
                e.Timezone = timezone;
            });
        }
    }
}