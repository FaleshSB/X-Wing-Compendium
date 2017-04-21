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
        private System.Windows.Controls.Image upgradeImage = null;

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

        public UpgradeCanvas GetUpgradeCanvas()
        {                        
            if(upgradeImage == null)
            {
                BitmapImage resizedUpgradeImage = new BitmapImage();
                resizedUpgradeImage.BeginInit();
                resizedUpgradeImage.CacheOption = BitmapCacheOption.OnLoad;
                resizedUpgradeImage.UriSource = new Uri(@"D:\Documents\Game Stuff\X-Wing\Upgrade Cards\resized\" + id.ToString() + ".png");
                resizedUpgradeImage.EndInit();

                upgradeImage = new System.Windows.Controls.Image();
                upgradeImage.Source = resizedUpgradeImage;
            }

            UpgradeCanvas upgradeCanvas = new UpgradeCanvas(this, upgradeImage);


            return upgradeCanvas;
            /*
            upgradeCanvas.Width = Opt.ApResMod(width);
            upgradeCanvas.Height = Opt.ApResMod(height);
            upgradeCanvas.Margin = margin;

            UpgradeCard upgradeCard = GetUpgradeCard(Opt.ApResMod(width), Opt.ApResMod(height));
            //upgradeCard.MouseLeftButtonDown += new MouseButtonEventHandler(CardClicked);
            upgradeCard.MouseEnter += new MouseEventHandler(UpgradeMouseHover);
            upgradeCard.MouseLeave += new MouseEventHandler(UpgradeMouseHoverLeave);

            Canvas.SetLeft(upgradeCard, 0);
            Canvas.SetTop(upgradeCard, 0);
            upgradeCanvas.Children.Add(upgradeCard);

            InfoButton infoButton = new InfoButton(22, 22);
            infoButton.upgradeId = id;
            infoButton.Cursor = Cursors.Hand;
            infoButton.MouseLeftButtonDown += new MouseButtonEventHandler(UpgradeInfoClicked);
            //infoButton.MouseEnter += new MouseEventHandler(UpgradeInfoMouseHover);
            //infoButton.MouseLeave += new MouseEventHandler(PilotMouseHoverLeave);
            infoButton.MouseWheel += new MouseWheelEventHandler(ContentScroll);

            Canvas.SetLeft(infoButton, 0);
            Canvas.SetTop(infoButton, 0);
            upgradeCanvas.Children.Add(infoButton);

            OutlinedTextBlock numberOwned = new OutlinedTextBlock();
            numberOwned.Text = numberOwned.ToString();
            numberOwned.TextAlignment = TextAlignment.Left;
            numberOwned.Width = 30;
            numberOwned.Height = 30;
            numberOwned.StrokeThickness = 1;
            numberOwned.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            numberOwned.FontWeight = FontWeights.ExtraBold;
            numberOwned.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 207, 76));
            numberOwned.FontSize = Opt.ApResMod(20);
            numberOwned.FontFamily = new System.Windows.Media.FontFamily("Verdana");
            Canvas.SetLeft(numberOwned, 1);
            Canvas.SetBottom(numberOwned, 64);
            upgradeCanvas.Children.Add(numberOwned);

            AddButton addUpgrade = new AddButton(22, 22);
            addUpgrade.upgradeId = id;
            addUpgrade.MouseLeftButtonDown += new MouseButtonEventHandler(AddUpgradeClicked);
            addUpgrade.MouseWheel += new MouseWheelEventHandler(ContentScroll);
            addUpgrade.MouseEnter += new MouseEventHandler(UpgradeMouseHover);
            addUpgrade.MouseLeave += new MouseEventHandler(UpgradeMouseHoverLeave);
            addUpgrade.Visibility = Visibility.Hidden;
            Canvas.SetLeft(addUpgrade, 0);
            Canvas.SetTop(addUpgrade, 140);
            upgradeCanvas.Children.Add(addUpgrade);

            RemoveButton removeUpgrade = new RemoveButton(22, 22);
            removeUpgrade.upgradeId = id;
            removeUpgrade.MouseLeftButtonDown += new MouseButtonEventHandler(RemoveUpgradeClicked);
            removeUpgrade.MouseWheel += new MouseWheelEventHandler(ContentScroll);
            removeUpgrade.MouseEnter += new MouseEventHandler(UpgradeMouseHover);
            removeUpgrade.MouseLeave += new MouseEventHandler(UpgradeMouseHoverLeave);
            removeUpgrade.Visibility = Visibility.Hidden;
            Canvas.SetLeft(removeUpgrade, 0);
            Canvas.SetTop(removeUpgrade, 190);
            upgradeCanvas.Children.Add(removeUpgrade);
            */
        }
    }
}
