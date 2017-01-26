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

        public void AddPilot(Pilot pilot)
        {
            pilots.Add(pilot);
        }

        public void AddUpgrade(int pilotKey, Upgrade upgrade)
        {
            pilots.ElementAt(pilotKey).AddUpgrade(upgrade);
        }

        public PilotCard GetPilotCard(int pilotKey)
        {
            PilotCard pilotCard = pilots.ElementAt(pilotKey).GetPilotCard();
            pilotCard.pilotKey = pilotKey;

            return pilotCard;
        }

        public UpgradeCard GetUpgradeCard(int pilotKey, int upgradeKey)
        {
            UpgradeCard upgradeCard = pilots.ElementAt(pilotKey).GetUpgrades().ElementAt(upgradeKey).GetUpgradeCard();
            upgradeCard.SetPilotKey(pilotKey);
            upgradeCard.SetUpgradeKey(upgradeKey);

            return upgradeCard;
        }

        public int GetNumberOfPilots()
        {
            return pilots.Count;
        }

        public int GetNumberOfUpgrades(int pilotKey)
        {
            return pilots.ElementAt(pilotKey).GetUpgrades().Count;
        }
    }
}
