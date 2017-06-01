using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    public static class CanonicalUpgradeNames
    {
        public static Dictionary<UpgradeType, string> canonicalUpgradeNames = new Dictionary<UpgradeType, string>();

        static CanonicalUpgradeNames()
        {
            foreach (UpgradeType upgradeType in Enum.GetValues(typeof(UpgradeType)))
                switch (upgradeType)
                {
                    case UpgradeType.Elite:
                        canonicalUpgradeNames[upgradeType] = "ept";
                        break;
                    case UpgradeType.Astromech:
                        canonicalUpgradeNames[upgradeType] = "amd";
                        break;
                    case UpgradeType.Torpedo:
                        canonicalUpgradeNames[upgradeType] = "torpedo";
                        break;
                    case UpgradeType.Missile:
                        canonicalUpgradeNames[upgradeType] = "missile";
                        break;
                    case UpgradeType.Cannon:
                        canonicalUpgradeNames[upgradeType] = "cannon";
                        break;
                    case UpgradeType.Turret:
                        canonicalUpgradeNames[upgradeType] = "turret";
                        break;
                    case UpgradeType.Bomb:
                        canonicalUpgradeNames[upgradeType] = "bomb";
                        break;
                    case UpgradeType.Crew:
                        canonicalUpgradeNames[upgradeType] = "crew";
                        break;
                    case UpgradeType.SalvagedAstromech:
                        canonicalUpgradeNames[upgradeType] = "samd";
                        break;
                    case UpgradeType.System:
                        canonicalUpgradeNames[upgradeType] = "system";
                        break;
                    case UpgradeType.Title:
                        canonicalUpgradeNames[upgradeType] = "title";
                        break;
                    case UpgradeType.Modification:
                        canonicalUpgradeNames[upgradeType] = "mod";
                        break;
                    case UpgradeType.Illicit:
                        canonicalUpgradeNames[upgradeType] = "illicit";
                        break;
                    case UpgradeType.Cargo:
                        canonicalUpgradeNames[upgradeType] = "cargo";
                        break;
                    case UpgradeType.Hardpoint:
                        canonicalUpgradeNames[upgradeType] = "hardpoint";
                        break;
                    case UpgradeType.Team:
                        canonicalUpgradeNames[upgradeType] = "team";
                        break;
                    case UpgradeType.Tech:
                        canonicalUpgradeNames[upgradeType] = "tech";
                        break;
                    default:
                        break;
                }
        }
    }
}
