namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Windows.Media.Imaging;
    using System.IO;
    using System.Windows.Media;

    /// <summary>
    /// Wrapper around a brush. The reason we need this class is because
    /// when we are using an image brush we need to also have the width/height
    /// of the image associated with the brush, which we cannot get at the
    /// moment from the Silverlight APIs.
    /// </summary>
    public sealed class Kit3DBrush
    {
        public Kit3DBrush()
        {
        }

        public Kit3DBrush(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// You must use this constructor if you are using an image brush
        /// </summary>
        /// <param name="imageBrush"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Kit3DBrush(ImageBrush imageBrush, int width, int height) :
            this(width, height)
        {
            imageBrush.AlignmentX = AlignmentX.Left;
            imageBrush.AlignmentY = AlignmentY.Top;
            imageBrush.Stretch = Stretch.None;

            //TODO: Wish we could programatically get the width 
            //      of the image somehow.
            this.Brush = imageBrush;
        }

        /// <summary>
        /// Use this constructor for all brushes apart from the
        /// ImageBrush
        /// </summary>
        /// <param name="brush"></param>
        public Kit3DBrush(Brush brush)
        {
            this.Brush = brush;

            if (this.Brush is ImageBrush)
            {
                throw new ArgumentException("Invalid brush type passed to Kit3DBrush constructor, an ImageBrush must use the explicit ImageBrush constructor");
            }
        }

        public Kit3DBrush Clone()
        {
            Kit3DBrush b;

            if (this.Brush is ImageBrush)
            {
                ImageBrush imgBrush = new ImageBrush();
                imgBrush.AlignmentX = AlignmentX.Left;
                imgBrush.AlignmentY = AlignmentY.Top;
                imgBrush.Stretch = Stretch.None;

                //TODO: Match types of brushes

                //If the Bitmap image came from a URI source then we use that, otherwise
                //Uri uri = ((BitmapImage)((ImageBrush)this.Brush).ImageSource).UriSource;
                //imgBrush.ImageSource = new BitmapImage(new Uri(uri.OriginalString, (uri.IsAbsoluteUri) ? UriKind.Absolute : UriKind.Relative));

                //This is slower unless the original bitmap image was set from a source
                imgBrush.ImageSource = ((ImageBrush)this.Brush).ImageSource;
                b = new Kit3DBrush(imgBrush, this.Width, this.Height);
                b.IsOutlineBrush = this.IsOutlineBrush;
            }
            else
            {
                //TODO: Do we need to clone these brushes?
                b = this;
            }

            return b;
        }

        public bool IsOutlineBrush
        {
            get;
            set;
        }

        public Brush Brush
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }
    }
}
