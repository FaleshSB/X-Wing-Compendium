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
        public int displayOrder;
        public int uniqueBuildId;
        public Faction faction;
        public Dictionary<int, UniquePilot> pilots = new Dictionary<int, UniquePilot>();
        private double canvasSize = 1920;
        public int totalCost
        {
            get
            {
                int cost = 0;

                foreach (KeyValuePair<int, UniquePilot> uniquePilot in pilots)
                {
                    cost += uniquePilot.Value.totalCost;
                }

                return cost;
            }
        }
        
        public int AddPilot(int pilotId, bool isLoadingBuild = false)
        {
            int uniquePilotId = 0;
            UniquePilot newPilot;
            while (true)
            {
                int origionalNewPilotId = uniquePilotId;
                foreach (UniquePilot otherPilot in pilots.Values.ToList())
                {
                    if (otherPilot.id == uniquePilotId)
                    {
                        uniquePilotId++;
                        break;
                    }
                }
                if (origionalNewPilotId == uniquePilotId)
                {
                    newPilot = new UniquePilot(Pilots.pilots[pilotId], uniquePilotId);
                    break;
                }
            }
            pilots.Add(uniquePilotId, newPilot);
            if (isLoadingBuild == false) { Builds.SaveBuilds(); }
            return uniquePilotId;
        }
        public UniquePilot GetPilot(int uniquePilotId)
        {
            return pilots[uniquePilotId];
        }

        public void AddUpgrade(int uniquePilotId, int upgradeId, bool isLoadingBuild = false)
        {
            int uniqueUpgradeId = 0;
            while (true)
            {
                int origionalUniqueUpgradeId = uniqueUpgradeId;
                foreach (UniquePilot pilot in pilots.Values.ToList())
                {
                    foreach (KeyValuePair<int, Upgrade> upgradeToTest in pilot.upgrades)
                    {
                        if (upgradeToTest.Key == uniqueUpgradeId)
                        {
                            uniqueUpgradeId++;
                            break;
                        }
                    }
                }
                if (origionalUniqueUpgradeId == uniqueUpgradeId)
                {
                    pilots[uniquePilotId].upgrades[uniqueUpgradeId] = Upgrades.upgrades[upgradeId];
                    break;
                }
            }
            if (isLoadingBuild == false) { Builds.SaveBuilds(); }
        }

        public void RemoveUpgrade(int uniquePilotId, int upgradeId)
        {
            int upgradeToRemoveKey = 0;
            bool hasFoundUpgradeToRemove = false;

            foreach(KeyValuePair<int, Upgrade> upgrade in pilots[uniquePilotId].upgrades)
            {
                if (upgrade.Value.id == upgradeId)
                {
                    hasFoundUpgradeToRemove = true;
                    upgradeToRemoveKey = upgrade.Key;
                    break;
                }
            }
            if(hasFoundUpgradeToRemove)
            {
                pilots[uniquePilotId].upgrades.Remove(upgradeToRemoveKey);
                UpgradeModifiers.RemoveUpgrade(this, uniquePilotId, upgradeId);
                Upgrades.RemoveUnusableUpgrades(this, uniquePilotId);
                Builds.SaveBuilds();
            }            
        }

        public void RemovePilot(int uniquePilotId)
        {
            pilots.Remove(uniquePilotId);
            Builds.SaveBuilds();
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
