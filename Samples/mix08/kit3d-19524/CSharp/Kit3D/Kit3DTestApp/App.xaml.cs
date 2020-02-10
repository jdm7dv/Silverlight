using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TestApp
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Uncomment to see the different demos

            //this.RootVisual = new BoundsTesting();
            //this.RootVisual = new Sphere();
            //this.RootVisual = new BackMaterial();
            this.RootVisual = new TigerSolid();
            //this.RootVisual = new TigerTexture();
            //this.RootVisual = new CelestialBodies();
            //this.RootVisual = new AnimateAxis();
            //this.RootVisual = new Dodecahedron();
            //this.RootVisual = new RotatingCubes();
            //this.RootVisual = new HitTesting();
            //this.RootVisual = new HitTesting2();
            //this.RootVisual = new GeneralTransformTesting();
            //this.RootVisual = new BaseSample();
        }

        private void Application_Exit(object sender, EventArgs e)
        { }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        { }
    }
}