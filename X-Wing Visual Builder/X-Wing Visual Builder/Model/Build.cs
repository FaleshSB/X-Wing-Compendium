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
    class Build
    {
        private List<Pilot> pilots = new List<Pilot>();
        private double canvasSize = 1920;
        public int totalCost
        {
            get
            {
                int cost = 0;

                foreach (Pilot pilot in pilots)
                {
                    cost += pilot.cost;
                    foreach (Upgrade upgrade in pilot.upgrades)
                    {
                        cost += upgrade.cost;
                    }
                }

                return cost;
            }
        }

        public void AddPilot(Pilot pilot)
        {
            pilots.Add(pilot);
        }

        public void AddUpgrade(int pilotKey, Upgrade upgrade)
        {
            pilots.ElementAt(pilotKey).upgrades.Add(upgrade);
        }

        public PilotCard GetPilotCard(int pilotKey, double width, double height)
        {
            PilotCard pilotCard = pilots.ElementAt(pilotKey).GetPilotCard(width, height);
            pilotCard.pilotKey = pilotKey;

            return pilotCard;
        }

        public UpgradeCard GetUpgradeCard(int pilotKey, int upgradeKey, double width, double height)
        {
            UpgradeCard upgradeCard = pilots.ElementAt(pilotKey).upgrades.ElementAt(upgradeKey).GetUpgradeCard(width, height);
            upgradeCard.pilotKey = pilotKey;
            upgradeCard.upgradeKey = upgradeKey;

            return upgradeCard;
        }

        public void DeleteUpgrade(int pilotKey, int upgradeKey)
        {
            pilots.ElementAt(pilotKey).upgrades.RemoveAt(upgradeKey);
        }

        public void DeletePilot(int pilotKey)
        {
            pilots.RemoveAt(pilotKey);
        }

        public int GetNumberOfPilots()
        {
            return pilots.Count;
        }

        public int GetNumberOfUpgrades(int pilotKey)
        {
            return pilots.ElementAt(pilotKey).upgrades.Count;
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
