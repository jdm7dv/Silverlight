namespace SilverlightStore.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    public static class DependencyObjectExtensions
    {
        public static TChildType FindChild<TChildType>(this DependencyObject dependencyObject) where TChildType : DependencyObject
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            int childCount = VisualTreeHelper.GetChildrenCount(dependencyObject);

            for (int i = 0; i < childCount; ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);

                if (child is TChildType)
                {
                    return (TChildType)child;
                }

                child = child.FindChild<TChildType>();

                if (child != null)
                {
                    return (TChildType)child;
                }
            }

            return null;
        }

        public static IEnumerable<TChildType> FindChildren<TChildType>(this DependencyObject dependencyObject) where TChildType : DependencyObject
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            List<TChildType> children = new List<TChildType>();

            int childCount = VisualTreeHelper.GetChildrenCount(dependencyObject);

            for (int i = 0; i < childCount; ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);

                if (child is TChildType)
                {
                    children.Add((TChildType)child);
                }

                child = child.FindChild<TChildType>();

                if (child != null)
                {
                    children.Add((TChildType)child);
                }
            }

            return children;
        }
    }
}
