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
    class Upgrade
    {
        private Build build;

        private int id;
        private UpgradeType upgradeType;
        private int cost;
        private string name;
        private string description;
        private string faq;
        private Faction faction;
        private ShipSize shipSize;
        private Ship ship;
        private bool isWeapon;
        private bool isUnique;
        private bool isLimited;
        private int numberOfUpgradeSlots;
        private bool isDualCard;
        private bool needsTorpedoAndMissile;
        private bool needsBoost;
        private bool isTieOnly;
        private bool isXWingOnly;
        
        public Upgrade(int id, UpgradeType upgradeType, int cost, string name, string description, string faq, Faction faction, ShipSize shipSize,
                       Ship ship, bool isWeapon, bool isUnique, bool isLimited, int numberOfUpgradeSlots, bool isDualCard, bool needsTorpedoAndMissile,
                       bool needsBoost, bool isTieOnly, bool isXWingOnly)
        {
            this.id = id;
            this.upgradeType = upgradeType;
            this.cost = cost;
            this.name = name;
            this.description = description;
            this.faq = faq;
            this.faction = faction;
            this.shipSize = shipSize;
            this.ship = ship;
            this.isWeapon = isWeapon;
            this.isUnique = isUnique;
            this.isLimited = isLimited;
            this.numberOfUpgradeSlots = numberOfUpgradeSlots;
            this.isDualCard = isDualCard;
            this.needsTorpedoAndMissile = needsTorpedoAndMissile;
            this.needsBoost = needsBoost;
            this.isTieOnly = isTieOnly;
            this.isXWingOnly = isXWingOnly;
        }

        public void AddBuild(Build build)
        {
            this.build = build;
        }

        public UpgradeCard GetUpgradeCard()
        {
            double height = build.GetCanvasSize() / 8.311688312;
            double width = (height / 717) * 466;
            //BitmapImage webImage = new BitmapImage(new Uri("D:\\Documents\\Game Stuff\\Board Games\\X-Wing\\Cards\\Accuracy-corrector.png"));
            BitmapImage webImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\Upgrade Cards\" + id.ToString() + ".png"));
            webImage.

            UpgradeCard upgradeCard = new UpgradeCard();
            upgradeCard.Source = webImage;
            upgradeCard.Height = Math.Round(height);
            upgradeCard.Width = Math.Round(width);
            upgradeCard.UseLayoutRounding = true;

            return upgradeCard;
        }
    }
}
