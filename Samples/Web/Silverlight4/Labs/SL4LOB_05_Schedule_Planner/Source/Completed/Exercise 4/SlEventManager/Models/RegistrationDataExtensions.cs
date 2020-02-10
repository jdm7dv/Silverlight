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

namespace SlEventManager.Web
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using SlEventManager.Web.Resources;

    public partial class RegistrationData
    {
        #region Password Confirmation Field and Validation
        private string _passwordConfirmation;

        /// <summary>
        ///     Stores the password the user entered in the registration UI, even if it is
        ///     invalid. This way we can validate the password confirmation adequately all
        ///     the times
        /// </summary>
        /// <remarks>
        ///     This gets set on the <see cref="System.Windows.Controls.PasswordBox.PasswordChanged"/> event
        /// </remarks>
        [Display(AutoGenerateField = false)]
        internal string ActualPassword { get; set; }

        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Order = 4, Name = "PasswordConfirmationLabel", ResourceType = typeof(RegistrationDataResources))]
        [CustomValidation(typeof(RegistrationData), "CheckPasswordConfirmation")]
        public string PasswordConfirmation
        {
            get
            {
                return this._passwordConfirmation;
            }

            set
            {
                this.ValidateProperty("PasswordConfirmation", value);
                this._passwordConfirmation = value;
                this.RaisePropertyChanged("PasswordConfirmation");
            }
        }

        public static ValidationResult CheckPasswordConfirmation(string passwordConfirmation, ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException("validationContext");
            }

            RegistrationData registrationData = (RegistrationData)validationContext.ObjectInstance;

            if (registrationData.ActualPassword == passwordConfirmation)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorResources.ValidationErrorPasswordConfirmationMismatch, new string[] { "PasswordConfirmation" });
        }
        #endregion

        #region Make DisplayName Bindable
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            base.OnPropertyChanged(e);

            if (e.PropertyName == "UserName" || e.PropertyName == "FriendlyName")
            {
                this.RaisePropertyChanged("DisplayName");
            }
        }
        #endregion

        #region Convenience Methods
        /// <summary>
        ///     Creates a new <see cref="System.ServiceModel.DomainServices.Client.ApplicationServices.LoginParameters"/>
        ///     initialized with this entity's data (IsPersistent will default to false)
        /// </summary>
        public System.ServiceModel.DomainServices.Client.ApplicationServices.LoginParameters ToLoginParameters()
        {
            return new System.ServiceModel.DomainServices.Client.ApplicationServices.LoginParameters(this.UserName, this.Password, false, null);
        }
        #endregion
    }
}