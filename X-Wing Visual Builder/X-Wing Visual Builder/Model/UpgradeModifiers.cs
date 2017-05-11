using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    static class UpgradeModifiers
    {
        public static int UpgradeCost(Upgrade upgrade, List<Upgrade> upgrades)
        {
            int cost = upgrade.cost;

            // TIE/x1
            if (upgrades.Contains(Upgrades.upgrades[11029]) && upgrade.upgradeType == UpgradeType.System)
            {
                cost = Math.Max(0, upgrade.cost - 4);
            }

            return cost;
        }

        public static void RemoveUpgrade(Build build, int uniquePilotId, int upgradeId)
        {
            // Mist Hunter Title
            if (upgradeId == 11010 || upgradeId == 5005)
            {
                build.RemoveUpgrade(uniquePilotId, 5005);
                build.RemoveUpgrade(uniquePilotId, 11010);
            }
        }

        public static void AddUpgrade(Build build, int uniquePilotId, int upgradeId)
        {
            // Mist Hunter Title
            if (upgradeId == 11010)
            {
                // Add tractor beam
                build.AddUpgrade(uniquePilotId, 5005);
            }
        }

        public static void ChangePossibleUpgrades(UniquePilot uniquePilot, Dictionary<UpgradeType, int> possibleUpgrades)
        {
            // Heavy Scyk Interceptor
            if (uniquePilot.upgrades.Values.Contains(Upgrades.upgrades[11020]))
            {
                if (possibleUpgrades.ContainsKey(UpgradeType.Cannon) && possibleUpgrades[UpgradeType.Cannon] < 1)
                {
                    RemoveUpgradeHelper(possibleUpgrades, UpgradeType.Torpedo, 1);
                    RemoveUpgradeHelper(possibleUpgrades, UpgradeType.Missile, 1);
                }
                else if (possibleUpgrades.ContainsKey(UpgradeType.Missile) && possibleUpgrades[UpgradeType.Missile] < 1)
                {
                    RemoveUpgradeHelper(possibleUpgrades, UpgradeType.Torpedo, 1);
                    RemoveUpgradeHelper(possibleUpgrades, UpgradeType.Cannon, 1);
                }
                else if (possibleUpgrades.ContainsKey(UpgradeType.Torpedo) && possibleUpgrades[UpgradeType.Torpedo] < 1)
                {
                    RemoveUpgradeHelper(possibleUpgrades, UpgradeType.Missile, 1);
                    RemoveUpgradeHelper(possibleUpgrades, UpgradeType.Cannon, 1);
                }
            }
        }
        private static void RemoveUpgradeHelper(Dictionary<UpgradeType, int> possibleUpgrades, UpgradeType upgradeType, int numberToBeRemoved)
        {
            if (possibleUpgrades.ContainsKey(upgradeType))
            {
                possibleUpgrades[upgradeType] -= numberToBeRemoved;
            }
            else
            {
                possibleUpgrades[upgradeType] = 0 - numberToBeRemoved;
            }
        }

        public static bool SkipGetUpgrade(UniquePilot uniquePilot, Upgrade upgrade, bool isRemovingUpgrades)
        {
            // A-Wing Test Pilot
            if (isRemovingUpgrades == false && uniquePilot.upgrades.Values.Contains(Upgrades.upgrades[11001]) && upgrade.upgradeType == UpgradeType.Elite && uniquePilot.upgrades.Values.Contains(upgrade)) { return true; }
            // TIE Shuttle
            else if (uniquePilot.upgrades.Values.Contains(Upgrades.upgrades[11031]) && upgrade.upgradeType == UpgradeType.Crew && upgrade.cost > 4) { return true; }
            // Royal Guard TIE
            else if (isRemovingUpgrades == false && uniquePilot.upgrades.Values.Contains(Upgrades.upgrades[11034]) && upgrade.upgradeType == UpgradeType.Modification && uniquePilot.upgrades.Values.Contains(upgrade)) { return true; }
            // Lightweight Frame
            else if (upgrade.id == 12026 && uniquePilot.pilot.ship.agility > 2) { return true; }
            // R2-D6
            else if (upgrade.id == 2006 && (uniquePilot.pilot.possibleUpgrades.ContainsKey(UpgradeType.Elite) && uniquePilot.pilot.possibleUpgrades[UpgradeType.Elite] > 0)) { return true; }
            
            return false;
        }
    }
}
