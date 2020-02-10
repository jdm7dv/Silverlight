namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using Kit3D.Windows.Controls;
    using System.Collections.Generic;

    public abstract class Visual3D
    {
        internal delegate void BecameDirtyEventHandler();
        internal event BecameDirtyEventHandler BecameDirty;
        private Viewport3D viewport;

        internal abstract void Render(List<Triangle> trianglesToRender, List<ModelVisual3D> renderedVisuals);

        internal Viewport3D Viewport
        {
            get { return this.viewport; }
            set
            {
                this.viewport = value;
                OnViewportChanged();
            }
        }

        protected internal virtual void OnViewportChanged()
        {}

        protected internal void OnBecameDirty()
        {
            BecameDirtyEventHandler handler = BecameDirty;
            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        /// Returns the transform to convert points from the visual to the ancestor.
        /// Currently onnly ModelVisual3D instances work with this method.
        /// </summary>
        /// <param name="visual">Must be a Viewport3D instance otherwise the method returns null</param>
        /// <returns></returns>
        public GeneralTransform3DTo2D TransformToAncestor(Visual visual)
        {
            //TODO: Fully support - for now just support Viewport3D
            Viewport3D viewport = visual as Viewport3D;

            if(viewport == null)
            {
                return null;
            }

            if (viewport.Camera == null)
            {
                return null;
            }

            /*
            //The viewport should be the same viewport that the model visual
            //is assocaiated with
            if (viewport != this.Viewport)
            {
                return null;
            }
            */

            ModelVisual3D modelVisual = this as ModelVisual3D;
            if(modelVisual == null)
            {
                return null;
            }

            Matrix3D matrix = modelVisual.Transform.Value;

            //Calculate the camera transform to NDC values then the NDC to screen values
            matrix.Append(viewport.Camera.Value);
            matrix.Append(viewport.NDCToScreenTransform);

            //TODO: Remove
            //matrix.Append(viewport.Camera.WorldToScreenTransform);

            return new GeneralTransform3DTo2D(matrix);
        }
    }
}
