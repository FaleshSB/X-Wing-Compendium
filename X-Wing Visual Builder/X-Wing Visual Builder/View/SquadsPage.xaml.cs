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
    public partial class SquadsPage : DefaultPage, IDeleteCard
    {
        private int upgradeCardWidth = 150;
        private int upgradeCardHeight = 231;
        private int pilotCardWidth = 292;
        private int pilotCardHeight = 410;
        private int upgradeCardMargin = 4;
        private Canvas pilotAndUpgradeInfoCanvas;
        private AlignableWrapPanel contentWrapPanel;
        private AlignableWrapPanel buildWrapPanel;
        private Dictionary<int, Dictionary<int, CardCanvas>> upgradeCanvasCache = new Dictionary<int, Dictionary<int, CardCanvas>>();
        private Dictionary<int, Dictionary<int, CardCanvas>> pilotCanvasCache = new Dictionary<int, Dictionary<int, CardCanvas>>();


        public SquadsPage()
        {
            Pages.pages[PageName.Squads] = this;
            InitializeComponent();

            contentWrapPanel = new AlignableWrapPanel();
            contentWrapPanel.Name = "contentWrapPanel";
            contentWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentScrollViewer.Content = contentWrapPanel;

            Button addImperialBuildButton = new Button();
            addImperialBuildButton.Name = "addRebelBuildButton";
            addImperialBuildButton.Content = "Add Imperial Build";
            addImperialBuildButton.Width = 130;
            addImperialBuildButton.Height = 40;
            addImperialBuildButton.FontSize = 16;
            addImperialBuildButton.FontWeight = FontWeights.Bold;
            addImperialBuildButton.Click += new RoutedEventHandler(AddImperialBuildClicked);
            addImperialBuildButton.UseLayoutRounding = true;
            Canvas.SetLeft(addImperialBuildButton, 300);
            Canvas.SetTop(addImperialBuildButton, 10);
            topToolsCanvas.Children.Add(addImperialBuildButton);

            Button addBuildButton = new Button();
            addBuildButton.Name = "addRebelBuildButton";
            addBuildButton.Content = "Add Rebel Build";
            addBuildButton.Width = 130;
            addBuildButton.Height = 40;
            addBuildButton.FontSize = 16;
            addBuildButton.FontWeight = FontWeights.Bold;
            addBuildButton.Click += new RoutedEventHandler(AddRebelBuildClicked);
            addBuildButton.UseLayoutRounding = true;
            Canvas.SetLeft(addBuildButton, 400);
            Canvas.SetTop(addBuildButton, 10);
            topToolsCanvas.Children.Add(addBuildButton);
            
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

        public void buildChanged(int uniqueBuildId)
        {
            upgradeCanvasCache.Remove(uniqueBuildId);
        }

        private void browseCardsClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate((BrowseCardsPage)Pages.pages[PageName.BrowseCards]);
        }

        private void AddRebelBuildClicked(object sender, RoutedEventArgs e)
        {
            Builds.AddBuild(Faction.Rebel);
            DisplayContent();
        }
        private void AddImperialBuildClicked(object sender, RoutedEventArgs e)
        {
            Builds.AddBuild(Faction.Imperial);
            DisplayContent();
        }

        private void contentCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DisplayContent();
        }

        public void DeleteUpgradeClicked(int uniqueBuildId, int uniquePilotId, int upgradeId)
        {
            Builds.GetBuild(uniqueBuildId).RemoveUpgrade(uniquePilotId, upgradeId);            
            DisplayContent();
        }
        public void DeletePilotClicked(int uniqueBuildId, int uniquePilotId)
        {
            Builds.GetBuild(uniqueBuildId).RemovePilot(uniquePilotId);
            DisplayContent();
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
            buildChanged(addUpgradeButton.uniqueBuildId);
            DisplayContent();
        }

        private void AddUpgrade(object sender, RoutedEventArgs e)
        {
            BuildPilotUpgrade addUpgradeButton = (BuildPilotUpgrade)sender;
            BrowseCardsPage browseCardsPage = (BrowseCardsPage)Pages.pages[PageName.BrowseCards];
            browseCardsPage.AddUpgrade(addUpgradeButton.uniquePilotId, Builds.GetBuild(addUpgradeButton.uniqueBuildId));
            NavigationService.Navigate(browseCardsPage);
        }

        protected override void DisplayContent()
        {
            foreach (object child in contentWrapPanel.Children)
            {
                AlignableWrapPanel buildWrapPanelToClear = (AlignableWrapPanel)child;
                foreach (object childChild in buildWrapPanelToClear.Children)
                {
                    Canvas pilotCanvasToClear = (Canvas)childChild;
                    pilotCanvasToClear.Children.Clear();
                }
                buildWrapPanelToClear.Children.Clear();
            }
            contentWrapPanel.Children.Clear();

            foreach (Build build in Builds.builds.OrderByDescending(build => build.uniqueBuildId).ToList())
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

                List<UniquePilot> pilots = build.pilots.Values.OrderByDescending(uniquePilot => uniquePilot.pilot.pilotSkill).ThenByDescending(uniquePilot => uniquePilot.pilot.cost).ToList();

                foreach (UniquePilot uniquePilot in pilots)
                {
                    double left = 0;
                    double height = 0;
                    double currentLeftOffset = 0;
                    double currentHeightOffset = 0;
                    pilotAndUpgradeInfoCanvas = new Canvas();

                    if (pilotCanvasCache.ContainsKey(build.uniqueBuildId) == false || pilotCanvasCache[build.uniqueBuildId].ContainsKey(uniquePilot.id) == false)
                    {
                        if (pilotCanvasCache.ContainsKey(build.uniqueBuildId) == false)
                        {
                            pilotCanvasCache[build.uniqueBuildId] = new Dictionary<int, CardCanvas>();
                        }
                        pilotCanvasCache[build.uniqueBuildId][uniquePilot.id] = uniquePilot.pilot.GetCanvas(pilotCardWidth, pilotCardHeight, new Thickness(2, 2, 2, 2), this);
                        pilotCanvasCache[build.uniqueBuildId][uniquePilot.id].AddDeleteButtonEvent(this, build.uniqueBuildId, uniquePilot.id);
                    }
                    Canvas.SetLeft(pilotCanvasCache[build.uniqueBuildId][uniquePilot.id], left);
                    Canvas.SetTop(pilotCanvasCache[build.uniqueBuildId][uniquePilot.id], height);
                    pilotAndUpgradeInfoCanvas.Children.Add(pilotCanvasCache[build.uniqueBuildId][uniquePilot.id]);

                    BuildPilotUpgrade addUpgrade;
                    addUpgrade = new BuildPilotUpgrade();
                    addUpgrade.uniquePilotId = uniquePilot.id;
                    addUpgrade.uniqueBuildId = build.uniqueBuildId;
                    addUpgrade.FontSize = 16;
                    addUpgrade.FontWeight = FontWeights.Bold;
                    addUpgrade.Click += new RoutedEventHandler(AddUpgrade);
                    addUpgrade.Content = "Add Upgrade";
                    Canvas.SetLeft(addUpgrade, left);
                    Canvas.SetTop(addUpgrade, height + Opt.ApResMod(pilotCardHeight) + 10);
                    pilotAndUpgradeInfoCanvas.Children.Add(addUpgrade);

                    BuildPilotUpgrade swapPilot;
                    swapPilot = new BuildPilotUpgrade();
                    swapPilot.uniquePilotId = uniquePilot.id;
                    swapPilot.uniqueBuildId = build.uniqueBuildId;
                    swapPilot.FontSize = 16;
                    swapPilot.FontWeight = FontWeights.Bold;
                    swapPilot.Click += new RoutedEventHandler(SwapPilot);
                    swapPilot.Content = "Swap Pilot";
                    Canvas.SetLeft(swapPilot, left + 150);
                    Canvas.SetTop(swapPilot, height + Opt.ApResMod(pilotCardHeight) + 10);
                    pilotAndUpgradeInfoCanvas.Children.Add(swapPilot);

                    Label pilotTotalCostLabel;
                    pilotTotalCostLabel = new Label();
                    pilotTotalCostLabel.Content = uniquePilot.totalCost;
                    pilotTotalCostLabel.FontSize = 30;
                    pilotTotalCostLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    Canvas.SetLeft(pilotTotalCostLabel, left + 240);
                    Canvas.SetTop(pilotTotalCostLabel, height + Opt.ApResMod(pilotCardHeight));
                    pilotAndUpgradeInfoCanvas.Children.Add(pilotTotalCostLabel);

                    currentLeftOffset += pilotCardWidth;

                    int currentUpgradeNumber = 0;
                    foreach (KeyValuePair<int, Upgrade> upgrade in uniquePilot.upgrades.OrderBy(upgrade => upgrade.Value.upgradeType.ToString()).ThenByDescending(upgrade => upgrade.Value.cost).ThenByDescending(upgrade => upgrade.Value.name))
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

                        if (upgradeCanvasCache.ContainsKey(build.uniqueBuildId) == false || upgradeCanvasCache[build.uniqueBuildId].ContainsKey(upgrade.Key) == false)
                        {
                            if (upgradeCanvasCache.ContainsKey(build.uniqueBuildId) == false)
                            {
                                upgradeCanvasCache[build.uniqueBuildId] = new Dictionary<int, CardCanvas>();
                            }
                            upgradeCanvasCache[build.uniqueBuildId][upgrade.Key] = upgrade.Value.GetCanvas(upgradeCardWidth, upgradeCardHeight, new Thickness(2, 2, 2, 2), this);
                            upgradeCanvasCache[build.uniqueBuildId][upgrade.Key].AddDeleteButtonEvent(this, build.uniqueBuildId, uniquePilot.id);
                        }

                        Canvas.SetLeft(upgradeCanvasCache[build.uniqueBuildId][upgrade.Key], left);
                        Canvas.SetTop(upgradeCanvasCache[build.uniqueBuildId][upgrade.Key], height);
                        pilotAndUpgradeInfoCanvas.Children.Add(upgradeCanvasCache[build.uniqueBuildId][upgrade.Key]);

                        currentUpgradeNumber++;
                    }

                    pilotAndUpgradeInfoCanvas.Height = (Opt.ApResMod(upgradeCardHeight) * 2) + Opt.ApResMod(upgradeCardMargin);
                    pilotAndUpgradeInfoCanvas.Width = Opt.ApResMod(pilotCardWidth) + Math.Ceiling((double)uniquePilot.upgrades.Count / 2) * Opt.ApResMod(upgradeCardMargin) + Math.Ceiling((double)uniquePilot.upgrades.Count / 2) * Opt.ApResMod(upgradeCardWidth);
                    pilotAndUpgradeInfoCanvas.Margin = new Thickness(10, 20, 10, 0);
                    buildWrapPanel.Children.Add(pilotAndUpgradeInfoCanvas);
                }
                contentWrapPanel.Children.Add(buildWrapPanel);
            }
        }
    }
}
