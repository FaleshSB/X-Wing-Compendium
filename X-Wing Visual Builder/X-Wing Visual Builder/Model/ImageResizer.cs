using System.Drawing;
using System.Drawing.Drawing2D;

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
    }
}
