namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Collections.Generic;

    public class Transform3DCollection : Freezable, ICollection<Transform3D>
    {
        private List<Transform3D> items;

        public Transform3DCollection()
        {
            this.items = new List<Transform3D>();
        }

        public Transform3DCollection(int capacity)
        {
            this.items = new List<Transform3D>(capacity);
        }

        public Transform3DCollection(IEnumerable<Transform3D> collection)
        {
            this.items = new List<Transform3D>(collection);
        }

        #region ICollection<Transform3D> Members

        public void Add(Transform3D item)
        {
            this.items.Add(item);
            item.Changed += new EventHandler(item_Changed);
            OnChanged();
        }

        void item_Changed(object sender, EventArgs e)
        {
            OnChanged();
        }

        public void Clear()
        {
            foreach (Transform3D transform in this.items)
            {
                transform.Changed -= new EventHandler(item_Changed);
            }
            this.items.Clear();
            OnChanged();
        }

        public bool Contains(Transform3D item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(Transform3D[] array, int arrayIndex)
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

        public bool Remove(Transform3D item)
        {
            bool result = this.items.Remove(item);
            if (result)
            {
                item.Changed -= new EventHandler(item_Changed);
                OnChanged();
            }
            return result;
        }

        #endregion

        #region IEnumerable<Transform3D> Members

        public IEnumerator<Transform3D> GetEnumerator()
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

        public Transform3D this[int index]
        {
            get
            {
                return this.items[index];
            }
            set
            {
                if (index < this.items.Count)
                {
                    this.items[index].Changed -= new EventHandler(item_Changed);
                }
                this.items[index] = value;
            }
        }
    }
}
