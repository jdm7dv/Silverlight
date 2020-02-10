
using System;
using Microsoft.Practices.Unity;

namespace Silverlight.Weblog.Common.IoC
{
    public static class IoC
    {
       private static UnityContainer container = new UnityContainer();

       public static void Register<TFrom, TTo>()
           where TTo : TFrom
       {
           container.RegisterType<TFrom, TTo>(); 
       }

       public static void Register(Type from, Type to)
       {
           container.RegisterType(from, to);
       }

       public static void Register(Type from, object instance)
       {
           container.RegisterInstance(from, instance);
       }

        public static T Get<T>()
        {
            return container.Resolve<T>();
        }

        public static object Get(Type type)
        {
            return container.Resolve(type);
        }

        public static void Register<T>(T instance)
        {
            container.RegisterInstance<T>(instance);
        }

        public static void BuildUp<T>(T t)
        {
            container.BuildUp(t);
        }

        public static void Clear()
        {
            container = new UnityContainer();
        }
    }

}
