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
    public class PilotCard : Image, IGeneralId
    {
        public int uniquePilotId;
        public int uniqueBuildId;
        public int pilotId;
        public int id { get { return pilotId; } }
    }
}
