using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.DomainServices;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL.PartialClasses;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;

namespace Silverlight.Weblog.Server.DAL
{
    [MetadataType(typeof(CommentMetadata))]
    public partial class Comment 
    {
    }

    public class CommentMetadata
    {
        [Include]
        public object BlogPost;

        [Include]
        public object ParentComment;

        [Required(ErrorMessage = "Please write down your Name.")]
        public object Name;

        [Required(ErrorMessage = "Please write down your Email.")]
        public object Email;

        [Required(ErrorMessage = "Please write down your Comments.")]
        public object HTML;
    }
}