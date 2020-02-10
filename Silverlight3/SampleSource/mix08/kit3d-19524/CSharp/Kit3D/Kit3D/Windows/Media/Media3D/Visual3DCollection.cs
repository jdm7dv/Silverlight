namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Collections.Generic;
    using Kit3D.Windows.Controls;

    //TODO: Visuals should not be allowed to belong to more than one
    //      viewport at any time.
    public class Visual3DCollection : Freezable, ICollection<Visual3D>
    {
        private List<Visual3D> items;

        public Visual3DCollection()
        {
            this.items = new List<Visual3D>();
        }

        public Visual3DCollection(int capacity)
        {
            this.items = new List<Visual3D>(capacity);
        }

        public Visual3DCollection(IEnumerable<Visual3D> collection)
        {
            this.items = new List<Visual3D>(collection);
        }

        #region ICollection<Visual3D> Members

        public void Add(Visual3D item)
        {
            this.items.Add(item);
            item.BecameDirty += new Visual3D.BecameDirtyEventHandler(item_BecameDirty);
            item.Viewport = this.Viewport;
            OnChanged();
        }

        void item_BecameDirty()
        {
            OnChanged();
        }

        public void Clear()
        {
            foreach (Visual3D visual in this.items)
            {
                visual.Viewport = null;
                visual.BecameDirty -= new Visual3D.BecameDirtyEventHandler(item_BecameDirty);
            }

            this.items.Clear();
            OnChanged();
        }

        public bool Contains(Visual3D item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(Visual3D[] array, int arrayIndex)
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

        public bool Remove(Visual3D item)
        {
            bool result = this.items.Remove(item);
            if (result)
            {
                item.BecameDirty -= new Visual3D.BecameDirtyEventHandler(item_BecameDirty);
                item.Viewport = null;
                OnChanged();
            }
            return result;
        }

        #endregion

        #region IEnumerable<Visual3D> Members

        public IEnumerator<Visual3D> GetEnumerator()
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

        public Visual3D this[int index]
        {
            get
            {
                return this.items[index];
            }
        }

        internal void Render(List<Triangle> trianglesToRender, List<ModelVisual3D> renderedVisuals)
        {
            foreach (Visual3D visual in this.items)
            {
                visual.Render(trianglesToRender, renderedVisuals);
            }
        }

        internal Viewport3D Viewport
        {
            //TODO: Add remove event handlers, when removing take away the viewport property
            get;
            set;
        }
    }
}
