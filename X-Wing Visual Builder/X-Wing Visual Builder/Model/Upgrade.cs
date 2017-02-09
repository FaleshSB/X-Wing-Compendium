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

        private string name;
        private string type;
        private int cost;
        private string description;
        private string faq;

        public Upgrade()
        {

        }
        public Upgrade(string type, int cost, string name, string description, string faq)
        {
            this.type = type;
            this.cost = cost;
            this.name = name;
            this.description = description;
            this.faq = faq;
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
            BitmapImage webImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\conner-net.png"));
            UpgradeCard upgradeCard = new UpgradeCard();
            upgradeCard.Source = webImage;
            upgradeCard.Height = Math.Round(height);
            upgradeCard.Width = Math.Round(width);
            upgradeCard.UseLayoutRounding = true;

            return upgradeCard;
        }
    }
}
