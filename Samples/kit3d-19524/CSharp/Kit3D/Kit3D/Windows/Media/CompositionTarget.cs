namespace Kit3D.Windows.Media
{
    using System;
    using System.Windows.Threading;

    //See: http://msdn.microsoft.com/en-us/library/ms748838.aspx
    public static class CompositionTarget
    {
        public static event EventHandler Rendering;
        internal static event EventHandler RenderingComplete;

        static CompositionTarget()
        {
            System.Windows.Media.CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        static void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;

            //All public classes can hook in here
            EventHandler handler = Rendering;
            if (handler != null)
            {
                handler(null, null);
            }

            //Now inform internal classes they can render
            handler = RenderingComplete;
            if (handler != null)
            {
                handler(null, null);
            }
        }

    }
}
