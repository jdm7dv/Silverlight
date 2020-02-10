using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.DomainServices;
using System.Web.Ria.Data;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL.PartialClasses;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;

namespace Silverlight.Weblog.Server.DAL
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public string Password { get; set; }
    }

    
    public class UserMetadata
    {
        [Include]
        public object Posts;

        [Exclude]
        public string PasswordHash; 
    }
}