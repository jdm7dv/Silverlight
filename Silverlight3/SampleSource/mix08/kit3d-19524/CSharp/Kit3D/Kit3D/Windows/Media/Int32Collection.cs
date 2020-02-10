namespace Kit3D.Windows.Media
{
    using System;
    using System.Collections.Generic;

    public class Int32Collection : Freezable, ICollection<Int32>
    {
        private List<Int32> items;

        public Int32Collection()
        {
            this.items = new List<int>();
        }

        public Int32Collection(int capacity)
        {
            this.items = new List<int>(capacity);
        }

        public Int32Collection(IEnumerable<Int32> collection)
        {
            this.items = new List<int>(collection);
        }

        #region ICollection<int> Members

        public void Add(int item)
        {
            this.items.Add(item);
            OnChanged();
        }

        public void Clear()
        {
            this.items.Clear();
            OnChanged();
        }

        public bool Contains(int item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(int[] array, int arrayIndex)
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

        public bool Remove(int item)
        {
            bool result = this.items.Remove(item);
            if (result)
            {
                OnChanged();
            }
            return result;
        }

        #endregion

        #region IEnumerable<int> Members

        public IEnumerator<int> GetEnumerator()
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

        public Int32 this[int index]
        {
            get
            {
                return this.items[index];
            }
        }
    }
}

