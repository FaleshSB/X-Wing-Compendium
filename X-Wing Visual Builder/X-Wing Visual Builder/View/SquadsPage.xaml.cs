using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class SquadsPage : DefaultPage
    {
        private Build build;
        private int upgradeCardWidth = 150;
        private int upgradeCardHeight = 231;
        private int pilotCardWidth = 292;
        private int pilotCardHeight = 410;

        public SquadsPage()
        {
            build = new Build();
            InitializeComponent();

            build.AddPilot(Pilots.pilots[100]);
            build.AddPilot(Pilots.pilots[1300]);
            build.AddPilot(Pilots.pilots[900]);

            build.AddPilot(Pilots.pilots[400]);
            build.AddPilot(Pilots.pilots[2300]);
            build.AddPilot(Pilots.pilots[1900]);

            

            build.AddUpgrade(0, Upgrades.upgrades[1000]);
            build.AddUpgrade(0, Upgrades.upgrades[1001]);
            build.AddUpgrade(0, Upgrades.upgrades[1002]);
            build.AddUpgrade(0, Upgrades.upgrades[1003]);

            build.AddUpgrade(1, Upgrades.upgrades[2000]);
            build.AddUpgrade(1, Upgrades.upgrades[2001]);
            build.AddUpgrade(1, Upgrades.upgrades[2002]);
            build.AddUpgrade(1, Upgrades.upgrades[2003]);

            build.AddUpgrade(2, Upgrades.upgrades[3000]);
            build.AddUpgrade(2, Upgrades.upgrades[3001]);
            build.AddUpgrade(2, Upgrades.upgrades[3002]);
            build.AddUpgrade(2, Upgrades.upgrades[3003]);

            build.AddUpgrade(3, Upgrades.upgrades[4000]);
            build.AddUpgrade(3, Upgrades.upgrades[4001]);
            build.AddUpgrade(3, Upgrades.upgrades[4002]);
            build.AddUpgrade(3, Upgrades.upgrades[4003]);

            build.AddUpgrade(4, Upgrades.upgrades[5000]);
            build.AddUpgrade(4, Upgrades.upgrades[5001]);
            build.AddUpgrade(4, Upgrades.upgrades[5002]);
            build.AddUpgrade(4, Upgrades.upgrades[5003]);

            build.AddUpgrade(5, Upgrades.upgrades[6000]);
            build.AddUpgrade(5, Upgrades.upgrades[6001]);
            build.AddUpgrade(5, Upgrades.upgrades[6002]);
            build.AddUpgrade(5, Upgrades.upgrades[6003]);
        }
        
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            CloserTesting();
        }

        private void canvasArea_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CloserTesting();
        }

        private void PilotClicked(object sender, RoutedEventArgs e)
        {
            PilotCard pilotCard = (PilotCard)sender;
            int i = pilotCard.pilotKey;
        }

        private void DeleteUpgradeClicked(object sender, RoutedEventArgs e)
        {
            DeleteButton deleteButton = (DeleteButton)sender;
            build.DeleteUpgrade(deleteButton.pilotKey, deleteButton.upgradeKey);
            CloserTesting();
        }

        private void DeletePilotClicked(object sender, RoutedEventArgs e)
        {
            DeleteButton deleteButton = (DeleteButton)sender;
            build.DeletePilot(deleteButton.pilotKey);
            CloserTesting();
        }

        private void CloserTesting()
        {
            build.SetCanvasSize(canvasArea.ActualWidth);
            canvasArea.Children.Clear();

            Label cost = new Label();
            cost.Content = build.totalCost;
            cost.FontSize = 30;
            cost.Foreground = new SolidColorBrush(Color.FromRgb(250, 30, 30));
            Canvas.SetLeft(cost, 10);
            Canvas.SetTop(cost, 10);
            canvasArea.Children.Add(cost);


            double cardGap = Math.Round(canvasArea.ActualWidth / 640);

            List<double[]> pilotsAndWidthRemainingInRows = CalculatePilotsAndWidthRemainingInRows(build, cardGap);
            
            int currentPilotKey = -1;
            double currentHeightOffset = 40;
            double currentLeftOffset;
            double spacersGap;
            DeleteButton deleteButton;
            for (int i = 0; i < pilotsAndWidthRemainingInRows.Count; i++)
            {
                currentLeftOffset = 0;
                spacersGap = pilotsAndWidthRemainingInRows.ElementAt(i)[1] / (pilotsAndWidthRemainingInRows.ElementAt(i)[0] + 1 );
                for (int y = 0; y < pilotsAndWidthRemainingInRows.ElementAt(i)[0]; y++)
                {
                    currentPilotKey++;
                    double left = currentLeftOffset + spacersGap;
                    double height = currentHeightOffset + 40;
                    PilotCard pilotCard = build.GetPilotCard(currentPilotKey, Options.ApplyResolutionMultiplier(pilotCardWidth), Options.ApplyResolutionMultiplier(pilotCardHeight));
                    pilotCard.MouseLeftButtonDown += new MouseButtonEventHandler(PilotClicked);
                    Canvas.SetLeft(pilotCard, left);
                    Canvas.SetTop(pilotCard, height);
                    canvasArea.Children.Add(pilotCard);

                    deleteButton = new DeleteButton();
                    deleteButton.pilotKey = pilotCard.pilotKey;
                    deleteButton.MouseLeftButtonDown += new MouseButtonEventHandler(DeletePilotClicked);
                    Canvas.SetLeft(deleteButton, left);
                    Canvas.SetTop(deleteButton, height);
                    canvasArea.Children.Add(deleteButton);

                    currentLeftOffset += spacersGap + pilotCard.Width;

                    if(build.GetNumberOfUpgrades(currentPilotKey) > 0)
                    {
                        for (int z = 0; z < build.GetNumberOfUpgrades(currentPilotKey); z++)
                        {
                            left = currentLeftOffset + cardGap;
                            UpgradeCard upgradeCard = build.GetUpgradeCard(currentPilotKey, z, Options.ApplyResolutionMultiplier(upgradeCardWidth), Options.ApplyResolutionMultiplier(upgradeCardHeight));
                            if (z % 2 == 0)
                            {
                                height = currentHeightOffset;
                            }
                            else
                            {
                                height = + currentHeightOffset + Options.ApplyResolutionMultiplier(upgradeCardHeight) + cardGap;
                                currentLeftOffset += cardGap + Options.ApplyResolutionMultiplier(upgradeCardWidth);
                            }
                            if(z + 1 == build.GetNumberOfUpgrades(currentPilotKey) && build.GetNumberOfUpgrades(currentPilotKey) % 2 == 1)
                            {
                                currentLeftOffset += cardGap + Options.ApplyResolutionMultiplier(upgradeCardWidth);
                            }
                            
                            Canvas.SetLeft(upgradeCard, left);
                            Canvas.SetTop(upgradeCard, height);
                            canvasArea.Children.Add(upgradeCard);

                            deleteButton = new DeleteButton();
                            deleteButton.pilotKey = upgradeCard.pilotKey;
                            deleteButton.upgradeKey = upgradeCard.upgradeKey;
                            deleteButton.MouseLeftButtonDown += new MouseButtonEventHandler(DeleteUpgradeClicked);
                            Canvas.SetLeft(deleteButton, left);
                            Canvas.SetTop(deleteButton, height);
                            canvasArea.Children.Add(deleteButton);
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
                    pilotAndUpgradesWidth = Options.ApplyResolutionMultiplier(pilotCardWidth) + (Math.Ceiling((double)numberOfUpgrades / 2) * (Options.ApplyResolutionMultiplier(upgradeCardWidth) + cardGap));
                }
                else
                {
                    pilotAndUpgradesWidth = Options.ApplyResolutionMultiplier(pilotCardWidth);
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
