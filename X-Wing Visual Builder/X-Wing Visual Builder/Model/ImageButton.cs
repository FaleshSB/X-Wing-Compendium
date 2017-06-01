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
                    buttonSize = new System.Drawing.Size((int)Math.Round(173 * scale), (int)Math.Round(60 * scale));
                    break;
                case "delete_squad":
                    buttonSize = new System.Drawing.Size((int)Math.Round(224 * scale), (int)Math.Round(60 * scale));
                    break;
                case "up":
                    buttonSize = new System.Drawing.Size((int)Math.Round(40 * scale), (int)Math.Round(60 * scale));
                    break;
                case "down":
                    buttonSize = new System.Drawing.Size((int)Math.Round(40 * scale), (int)Math.Round(60 * scale));
                    break;
                case "copy_for_web":
                    buttonSize = new System.Drawing.Size((int)Math.Round(227 * scale), (int)Math.Round(60 * scale));
                    break; 
                case "add_upgrade":
                    buttonSize = new System.Drawing.Size((int)Math.Round(225 * scale), (int)Math.Round(60 * scale));
                    break;
                case "swap_pilot":
                    buttonSize = new System.Drawing.Size((int)Math.Round(190 * scale), (int)Math.Round(60 * scale));
                    break;
                case "add_imperial_squad":
                    buttonSize = new System.Drawing.Size((int)Math.Round(319 * scale), (int)Math.Round(60 * scale));
                    break;
                case "add_rebel_squad":
                    buttonSize = new System.Drawing.Size((int)Math.Round(274 * scale), (int)Math.Round(60 * scale));
                    break;
                case "add_scum_squad":
                    buttonSize = new System.Drawing.Size((int)Math.Round(274 * scale), (int)Math.Round(60 * scale));
                    break;
                case "browse_cards":
                    buttonSize = new System.Drawing.Size((int)Math.Round(236 * scale), (int)Math.Round(60 * scale));
                    break;
                case "quiz":
                    buttonSize = new System.Drawing.Size((int)Math.Round(100 * scale), (int)Math.Round(60 * scale));
                    break;
                case "manage_squads":
                    buttonSize = new System.Drawing.Size((int)Math.Round(262 * scale), (int)Math.Round(60 * scale));
                    break;
                case "show_name":
                    buttonSize = new System.Drawing.Size((int)Math.Round(203 * scale), (int)Math.Round(60 * scale));
                    break;
                case "next":
                    buttonSize = new System.Drawing.Size((int)Math.Round(203 * scale), (int)Math.Round(60 * scale));
                    break;
                case "info":
                    buttonSize = new System.Drawing.Size((int)Math.Round(22 * scale), (int)Math.Round(22 * scale));
                    break;
                case "close":
                    buttonSize = new System.Drawing.Size((int)Math.Round(22 * scale), (int)Math.Round(22 * scale));
                    break;
                case "add":
                    buttonSize = new System.Drawing.Size((int)Math.Round(22 * scale), (int)Math.Round(22 * scale));
                    break;
                case "remove":
                    buttonSize = new System.Drawing.Size((int)Math.Round(22 * scale), (int)Math.Round(22 * scale));
                    break;
                case "copy_for_vassal":
                    buttonSize = new System.Drawing.Size((int)Math.Round(266 * scale), (int)Math.Round(60 * scale));
                    break;
                case "copy":
                    buttonSize = new System.Drawing.Size((int)Math.Round(106 * scale), (int)Math.Round(60 * scale));
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
            await Task.Delay(Opt.buttonDelay);
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
