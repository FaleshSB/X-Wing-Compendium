using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace X_Wing_Visual_Builder.Model
{
    class AddButton : Image
    {
        public int uniqueBuildId;
        public int uniquePilotId;
        public int upgradeId;
        public int pilotId;

        public AddButton()
        {
            BitmapImage webImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\addbutton.png"));
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
            this.Source = webImage;
            this.Height = 30;
            this.Width = 30;
            this.UseLayoutRounding = true;
        }
    }
}
