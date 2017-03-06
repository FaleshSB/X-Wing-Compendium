using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace X_Wing_Visual_Builder.Model
{
    class AddUpgradeButton : Button
    {
        public int uniqueBuildId { get; set; }
        public int uniquePilotId { get; set; }
    }
}
