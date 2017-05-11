using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace X_Wing_Visual_Builder.Model
{
    class BuildPilotUpgrade : Button
    {
        public int uniqueBuildId;
        public int uniquePilotId;

        public BuildPilotUpgrade()
        {
            Cursor = Cursors.Hand;
        }

    }
}
