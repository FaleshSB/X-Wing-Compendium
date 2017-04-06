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
    public class Ship
    {
        public int id;
        public ShipType shipType;
        public string name;
        public ShipSize shipSize;
        public bool isTIE;
        public bool isXWing;
        public int attack;
        public int agility;
        public int hull;
        public int shields;
        public List<Action> actions = new List<Action>();
        public Faction faction;

        public Ship(int id, ShipType shipType, string name, ShipSize shipSize, bool isTIE, bool isXWing, int attack, int agility, int hull, int shields, List<Action> actions, Faction faction)
        {
            this.id = id;
            this.shipType = shipType;
            this.name = name;
            this.shipSize = shipSize;
            this.isTIE = isTIE;
            this.isXWing = isXWing;
            this.attack = attack;
            this.agility = agility;
            this.hull = hull;
            this.shields = shields;
            this.actions = actions;
            this.faction = faction;
        }

        public ManeuverCard GetManeuverCard(double widthD, double heightD)
        {
            int height = (int)heightD;
            int width = (int)widthD;

            BitmapImage resizedUpgradeImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\Maneuvers\25.png"));

            ManeuverCard maneuverCard = new ManeuverCard();
            maneuverCard.Source = resizedUpgradeImage;
            maneuverCard.Height = Convert.ToDouble(height);
            maneuverCard.Width = Convert.ToDouble(width);
            maneuverCard.UseLayoutRounding = true;
            RenderOptions.SetBitmapScalingMode(maneuverCard, BitmapScalingMode.HighQuality);

            return maneuverCard;
        }
    }
}
