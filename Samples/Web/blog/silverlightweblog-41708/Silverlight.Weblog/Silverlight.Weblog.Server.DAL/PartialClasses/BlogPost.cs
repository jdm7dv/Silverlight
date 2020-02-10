using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.DomainServices;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL.PartialClasses;
using Silverlight.Weblog.Server.DAL.Services;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;

namespace Silverlight.Weblog.Server.DAL
{
    [MetadataType(typeof(BlogPostMetadata))]
    public partial class BlogPost
    {
        [Dependency]

        protected IRepository<PostCategory> PostCategoryRepository { get; set; }

        public void SetTitleToPermlink(string title)
        {
                    // do not change permlink aftert it was initialized
            if (!string.IsNullOrEmpty(Permlink))
                return;

            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(title))
            {
                string[] AllowedCharcatersInTitleURL = new string[]
                                                           {
                                                               "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k",
                                                               "l", "m", "n", "o", "p",
                                                               "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                                               "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
                                                           };

                bool IsLastCharAllowed = false;
                foreach (char curChar in title)
                {
                    bool IsCurCharAllowedInURL = AllowedCharcatersInTitleURL.Contains(curChar.ToString().ToLower());

                    if (IsCurCharAllowedInURL)
                    {
                        // if the last char is allowed - make this char lower case
                        if (IsLastCharAllowed)
                            sb.Append(curChar.ToString().ToLower());
                            // or if the last char wasn't allowed, make this char uppper case
                        else
                            sb.Append(curChar.ToString().ToUpper());
                    }

                    IsLastCharAllowed = IsCurCharAllowedInURL;
                }
            }
            
            if (!string.IsNullOrEmpty(sb.ToString()))
                Permlink = sb.ToString();
            else
                Permlink = Guid.NewGuid().ToString();
        }
    }

    public class BlogPostMetadata
    {
        [Include]
        public object User;
        [Include]
        public object Comments;

        [Include]
        public object PostCategories;

        [Include]
        [Association("Categories_BlogPost", "ID", "ID", IsForeignKey = true)]
        public object Categories;
    }
}
