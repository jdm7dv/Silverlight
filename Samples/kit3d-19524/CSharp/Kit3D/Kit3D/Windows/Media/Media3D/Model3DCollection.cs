namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Collections.Generic;

    public class Model3DCollection : Freezable, ICollection<Model3D>
    {
        internal delegate void TransformChangedEventHandler();
        internal event TransformChangedEventHandler TransformChanged;

        internal delegate void MaterialChangedEventHandler();
        internal event MaterialChangedEventHandler MaterialChanged;

        internal delegate void GeometryChangedEventHandler(Geometry3D.GeometryChangeType changeType);
        internal event GeometryChangedEventHandler GeometryChanged;

        List<Model3D> items;

        public Model3DCollection()
        {
            this.items = new List<Model3D>();
        }

        public Model3DCollection(int capacity)
        {
            this.items = new List<Model3D>(capacity);
        }

        public Model3DCollection(IEnumerable<Model3D> collection)
        {
            this.items = new List<Model3D>(collection);
        }

        #region ICollection<Model3D> Members

        public void Add(Model3D item)
        {
            this.items.Add(item);
            AddHandlers(item);
            model_GeometryChanged(Geometry3D.GeometryChangeType.GeometrySet);
        }

        public void Clear()
        {
            foreach (Model3D model in this.items)
            {
                RemoveHandlers(model);
            }
            this.items.Clear();
            model_GeometryChanged(Geometry3D.GeometryChangeType.GeometrySet);
        }

        public bool Contains(Model3D item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(Model3D[] array, int arrayIndex)
        {
            throw new NotImplementedException();
            //this.items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Model3D item)
        {
            bool result = this.items.Remove(item);
            if (result)
            {
                RemoveHandlers(item);
                model_GeometryChanged(Geometry3D.GeometryChangeType.GeometrySet);
            }
            return result;
        }

        #endregion

        #region IEnumerable<Model3D> Members

        public IEnumerator<Model3D> GetEnumerator()
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

        public Model3D this[int index]
        {
            get
            {
                return this.items[index];
            }
            set
            {
                Model3D model = this.items[index];
                if (model != null)
                {
                    RemoveHandlers(model);
                }
                this.items[index] = value;
                model_GeometryChanged(Geometry3D.GeometryChangeType.GeometrySet);
            }
        }

        private void AddHandlers(Model3D model)
        {
            model.TransformChanged += new Model3D.TransformChangedEventHandler(model_TransformChanged);
            model.MaterialChanged += new Model3D.MaterialChangedEventHandler(model_MaterialChanged);
            model.GeometryChanged += new Model3D.GeometryChangedEventHandler(model_GeometryChanged);
        }

        private void RemoveHandlers(Model3D model)
        {
            model.TransformChanged -= new Model3D.TransformChangedEventHandler(model_TransformChanged);
            model.MaterialChanged -= new Model3D.MaterialChangedEventHandler(model_MaterialChanged);
            model.GeometryChanged -= new Model3D.GeometryChangedEventHandler(model_GeometryChanged);
        }

        private void model_GeometryChanged(Geometry3D.GeometryChangeType changeType)
        {
            GeometryChangedEventHandler handler = GeometryChanged;
            if (handler != null)
            {
                handler(changeType);
            }
        }

        private void model_MaterialChanged()
        {
            MaterialChangedEventHandler handler = MaterialChanged;
            if (handler != null)
            {
                handler();
            }
        }

        private void model_TransformChanged()
        {
            TransformChangedEventHandler handler = TransformChanged;
            if (handler != null)
            {
                handler();
            }
        }
    }
}
