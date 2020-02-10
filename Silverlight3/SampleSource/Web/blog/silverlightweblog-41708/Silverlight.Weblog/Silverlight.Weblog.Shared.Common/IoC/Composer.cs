using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Silverlight.Weblog.Shared.Common.IoC
{
    public static class Composer
    {
        private static AggregateCatalog _catalog = new AggregateCatalog();
        private static CompositionContainer _container = new CompositionContainer(_catalog);

        public static void AddAssembly(Assembly assembly)
        {
            _catalog.Catalogs.Add(new AssemblyCatalog(assembly));
        }

        public static void Compose<T>(T ComposableObject)
        {
            PartInitializer.SatisfyImports(ComposableObject);
        }

        public static void Reset()
        {
            _catalog = new AggregateCatalog();
            _container = new CompositionContainer();
        }
    }
}
