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


            Upgrades upgrades = new Upgrades();

            build.AddUpgrade(0, upgrades.GetUpgrade(13006));
            build.AddUpgrade(0, upgrades.GetUpgrade(14004));
            build.AddUpgrade(0, upgrades.GetUpgrade(8030));
            build.AddUpgrade(0, upgrades.GetUpgrade(7001));

            build.AddUpgrade(1, upgrades.GetUpgrade(6001));
            build.AddUpgrade(1, upgrades.GetUpgrade(5001));
            build.AddUpgrade(1, upgrades.GetUpgrade(4001));
            build.AddUpgrade(1, upgrades.GetUpgrade(3001));

            build.AddUpgrade(2, upgrades.GetUpgrade(2001));
            build.AddUpgrade(2, upgrades.GetUpgrade(1001));
            build.AddUpgrade(2, upgrades.GetUpgrade(9001));
            build.AddUpgrade(2, upgrades.GetUpgrade(10001));
            
            build.AddUpgrade(3, upgrades.GetUpgrade(11001));

            build.AddUpgrade(4, upgrades.GetUpgrade(12001));
            build.AddUpgrade(4, upgrades.GetUpgrade(7003));
            build.AddUpgrade(4, upgrades.GetUpgrade(6002));

            build.AddUpgrade(5, upgrades.GetUpgrade(4006));
            build.AddUpgrade(5, upgrades.GetUpgrade(3007));
            build.AddUpgrade(5, upgrades.GetUpgrade(2004));
            build.AddUpgrade(5, upgrades.GetUpgrade(1009));
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
            

            

            double cardGap = Math.Round(canvasArea.ActualWidth / 640);

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

        private List<double[]> CalculatePilotsAndWidthRemainingInRows(Build build, double cardGap)
        {
            List<double[]> pilotsAndWidthRemainingInRows = new List<double[]>();
            double totalWidth = 0;
            int pilotsInRow = 0;
            int numberOfUpgrades = 0;
            double pilotAndUpgradesWidth = 0;
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
                    pilotsAndWidthRemainingInRows.Add(new double[2] { pilotsInRow, canvasArea.ActualWidth - totalWidth });
                    pilotsInRow = 1;
                    totalWidth = pilotAndUpgradesWidth;
                }
                else
                {
                    pilotsInRow++;
                    totalWidth += pilotAndUpgradesWidth;
                }
            }
            pilotsAndWidthRemainingInRows.Add(new double[2] { pilotsInRow, canvasArea.ActualWidth - totalWidth });
            return pilotsAndWidthRemainingInRows;
        }
    }
}
