﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace X_Wing_Visual_Builder.Model
{
    class DeleteButton : Image
    {
        public int buildId { get; set; }
        public int pilotKey { get; set; }
        public int upgradeKey { get; set; }

        public DeleteButton()
        {
            BitmapImage webImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\deletebutton.png"));
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
            this.Source = webImage;
            this.Height = 30;
            this.Width = 30;
            this.UseLayoutRounding = true;
        }
    }
}
