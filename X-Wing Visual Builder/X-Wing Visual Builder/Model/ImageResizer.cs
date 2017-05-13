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


        public static BitmapImage ResizeImage(Image image, Size size)
        {
            if (image == null || size.IsEmpty)
                return null;

            Bitmap resizedImage = new Bitmap(size.Width, size.Height, image.PixelFormat);
            resizedImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                Point location = new Point(0, 0);
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.DrawImage(image, new Rectangle(location, size),
                                    new Rectangle(location, image.Size), GraphicsUnit.Pixel);
            }


            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Position = 0;

            BitmapImage finalBitmapImage = new BitmapImage();
            finalBitmapImage.BeginInit();
            finalBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            finalBitmapImage.StreamSource = memoryStream;
            finalBitmapImage.EndInit();
            finalBitmapImage.Freeze();


            image.Dispose();
            memoryStream.Dispose();

            return finalBitmapImage;
        }
    }
}
