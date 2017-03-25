﻿using System;
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

            //TIE/x1
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
                build.AddUpgrade(uniquePilotId, Upgrades.upgrades[5005]);
            }
        }

        public static void ChangePossibleUpgrades(Pilot pilot, Dictionary<UpgradeType, int> possibleUpgrades)
        {
            // Heavy Scyk Interceptor
            if (pilot.upgrades.Contains(Upgrades.upgrades[11020]))
            {
                if (possibleUpgrades.ContainsKey(UpgradeType.Cannon) && possibleUpgrades[UpgradeType.Cannon] < 1)
                {
                    Upgrades.AddOrRemoveFromPossibleUpgrades(possibleUpgrades, UpgradeType.Torpedo, -1);
                    Upgrades.AddOrRemoveFromPossibleUpgrades(possibleUpgrades, UpgradeType.Missile, -1);
                }
                else if (possibleUpgrades.ContainsKey(UpgradeType.Missile) && possibleUpgrades[UpgradeType.Missile] < 1)
                {
                    Upgrades.AddOrRemoveFromPossibleUpgrades(possibleUpgrades, UpgradeType.Torpedo, -1);
                    Upgrades.AddOrRemoveFromPossibleUpgrades(possibleUpgrades, UpgradeType.Cannon, -1);
                }
                else if (possibleUpgrades.ContainsKey(UpgradeType.Torpedo) && possibleUpgrades[UpgradeType.Torpedo] < 1)
                {
                    Upgrades.AddOrRemoveFromPossibleUpgrades(possibleUpgrades, UpgradeType.Missile, -1);
                    Upgrades.AddOrRemoveFromPossibleUpgrades(possibleUpgrades, UpgradeType.Cannon, -1);
                }
            }
        }

        public static bool SkipGetUpgrade(Pilot pilot, Upgrade upgrade)
        {
            // A-Wing Test Pilot
            if (pilot.upgrades.Contains(Upgrades.upgrades[11001]) && upgrade.upgradeType == UpgradeType.Elite && pilot.upgrades.Contains(upgrade)) { return true; }
            // TIE Shuttle
            else if (pilot.upgrades.Contains(Upgrades.upgrades[11031]) && upgrade.upgradeType == UpgradeType.Crew && upgrade.cost > 4) { return true; }
            // Royal Guard TIE
            else if (pilot.upgrades.Contains(Upgrades.upgrades[11034]) && upgrade.upgradeType == UpgradeType.Modification && pilot.upgrades.Contains(upgrade)) { return true; }
            return false;
        }
    }
}