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
        private UpgradeType upgradeType;

        private double cardWidth;
        private double cardHeight;

        public Upgrade(double canvasArea)
        {
            cardWidth = canvasArea / 14.117647;
            cardHeight = cardWidth * 1.544;
        }

        public UpgradeCard GetUpgradeCard()
        {
            BitmapImage webImage = new BitmapImage(new Uri("D:\\Documents\\Game Stuff\\Board Games\\X-Wing\\Cards\\Accuracy-corrector.png"));
            UpgradeCard upgradeCard = new UpgradeCard();
            upgradeCard.Source = webImage;
            upgradeCard.Height = cardHeight;
            upgradeCard.Width = cardWidth;

            return upgradeCard;
        }

        public double GetCardWidth()
        {
            return cardWidth;
        }
    }
}
