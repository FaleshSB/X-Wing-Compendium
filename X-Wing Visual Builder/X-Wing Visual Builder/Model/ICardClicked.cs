using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    public interface ICardClicked
    {
        void UpgradeClicked(int upgradeId);
        void PilotClicked(int pilotId);
    }
}
