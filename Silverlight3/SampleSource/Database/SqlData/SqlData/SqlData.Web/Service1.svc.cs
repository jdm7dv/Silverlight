using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SqlData.Web
{
   // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config.
   public class Service1 : IService1
   {

      #region IService1 Members

      public List<Customer> GetCustomersByLastName( string lastName )
      {
         DataClasses1DataContext db = new DataClasses1DataContext();
         var matchingCustomers = from cust in db.Customers
                                 where cust.LastName.StartsWith( lastName )
                                 select cust;
         return matchingCustomers.ToList();
      }

      #endregion
   }
}
