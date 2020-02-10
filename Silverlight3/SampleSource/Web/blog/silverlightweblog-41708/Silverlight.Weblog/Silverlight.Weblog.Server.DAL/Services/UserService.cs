using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using Silverlight.Weblog.Server.DAL.PartialClasses;

namespace Silverlight.Weblog.Server.DAL.Services
{
    public class UserService : IUserService
    {
        [Dependency]
        public IRepository<User> UserRepository { get; set; }

        public bool IsValid(string Username, string Password)
        {
            string passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "sha1");
            return UserRepository.Get().Any(
                u => u.Username == Username
                  && u.PasswordHash == passwordHash);
        }

        public User GetUserByUserName(string username)
        {
            return UserRepository.Get().First(u => u.Username == username);
        }

        public User GetUserByUserNameOrDefaultUser(string username)
        {
            User ReturnValue = UserRepository.Get()
                                             //.With(u => u.Posts)
                                             .FirstOrDefault(u => u.Username == username);


            if (ReturnValue != null)
                return ReturnValue;
            else
                return UserRepository.Get()
                                     //.With(u => u.Posts)
                                     .First(u => u.IsDefaultBlog.HasValue && u.IsDefaultBlog.Value);
        }
    }
}
