using System.IO.IsolatedStorage;

namespace Silverlight.Weblog.Client.Default.Widgets
{
    public class UserDetailsService : IUserDetailsService
    {
        private const string UsernameIsoStoreKey = "___Username";
        private const string EmailIsoStoreKey = "___Email";
        private const string UrlIsoStoreKey = "___Url";

        public bool HasUser
        {
            get { return IsolatedStorageSettings.ApplicationSettings.Contains(UsernameIsoStoreKey); }
        }

        public string Username
        {
            get { return IsolatedStorageSettings.ApplicationSettings[UsernameIsoStoreKey].ToString(); }
        }

        public string Email
        {
            get { return IsolatedStorageSettings.ApplicationSettings[EmailIsoStoreKey].ToString(); }
        }

        public string Url
        {
            get { return IsolatedStorageSettings.ApplicationSettings[UrlIsoStoreKey].ToString(); }
        }

        public void SaveUserDetails(string username, string email, string url)
        {
            IsolatedStorageSettings.ApplicationSettings[UsernameIsoStoreKey] = username;
            IsolatedStorageSettings.ApplicationSettings[EmailIsoStoreKey] = email;
            IsolatedStorageSettings.ApplicationSettings[UrlIsoStoreKey] = url;
        }

    }
}