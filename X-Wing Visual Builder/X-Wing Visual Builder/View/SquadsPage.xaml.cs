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
            addImperialBuildButton.FontSize = 16;
            addImperialBuildButton.FontWeight = FontWeights.Bold;
            addImperialBuildButton.Click += new RoutedEventHandler(AddImperialBuildClicked);
            addImperialBuildButton.Margin = new Thickness(4);
            addImperialBuildButton.Padding = new Thickness(5, 2, 5, 2);
            addImperialBuildButton.UseLayoutRounding = true;
            topToolsWrapPanel.Children.Add(addImperialBuildButton);

            Button addRebelBuildButton = new Button();
            addRebelBuildButton.Name = "addRebelBuildButton";
            addRebelBuildButton.Content = "Add Rebel Build";
            addRebelBuildButton.FontSize = 16;
            addRebelBuildButton.FontWeight = FontWeights.Bold;
            addRebelBuildButton.Click += new RoutedEventHandler(AddRebelBuildClicked);
            addRebelBuildButton.Margin = new Thickness(4);
            addRebelBuildButton.Padding = new Thickness(5, 2, 5, 2);
            addRebelBuildButton.UseLayoutRounding = true;
            topToolsWrapPanel.Children.Add(addRebelBuildButton);

            Button addScummBuildButton = new Button();
            addScummBuildButton.Name = "addRebelBuildButton";
            addScummBuildButton.Content = "Add Rebel Build";
            addScummBuildButton.FontSize = 16;
            addScummBuildButton.FontWeight = FontWeights.Bold;
            addScummBuildButton.Click += new RoutedEventHandler(AddScumBuildClicked);
            addScummBuildButton.Margin = new Thickness(4);
            addScummBuildButton.Padding = new Thickness(5, 2, 5, 2);
            addScummBuildButton.UseLayoutRounding = true;
            topToolsWrapPanel.Children.Add(addScummBuildButton);

            Button browseCards = new Button();
            browseCards.Name = "browseCards";
            browseCards.Content = "Browse Cards";
            browseCards.FontSize = 16;
            browseCards.FontWeight = FontWeights.Bold;
            browseCards.Click += new RoutedEventHandler(browseCardsClicked);
            browseCards.Margin = new Thickness(4);
            browseCards.Padding = new Thickness(5, 2, 5, 2);
            browseCards.UseLayoutRounding = true;
            topToolsWrapPanel.Children.Add(browseCards);
        }

        private void browseCardsClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate((BrowseCardsPage)Pages.pages[PageName.BrowseCards]);
        }

        private void AddScumBuildClicked(object sender, RoutedEventArgs e)
        {
            Builds.AddBuild(Faction.Scum);
            DisplayContent();
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
            contentWrapPanel.Children.Clear();

            foreach (Build build in Builds.builds.OrderByDescending(build => build.uniqueBuildId).ToList())
            {
                buildWrapPanel = new AlignableWrapPanel();
                buildWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;

                AlignableWrapPanel spacerCanvas = new AlignableWrapPanel();
                spacerCanvas.HorizontalContentAlignment = HorizontalAlignment.Center;

                Canvas topSpacer = new Canvas();
                topSpacer.Width = 9999;
                topSpacer.Height = 50;
                spacerCanvas.Children.Add(topSpacer);

                BuildPilotUpgrade addPilot;
                addPilot = new BuildPilotUpgrade();
                addPilot.uniqueBuildId = build.uniqueBuildId;
                addPilot.FontSize = 16;
                addPilot.FontWeight = FontWeights.Bold;
                addPilot.Click += new RoutedEventHandler(AddPilotClicked);
                addPilot.Content = "Add Pilot";
                addPilot.Margin = new Thickness(0);
                addPilot.Padding = new Thickness(4, 1, 4, 1);
                spacerCanvas.Children.Add(addPilot);

                Label totalCostLabel;
                totalCostLabel = new Label();
                totalCostLabel.Content = build.totalCost;
                totalCostLabel.Height = 25;
                totalCostLabel.Width = 50;
                totalCostLabel.FontSize = 20;
                totalCostLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                totalCostLabel.VerticalContentAlignment = VerticalAlignment.Center;
                totalCostLabel.Margin = new Thickness(0);
                totalCostLabel.Padding = new Thickness(0);
                totalCostLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                spacerCanvas.Children.Add(totalCostLabel);

                BuildPilotUpgrade deleteBuild;
                deleteBuild = new BuildPilotUpgrade();
                deleteBuild.uniqueBuildId = build.uniqueBuildId;
                deleteBuild.FontSize = 16;
                deleteBuild.FontWeight = FontWeights.Bold;
                deleteBuild.Click += new RoutedEventHandler(DeleteBuildClicked);
                deleteBuild.Content = "Delete Build";
                deleteBuild.Margin = new Thickness(0);
                deleteBuild.Padding = new Thickness(4, 1, 4, 1);
                spacerCanvas.Children.Add(deleteBuild);

                Canvas bottomSpacer = new Canvas();
                bottomSpacer.Width = 9999;
                bottomSpacer.Height = 1;
                spacerCanvas.Children.Add(bottomSpacer);

                buildWrapPanel.Children.Add(spacerCanvas);


                List<UniquePilot> pilots = build.pilots.Values.OrderByDescending(uniquePilot => uniquePilot.pilot.pilotSkill).ThenByDescending(uniquePilot => uniquePilot.pilot.cost).ToList();

                foreach (UniquePilot uniquePilot in pilots)
                {
                    double left = 0;
                    double height = 0;
                    double currentLeftOffset = 0;
                    double currentHeightOffset = 0;
                    pilotAndUpgradeInfoCanvas = new Canvas();
                    
                    CardCanvas pilotCanvas = uniquePilot.pilot.GetCanvas(pilotCardWidth, pilotCardHeight, new Thickness(2, 2, 2, 2), this);
                    pilotCanvas.AddDeleteButtonEvent(this, build.uniqueBuildId, uniquePilot.id);
                    Canvas.SetLeft(pilotCanvas, left);
                    Canvas.SetTop(pilotCanvas, height);
                    pilotAndUpgradeInfoCanvas.Children.Add(pilotCanvas);

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
                        
                        CardCanvas upgradeCanvas = upgrade.Value.GetCanvas(upgradeCardWidth, upgradeCardHeight, new Thickness(2, 2, 2, 2), this);
                        upgradeCanvas.AddDeleteButtonEvent(this, build.uniqueBuildId, uniquePilot.id);

                        Canvas.SetLeft(upgradeCanvas, left);
                        Canvas.SetTop(upgradeCanvas, height);
                        pilotAndUpgradeInfoCanvas.Children.Add(upgradeCanvas);

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
