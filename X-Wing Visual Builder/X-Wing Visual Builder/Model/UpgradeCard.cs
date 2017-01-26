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
    class UpgradeCard : Image
    {
        private int pilotKey;
        private int upgradeKey;

        public int GetPilotKey()
        {
            return pilotKey;
        }
        public void SetPilotKey(int newPilotKey)
        {
            pilotKey = newPilotKey;
        }

        public int GetUpgradeKey()
        {
            return upgradeKey;
        }
        public void SetUpgradeKey(int newUpgradeKey)
        {
            upgradeKey = newUpgradeKey;
        }
    }
}
