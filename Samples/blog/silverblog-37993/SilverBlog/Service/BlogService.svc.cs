using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;

namespace SilverBlog.Service
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BlogService
    {
        [OperationContract]
        public DTO.Entry TestService()
        {
            // Add your operation implementation here
            return BusinessLayer.Entry.Testmethod();
        }

        [OperationContract]
        public void CreateEntry(DTO.Entry entry)
        {
            BusinessLayer.Entry.CreateEntry(entry);
        }

        [OperationContract]
        public IEnumerable<DTO.Entry> GetLast10Entries()
        {
            IEnumerable<DTO.Entry> list = BusinessLayer.Entry.GetLast10Entries();
            return list;
        }
    }
}
