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

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Text;

namespace OOBWindows
{
    public class DataHelper
    {
        static string[] firstNames = { "Michelle", "James", "Tina", "Danny", "Todd", "Bill", "Jim", "Jamie", "Tim", "John", "Loretta", "Rachel", "Melissa", "Kaylene", "Lori" };
        static string[] lastNames = { "Jones", "Johnson", "Riggs", "Wahlin", "Carey", "Cody", "Davies", "Riscovich", "Garfield", "Gibson", "Ferguson", "Fillmore", "Fisher", "Olson", "Ortiz" };
        static string[] states = { "Arizona", "Arkansas", "California", "Colorado", "Delaware", "Hawaii", "Georgia", "Idaho", "Illinois", "Iowa", "Kansas", "Kentucky", "Lousiana", "Maine", "North Carolina" };
        
        public static List<Customer> GetCustomers()
        {
            List<Customer> custs = new List<Customer>();
            for (int i = 0; i < 15; i++)
            {
                Customer cust = new Customer();
                cust.CustomerID = i;
                cust.FirstName = firstNames[i];
                cust.LastName = lastNames[i];
                cust.State = states[i];
                cust.Phone = "123-123-1234";
                custs.Add(cust);
            }
            return custs;
        }

        public static string GetCustomersAsHtmlTable()
        {
            List<Customer> custs = GetCustomers();
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='400'>");
            foreach (var cust in custs)
            {
                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append(cust.FirstName);
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(cust.LastName);
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(cust.Phone);
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(cust.State);
                sb.Append("</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
    }
}