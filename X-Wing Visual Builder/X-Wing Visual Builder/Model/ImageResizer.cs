using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace X_Wing_Visual_Builder.Model
{
    public static class ImageResizer
    {
        public static Image Resize(Image image, Size size)
        {
            if (image == null || size.IsEmpty)
                return null;

            var resizedImage = new Bitmap(size.Width, size.Height, image.PixelFormat);
            resizedImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(resizedImage))
            {
                var location = new Point(0, 0);
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.DrawImage(image, new Rectangle(location, size),
                                   new Rectangle(location, image.Size), GraphicsUnit.Pixel);
            }

            return resizedImage;
        }

        public static TransformedBitmap ResizeImageWpf(BitmapImage myBitmapImage, Size size)
        {
            // BitmapSource objects like BitmapImage can only have their properties
            // changed within a BeginInit/EndInit block.
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(@"C:\Documents and Settings\All Users\Documents\My Pictures\Sample Pictures\Water Lilies.jpg");

            // To save significant application memory, set the DecodePixelWidth or  
            // DecodePixelHeight of the BitmapImage value of the image source to the desired 
            // height or width of the rendered image. If you don't do this, the application will 
            // cache the image as though it were rendered as its normal size rather then just 
            // the size that is displayed.
            // Note: In order to preserve aspect ratio, set DecodePixelWidth
            // or DecodePixelHeight but not both.
            myBitmapImage.DecodePixelWidth = 200;
            myBitmapImage.EndInit();

            /////////////////// Create a BitmapSource that Rotates the image //////////////////////
            // Use the BitmapImage created above as the source for a new BitmapSource object
            // that will be scaled to a different size. Create a new BitmapSource by   
            // scaling the original one.                                               
            // Note: New BitmapSource does not cache. It is always pulled when required.

            // Create the new BitmapSource that will be used to scale the size of the source.
            TransformedBitmap myRotatedBitmapSource = new TransformedBitmap();

            // BitmapSource objects like TransformedBitmap can only have their properties
            // changed within a BeginInit/EndInit block.
            myRotatedBitmapSource.BeginInit();

            // Use the BitmapSource object defined above as the source for this BitmapSource.
            // This creates a "chain" of BitmapSource objects which essentially inherit from each other.
            myRotatedBitmapSource.Source = myBitmapImage;

            // Flip the source 90 degrees.
            myRotatedBitmapSource.Transform = new ScaleTransform(size.Width, size.Height);
            myRotatedBitmapSource.EndInit(); 
            return myRotatedBitmapSource;
        }


        public static BitmapImage ResizeImage(Image image, Size size)
        {
            if (image == null || size.IsEmpty)
                return null;

            var resizedImage = new Bitmap(size.Width, size.Height, image.PixelFormat);
            resizedImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(resizedImage))
            {
                var location = new Point(0, 0);
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.DrawImage(image, new Rectangle(location, size),
                                    new Rectangle(location, image.Size), GraphicsUnit.Pixel);
            }


            var memoryStream = new MemoryStream();
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Position = 0;

            var finalBitmapImage = new BitmapImage();
            finalBitmapImage.BeginInit();
            finalBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            finalBitmapImage.StreamSource = memoryStream;
            finalBitmapImage.EndInit();
            

            image.Dispose();
            memoryStream.Dispose();

            return finalBitmapImage;
        }
    }
}
