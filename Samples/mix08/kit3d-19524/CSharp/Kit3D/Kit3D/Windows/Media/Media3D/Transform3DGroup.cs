namespace Kit3D.Windows.Media.Media3D
{
    using System;

    public class Transform3DGroup : Transform3D
    {
        private Transform3DCollection children;

        public Transform3DGroup()
        {
            this.Children = new Transform3DCollection();
        }

        public Transform3DCollection Children
        {
            get { return this.children; }
            set
            {
                if (this.children != null)
                {
                    this.children.Changed -= new EventHandler(children_Changed);
                }
                this.children = value;
                if (this.children != null)
                {
                    this.children.Changed += new EventHandler(children_Changed);
                }
                OnChanged();
            }
        }

        private void children_Changed(object sender, EventArgs e)
        {
            OnChanged();
        }

        public override Matrix3D Value
        {
            get
            {
                if ((this.children == null) || 
                    (this.children.Count == 0))
                {
                    return Matrix3D.Identity;
                }

                Matrix3D result = Matrix3D.Identity;
                foreach (Transform3D transform in this.children)
                {
                    result.Append(transform.Value);
                }
                return result;
            }
        }
    }
}
