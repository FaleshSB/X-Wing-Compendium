using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    public interface IDeleteCard
    {
        void DeletePilotClicked(int uniqueBuildId, int uniquePilotId);
        void DeleteUpgradeClicked(int uniqueBuildId, int uniquePilotId, int upgradeId);
    }
}
