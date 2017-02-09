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
        private List<Upgrade> upgrades = new List<Upgrade>();
        private Build build;

        public void AddBuild(Build build)
        {
            this.build = build;
        }

        // TODO add where you can buy this card
        // TODO add maneuver card array[5][9] maneuver[4][0] = Maneuver.Green is a green 4 turn left, maneuver[5][8] = Maneuver.Red is a red 5 K turn    

        public PilotCard GetPilotCard()
        {
            double height = build.GetCanvasSize() / 4.507042254;
            double width = (height / 426) * 300;

            BitmapImage webImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\Pilots\BlueAce.png"));
            PilotCard pilotCard = new PilotCard();
            pilotCard.Source = webImage;
            pilotCard.Height = Math.Round(height);
            pilotCard.Width = Math.Round(width);
            pilotCard.UseLayoutRounding = true;

            return pilotCard;
        }

        public void AddUpgrade(Upgrade upgrade)
        {
            upgrades.Add(upgrade);
        }

        public List<Upgrade> GetUpgrades()
        {
            return upgrades;
        }
    }
}
