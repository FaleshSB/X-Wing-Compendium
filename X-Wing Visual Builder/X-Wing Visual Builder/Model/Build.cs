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

namespace X_Wing_Visual_Builder.Model
{
    public class Build
    {
        public int id { get; set; }
        public Faction faction;
        public Dictionary<int, Pilot> pilots { get; set; } = new Dictionary<int, Pilot>();
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

        public Build()
        {

        }

        public Dictionary<UpgradeType, int> GetPossibleUpgrades(int uniquePilotId)
        {
            Dictionary<UpgradeType, int> possibleUpgrades = pilots[uniquePilotId].possibleUpgrades;
            foreach(Upgrade upgrade in pilots[uniquePilotId].upgrades)
            {
                possibleUpgrades[upgrade.upgradeType] -= upgrade.numberOfUpgradeSlots;
            }
            return possibleUpgrades;
        }

        public void AddPilot(Pilot pilot)
        {
            int newPilotId = 0;
            while (true)
            {
                int origionalNewPilotId = newPilotId;
                foreach (KeyValuePair<int, Pilot> otherPilot in pilots)
                {
                    if (otherPilot.Value.uniquePilotId == newPilotId)
                    {
                        newPilotId++;
                        break;
                    }
                }
                if (origionalNewPilotId == newPilotId)
                {
                    pilot.uniquePilotId = newPilotId;
                    break;
                }
            }
            pilots.Add(pilot.uniquePilotId, pilot);
            Builds.SaveBuilds();
        }
        public Pilot GetPilot(int pilotId)
        {
            return pilots[pilotId];
        }

        public void AddUpgrade(int uniquePilotId, Upgrade upgrade)
        {
            pilots[uniquePilotId].upgrades.Add(upgrade);
            Builds.SaveBuilds();
        }

        public PilotCard GetPilotCard(int uniquePilotId, double width, double height)
        {
            PilotCard pilotCard = pilots[uniquePilotId].GetPilotCard(width, height);
            pilotCard.pilotKey = uniquePilotId;

            return pilotCard;
        }

        public UpgradeCard GetUpgradeCard(int uniquePilotId, int upgradeKey, double width, double height)
        {
            UpgradeCard upgradeCard = pilots[uniquePilotId].upgrades.ElementAt(upgradeKey).GetUpgradeCard(width, height);
            upgradeCard.pilotKey = uniquePilotId;
            upgradeCard.upgradeKey = upgradeKey;

            return upgradeCard;
        }

        public void DeleteUpgrade(int uniquePilotId, int upgradeKey)
        {
            pilots[uniquePilotId].upgrades.RemoveAt(upgradeKey);
            Builds.SaveBuilds();
        }

        public void DeletePilot(int uniquePilotId)
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
