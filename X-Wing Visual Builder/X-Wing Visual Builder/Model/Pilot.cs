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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace X_Wing_Visual_Builder.Model
{
    public class Pilot
    {
        public int totalCost
        {
            get
            {
                int totalCost = 0;
                totalCost += cost;
                foreach (Upgrade upgrade in upgrades)
                {
                   totalCost += UpgradeModifiers.UpgradeCost(upgrade, upgrades);
                }
                return totalCost;
            }
        }
        public int uniquePilotId;
        public int id;
        public int cost;
        public string name;
        public string description;
        public string faq;
        public Ship ship;
        public bool isUnique;
        public List<Action> usableActions
        {
            get
            {
                List<Action> usableActions = new List<Action>(ship.actions);
                foreach (Upgrade upgrade in upgrades)
                {
                    usableActions.AddRange(upgrade.addsActions);
                }

                return usableActions;
            }
        }
        private int _pilotSkill;
        public int pilotSkill
        {
            get
            {
                int pilotSkill = _pilotSkill;
                foreach (Upgrade upgrade in upgrades)
                {
                    pilotSkill += upgrade.addsPilotSkill;
                }
                return pilotSkill;
            }
            set
            {
                _pilotSkill = value;
            }
        }
        private Dictionary<UpgradeType, int> _possibleUpgrades = new Dictionary<UpgradeType, int>();
        public Dictionary<UpgradeType, int> possibleUpgrades
        {
            get
            {
                Dictionary<UpgradeType, int> possibleUpgrades = new Dictionary<UpgradeType, int>(_possibleUpgrades);
                foreach (Upgrade upgrade in upgrades)
                {
                    if (possibleUpgrades.ContainsKey(upgrade.upgradeType))
                    {
                        possibleUpgrades[upgrade.upgradeType] -= upgrade.numberOfUpgradeSlots;
                    }

                    foreach (KeyValuePair<UpgradeType, int> upgradeAdded in upgrade.upgradesAdded)
                    {
                        if (possibleUpgrades.ContainsKey(upgradeAdded.Key))
                        {
                            possibleUpgrades[upgradeAdded.Key] += upgradeAdded.Value;
                        }
                        else
                        {
                            possibleUpgrades[upgradeAdded.Key] = 0 + upgradeAdded.Value;
                        }
                    }
                    foreach (KeyValuePair<UpgradeType, int> upgradeRemoved in upgrade.upgradesRemoved)
                    {
                        if (possibleUpgrades.ContainsKey(upgradeRemoved.Key))
                        {
                            possibleUpgrades[upgradeRemoved.Key] -= upgradeRemoved.Value;
                        }
                        else
                        {
                            possibleUpgrades[upgradeRemoved.Key] = 0 - upgradeRemoved.Value;
                        }
                    }
                    
                }
                UpgradeModifiers.ChangePossibleUpgrades(this, possibleUpgrades);
                return possibleUpgrades;
            }
            set
            {
                _possibleUpgrades = value;
            }
        }
        public Faction faction;
        public bool hasAbility;

        public List<Upgrade> upgrades = new List<Upgrade>();

        public Pilot(int id, ShipType shipType, bool isUnique, string name, int pilotSkill, string description, Dictionary<UpgradeType, int> possibleUpgrades, int cost, string faq, Faction faction, bool hasAbility)
        {
            this.id = id;
            this.isUnique = isUnique;
            this.name = name;
            this.pilotSkill = pilotSkill;
            this.description = description;
            this.possibleUpgrades = possibleUpgrades;
            this.cost = cost;
            this.faq = faq;
            this.faction = faction;
            this.hasAbility = hasAbility;

            this.ship = Ships.ships[shipType][faction];
        }
        
        public Pilot GetPilotClone()
        {
            Pilot pilotClone = (Pilot)MemberwiseClone();
            pilotClone.upgrades = new List<Upgrade>();
            return pilotClone;
        }

        public PilotCard GetPilotCard(double width, double height, int uniqueBuildId = 0)
        {
            BitmapImage resizedUpgradeImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\Pilot Cards\" + id.ToString() + ".png"));

            PilotCard pilotCard = new PilotCard();
            pilotCard.id = id;
            pilotCard.Source = resizedUpgradeImage;
            pilotCard.Height = height;
            pilotCard.Width = width;
            pilotCard.UseLayoutRounding = true;
            RenderOptions.SetBitmapScalingMode(pilotCard, BitmapScalingMode.HighQuality);
            pilotCard.uniquePilotId = uniquePilotId;
            pilotCard.uniqueBuildId = uniqueBuildId;

            return pilotCard;
        }
    }
}
