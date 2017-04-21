using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using X_Wing_Visual_Builder.View;

namespace X_Wing_Visual_Builder.Model
{
    public class Build
    {
        public int uniqueBuildId;
        public Faction faction;
        public Dictionary<int, Pilot> pilots = new Dictionary<int, Pilot>();
        private double canvasSize = 1920;
        public int totalCost
        {
            get
            {
                int cost = 0;

                foreach (KeyValuePair<int, Pilot> pilot in pilots)
                {
                    cost += pilot.Value.totalCost;
                }

                return cost;
            }
        }
        
        public void AddPilot(Pilot pilot, bool hasUniqueId = false)
        {
            if (hasUniqueId == false)
            {
                int uniquePilotId = 0;
                while (true)
                {
                    int origionalNewPilotId = uniquePilotId;
                    foreach (Pilot otherPilot in pilots.Values.ToList())
                    {
                        if (otherPilot.uniquePilotId == uniquePilotId)
                        {
                            uniquePilotId++;
                            break;
                        }
                    }
                    if (origionalNewPilotId == uniquePilotId)
                    {
                        pilot.uniquePilotId = uniquePilotId;
                        break;
                    }
                }
            }
            pilots.Add(pilot.uniquePilotId, pilot);
            Builds.SaveBuilds();
        }
        public Pilot GetPilot(int uniquePilotId)
        {
            return pilots[uniquePilotId];
        }

        public void AddUpgrade(int uniquePilotId, int upgradeId)
        {
            Upgrade upgrade = Upgrades.GetUpgradeClone(upgradeId);
            
            int uniqueUpgradeId = 0;
            while (true)
            {
                int origionalUniqueUpgradeId = uniqueUpgradeId;
                foreach (Pilot pilot in pilots.Values.ToList())
                {
                    foreach (Upgrade upgradeToTest in pilot.upgrades.Values.ToList())
                    {
                        if (upgradeToTest.uniqueUpgradeId == uniqueUpgradeId)
                        {
                            uniqueUpgradeId++;
                            break;
                        }
                    }
                }
                if (origionalUniqueUpgradeId == uniqueUpgradeId)
                {
                    upgrade.uniqueUpgradeId = uniqueUpgradeId;
                    break;
                }
            }

            pilots[uniquePilotId].upgrades[upgrade.uniqueUpgradeId] = upgrade;
            Builds.SaveBuilds();
        }
        /*
        public UpgradeCard GetUpgradeCard(int uniquePilotId, int upgradeKey, double width, double height)
        {
            UpgradeCard upgradeCard = pilots[uniquePilotId].upgrades.ElementAt(upgradeKey).GetUpgradeCard(width, height);
            upgradeCard.pilotKey = uniquePilotId;
            upgradeCard.upgradeId = upgradeKey;

            return upgradeCard;
        }
        */
        public void RemoveUpgrade(int uniquePilotId, int upgradeId)
        {
            foreach(Upgrade upgrade in pilots[uniquePilotId].upgrades.Values.ToList())
            {
                if (upgrade.id == upgradeId)
                {
                    pilots[uniquePilotId].upgrades.Remove(upgrade.uniqueUpgradeId);
                }
            }            
            UpgradeModifiers.RemoveUpgrade(this, uniquePilotId, upgradeId);
            Upgrades.RemoveUnusableUpgrades(this, uniquePilotId);
            Builds.SaveBuilds();
        }
        public void RemoveUpgrade(int uniqueUpgradeId)
        {
            Upgrade upgrade = null;
            Pilot affectedPilot = null;
            foreach (Pilot pilot in pilots.Values.ToList())
            {
                if(pilot.upgrades.ContainsKey(uniqueUpgradeId))
                {
                    upgrade = pilot.upgrades[uniqueUpgradeId];
                    affectedPilot = pilot;
                    pilot.upgrades.Remove(uniqueUpgradeId);
                }
            }
            if (upgrade != null && affectedPilot != null)
            {
                UpgradeModifiers.RemoveUpgrade(this, affectedPilot.uniquePilotId, upgrade.id);
                Upgrades.RemoveUnusableUpgrades(this, affectedPilot.uniquePilotId);
            }

            Builds.SaveBuilds();
            SquadsPage squadsPage = (SquadsPage)Pages.pages[PageName.Squads];
            squadsPage.buildChanged(uniqueBuildId);
        }

        public void RemovePilot(int uniquePilotId)
        {
            pilots.Remove(uniquePilotId);
            Builds.SaveBuilds();
            SquadsPage squadsPage = (SquadsPage)Pages.pages[PageName.Squads];
            squadsPage.buildChanged(uniqueBuildId);
        }

        public int GetNumberOfPilots()
        {
            return pilots.Count;
        }

        public int GetNumberOfUpgrades(int uniquePilotId)
        {
            return pilots[uniquePilotId].upgrades.Count;
        }

        public void SetCanvasSize(double canvasSize)
        {
            this.canvasSize = canvasSize;
        }

        public double GetCanvasSize()
        {
            return canvasSize;
        }
    }
}
