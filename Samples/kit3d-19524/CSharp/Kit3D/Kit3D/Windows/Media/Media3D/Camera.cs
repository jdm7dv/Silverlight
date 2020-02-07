namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using Kit3D.Windows.Controls;
    using Kit3D.Windows.Media.Animation;

    public abstract class Camera : Animatable
    {
        private Viewport3D viewport;

        protected Camera()
        {
        }

        /// <summary>
        /// The viewport associated with the camera
        /// </summary>
        internal Viewport3D Viewport
        {
            get
            {
                return this.viewport;
            }
            set
            {
                this.viewport = value;

                //TODO:Do we really need to signal this here?  Check.
                OnChanged();
            }
        }

        /// <summary>
        /// Returns the overall transform that the camera applied to world points
        /// </summary>
        internal abstract Matrix3D Value
        {
            get;
        }

        /// <summary>
        /// Indicates that the camera should re-evaluate its internal state
        /// </summary>
        internal abstract void Invalidate();
    }
}
