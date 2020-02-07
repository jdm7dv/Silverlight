namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;

    public class Point3DCollection : Freezable, ICollection<Point3D>
    {
        private Point minPoint = new Point(double.PositiveInfinity, double.PositiveInfinity);
        private Point maxPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);

        private List<Point3D> points;

        public Point3DCollection()
        {
            this.points = new List<Point3D>();
        }

        public Point3DCollection(int capacity)
        {
            this.points = new List<Point3D>(capacity);
        }

        public Point3DCollection(IEnumerable<Point3D> collection)
        {
            this.points = new List<Point3D>(collection);
        }

        #region ICollection<Point3D> Members

        public void Add(Point3D item)
        {
            this.points.Add(item);
            OnChanged();
        }

        public void Clear()
        {
            this.points.Clear();
            OnChanged();
        }

        public bool Contains(Point3D item)
        {
            return this.points.Contains(item);
        }

        public void CopyTo(Point3D[] array, int arrayIndex)
        {
            this.points.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.points.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Point3D item)
        {
            bool result = this.points.Remove(item);
            if (result)
            {
                OnChanged();
            }
            return result;
        }

        #endregion

        #region IEnumerable<Point3D> Members

        public IEnumerator<Point3D> GetEnumerator()
        {
            return this.points.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.points.GetEnumerator();
        }

        public Point3D this[int index]
        {
            get
            {
                return this.points[index];
            }
        }

        #endregion
    }
}
