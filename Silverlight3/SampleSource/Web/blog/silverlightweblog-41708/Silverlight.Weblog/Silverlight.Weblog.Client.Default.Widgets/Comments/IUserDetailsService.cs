namespace Silverlight.Weblog.Client.Default.Widgets
{
    public interface IUserDetailsService
    {
        bool HasUser { get;  }
        string Username { get;  }
        string Email { get; }
        string Url { get;  }
        void SaveUserDetails(string name, string email, string url);
    }
}