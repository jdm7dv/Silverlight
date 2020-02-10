namespace SilverlightStore.Services
{
    using System.Windows;
    using System.Windows.Browser;

    public class ClipboardService : IApplicationService
    {
        private ScriptObject _clipboardData;

        public static ClipboardService Current
        {
            get
            {
                return ServiceHelper.FindService<ClipboardService>();
            }
        }

        public string Contents
        {
            get
            {
                string value = null;
                
                if (this._clipboardData != null)
                {
                    value = this._clipboardData.Invoke("getData", "Text") as string;
                }

                return value;
            }
            set
            {
                if (this._clipboardData != null)
                {
                    this._clipboardData.Invoke("setData", "Text", value);
                }
            }
        }

        public void StartService(ApplicationServiceContext context)
        {
            this._clipboardData = HtmlPage.Window.GetProperty("clipboardData") as ScriptObject;
        }

        public void StopService()
        {
            this._clipboardData = null;
        }
    }
}
