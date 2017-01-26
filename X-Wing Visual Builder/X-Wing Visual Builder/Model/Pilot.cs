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
    class Pilot
    {
        private Faction faction;
        private UpgradeType[] upgradeTypes;
        private Action[] actions;
        private Ship ship;
        private List<Upgrade> upgrades = new List<Upgrade>();

        private byte cost;
        private string pilotAbility;

        public double pilotCardAndUpgradesWidth = 0;

        private double cardWidth;
        private double cardHeight;

        public Pilot(double canvasArea)
        {
            cardWidth = canvasArea / 6.4;
            cardHeight = cardWidth * 1.42;
        }

        // TODO add where you can buy this card
        // TODO add maneuver card array[5][9] maneuver[4][0] = Maneuver.Green is a green 4 turn left, maneuver[5][8] = Maneuver.Red is a red 5 K turn    
        /*
        private byte pilotSkill;
        private byte primaryWeaponValue;
        private byte agility;
        private byte hull;
        private byte shields;
        */

        public PilotCard GetPilotCard()
        {
            BitmapImage webImage = new BitmapImage(new Uri("D:\\Documents\\Game Stuff\\Board Games\\X-Wing\\Pilots\\BlueAce.png"));
            PilotCard pilotCard = new PilotCard();
            pilotCard.Source = webImage;
            pilotCard.Height = cardHeight;
            pilotCard.Width = cardWidth;

            return pilotCard;
        }

        public void AddUpgrade(Upgrade upgrade)
        {
            upgrades.Add(upgrade);
            pilotCardAndUpgradesWidth = cardWidth + 40 + (Math.Ceiling((double)GetUpgrades().Count / 2) * (upgrade.GetCardWidth() + 10));
        }

        public List<Upgrade> GetUpgrades()
        {
            return upgrades;
        }

        public double GetPilotCardAndUpgradesWidth()
        {
            if (pilotCardAndUpgradesWidth == 0)
            {
                pilotCardAndUpgradesWidth = cardWidth;
            }
            return pilotCardAndUpgradesWidth;
        }
    }
}
