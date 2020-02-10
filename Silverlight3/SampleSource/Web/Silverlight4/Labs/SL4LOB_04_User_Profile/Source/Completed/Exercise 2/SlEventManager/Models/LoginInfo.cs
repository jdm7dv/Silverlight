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

namespace SlEventManager.LoginUI
{
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using SlEventManager.Web.Resources;
    using SlEventManager.Resources;

    /// <summary>
    ///     This internal entity is used to ease the binding between the UI controls
    ///     (DataForm and the label displaying a validation error) and the log on
    ///     credentials entered by the user
    /// </summary>
    public class LoginInfo : Entity
    {
        [Display(Name = "UserNameLabel", ResourceType = typeof(RegistrationDataResources))]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "PasswordLabel", ResourceType = typeof(RegistrationDataResources))]
        [Required]
        public string Password { get; set; }

        [Display(Name = "RememberMeLabel", ResourceType = typeof(ApplicationStrings))]
        public bool RememberMe { get; set; }

        /// <summary>
        ///     Creates a new <see cref="System.ServiceModel.DomainServices.Client.ApplicationServices.LoginParameters"/>
        ///     using the data stored in this entity
        /// </summary>
        public System.ServiceModel.DomainServices.Client.ApplicationServices.LoginParameters ToLoginParameters()
        {
            return new System.ServiceModel.DomainServices.Client.ApplicationServices.LoginParameters(this.UserName, this.Password, this.RememberMe, null);
        }
    }
}