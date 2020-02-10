namespace Kit3D.Windows.Media
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;

    public class PointCollection : Freezable, ICollection<Point>
    {
        private List<Point> points;

        public PointCollection()
        {
            this.points = new List<Point>();
        }

        public PointCollection(int capacity)
        {
            this.points = new List<Point>(capacity);
        }

        public PointCollection(IEnumerable<Point> collection)
        {
            this.points = new List<Point>(collection);
        }

        #region ICollection<Point> Members

        public void Add(Point item)
        {
            this.points.Add(item);
            OnChanged();
        }

        public void Clear()
        {
            this.points.Clear();
            OnChanged();
        }

        public bool Contains(Point item)
        {
            return this.points.Contains(item);
        }

        public void CopyTo(Point[] array, int arrayIndex)
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

        public bool Remove(Point item)
        {
            bool result = this.points.Remove(item);
            if (result)
            {
                OnChanged();
            }
            return result;
        }

        #endregion

        #region IEnumerable<Point> Members

        public IEnumerator<Point> GetEnumerator()
        {
            return this.points.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.points.GetEnumerator();
        }

        public Point this[int index]
        {
            get
            {
                return this.points[index];
            }
            //TODO: Set implementation
        }

        #endregion
    }
}

