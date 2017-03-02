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
        private Build build { get; set; } = new Build();
        public void SetBuild(Build build)
        {
            this.build = build;
        }
        private int upgradeCardWidth = 150;
        private int upgradeCardHeight = 231;
        private int pilotCardWidth = 292;
        private int pilotCardHeight = 410;

        public SquadsPage()
        {
            Pages.pages[PageName.SquadsPage] = this;            
            InitializeComponent();
        }

        private void contentCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DisplayContent();
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
            DisplayContent();
        }

        private void DeletePilotClicked(object sender, RoutedEventArgs e)
        {
            DeleteButton deleteButton = (DeleteButton)sender;
            build.DeletePilot(deleteButton.pilotKey);
            DisplayContent();
        }

        protected override void DisplayContent()
        {
            build.SetCanvasSize(contentCanvas.ActualWidth);
            contentCanvas.Children.Clear();

            Label cost = new Label();
            cost.Content = build.totalCost;
            cost.FontSize = 30;
            cost.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            Canvas.SetLeft(cost, 940);
            Canvas.SetTop(cost, 10);
            contentCanvas.Children.Add(cost);

            Button addPilot = new Button();
            addPilot.Click += new RoutedEventHandler(AddPilotClicked);
            addPilot.Content = "Add Pilot";
            Canvas.SetLeft(addPilot, 870);
            Canvas.SetTop(addPilot, 25);
            contentCanvas.Children.Add(addPilot);

            double cardGap = Math.Round(contentCanvas.ActualWidth / 640);

            List<double[]> pilotsAndWidthRemainingInRows = CalculatePilotsAndWidthRemainingInRows(build, cardGap);
            
            int currentPilotKey = -1;
            double currentHeightOffset = 60;
            double currentLeftOffset;
            double spacersGap;
            DeleteButton deleteButton;
            AddUpgradeButton addUpgrade;
            Label pilotTotalCostLabel;
            for (int i = 0; i < pilotsAndWidthRemainingInRows.Count; i++)
            {
                currentLeftOffset = 0;
                spacersGap = pilotsAndWidthRemainingInRows.ElementAt(i)[1] / (pilotsAndWidthRemainingInRows.ElementAt(i)[0] + 1 );
                for (int y = 0; y < pilotsAndWidthRemainingInRows.ElementAt(i)[0]; y++)
                {
                    currentPilotKey++;
                    double left = currentLeftOffset + spacersGap;
                    double height = currentHeightOffset;
                    PilotCard pilotCard = build.GetPilotCard(currentPilotKey, Opt.ApResMod(pilotCardWidth), Opt.ApResMod(pilotCardHeight));
                    pilotCard.MouseLeftButtonDown += new MouseButtonEventHandler(PilotClicked);
                    Canvas.SetLeft(pilotCard, left);
                    Canvas.SetTop(pilotCard, height);
                    contentCanvas.Children.Add(pilotCard);

                    deleteButton = new DeleteButton();
                    deleteButton.pilotKey = pilotCard.pilotKey;
                    deleteButton.MouseLeftButtonDown += new MouseButtonEventHandler(DeletePilotClicked);
                    Canvas.SetLeft(deleteButton, left);
                    Canvas.SetTop(deleteButton, height);
                    contentCanvas.Children.Add(deleteButton);

                    addUpgrade = new AddUpgradeButton();
                    addUpgrade.pilotKey = pilotCard.pilotKey;
                    addUpgrade.FontSize = 16;
                    addUpgrade.FontWeight = FontWeights.Bold;
                    addUpgrade.Click += new RoutedEventHandler(AddUpgrade);
                    addUpgrade.Content = "Add Upgrade";
                    Canvas.SetLeft(addUpgrade, left);
                    Canvas.SetTop(addUpgrade, height + Opt.ApResMod(pilotCardHeight) + 10);
                    contentCanvas.Children.Add(addUpgrade);
                    
                    pilotTotalCostLabel = new Label();
                    pilotTotalCostLabel.Content = build.GetPilot(currentPilotKey).totalCost;
                    pilotTotalCostLabel.FontSize = 30;
                    pilotTotalCostLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    Canvas.SetLeft(pilotTotalCostLabel, left + 240);
                    Canvas.SetTop(pilotTotalCostLabel, height + Opt.ApResMod(pilotCardHeight));
                    contentCanvas.Children.Add(pilotTotalCostLabel);

                    currentLeftOffset += spacersGap + pilotCard.Width;

                    if(build.GetNumberOfUpgrades(currentPilotKey) > 0)
                    {
                        for (int z = 0; z < build.GetNumberOfUpgrades(currentPilotKey); z++)
                        {
                            left = currentLeftOffset + cardGap;
                            UpgradeCard upgradeCard = build.GetUpgradeCard(currentPilotKey, z, Opt.ApResMod(upgradeCardWidth), Opt.ApResMod(upgradeCardHeight));
                            if (z % 2 == 0)
                            {
                                height = currentHeightOffset;
                            }
                            else
                            {
                                height = + currentHeightOffset + Opt.ApResMod(upgradeCardHeight) + cardGap;
                                currentLeftOffset += cardGap + Opt.ApResMod(upgradeCardWidth);
                            }
                            if(z + 1 == build.GetNumberOfUpgrades(currentPilotKey) && build.GetNumberOfUpgrades(currentPilotKey) % 2 == 1)
                            {
                                currentLeftOffset += cardGap + Opt.ApResMod(upgradeCardWidth);
                            }
                            
                            Canvas.SetLeft(upgradeCard, left);
                            Canvas.SetTop(upgradeCard, height);
                            contentCanvas.Children.Add(upgradeCard);

                            deleteButton = new DeleteButton();
                            deleteButton.pilotKey = upgradeCard.pilotKey;
                            deleteButton.upgradeKey = upgradeCard.upgradeKey;
                            deleteButton.MouseLeftButtonDown += new MouseButtonEventHandler(DeleteUpgradeClicked);
                            Canvas.SetLeft(deleteButton, left);
                            Canvas.SetTop(deleteButton, height);
                            contentCanvas.Children.Add(deleteButton);
                        }
                    }
                }
                currentHeightOffset += 500;
            }
        }

        private void AddPilotClicked(object sender, RoutedEventArgs e)
        {
            BrowseCardsPage browseCardsPage = (BrowseCardsPage)Pages.pages[PageName.BrowseCards];
            browseCardsPage.AddPilot(build);
            NavigationService.Navigate(browseCardsPage);
        }

        private void AddUpgrade(object sender, RoutedEventArgs e)
        {
            AddUpgradeButton addUpgradeButton = (AddUpgradeButton)sender;
            BrowseCardsPage browseCardsPage = (BrowseCardsPage)Pages.pages[PageName.BrowseCards];
            browseCardsPage.AddAvailibleUpdates(addUpgradeButton.pilotKey, build);
            NavigationService.Navigate(browseCardsPage);
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
                    pilotAndUpgradesWidth = Opt.ApResMod(pilotCardWidth) + (Math.Ceiling((double)numberOfUpgrades / 2) * (Opt.ApResMod(upgradeCardWidth) + cardGap));
                }
                else
                {
                    pilotAndUpgradesWidth = Opt.ApResMod(pilotCardWidth);
                }

                if(pilotAndUpgradesWidth + totalWidth > (contentCanvas.ActualWidth - 100))
                {
                    pilotsAndWidthRemainingInRows.Add(new double[2] { pilotsInRow, contentCanvas.ActualWidth - totalWidth });
                    pilotsInRow = 1;
                    totalWidth = pilotAndUpgradesWidth;
                }
                else
                {
                    pilotsInRow++;
                    totalWidth += pilotAndUpgradesWidth;
                }
            }
            pilotsAndWidthRemainingInRows.Add(new double[2] { pilotsInRow, contentCanvas.ActualWidth - totalWidth });
            return pilotsAndWidthRemainingInRows;
        }
    }
}
