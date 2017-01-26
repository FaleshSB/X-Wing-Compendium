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
    class PilotCard : Image
    {
        public int pilotKey;

        public int GetPilotKey()
        {
            return pilotKey;
        }
        public void SetPilotKey(int newPilotKey)
        {
                pilotKey = newPilotKey;
        }
    }
}
