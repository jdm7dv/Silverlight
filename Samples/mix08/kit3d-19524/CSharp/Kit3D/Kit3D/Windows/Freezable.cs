namespace Kit3D.Windows
{
    using System;

    public class Freezable
    {
        public event EventHandler Changed;

        protected Freezable()
        {
        }

        protected virtual void OnChanged()
        {
            EventHandler handler = Changed;
            if (handler != null)
            {
                //TODO: What event args do we want here, check WPF
                handler(this, null);
            }
        }
    }
}
