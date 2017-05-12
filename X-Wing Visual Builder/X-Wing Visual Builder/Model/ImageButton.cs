using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace X_Wing_Visual_Builder.Model
{
    public class ImageButton : System.Windows.Controls.Image
    {
        public int uniqueBuildId;
        public int uniquePilotId;
        private string imageName;
        private Size buttonSize;
        private string filteredLocation;

        public ImageButton(string imageName, double scale)
        {
            switch (imageName)
            {
                case "add_pilot":
                    buttonSize = new System.Drawing.Size((int)Math.Round(171 * scale), (int)Math.Round(60 * scale));
                    break;
                default:
                    break;
            }


            this.imageName = imageName;
            this.filteredLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "") + "\\Misc\\";
           
            Source = ImageResizer.ResizeImage(System.Drawing.Image.FromFile(filteredLocation + imageName + ".png"), buttonSize);
            Width = buttonSize.Width;
            Height = buttonSize.Height;
            UseLayoutRounding = true;
            MouseEnter += new MouseEventHandler(ButtonHover);
            MouseLeave += new MouseEventHandler(ButtonStopHover);
            MouseDown += new MouseButtonEventHandler(ButtonClicked);
            Cursor = Cursors.Hand;
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
        }

        private async void ButtonClicked(object sender, MouseButtonEventArgs e)
        {
            Source = ImageResizer.ResizeImage(System.Drawing.Image.FromFile(filteredLocation + imageName + "_pressed.png"), buttonSize);
            await Task.Delay(100);
            Source = ImageResizer.ResizeImage(System.Drawing.Image.FromFile(filteredLocation + imageName + ".png"), buttonSize);
        }

        private void ButtonStopHover(object sender, MouseEventArgs e)
        {
            Source = ImageResizer.ResizeImage(System.Drawing.Image.FromFile(filteredLocation + imageName + ".png"), buttonSize);
        }

        private void ButtonHover(object sender, MouseEventArgs e)
        {
            Source = ImageResizer.ResizeImage(System.Drawing.Image.FromFile(filteredLocation + imageName + "_hover.png"), buttonSize);
        }
    }
}
