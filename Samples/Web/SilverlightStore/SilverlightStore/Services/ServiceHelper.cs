namespace SilverlightStore.Services
{
    using System.Windows;

    public static class ServiceHelper
    {
        public static TService FindService<TService>() where TService : class
        {
            foreach (var serviceEntry in Application.Current.ApplicationLifetimeObjects)
            {
                TService currentService = serviceEntry as TService;
                if (currentService != null)
                {
                    return currentService;
                }
            }

            return null;
        }
    }
}
