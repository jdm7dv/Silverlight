namespace Kit3D.Windows.Media
{
    using System;
    using Kit3D.Windows.Controls;
    using Kit3D.Windows.Media.Media3D;
    using System.Collections.Generic;

    public static class VisualTreeHelper
    {
        /// <summary>
        /// This method allows you to interact with a 3D scene.  This does not match the
        /// WPF parameter exactly since I cannot create the same inheritance chain, so the
        /// first paramter is a Viewport3D instead of a Visual.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="filterCallback">Must be null, currently not supported</param>
        /// <param name="resultCallback"></param>
        /// <param name="hitTestParameters"></param>
        public static void HitTest(Viewport3D visual, HitTestFilterCallback filterCallback, HitTestResultCallback resultCallback, HitTestParameters hitTestParameters)
        {
            if (visual == null)
            {
                return;
            }

            if (resultCallback == null)
            {
                return;
            }

            if (hitTestParameters == null)
            {
                return;
            }

            //Don't support this functionality right now
            if (filterCallback != null)
            {
                throw new ArgumentException("The filterCallback parameter must be null, no other value is currently supported");
            }

            //Get all of the ModelVisuals that intersect with this ray
            List<HitResult> hitResults = visual.HitTest3D(hitTestParameters as PointHitTestParameters);

            //Get all of the models/visuals that intersect with the point the user clicked on
            //then call the users callback to allow them to interact with the 3D scene.
            foreach(HitResult hitResult in hitResults)
            {
                RayMeshGeometry3DHitTestResult result = new RayMeshGeometry3DHitTestResult(hitResult);

                //The user can choose to stop the iteration of the ModelVisual3D instances
                //at any time if they have found the item they are interested in.
                HitTestResultBehavior r = resultCallback(result);
                if (r == HitTestResultBehavior.Stop)
                {
                    break;
                }
            }
        }
    }
}

