using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using X_Wing_Visual_Builder.Model;

namespace X_Wing_Visual_Builder.View
{
    /// <summary>
    /// Interaction logic for Build.xaml
    /// </summary>
    public partial class DisplayBuild : Page
    {
        private Build build = new Build();

        public DisplayBuild()
        {
            InitializeComponent();

            build.AddPilot(new Pilot());
            build.AddPilot(new Pilot());
            build.AddPilot(new Pilot());

            build.AddPilot(new Pilot());
            build.AddPilot(new Pilot());
            build.AddPilot(new Pilot());

            build.AddUpgrade(0, new Upgrade());
            build.AddUpgrade(0, new Upgrade());
            build.AddUpgrade(0, new Upgrade());
            build.AddUpgrade(0, new Upgrade());

            build.AddUpgrade(1, new Upgrade());
            build.AddUpgrade(1, new Upgrade());
            build.AddUpgrade(1, new Upgrade());
            build.AddUpgrade(1, new Upgrade());

            build.AddUpgrade(2, new Upgrade());
            build.AddUpgrade(2, new Upgrade());
            
            build.AddUpgrade(3, new Upgrade());

            build.AddUpgrade(4, new Upgrade());
            build.AddUpgrade(4, new Upgrade());
            build.AddUpgrade(4, new Upgrade());

            build.AddUpgrade(5, new Upgrade());
            build.AddUpgrade(5, new Upgrade());
            build.AddUpgrade(5, new Upgrade());
            build.AddUpgrade(5, new Upgrade());

            build.SetCanvasSize(canvasArea.ActualWidth);
        }
        
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            //AllignCards();
            //Testing();
            CloserTesting();
        }

        private void canvasArea_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CloserTesting();
        }

        private void PilotClicked(object sender, RoutedEventArgs e)
        {
            PilotCard pilotCard = (PilotCard)sender;
            int i = pilotCard.GetPilotKey();
        }

        private void CloserTesting()
        {
            build.SetCanvasSize(canvasArea.ActualWidth);
            canvasArea.Children.Clear();
            

            

            int cardGap = 3;

            List<double[]> pilotsAndWidthRemainingInRows = CalculatePilotsAndWidthRemainingInRows(build, cardGap);
            
            int currentPilotKey = -1;
            double currentHeightOffset = 40;
            double currentLeftOffset;
            double spacersGap;
            for (int i = 0; i < pilotsAndWidthRemainingInRows.Count; i++)
            {
                currentLeftOffset = 0;
                spacersGap = pilotsAndWidthRemainingInRows.ElementAt(i)[1] / (pilotsAndWidthRemainingInRows.ElementAt(i)[0] + 1 );
                for (int y = 0; y < pilotsAndWidthRemainingInRows.ElementAt(i)[0]; y++)
                {
                    currentPilotKey++;
                    double left = currentLeftOffset + spacersGap;
                    double height = currentHeightOffset + 40;
                    PilotCard pilotCard = build.GetPilotCard(currentPilotKey);
                    pilotCard.MouseLeftButtonDown += new MouseButtonEventHandler(PilotClicked);
                    Canvas.SetLeft(pilotCard, left);
                    Canvas.SetTop(pilotCard, height);
                    canvasArea.Children.Add(pilotCard);

                    currentLeftOffset += spacersGap + pilotCard.Width;

                    if(build.GetNumberOfUpgrades(currentPilotKey) > 0)
                    {
                        for (int z = 0; z < build.GetNumberOfUpgrades(currentPilotKey); z++)
                        {
                            left = currentLeftOffset + cardGap;
                            UpgradeCard upgradeCard = build.GetUpgradeCard(currentPilotKey, z);
                            if (z % 2 == 0)
                            {
                                height = currentHeightOffset;
                            }
                            else
                            {
                                height = + currentHeightOffset + upgradeCard.Height + cardGap;
                                currentLeftOffset += cardGap + upgradeCard.Width;
                            }
                            if(z + 1 == build.GetNumberOfUpgrades(currentPilotKey) && build.GetNumberOfUpgrades(currentPilotKey) % 2 == 1)
                            {
                                currentLeftOffset += cardGap + upgradeCard.Width;
                            }
                            
                            Canvas.SetLeft(upgradeCard, left);
                            Canvas.SetTop(upgradeCard, height);
                            canvasArea.Children.Add(upgradeCard);
                        }
                    }
                }
                currentHeightOffset += 500;
            }
        }

        private List<double[]> CalculatePilotsAndWidthRemainingInRows(Build build, int cardGap)
        {
            List<double[]> pilotsInRows = new List<double[]>();
            double totalWidth = 0;
            int pilotsInRow = 0;
            int numberOfUpgrades = 0;
            double pilotAndUpgradesWidth = 0;
            //double[] pilotWidthAndRemainingWidth = new double[2];
            for (int i = 0; i < build.GetNumberOfPilots(); i++)
            {
                numberOfUpgrades = build.GetNumberOfUpgrades(i);
                if (numberOfUpgrades > 0)
                {
                    pilotAndUpgradesWidth = build.GetPilotCard(i).Width + (Math.Ceiling((double)numberOfUpgrades / 2) * (build.GetUpgradeCard(i, 0).Width + cardGap));
                }
                else
                {
                    pilotAndUpgradesWidth = build.GetPilotCard(i).Width;
                }

                if(pilotAndUpgradesWidth + totalWidth > (canvasArea.ActualWidth - 100))
                {
                    //pilotWidthAndRemainingWidth = new double[2];
                    //pilotWidthAndRemainingWidth[0] = pilotsInRow;
                    //pilotWidthAndRemainingWidth[1] = totalWidth - pilotAndUpgradesWidth;
                    pilotsInRows.Add(new double[2] { pilotsInRow, canvasArea.ActualWidth - totalWidth });
                    pilotsInRow = 1;
                    totalWidth = pilotAndUpgradesWidth;
                }
                else
                {
                    pilotsInRow++;
                    totalWidth += pilotAndUpgradesWidth;
                }
            }
            //pilotWidthAndRemainingWidth = new double[2];
            //pilotWidthAndRemainingWidth[0] = pilotsInRow;
            //pilotWidthAndRemainingWidth[1] = totalWidth - pilotAndUpgradesWidth;
            pilotsInRows.Add(new double[2] { pilotsInRow, canvasArea.ActualWidth - totalWidth });
            return pilotsInRows;
        }







        private void Testing()
        {/*
            List<Pilot> pilots = new List<Pilot>();
            Pilot pilotOne = new Pilot();
            Pilot pilotTwo = new Pilot();
            Pilot pilotThree = new Pilot();
            Upgrade upgrade = new Upgrade();
            pilotOne.AddUpgrade(new Upgrade());
            pilotOne.AddUpgrade(new Upgrade());
            pilotOne.AddUpgrade(new Upgrade());
            pilotOne.AddUpgrade(new Upgrade());
            pilotOne.AddUpgrade(new Upgrade());

            pilotTwo.AddUpgrade(new Upgrade());
            pilotTwo.AddUpgrade(new Upgrade());

            pilots.Add(pilotOne);
            pilots.Add(pilotTwo);
            pilots.Add(pilotThree);


            double totalWidth = 0;
            List<Pilot> pilotsInRow = new List<Pilot>();
            for (int i = 0; i < pilots.Count; i++)
            {
                totalWidth += pilots.ElementAt(i).GetPilotCardAndUpgradesWidth();
                if(totalWidth > canvasArea.ActualWidth)
                {
                    totalWidth = 0;
                    // display pilotsInRow
                    pilotsInRow.Clear();
                }
                else
                {
                    pilotsInRow.Add(pilots.ElementAt(i));
                }
            }
            if(pilotsInRow.Count > 0)
            {
                double totalWidthUsed = 0;
                for (int i = 0; i < pilotsInRow.Count; i++)
                {
                    totalWidthUsed += pilotsInRow.ElementAt(i).GetPilotCardAndUpgradesWidth();
                }
                double spacersGaps = (canvasArea.ActualWidth - totalWidthUsed) / (pilotsInRow.Count * 2);

                double currentLeftOffset = 0;
                for (int i = 0; i < pilotsInRow.Count; i++)
                {
                    double left = currentLeftOffset + spacersGaps;
                    double height = 100;
                    PilotCard pilotCard = pilotsInRow.ElementAt(i).GetPilotCard();
                    pilotCard.SetPilotKey(9);
                    pilotCard.MouseLeftButtonDown += new MouseButtonEventHandler(PilotClicked);
                    Canvas.SetLeft(pilotCard, left);
                    Canvas.SetTop(pilotCard, height);
                    canvasArea.Children.Add(pilotCard);

                    for(int x = 0; x < pilotsInRow.ElementAt(i).GetUpgrades().Count; x++)
                    {
                        left = currentLeftOffset + spacersGaps + pilotCard.Width + 5;
                        if(x > 1)
                        {
                            left += (Math.Ceiling(((double)x - 1) / 2) * (upgrade.GetCardWidth() + 5));
                        }
                        UpgradeCard upgradeCard = pilotsInRow.ElementAt(i).GetUpgrades().ElementAt(x).GetUpgradeCard();
                        if (x % 2 == 0)
                        {
                            height = 100;
                        }
                        else
                        {
                            height = + 100 + upgradeCard.Height + 5;
                        }
                        
                        
                        Canvas.SetLeft(upgradeCard, left);
                        Canvas.SetTop(upgradeCard, height);
                        canvasArea.Children.Add(upgradeCard);
                    }
           

                    currentLeftOffset += spacersGaps + pilotsInRow.ElementAt(i).GetPilotCardAndUpgradesWidth();
                }
            }



                /*
                double pilotOneWidth = 300 + 10 + ((pilotOne.GetUpgrades().Count * 123) + 20);
                double pilotTwoWidth = 300 + 10 + ((pilotTwo.GetUpgrades().Count * 123) + 20);
                double pilotThreeWidth = 300 + 10 + ((pilotThree.GetUpgrades().Count * 123) + 20);
                double width = pilots.ElementAt(0).GetPilotCard().Width;
                double totalUsedWidth = pilotOneWidth + pilotTwoWidth + pilotThreeWidth;
                double freeWidth = canvasArea.ActualWidth - totalUsedWidth;

                // (freeWidth / (pilots * 2)) pilotcard & upgrades (freeWidth / 6) (freeWidth / 6) pilotcard & upgrades (freeWidth / 6) (freeWidth / 6) pilotcard & upgrades (freeWidth / 6) 


                double totalWidth = 0;
                List<Pilot> pilotsInRow = new List<Pilot>();
                for (int i = 0; i < pilots.Count; i++)
                {
                    totalWidth += pilots.ElementAt(i).GetPilotCard().Width + 40;
                    if (pilots.ElementAt(i).GetUpgrades().Count > 0)
                    {
                        totalWidth += Math.Ceiling((double)pilots.ElementAt(i).GetUpgrades().Count / 2) * (pilots.ElementAt(i).GetUpgrades().ElementAt(0).GetUpgradeCard().Width + 10);
                    }

                    if(totalWidth > canvasArea.ActualWidth)
                    {
                        double currentWidth = 0;
                        for (int x = 0; x < pilotsInRow.Count; x++)
                        {
                        }
                    }
                    else
                    {
                        pilotsInRow.Add(pilots.ElementAt(i));
                    }
                }*/
        }



        private void AllignCards()
        {
            /*
            //Shape one = new Rectangle() { Fill = Brushes.Blue, Height = 5, Width = 5, RadiusX = 4, RadiusY = 4 };
            //Shape two = new Rectangle() { Fill = Brushes.Blue, Height = 5, Width = 5, RadiusX = 4, RadiusY = 4 };
            //Shape three = new Rectangle() { Fill = Brushes.Blue, Height = 5, Width = 5, RadiusX = 4, RadiusY = 4 };
            List<Shape> pilotCards = new List<Shape>();
            //pilotCards.Add(one);
            //pilotCards.Add(two);
            //pilotCards.Add(three);
            pilotCards.Add(new System.Windows.Shapes.Rectangle() { Fill = System.Windows.Media.Brushes.Blue, Height = 45, Width = 45, RadiusX = 4, RadiusY = 4 });


            //System.Drawing.Image image = System.Drawing.Image.FromFile(@"D:\Documents\Game Stuff\Board Games\X-Wing\Pilots\BlueAce.png");

            for (int i = 0; i < pilotCards.Count; i++)
            {
                double devidedWidth = canvasArea.ActualWidth / pilotCards.Count;
                double devidedHeight = canvasArea.ActualHeight / pilotCards.Count;
                double left = ((devidedWidth * i) + (devidedWidth / 2)) - (pilotCards.ElementAt(i).ActualWidth / 2);
                double height = ((devidedHeight * i) + (devidedHeight / 2)) - (pilotCards.ElementAt(i).ActualHeight / 2);

                Canvas.SetLeft(pilotCards.ElementAt(i), left);
                Canvas.SetTop(pilotCards.ElementAt(i), height);
                canvasArea.Children.Add(pilotCards.ElementAt(i));
            }
            */

            List<Image> pilotCards = new List<Image>();

            BitmapImage webImage = new BitmapImage(new Uri("D:\\Documents\\Game Stuff\\Board Games\\X-Wing\\Pilots\\BlueAce.png"));
            Image pilotCard = new Image();
            pilotCard.Source = webImage;
            pilotCard.Height = 426;
            pilotCard.Width = 300;
            pilotCards.Add(pilotCard);

            webImage = new BitmapImage(new Uri("D:\\Documents\\Game Stuff\\Board Games\\X-Wing\\Pilots\\BlueAce.png"));
            pilotCard = new Image();
            pilotCard.Source = webImage;
            pilotCard.Height = 426;
            pilotCard.Width = 300;
            pilotCards.Add(pilotCard);

            webImage = new BitmapImage(new Uri("D:\\Documents\\Game Stuff\\Board Games\\X-Wing\\Pilots\\BlueAce.png"));
            pilotCard = new Image();
            pilotCard.Source = webImage;
            pilotCard.Height = 426;
            pilotCard.Width = 300;
            pilotCards.Add(pilotCard);

            webImage = new BitmapImage(new Uri("D:\\Documents\\Game Stuff\\Board Games\\X-Wing\\Pilots\\BlueAce.png"));
            pilotCard = new Image();
            pilotCard.Source = webImage;
            pilotCard.Height = 426;
            pilotCard.Width = 300;
            pilotCards.Add(pilotCard);


            int pilotCardWidth = 300;
            int pilotCardHeight = 426;

            double height = 50;
            for (int i = 0; i < pilotCards.Count; i++)
            {
                double devidedWidth = canvasArea.ActualWidth / pilotCards.Count;
                double devidedHeight = canvasArea.ActualHeight / pilotCards.Count;
                double cardWidth = pilotCards.ElementAt(i).ActualWidth;
                double left = ((devidedWidth * i) + (devidedWidth / 2)) - (pilotCardWidth / 2);
                

                Canvas.SetLeft(pilotCards.ElementAt(i), left);
                Canvas.SetTop(pilotCards.ElementAt(i), height);
                canvasArea.Children.Add(pilotCards.ElementAt(i));
            }
        }
    }
}
