using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Drawing;


namespace X_Wing_Visual_Builder.Model
{
    public class Upgrade
    {
        public int id;
        public int uniqueUpgradeId;
        public int cost;
        public string name;
        public string description;
        public List<string> faq = new List<string>();
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
        private int _numberOwned;
        public int numberOwned
        {
            get { return _numberOwned; }
            set { _numberOwned = (value < 0) ? 0 : value; Upgrades.SaveNumberOfUpgradesOwned(); }
        }

        private BitmapImage resizedUpgradeImage = null;

        public Upgrade(int id, UpgradeType upgradeType, int cost, string name, string description, List<string> faq, Faction faction, ShipSize shipSize,
                       ShipType ship, bool isWeapon, bool isUnique, bool isLimited, int numberOfUpgradeSlots, bool isDualCard, bool isTieOnly, bool isXWingOnly,
                       Dictionary<UpgradeType, int> upgradesAdded, Dictionary<UpgradeType, int> upgradesRemoved, int requiresPilotSkill, List<Action> requiresActions,
                       List<int> requiresUpgrades, List<Action> addsActions, int addsPilotSkill, int numberOwned)
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
            this.numberOwned = numberOwned;
        }


        public Upgrade GetUpgradeClone()
        {
            Upgrade upgradeClone = (Upgrade)MemberwiseClone();
            return upgradeClone;
        }

        public UpgradeCard GetUpgradeCard(double width, double height)
        {
            if(resizedUpgradeImage == null)
            {
                resizedUpgradeImage = new BitmapImage();
                resizedUpgradeImage.BeginInit();
                resizedUpgradeImage.CacheOption = BitmapCacheOption.OnLoad;
                resizedUpgradeImage.UriSource = new Uri(@"D:\Documents\Game Stuff\X-Wing\Upgrade Cards\resized\" + id.ToString() + ".png");
                resizedUpgradeImage.EndInit();
            }

            UpgradeCard upgradeCard = new UpgradeCard();
            upgradeCard.upgradeId = id;
            upgradeCard.Source = resizedUpgradeImage;
            upgradeCard.UseLayoutRounding = true;
            upgradeCard.Height = height;
            upgradeCard.Width = width;
            RenderOptions.SetBitmapScalingMode(upgradeCard, BitmapScalingMode.HighQuality);

            return upgradeCard;
        }

        public UpgradeCanvas GetUpgradeCanvas(DefaultPage currentPage, double width, double height, Thickness margin)
        {                        
            if(resizedUpgradeImage == null)
            {
                resizedUpgradeImage = new BitmapImage();
                resizedUpgradeImage.BeginInit();
                resizedUpgradeImage.CacheOption = BitmapCacheOption.OnLoad;
                resizedUpgradeImage.UriSource = new Uri(@"D:\Documents\Game Stuff\X-Wing\Upgrade Cards\resized\" + id.ToString() + ".png");
                resizedUpgradeImage.EndInit();
            }

            System.Windows.Controls.Image upgradeImage = new System.Windows.Controls.Image();
            upgradeImage.Source = resizedUpgradeImage;

            UpgradeCanvas upgradeCanvas = new UpgradeCanvas(this, currentPage, upgradeImage, width, height, margin);


            return upgradeCanvas;
        }
    }
}
