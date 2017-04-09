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
        Dictionary<int, List<int>> maneuvers;

        public Ship(int id, ShipType shipType, string name, ShipSize shipSize, bool isTIE, bool isXWing, int attack, int agility, int hull, int shields,
                    List<Action> actions, Faction faction, Dictionary<int, List<int>> maneuvers)
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
            this.maneuvers = maneuvers;
        }

        public ManeuverCard GetManeuverCard(double size)
        {
            double height = size * maneuvers.Count;
            double width = size * maneuvers.First().Value.Count;
            ManeuverCard maneuverCard = new ManeuverCard();
            maneuverCard.Source = CombineImages();
            maneuverCard.Height = height;
            maneuverCard.Width = width;
            maneuverCard.UseLayoutRounding = true;
            RenderOptions.SetBitmapScalingMode(maneuverCard, BitmapScalingMode.HighQuality);

            return maneuverCard;
        }

        private BitmapImage CombineImages()
        {
            System.Drawing.Image testSize = System.Drawing.Image.FromFile(@"D:\\Documents\\Game Stuff\\X-Wing\\Maneuvers\\0.png");
            
            int maneuverHeight = Convert.ToInt32(testSize.Height);
            int finalManeuverCardWidth = Convert.ToInt32(testSize.Width * maneuvers[2].Count);
            int finalManeuverCardHeight = Convert.ToInt32(testSize.Height * maneuvers.Count);  
                      
            Bitmap finalManeuverCard = new Bitmap(finalManeuverCardWidth, finalManeuverCardHeight);
            Graphics g = Graphics.FromImage(finalManeuverCard);
            g.Clear(System.Drawing.SystemColors.AppWorkspace);
            int row = 1;
            foreach (KeyValuePair<int, List<int>> maneuverRow in maneuvers)
            {
                int currentHeight = finalManeuverCardHeight - (row * maneuverHeight);
                int nIndex = 0;
                int currentWidth = 0;
                foreach (int maneuverName in maneuverRow.Value)
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(@"D:\\Documents\\Game Stuff\\X-Wing\\Maneuvers\\" + maneuverName.ToString() + ".png");
                    if (nIndex == 0)
                    {
                        g.DrawImage(img, new System.Drawing.Point(0, currentHeight));
                        nIndex++;
                        currentWidth = img.Width;
                    }
                    else
                    {
                        g.DrawImage(img, new System.Drawing.Point(currentWidth, currentHeight));
                        currentWidth += img.Width;
                    }
                    img.Dispose();
                }
                row++;
            }
            g.Dispose();
            BitmapImage finalConvertedManeuverCard = ConvertBitmapToBitmapImage.Convert(finalManeuverCard);
            finalManeuverCard.Dispose();
            return finalConvertedManeuverCard;
        }
    }
}
