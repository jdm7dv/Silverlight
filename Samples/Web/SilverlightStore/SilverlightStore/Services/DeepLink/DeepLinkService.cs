namespace SilverlightStore.Services
{
    using System.Windows;

    public class DeepLinkService : IApplicationService
    {
        public const string DeepLinkInitParamsKey = "DeepLink";

        private string _deepLinkValue;

        public static DeepLinkService Current
        {
            get
            {
                return ServiceHelper.FindService<DeepLinkService>();
            }
        }

        public string DeepLink
        {
            get
            {
                return this._deepLinkValue;
            }
        }

        public void StartService(ApplicationServiceContext context)
        {
            context.ApplicationInitParams.TryGetValue(DeepLinkInitParamsKey, out this._deepLinkValue);
        }

        public void StopService()
        {
            return;
        }
    }
}
