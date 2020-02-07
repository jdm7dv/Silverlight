using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silverlight.Weblog.Server.DAL.Services.Interfaces
{
    public interface IUserService
    {
        bool IsValid(string Username, string Password);
        User GetUserByUserName(string username);
        User GetUserByUserNameOrDefaultUser(string username);
    }
}
