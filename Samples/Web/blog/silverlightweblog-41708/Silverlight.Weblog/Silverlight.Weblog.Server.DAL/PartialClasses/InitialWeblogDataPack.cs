using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.DomainServices;
using Silverlight.Weblog.Server.DAL;

namespace Silverlight.Weblog.UI.Web.RiaServices
{
    [DataContract]
    public class InitialWeblogDataPack 
    {
        /// <summary>
        /// This property does absoloutely nothing. 
        /// Ria Services vehemently insists each entity needs a key - fine, here's one. 
        /// </summary>
        [Key()]
        public int MockKey { get; set; }

        [Include]
        [DataMember]
        [Association("RequiredForInclude1", "MockKey", "ID")]
        public User User { get; set;}
    }
}