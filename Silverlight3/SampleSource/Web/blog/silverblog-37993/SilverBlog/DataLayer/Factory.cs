using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Metadata.Edm;
using System.Reflection;
using System.Data.EntityClient;
using System.Data.SqlClient;

namespace SilverBlog.DataLayer
{
    public static class Factory
    {
        public static Model.ModelContainer GetContainer()
        {
            MetadataWorkspace workspace = new MetadataWorkspace(new string[] { "res://*/" }, new Assembly[] { Assembly.GetExecutingAssembly() });

            SqlConnection sqlConnection = new SqlConnection(@"Data Source=.\SqlExpress;Initial Catalog=Test;Integrated Security=SSPI;");
            EntityConnection entityConnection = new EntityConnection(workspace, sqlConnection);

            return new Model.ModelContainer(entityConnection);        
        }
    }
}
