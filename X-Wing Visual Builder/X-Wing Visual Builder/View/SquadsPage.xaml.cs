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
        private bool isMouseOverPilot = false;
        private int hoveredUniquePilotId;
        private int hoveredUniqueBuildId;


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
            DisplayContent();
        }

        private void contentCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DisplayContent();
        }

        private void PilotClicked(object sender, RoutedEventArgs e)
        {
            PilotCard pilotCard = (PilotCard)sender;
            int i = pilotCard.uniquePilotId;
        }

        private void DeleteUpgradeClicked(object sender, RoutedEventArgs e)
        {
            DeleteButton deleteButton = (DeleteButton)sender;
            Builds.GetBuild(deleteButton.uniqueBuildId).RemoveUpgrade(deleteButton.uniquePilotId, deleteButton.upgradeId);            
            DisplayContent();
        }

        private void DeletePilotClicked(object sender, RoutedEventArgs e)
        {
            DeleteButton deleteButton = (DeleteButton)sender;
            Builds.GetBuild(deleteButton.uniqueBuildId).RemovePilot(deleteButton.uniquePilotId);
            DisplayContent();
        }

        protected override void DisplayContent()
        {
            contentWrapPanel.Children.Clear();
            foreach(Build build in Builds.builds.OrderByDescending(build => build.uniqueBuildId).ToList())
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

                BuildPilotUpgrade deleteBuild;
                deleteBuild = new BuildPilotUpgrade();
                deleteBuild.uniqueBuildId = build.uniqueBuildId;
                deleteBuild.FontSize = 16;
                deleteBuild.FontWeight = FontWeights.Bold;
                deleteBuild.Click += new RoutedEventHandler(DeleteBuildClicked);
                deleteBuild.Content = "Delete Build";
                Canvas.SetLeft(deleteBuild, 805);
                Canvas.SetTop(deleteBuild, 8);
                spacerCanvas.Children.Add(deleteBuild);

                BuildPilotUpgrade addPilot;
                addPilot = new BuildPilotUpgrade();
                addPilot.uniqueBuildId = build.uniqueBuildId;
                addPilot.FontSize = 16;
                addPilot.FontWeight = FontWeights.Bold;
                addPilot.Click += new RoutedEventHandler(AddPilotClicked);
                addPilot.Content = "Add Pilot";
                Canvas.SetLeft(addPilot, 945);
                Canvas.SetTop(addPilot, 8);
                spacerCanvas.Children.Add(addPilot);

                buildWrapPanel.Children.Add(spacerCanvas);
                
                List<Pilot> pilots = build.pilots.Values.OrderByDescending(pilot => pilot.pilotSkill).ThenByDescending(pilot => pilot.cost).ToList();

                foreach (Pilot pilot in pilots)
                {
                    double left = 0;
                    double height = 0;
                    double currentLeftOffset = 0;
                    double currentHeightOffset = 0;
                    pilotCanvas = new Canvas();

                    PilotCard pilotCard = pilot.GetPilotCard(Opt.ApResMod(pilotCardWidth), Opt.ApResMod(pilotCardHeight), build.uniqueBuildId);
                    pilotCard.MouseLeftButtonDown += new MouseButtonEventHandler(PilotClicked);
                    pilotCard.MouseEnter += new MouseEventHandler(PilotMouseHover);
                    pilotCard.MouseLeave += new MouseEventHandler(PilotMouseHoverLeave);
                    pilotCard.MouseWheel += new MouseWheelEventHandler(ContentScroll);
                    Canvas.SetLeft(pilotCard, left);
                    Canvas.SetTop(pilotCard, height);
                    pilotCanvas.Children.Add(pilotCard);

                    if (isMouseOverPilot && hoveredUniquePilotId == pilot.uniquePilotId && hoveredUniqueBuildId == build.uniqueBuildId)
                    {
                        /*
                        Rectangle maneuverCardBackground = new Rectangle();
                        maneuverCardBackground.Height = 100;
                        maneuverCardBackground.Width = 100;
                        maneuverCardBackground.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#aaaaaa"));
                        Canvas.SetLeft(maneuverCardBackground, left + ((Opt.ApResMod(pilotCardWidth) / 2) - (maneuverCardBackground.Width / 2)));
                        Canvas.SetTop(maneuverCardBackground, height);
                        pilotCanvas.Children.Add(maneuverCardBackground);
                        */

                        ManeuverCard maneuverCard = pilot.ship.GetManeuverCard(Math.Round(Opt.ApResMod(pilotCardWidth) / 11));
                        maneuverCard.uniqueBuildId = build.uniqueBuildId;
                        maneuverCard.uniquePilotId = pilot.uniquePilotId;
                        maneuverCard.MouseEnter += new MouseEventHandler(ManeuverMouseHover);
                        maneuverCard.MouseLeave += new MouseEventHandler(ManeuverMouseHoverLeave);
                        maneuverCard.MouseWheel += new MouseWheelEventHandler(ContentScroll);
                        Canvas.SetLeft(maneuverCard, left + ((Opt.ApResMod(pilotCardWidth) / 2) - (maneuverCard.Width / 2)));
                        Canvas.SetTop(maneuverCard, height);
                        pilotCanvas.Children.Add(maneuverCard);
                    }

                    DeleteButton deleteButton;
                    deleteButton = new DeleteButton();
                    deleteButton.uniqueBuildId = build.uniqueBuildId;
                    deleteButton.uniquePilotId = pilot.uniquePilotId;
                    deleteButton.MouseLeftButtonDown += new MouseButtonEventHandler(DeletePilotClicked);
                    Canvas.SetLeft(deleteButton, left + (Opt.ApResMod(pilotCardWidth) - deleteButton.Width));
                    Canvas.SetTop(deleteButton, height);
                    pilotCanvas.Children.Add(deleteButton);

                    BuildPilotUpgrade addUpgrade;
                    addUpgrade = new BuildPilotUpgrade();
                    addUpgrade.uniquePilotId = pilot.uniquePilotId;
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
                    swapPilot.uniquePilotId = pilot.uniquePilotId;
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
                    pilotTotalCostLabel.Content = pilot.totalCost;
                    pilotTotalCostLabel.FontSize = 30;
                    pilotTotalCostLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    Canvas.SetLeft(pilotTotalCostLabel, left + 240);
                    Canvas.SetTop(pilotTotalCostLabel, height + Opt.ApResMod(pilotCardHeight));
                    pilotCanvas.Children.Add(pilotTotalCostLabel);

                    currentLeftOffset += pilotCard.Width;

                    int currentUpgradeNumber = 0;
                    List<Upgrade> sortedUpgrades = pilot.upgrades;
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
                        deleteButton.uniquePilotId = pilot.uniquePilotId;
                        deleteButton.upgradeId = upgrade.id;
                        deleteButton.MouseLeftButtonDown += new MouseButtonEventHandler(DeleteUpgradeClicked);
                        Canvas.SetLeft(deleteButton, left + (Opt.ApResMod(upgradeCardWidth) - deleteButton.Width));
                        Canvas.SetTop(deleteButton, height);
                        pilotCanvas.Children.Add(deleteButton);

                        currentUpgradeNumber++;
                    }

                    pilotCanvas.Height = (Opt.ApResMod(upgradeCardHeight) * 2) + Opt.ApResMod(upgradeCardMargin);
                    pilotCanvas.Width = Opt.ApResMod(pilotCardWidth) + Math.Ceiling((double)pilot.upgrades.Count / 2) * Opt.ApResMod(upgradeCardMargin) + Math.Ceiling((double)pilot.upgrades.Count / 2) * Opt.ApResMod(upgradeCardWidth);
                    pilotCanvas.Margin = new Thickness(10, 20, 10, 0);
                    buildWrapPanel.Children.Add(pilotCanvas);
                }
                contentWrapPanel.Children.Add(buildWrapPanel);
            }
        }

        private void PilotMouseHover(object sender, MouseEventArgs e)
        {
            PilotCard hoveredPilot = (PilotCard)sender;
            hoveredUniquePilotId = hoveredPilot.uniquePilotId;
            hoveredUniqueBuildId = hoveredPilot.uniqueBuildId;
            isMouseOverPilot = true;
            DisplayContent();
        }
        private void PilotMouseHoverLeave(object sender, MouseEventArgs e)
        {
            isMouseOverPilot = false;
            DisplayContent();
        }
        private void ManeuverMouseHover(object sender, MouseEventArgs e)
        {
            ManeuverCard hoveredManeuver = (ManeuverCard)sender;
            hoveredUniquePilotId = hoveredManeuver.uniquePilotId;
            hoveredUniqueBuildId = hoveredManeuver.uniqueBuildId;
            isMouseOverPilot = true;
            DisplayContent();
        }
        private void ManeuverMouseHoverLeave(object sender, MouseEventArgs e)
        {
            isMouseOverPilot = false;
            DisplayContent();
        }
        private void ContentScroll(object sender, MouseWheelEventArgs e)
        {
            contentScrollViewer.ScrollToVerticalOffset(contentScrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
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
            BuildPilotUpgrade addUpgradeButton = (BuildPilotUpgrade)sender;
            BrowseCardsPage browseCardsPage = (BrowseCardsPage)Pages.pages[PageName.BrowseCards];
            browseCardsPage.AddPilot(Builds.GetBuild(addUpgradeButton.uniqueBuildId));
            NavigationService.Navigate(browseCardsPage);
        }

        private void DeleteBuildClicked(object sender, RoutedEventArgs e)
        {
            BuildPilotUpgrade addUpgradeButton = (BuildPilotUpgrade)sender;
            Builds.DeleteBuild(addUpgradeButton.uniqueBuildId);
            DisplayContent();
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
