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
    public class UpgradeCard : Image
    {
        public int pilotKey { get; set; }
        public int upgradeKey { get; set; }
    }
}
