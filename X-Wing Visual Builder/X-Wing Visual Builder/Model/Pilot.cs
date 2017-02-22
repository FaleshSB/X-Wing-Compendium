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
    class Pilot
    {
        public int id { get; set; }
        public Ship ship { get; set; }
        public bool isUnique { get; set; }
        public string name { get; set; }
        public int pilotSkill { get; set; }
        public string description { get; set; }
        public Dictionary<UpgradeType, int> possibleUpgrades { get; set; } = new Dictionary<UpgradeType, int>();
        public int cost { get; set; }
        public string faq { get; set; }
        public Faction faction { get; set; }

        public List<Upgrade> upgrades { get; set; } = new List<Upgrade>();

        public Pilot(int id, Ship ship, bool isUnique, string name, int pilotSkill, string description, Dictionary<UpgradeType, int> possibleUpgrades, int cost, string faq, Faction faction)
        {
            this.id = id;
            this.ship = ship;
            this.isUnique = isUnique;
            this.name = name;
            this.pilotSkill = pilotSkill;
            this.description = description;
            this.possibleUpgrades = possibleUpgrades;
            this.cost = cost;
            this.faq = faq;
            this.faction = faction;
        }
        
        public PilotCard GetPilotCard(double widthD, double heightD)
        {
            int height = (int)heightD;
            int width = (int)widthD;

            System.Drawing.Image sourceUpgradeImage = System.Drawing.Image.FromFile(@"D:\Documents\Game Stuff\X-Wing\Pilot Cards\" + id.ToString() + ".png");
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
            PilotCard pilotCard = new PilotCard();
            //upgradeCard.Source = webImage;
            pilotCard.Source = bi;
            pilotCard.Height = Convert.ToDouble(height);
            pilotCard.Width = Convert.ToDouble(width);
            pilotCard.UseLayoutRounding = true;
            RenderOptions.SetBitmapScalingMode(pilotCard, BitmapScalingMode.HighQuality);

            sourceUpgradeImage.Dispose();
            resizedUpgradeImage.Dispose();
            ms.Dispose();

            return pilotCard;
        }
    }
}
