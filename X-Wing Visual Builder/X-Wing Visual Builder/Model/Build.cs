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
        public Faction faction;
        private List<Pilot> pilots { get; set; } = new List<Pilot>();
        private double canvasSize = 1920;
        public int totalCost
        {
            get
            {
                int cost = 0;

                foreach (Pilot pilot in pilots)
                {
                    cost += pilot.totalCost;
                }

                return cost;
            }
        }
        private string buildInfo = "";

        public Build()
        {
            LoadBuild();
            //AddPilot(Pilots.GetPilotClone(300));
            //AddUpgrade(1, Upgrades.upgrades[1010]);
        }

        private void SaveBuild()
        {
            buildInfo = (int)faction + "|";
            for(int i=0;i< pilots.Count;i++)
            {
                buildInfo += i + "£" + pilots[i].id + "£";
                foreach(Upgrade upgrade in pilots[i].upgrades)
                {
                    buildInfo += upgrade.id + "$";
                }
                buildInfo = buildInfo.TrimEnd('$');
                buildInfo += "&";
            }
            buildInfo = buildInfo.TrimEnd('&');
            FileHandler fileHandler = new FileHandler();
            fileHandler.SaveFile("build.txt", buildInfo);
            pilots = pilots.OrderByDescending(pilots => pilots.pilotSkill).ThenByDescending(pilots => pilots.cost).ToList();
        }
        private void LoadBuild()
        {
            FileHandler fileHandler = new FileHandler();
            string[] allBuilds = fileHandler.LoadFile("build.txt");

            pilots.Clear();
            string[] buildInfo = allBuilds[0].Split('|');
            faction = (Faction)Int32.Parse(buildInfo[0]);

            string[] pilotBuilds = buildInfo[1].Split('&');
            foreach(string pilot in pilotBuilds)
            {
               string[] pilotInfo = pilot.Split('£');
                int pilotKey = Int32.Parse(pilotInfo[0]);
                int pilotId = Int32.Parse(pilotInfo[1]);
                AddPilot(Pilots.GetPilotClone(pilotId));
                string[] upgrades = pilotInfo[2].Split('$');
                foreach (string upgradeIdString in upgrades)
                {
                    int upgradeId;
                    if (int.TryParse(upgradeIdString, out upgradeId))
                    {
                        AddUpgrade(pilotKey, Upgrades.upgrades[upgradeId]);
                    }
                }
            }
            pilots = pilots.OrderByDescending(pilots => pilots.pilotSkill).ThenByDescending(pilots => pilots.cost).ToList();
        }

        public void AddPilot(Pilot pilot)
        {
            pilots.Add(pilot);
            faction = pilot.faction;
            SaveBuild();
        }
        public Pilot GetPilot(int pilotId)
        {
            return pilots[pilotId];
        }

        public void AddUpgrade(int pilotKey, Upgrade upgrade)
        {
            pilots.ElementAt(pilotKey).upgrades.Add(upgrade);
            SaveBuild();
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
            SaveBuild();
        }

        public void DeletePilot(int pilotKey)
        {
            pilots.RemoveAt(pilotKey);
            SaveBuild();
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
