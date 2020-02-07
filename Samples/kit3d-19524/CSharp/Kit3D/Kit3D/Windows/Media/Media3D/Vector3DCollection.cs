namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Collections.Generic;

    public class Vector3DCollection : Freezable, ICollection<Vector3D>
    {
        private List<Vector3D> items;

        public Vector3DCollection()
        {
            this.items = new List<Vector3D>();
        }

        public Vector3DCollection(int capacity)
        {
            this.items = new List<Vector3D>(capacity);
        }

        public Vector3DCollection(IEnumerable<Vector3D> collection)
        {
            this.items = new List<Vector3D>(collection);
        }

        #region ICollection<Vector3D> Members

        public void Add(Vector3D item)
        {
            this.items.Add(item);
            OnChanged();
        }

        public void Clear()
        {
            this.items.Clear();
            OnChanged();
        }

        public bool Contains(Vector3D item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(Vector3D[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Vector3D item)
        {
            bool result = this.items.Remove(item);
            if (result)
            {
                OnChanged();
            }
            return result;
        }

        #endregion

        #region IEnumerable<Vector3D> Members

        public IEnumerator<Vector3D> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        #endregion

        public Vector3D this[int index]
        {
            get
            {
                return this.items[index];
            }
        }
    }
}
