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
    public class Upgrade
    {
        public int id;
        public int cost;
        public string name;
        public string description;
        public string faq;
        public UpgradeType upgradeType;
        public Faction faction;
        public ShipSize shipSize;
        public ShipType shipType;
        public bool isWeapon;
        public bool isUnique;
        public bool isLimited;
        public int numberOfUpgradeSlots;
        public bool isDualCard;
        public bool isTieOnly;
        public bool isXWingOnly;
        public Dictionary<UpgradeType, int> upgradesAdded;
        public Dictionary<UpgradeType, int> upgradesRemoved;
        public int requiresPilotSkill;
        public List<Action> requiresActions;
        public List<int> requiresUpgrades;
        public List<Action> addsActions;
        public int addsPilotSkill;

        public Upgrade(int id, UpgradeType upgradeType, int cost, string name, string description, string faq, Faction faction, ShipSize shipSize,
                       ShipType ship, bool isWeapon, bool isUnique, bool isLimited, int numberOfUpgradeSlots, bool isDualCard, bool isTieOnly, bool isXWingOnly,
                       Dictionary<UpgradeType, int> upgradesAdded, Dictionary<UpgradeType, int> upgradesRemoved, int requiresPilotSkill, List<Action> requiresActions,
                       List<int> requiresUpgrades, List<Action> addsActions, int addsPilotSkill)
        {
            this.id = id;
            this.upgradeType = upgradeType;
            this.cost = cost;
            this.name = name;
            this.description = description;
            this.faq = faq;
            this.faction = faction;
            this.shipSize = shipSize;
            this.shipType = ship;
            this.isWeapon = isWeapon;
            this.isUnique = isUnique;
            this.isLimited = isLimited;
            this.numberOfUpgradeSlots = numberOfUpgradeSlots;
            this.isDualCard = isDualCard;
            this.isTieOnly = isTieOnly;
            this.isXWingOnly = isXWingOnly;
            this.upgradesAdded = upgradesAdded;
            this.upgradesRemoved = upgradesRemoved;
            this.requiresPilotSkill = requiresPilotSkill;
            this.requiresActions = requiresActions;
            this.requiresUpgrades = requiresUpgrades;
            this.addsActions = addsActions;
            this.addsPilotSkill = addsPilotSkill;
        }

        public UpgradeCard GetUpgradeCard(double width, double height)
        {
            BitmapImage resizedUpgradeImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\Upgrade Cards\" + id.ToString() + ".png"));

            UpgradeCard upgradeCard = new UpgradeCard();
            upgradeCard.id = id;
            upgradeCard.Source = resizedUpgradeImage;
            upgradeCard.UseLayoutRounding = true;
            upgradeCard.Height = height;
            upgradeCard.Width = width;
            RenderOptions.SetBitmapScalingMode(upgradeCard, BitmapScalingMode.HighQuality);

            return upgradeCard;
        }
    }
}
