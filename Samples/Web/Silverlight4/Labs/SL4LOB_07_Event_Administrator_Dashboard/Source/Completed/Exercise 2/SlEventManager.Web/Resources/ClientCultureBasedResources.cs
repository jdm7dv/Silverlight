// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace SlEventManager.Web.Resources
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web;

    internal static class ClientCultureBasedResources
    {
        /// <summary>
        ///    Returns the CultureInfo that represents the culture for looking up
        ///    resources.
        ///    
        ///    The default implementation will try to load the culture from the
        ///    'SlEventManager-culture' cookie (which gets set by the Siverlight ASPX
        ///    wrapper, see SlEventManagerTestPage.aspx). If there is no such
        ///    cookie, this will use the preferred language as sent by the browser, although
        ///    be aware that IE does not send this information.
        /// 
        ///    Change this if you want to change or enhance this logic (for example,
        ///    if your app lets the user change the display language and store that
        ///    as a profile setting, you should change this method so it queries for
        ///    that profile setting if the user is logged on)
        /// </summary>
        private static CultureInfo CurrentCulture
        {
            get
            {
                string clientCulture = HttpContext.Current.Request.Cookies["SlEventManager-culture"].Value;
                if (clientCulture != null)
                {
                    return CultureInfo.GetCultureInfo(clientCulture);
                }
                else
                {
                    // Not guaranteed to have the correct value.
                    return Thread.CurrentThread.CurrentCulture;
                }
            }
        }

        /// <summary>
        ///     Ensures any resource strings accessed by <paramref name="resourceGrabber" /> will
        ///     be returned in the culture the silverlight app is expecting.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static string GetResource(Func<string> resourceGrabber)
        {
            Thread.CurrentThread.CurrentUICulture = CurrentCulture;
            return resourceGrabber();
        }
    }
}