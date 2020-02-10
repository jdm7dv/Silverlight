using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SqlData
{
   public partial class Page : UserControl
   {
      public Page()
      {
         InitializeComponent();
         Loaded += new RoutedEventHandler( Page_Loaded );
      }

      void Page_Loaded( object sender, RoutedEventArgs e )
      {
         Search.Click += new RoutedEventHandler( Search_Click );
      }

      void Search_Click( object sender, RoutedEventArgs e )
      {
         ServiceReference1.Service1Client webService = new SqlData.ServiceReference1.Service1Client();
         webService.GetCustomersByLastNameCompleted +=
             new EventHandler<SqlData.ServiceReference1.GetCustomersByLastNameCompletedEventArgs>
                 ( webService_GetCustomersByLastNameCompleted );
         webService.GetCustomersByLastNameAsync( LastName.Text );
      }

      void webService_GetCustomersByLastNameCompleted(
          object sender,
         SqlData.ServiceReference1.GetCustomersByLastNameCompletedEventArgs e)
      {

         theDataGrid.ItemsSource = e.Result;
      }
   }
}
