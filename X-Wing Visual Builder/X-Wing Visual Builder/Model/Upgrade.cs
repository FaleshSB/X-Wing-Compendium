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
        /*
        public UpgradeCard GetUpgradeCard(double width, double height)
        {
            BitmapImage webImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\Upgrade Cards\resized\" + id.ToString() + "150.jpg"));
            UpgradeCard upgradeCard = new UpgradeCard();
            RenderOptions.SetBitmapScalingMode(upgradeCard, BitmapScalingMode.HighQuality);
            upgradeCard.Source = webImage;
            upgradeCard.Height = Math.Round(height);
            upgradeCard.Width = Math.Round(width);
            upgradeCard.UseLayoutRounding = true;

            return upgradeCard;
        }
        */
        public UpgradeCard GetUpgradeCard(double widthD, double heightD)
        {
            int height = (int)heightD;
            int width = (int)widthD;

            //System.Drawing.Image sourceUpgradeImage = System.Drawing.Image.FromFile(@"D:\Documents\Game Stuff\X-Wing\Upgrade Cards\" + id.ToString() + ".png");
            //BitmapImage resizedUpgradeImage = ImageResizer.ResizeImage(sourceUpgradeImage, new System.Drawing.Size(width, height));
            //sourceUpgradeImage.Dispose();

            BitmapImage resizedUpgradeImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\Upgrade Cards\" + id.ToString() + ".png"));

            UpgradeCard upgradeCard = new UpgradeCard();
            upgradeCard.id = id;
            upgradeCard.Source = resizedUpgradeImage;
            upgradeCard.Height = Convert.ToDouble(height);
            upgradeCard.Width = Convert.ToDouble(width);
            upgradeCard.UseLayoutRounding = true;
            RenderOptions.SetBitmapScalingMode(upgradeCard, BitmapScalingMode.HighQuality);
            
            return upgradeCard;
        }



        /*
        private UpgradeCard ResizeMethodOne()
        {
            double height = build.GetCanvasSize() / 8.311688312;
            double width = ((build.GetCanvasSize() / 8.311688312) / 717) * 466;
            BitmapImage webImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\Upgrade Cards\resized\" + id.ToString() + "150.jpg"));
            UpgradeCard upgradeCard = new UpgradeCard();
            RenderOptions.SetBitmapScalingMode(upgradeCard, BitmapScalingMode.HighQuality);
            upgradeCard.Source = webImage;
            upgradeCard.Height = Math.Round(height);
            upgradeCard.Width = Math.Round(width);
            upgradeCard.UseLayoutRounding = true;

            return upgradeCard;
        }

        private UpgradeCard ResizeMethodTwo()
        {
            int height = Convert.ToInt32(Math.Round(build.GetCanvasSize() / 7.48));// 6.8
            int width = Convert.ToInt32(Math.Round(((build.GetCanvasSize() / 7.48) / 717) * 466));// 7.48

            System.Drawing.Image sourceUpgradeImage = System.Drawing.Image.FromFile(@"D:\Documents\Game Stuff\X-Wing\Upgrade Cards\" + id.ToString() + ".png");
            System.Drawing.Image resizedUpgradeImage = ImageResizer.Resize(sourceUpgradeImage, new System.Drawing.Size(width, height));
            var ms = new MemoryStream();
            resizedUpgradeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;

            var bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.StreamSource = ms;
            bi.EndInit();

            //BitmapImage webImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\Upgrade Cards\" + id.ToString() + ".png"));
            UpgradeCard upgradeCard = new UpgradeCard();
            //upgradeCard.Source = webImage;
            upgradeCard.Source = bi;
            upgradeCard.Height = Convert.ToDouble(height);
            upgradeCard.Width = Convert.ToDouble(width);
            upgradeCard.UseLayoutRounding = true;
            RenderOptions.SetBitmapScalingMode(upgradeCard, BitmapScalingMode.HighQuality);

            sourceUpgradeImage.Dispose();
            resizedUpgradeImage.Dispose();
            ms.Dispose();

            return upgradeCard;
        }
        */
    }
}
