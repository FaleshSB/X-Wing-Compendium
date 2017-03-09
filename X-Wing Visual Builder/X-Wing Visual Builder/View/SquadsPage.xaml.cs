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
        private int upgradeCardWidth = 150;
        private int upgradeCardHeight = 231;
        private int pilotCardWidth = 292;
        private int pilotCardHeight = 410;
        private int upgradeCardMargin = 4;
        private Canvas pilotCanvas;
        private AlignableWrapPanel contentWrapPanel;
        private AlignableWrapPanel buildWrapPanel;

        public SquadsPage()
        {
            Pages.pages[PageName.SquadsPage] = this;
            InitializeComponent();

            contentWrapPanel = new AlignableWrapPanel();
            contentWrapPanel.Name = "contentWrapPanel";
            contentWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentScrollViewer.Content = contentWrapPanel;

            Button addBuildButton = new Button();
            addBuildButton.Name = "addBuildButton";
            addBuildButton.Content = "Add Build";
            addBuildButton.Width = 130;
            addBuildButton.Height = 40;
            addBuildButton.FontSize = 16;
            addBuildButton.FontWeight = FontWeights.Bold;
            addBuildButton.Click += new RoutedEventHandler(AddBuildClicked);
            addBuildButton.UseLayoutRounding = true;
            Canvas.SetLeft(addBuildButton, 400);
            Canvas.SetTop(addBuildButton, 10);
            topToolsCanvas.Children.Add(addBuildButton);

            Button addPilotButton = new Button();
            addPilotButton.Name = "addPilotButton";
            addPilotButton.Content = "Add Pilot";
            addPilotButton.Width = 130;
            addPilotButton.Height = 40;
            addPilotButton.FontSize = 16;
            addPilotButton.FontWeight = FontWeights.Bold;
            addPilotButton.Click += new RoutedEventHandler(AddPilotClicked);
            addPilotButton.UseLayoutRounding = true;
            Canvas.SetLeft(addPilotButton, 530);
            Canvas.SetTop(addPilotButton, 10);
            topToolsCanvas.Children.Add(addPilotButton);

            Button browseCards = new Button();
            browseCards.Name = "browseCards";
            browseCards.Content = "Browse Cards";
            browseCards.Width = 130;
            browseCards.Height = 40;
            browseCards.FontSize = 16;
            browseCards.FontWeight = FontWeights.Bold;
            browseCards.Click += new RoutedEventHandler(browseCardsClicked);
            browseCards.UseLayoutRounding = true;
            Canvas.SetLeft(browseCards, 660);
            Canvas.SetTop(browseCards, 10);
            topToolsCanvas.Children.Add(browseCards);

            topToolsCanvas.Height = 40;
        }

        private void browseCardsClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate((BrowseCardsPage)Pages.pages[PageName.BrowseCards]);
        }

        private void AddBuildClicked(object sender, RoutedEventArgs e)
        {
            Builds.AddBuild(Faction.Rebel);
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
            Builds.GetBuild(deleteButton.uniqueBuildId).DeleteUpgrade(deleteButton.uniquePilotId, deleteButton.upgradeId);
            DisplayContent();
        }

        private void DeletePilotClicked(object sender, RoutedEventArgs e)
        {
            DeleteButton deleteButton = (DeleteButton)sender;
            Builds.GetBuild(deleteButton.uniqueBuildId).DeletePilot(deleteButton.uniquePilotId);
            DisplayContent();
        }

        protected override void DisplayContent()
        {
            contentWrapPanel.Children.Clear();
            foreach(Build build in Builds.builds)
            { 
                buildWrapPanel = new AlignableWrapPanel();
                buildWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;

                Canvas spacerCanvas = new Canvas();
                spacerCanvas.Height = 30;
                spacerCanvas.Width = 1800;

                Label totalCostLabel;
                totalCostLabel = new Label();
                totalCostLabel.Content = build.totalCost;
                totalCostLabel.FontSize = 20;
                totalCostLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                Canvas.SetLeft(totalCostLabel, 900);
                Canvas.SetTop(totalCostLabel, 0);
                spacerCanvas.Children.Add(totalCostLabel);

                buildWrapPanel.Children.Add(spacerCanvas);
                
                List<KeyValuePair<int, Pilot>> pilots = build.pilots.OrderByDescending(pilot => pilot.Value.pilotSkill).ThenByDescending(pilot => pilot.Value.cost).ToList();

                foreach (KeyValuePair<int, Pilot> pilot in pilots)
                {
                    double left = 0;
                    double height = 0;
                    double currentLeftOffset = 0;
                    double currentHeightOffset = 0;
                    pilotCanvas = new Canvas();

                    PilotCard pilotCard = pilot.Value.GetPilotCard(Opt.ApResMod(pilotCardWidth), Opt.ApResMod(pilotCardHeight));
                    pilotCard.MouseLeftButtonDown += new MouseButtonEventHandler(PilotClicked);
                    Canvas.SetLeft(pilotCard, left);
                    Canvas.SetTop(pilotCard, height);
                    pilotCanvas.Children.Add(pilotCard);

                    DeleteButton deleteButton;
                    deleteButton = new DeleteButton();
                    deleteButton.uniqueBuildId = build.uniqueBuildId;
                    deleteButton.uniquePilotId = pilot.Value.uniquePilotId;
                    deleteButton.MouseLeftButtonDown += new MouseButtonEventHandler(DeletePilotClicked);
                    Canvas.SetLeft(deleteButton, left + (Opt.ApResMod(pilotCardWidth) - deleteButton.Width));
                    Canvas.SetTop(deleteButton, height);
                    pilotCanvas.Children.Add(deleteButton);

                    BuildPilotUpgrade addUpgrade;
                    addUpgrade = new BuildPilotUpgrade();
                    addUpgrade.uniquePilotId = pilot.Value.uniquePilotId;
                    addUpgrade.uniqueBuildId = build.uniqueBuildId;
                    addUpgrade.FontSize = 16;
                    addUpgrade.FontWeight = FontWeights.Bold;
                    addUpgrade.Click += new RoutedEventHandler(AddUpgrade);
                    addUpgrade.Content = "Add Upgrade";
                    Canvas.SetLeft(addUpgrade, left);
                    Canvas.SetTop(addUpgrade, height + Opt.ApResMod(pilotCardHeight) + 10);
                    pilotCanvas.Children.Add(addUpgrade);

                    BuildPilotUpgrade swapPilot;
                    swapPilot = new BuildPilotUpgrade();
                    swapPilot.uniquePilotId = pilot.Value.uniquePilotId;
                    swapPilot.uniqueBuildId = build.uniqueBuildId;
                    swapPilot.FontSize = 16;
                    swapPilot.FontWeight = FontWeights.Bold;
                    swapPilot.Click += new RoutedEventHandler(SwapPilot);
                    swapPilot.Content = "Swap Pilot";
                    Canvas.SetLeft(swapPilot, left + 150);
                    Canvas.SetTop(swapPilot, height + Opt.ApResMod(pilotCardHeight) + 10);
                    pilotCanvas.Children.Add(swapPilot);

                    Label pilotTotalCostLabel;
                    pilotTotalCostLabel = new Label();
                    pilotTotalCostLabel.Content = pilot.Value.totalCost;
                    pilotTotalCostLabel.FontSize = 30;
                    pilotTotalCostLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    Canvas.SetLeft(pilotTotalCostLabel, left + 240);
                    Canvas.SetTop(pilotTotalCostLabel, height + Opt.ApResMod(pilotCardHeight));
                    pilotCanvas.Children.Add(pilotTotalCostLabel);

                    currentLeftOffset += pilotCard.Width;

                    int currentUpgradeNumber = 0;
                    List<Upgrade> sortedUpgrades = pilot.Value.upgrades;
                    sortedUpgrades = sortedUpgrades.OrderBy(upgrade => upgrade.upgradeType).ThenByDescending(upgrade => upgrade.cost).ToList();
                    foreach (Upgrade upgrade in sortedUpgrades)
                    {
                        if (currentUpgradeNumber == 0) { currentLeftOffset += Opt.ApResMod(upgradeCardMargin); }
                        left = currentLeftOffset;
                        if (currentUpgradeNumber % 2 == 0)
                        {
                            height = currentHeightOffset;
                        }
                        else
                        {
                            height += currentHeightOffset + Opt.ApResMod(upgradeCardHeight) + Opt.ApResMod(upgradeCardMargin);
                            currentLeftOffset += Opt.ApResMod(upgradeCardWidth) + Opt.ApResMod(upgradeCardMargin);
                        }

                        UpgradeCard upgradeCard = upgrade.GetUpgradeCard(Opt.ApResMod(upgradeCardWidth), Opt.ApResMod(upgradeCardHeight));
                        Canvas.SetLeft(upgradeCard, left);
                        Canvas.SetTop(upgradeCard, height);
                        pilotCanvas.Children.Add(upgradeCard);

                        deleteButton = new DeleteButton();
                        deleteButton.uniqueBuildId = build.uniqueBuildId;
                        deleteButton.uniquePilotId = pilot.Value.uniquePilotId;
                        deleteButton.upgradeId = upgrade.id;
                        deleteButton.MouseLeftButtonDown += new MouseButtonEventHandler(DeleteUpgradeClicked);
                        Canvas.SetLeft(deleteButton, left + (Opt.ApResMod(upgradeCardWidth) - deleteButton.Width));
                        Canvas.SetTop(deleteButton, height);
                        pilotCanvas.Children.Add(deleteButton);

                        currentUpgradeNumber++;
                    }

                    pilotCanvas.Height = (Opt.ApResMod(upgradeCardHeight) * 2) + Opt.ApResMod(upgradeCardMargin);
                    pilotCanvas.Width = Opt.ApResMod(pilotCardWidth) + Math.Ceiling((double)pilot.Value.upgrades.Count / 2) * Opt.ApResMod(upgradeCardMargin) + Math.Ceiling((double)pilot.Value.upgrades.Count / 2) * Opt.ApResMod(upgradeCardWidth);
                    pilotCanvas.Margin = new Thickness(10, 20, 10, 0);
                    buildWrapPanel.Children.Add(pilotCanvas);
                }
                contentWrapPanel.Children.Add(buildWrapPanel);
            }
        }

        private void SwapPilot(object sender, RoutedEventArgs e)
        {
            BuildPilotUpgrade SwapPilotButton = (BuildPilotUpgrade)sender;
            BrowseCardsPage browseCardsPage = (BrowseCardsPage)Pages.pages[PageName.BrowseCards];
            browseCardsPage.SwapPilot(Builds.GetBuild(SwapPilotButton.uniqueBuildId), Builds.GetBuild(SwapPilotButton.uniqueBuildId).GetPilot(SwapPilotButton.uniquePilotId));
            NavigationService.Navigate(browseCardsPage);
        }

        private void AddPilotClicked(object sender, RoutedEventArgs e)
        {
            BrowseCardsPage browseCardsPage = (BrowseCardsPage)Pages.pages[PageName.BrowseCards];
            browseCardsPage.AddPilot(Builds.GetBuild(3));
            NavigationService.Navigate(browseCardsPage);
        }

        private void AddUpgrade(object sender, RoutedEventArgs e)
        {
            BuildPilotUpgrade addUpgradeButton = (BuildPilotUpgrade)sender;
            BrowseCardsPage browseCardsPage = (BrowseCardsPage)Pages.pages[PageName.BrowseCards];
            browseCardsPage.AddUpgrade(addUpgradeButton.uniquePilotId, Builds.GetBuild(addUpgradeButton.uniqueBuildId));
            NavigationService.Navigate(browseCardsPage);
        }
    }
}
