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
    public class UpgradeCard : Image, IUpgradeId, IGeneralId
    {
        public int pilotKey;
        public int upgradeId { get; set; }
        public int id { get { return upgradeId; } }
    }
}
