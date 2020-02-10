using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Ria;
using System.Windows.Shapes;

namespace Silverlight.Weblog.Client.DAL.RiaServices
{
    public static class EntityExtensions
    {
        public static bool Validate(this Entity entity)
        {
            return Validator.TryValidateObject(entity, 
                                               new ValidationContext(entity, null, null),
                                               new List<ValidationResult>());
        }
    }
}
