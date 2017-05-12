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
        private AlignableWrapPanel contentWrapPanel;


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
            addImperialBuildButton.FontSize = Opt.ApResMod(16);
            addImperialBuildButton.FontWeight = FontWeights.Bold;
            addImperialBuildButton.Click += new RoutedEventHandler(AddImperialBuildClicked);
            addImperialBuildButton.Margin = ScaledThicknessFactory.GetThickness(4);
            addImperialBuildButton.Padding = ScaledThicknessFactory.GetThickness(5, 2, 5, 2);
            addImperialBuildButton.UseLayoutRounding = true;
            topToolsWrapPanel.Children.Add(addImperialBuildButton);

            Button addRebelBuildButton = new Button();
            addRebelBuildButton.Name = "addRebelBuildButton";
            addRebelBuildButton.Content = "Add Rebel Build";
            addRebelBuildButton.FontSize = Opt.ApResMod(16);
            addRebelBuildButton.FontWeight = FontWeights.Bold;
            addRebelBuildButton.Click += new RoutedEventHandler(AddRebelBuildClicked);
            addRebelBuildButton.Margin = ScaledThicknessFactory.GetThickness(4);
            addRebelBuildButton.Padding = ScaledThicknessFactory.GetThickness(5, 2, 5, 2);
            addRebelBuildButton.UseLayoutRounding = true;
            topToolsWrapPanel.Children.Add(addRebelBuildButton);

            Button addScummBuildButton = new Button();
            addScummBuildButton.Name = "addRebelBuildButton";
            addScummBuildButton.Content = "Add Rebel Build";
            addScummBuildButton.FontSize = Opt.ApResMod(16);
            addScummBuildButton.FontWeight = FontWeights.Bold;
            addScummBuildButton.Click += new RoutedEventHandler(AddScumBuildClicked);
            addScummBuildButton.Margin = ScaledThicknessFactory.GetThickness(4);
            addScummBuildButton.Padding = ScaledThicknessFactory.GetThickness(5, 2, 5, 2);
            addScummBuildButton.UseLayoutRounding = true;
            topToolsWrapPanel.Children.Add(addScummBuildButton);

            Button browseCards = new Button();
            browseCards.Name = "browseCards";
            browseCards.Content = "Browse Cards";
            browseCards.FontSize = Opt.ApResMod(16);
            browseCards.FontWeight = FontWeights.Bold;
            browseCards.Click += new RoutedEventHandler(browseCardsClicked);
            browseCards.Margin = ScaledThicknessFactory.GetThickness(4);
            browseCards.Padding = ScaledThicknessFactory.GetThickness(5, 2, 5, 2);
            browseCards.UseLayoutRounding = true;
            topToolsWrapPanel.Children.Add(browseCards);

            Button quiz = new Button();
            quiz.Name = "quiz";
            quiz.Content = "Quiz";
            quiz.FontSize = Opt.ApResMod(16);
            quiz.FontWeight = FontWeights.Bold;
            quiz.Click += new RoutedEventHandler(quizClicked);
            quiz.UseLayoutRounding = true;
            quiz.VerticalAlignment = VerticalAlignment.Center;
            quiz.Padding = ScaledThicknessFactory.GetThickness(5, 2, 5, 2);
            topToolsWrapPanel.Children.Add(quiz);

            //manuNavigationWrapPanel.Width = 

            /*
            Slider zoom = new Slider();
            zoom.Value = 1;
            zoom.Width = 70;
            zoom.Minimum = 0.33;
            zoom.Maximum = 3;
            zoom.VerticalAlignment = VerticalAlignment.Center;
            zoom.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ZoomChanged);
            topToolsWrapPanel.Children.Add(zoom);
            */
        }

        private void quizClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate((QuizPage)Pages.pages[PageName.Quiz]);
        }

        private void ZoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Opt.zoom = e.NewValue;
            /*
            foreach(Upgrade upgrade in Upgrades.upgrades.Values)
            {
                foreach(CardCanvas cardCanvas in upgrade.cardCanvasList)
                {
                    cardCanvas.ConstructCanvas();
                }
            }
            foreach (Pilot pilot in Pilots.pilots.Values)
            {
                foreach (CardCanvas cardCanvas in pilot.cardCanvasList)
                {
                    cardCanvas.ConstructCanvas();
                }
            }*/
            DisplayContent();
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
        
        private void MoveBuildUpClicked(object sender, RoutedEventArgs e)
        {
            BuildPilotUpgrade addUpgradeButton = (BuildPilotUpgrade)sender;
            Builds.MoveBuildUp(addUpgradeButton.uniqueBuildId);
            DisplayContent();
        }

        private void MoveBuildDownClicked(object sender, RoutedEventArgs e)
        {
            BuildPilotUpgrade addUpgradeButton = (BuildPilotUpgrade)sender;
            Builds.MoveBuildDown(addUpgradeButton.uniqueBuildId);
            DisplayContent();
        }

        private void DuplicateBuildClicked(object sender, RoutedEventArgs e)
        {
            BuildPilotUpgrade addUpgradeButton = (BuildPilotUpgrade)sender;
            Builds.DuplicateBuild(addUpgradeButton.uniqueBuildId);
            DisplayContent();
        }

        private void CopyBuildAsTextClicked(object sender, RoutedEventArgs e)
        {
            BuildPilotUpgrade CopyBuildAsText = (BuildPilotUpgrade)sender;
            Build build = Builds.GetBuild(CopyBuildAsText.uniqueBuildId);
            string buildText = "";
            foreach (UniquePilot uniquePilot in build.pilots.Values.OrderByDescending(uniquePilot => uniquePilot.pilot.pilotSkill).ThenByDescending(uniquePilot => uniquePilot.pilot.cost).ToList())
            {
                buildText += uniquePilot.pilot.name + " (" + uniquePilot.pilot.cost.ToString() + ")" + Environment.NewLine;
                foreach (Upgrade upgrade in uniquePilot.upgrades.Values.OrderBy(upgrade => upgrade.upgradeType.ToString()).ThenByDescending(upgrade => upgrade.cost).ThenByDescending(upgrade => upgrade.name))
                {
                    buildText += "\t" + upgrade.name + " (" + upgrade.cost.ToString() + ")" + Environment.NewLine;
                }
                buildText += "\tTotal Cost: " + uniquePilot.totalCost.ToString();
                buildText += Environment.NewLine;
                buildText += Environment.NewLine;
            }
            buildText = buildText.Trim();
            Clipboard.SetText(buildText);
        }

        protected override void DisplayContent()
        {
            contentWrapPanel.Children.Clear();
            foreach (Upgrade upgrade in Upgrades.upgrades.Values)
            {
                upgrade.cardCanvasList.Clear();
            }
            foreach (Pilot pilot in Pilots.pilots.Values)
            {
                pilot.cardCanvasList.Clear();
            }

            foreach (Build build in Builds.builds.OrderByDescending(build => build.displayOrder).ToList())
            {
                AlignableWrapPanel buildWrapPanel = new AlignableWrapPanel();
                buildWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;

                AlignableWrapPanel spacerWrapPanel = new AlignableWrapPanel();
                spacerWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;

                Canvas topSpacer = new Canvas();
                topSpacer.Width = 99999;
                topSpacer.Height = 50;
                spacerWrapPanel.Children.Add(topSpacer);

                BuildPilotUpgrade addPilot;
                addPilot = new BuildPilotUpgrade();
                addPilot.uniqueBuildId = build.uniqueBuildId;
                addPilot.FontSize = Opt.ApResMod(16);
                addPilot.FontWeight = FontWeights.Bold;
                addPilot.Click += new RoutedEventHandler(AddPilotClicked);
                addPilot.Content = "Add Pilot";
                addPilot.Margin = ScaledThicknessFactory.GetThickness(0);
                addPilot.Padding = ScaledThicknessFactory.GetThickness(4, 1, 4, 1);
                spacerWrapPanel.Children.Add(addPilot);

                Label totalCostLabel;
                totalCostLabel = new Label();
                totalCostLabel.Content = build.totalCost;
                totalCostLabel.Height = 25;
                totalCostLabel.Width = 50;
                totalCostLabel.FontSize = Opt.ApResMod(20);
                totalCostLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                totalCostLabel.VerticalContentAlignment = VerticalAlignment.Center;
                totalCostLabel.Margin = ScaledThicknessFactory.GetThickness(0);
                totalCostLabel.Padding = ScaledThicknessFactory.GetThickness(0);
                totalCostLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                spacerWrapPanel.Children.Add(totalCostLabel);

                BuildPilotUpgrade deleteBuild;
                deleteBuild = new BuildPilotUpgrade();
                deleteBuild.uniqueBuildId = build.uniqueBuildId;
                deleteBuild.FontSize = Opt.ApResMod(16);
                deleteBuild.FontWeight = FontWeights.Bold;
                deleteBuild.Click += new RoutedEventHandler(DeleteBuildClicked);
                deleteBuild.Content = "Delete Build";
                deleteBuild.Margin = ScaledThicknessFactory.GetThickness(0);
                deleteBuild.Padding = ScaledThicknessFactory.GetThickness(4, 1, 4, 1);
                spacerWrapPanel.Children.Add(deleteBuild);
                /*
                BuildPilotUpgrade DuplicateBuild;
                DuplicateBuild = new BuildPilotUpgrade();
                DuplicateBuild.uniqueBuildId = build.uniqueBuildId;
                DuplicateBuild.FontSize = Opt.ApResMod(16);
                DuplicateBuild.FontWeight = FontWeights.Bold;
                DuplicateBuild.Click += new RoutedEventHandler(DuplicateBuildClicked);
                DuplicateBuild.Content = "Duplicate Build";
                DuplicateBuild.Margin = ScaledThicknessFactory.GetThickness(0);
                DuplicateBuild.Padding = ScaledThicknessFactory.GetThickness(4, 1, 4, 1);
                spacerCanvas.Children.Add(DuplicateBuild);
                */
                BuildPilotUpgrade MoveBuildUp;
                MoveBuildUp = new BuildPilotUpgrade();
                MoveBuildUp.uniqueBuildId = build.uniqueBuildId;
                MoveBuildUp.FontSize = Opt.ApResMod(16);
                MoveBuildUp.FontWeight = FontWeights.Bold;
                MoveBuildUp.Click += new RoutedEventHandler(MoveBuildUpClicked);
                MoveBuildUp.Content = "Up";
                MoveBuildUp.Margin = ScaledThicknessFactory.GetThickness(8, 0, 8, 0);
                MoveBuildUp.Padding = ScaledThicknessFactory.GetThickness(4, 1, 4, 1);
                spacerWrapPanel.Children.Add(MoveBuildUp);

                BuildPilotUpgrade MoveBuildDown;
                MoveBuildDown = new BuildPilotUpgrade();
                MoveBuildDown.uniqueBuildId = build.uniqueBuildId;
                MoveBuildDown.FontSize = Opt.ApResMod(16);
                MoveBuildDown.FontWeight = FontWeights.Bold;
                MoveBuildDown.Click += new RoutedEventHandler(MoveBuildDownClicked);
                MoveBuildDown.Content = "Down";
                MoveBuildDown.Margin = ScaledThicknessFactory.GetThickness(0);
                MoveBuildDown.Padding = ScaledThicknessFactory.GetThickness(4, 1, 4, 1);
                spacerWrapPanel.Children.Add(MoveBuildDown);

                BuildPilotUpgrade CopyBuildAsText;
                CopyBuildAsText = new BuildPilotUpgrade();
                CopyBuildAsText.uniqueBuildId = build.uniqueBuildId;
                CopyBuildAsText.FontSize = Opt.ApResMod(16);
                CopyBuildAsText.FontWeight = FontWeights.Bold;
                CopyBuildAsText.Click += new RoutedEventHandler(CopyBuildAsTextClicked);
                CopyBuildAsText.Content = "Copy Build As Text";
                CopyBuildAsText.Margin = ScaledThicknessFactory.GetThickness(8, 0, 0, 0);
                CopyBuildAsText.Padding = ScaledThicknessFactory.GetThickness(4, 1, 4, 1);
                spacerWrapPanel.Children.Add(CopyBuildAsText);

                Canvas bottomSpacer = new Canvas();
                bottomSpacer.Width = 9999;
                bottomSpacer.Height = 1;
                spacerWrapPanel.Children.Add(bottomSpacer);

                buildWrapPanel.Children.Add(spacerWrapPanel);


                foreach (UniquePilot uniquePilot in build.pilots.Values.OrderByDescending(uniquePilot => uniquePilot.pilot.pilotSkill).ThenByDescending(uniquePilot => uniquePilot.pilot.cost).ToList())
                {
                    StackPanel pilotAndUpgradeInfoStackPanel = new StackPanel();
                    pilotAndUpgradeInfoStackPanel.Orientation = Orientation.Horizontal;


                    StackPanel pilotAndControlls = new StackPanel();
                    pilotAndControlls.Orientation = Orientation.Vertical;
                    pilotAndControlls.VerticalAlignment = VerticalAlignment.Top;

                    CardCanvas pilotCanvas = uniquePilot.pilot.GetCanvas(pilotCardWidth, pilotCardHeight, new Thickness(2, 2, 2, 2), this);
                    pilotCanvas.AddDeleteButtonEvent(this, build.uniqueBuildId, uniquePilot.id);
                    pilotAndControlls.Children.Add(pilotCanvas);

                    StackPanel controls = new StackPanel();
                    controls.Orientation = Orientation.Horizontal;

                    BuildPilotUpgrade addUpgrade;
                    addUpgrade = new BuildPilotUpgrade();
                    addUpgrade.uniquePilotId = uniquePilot.id;
                    addUpgrade.uniqueBuildId = build.uniqueBuildId;
                    addUpgrade.FontSize = Opt.ApResMod(16);
                    addUpgrade.FontWeight = FontWeights.Bold;
                    addUpgrade.Click += new RoutedEventHandler(AddUpgrade);
                    addUpgrade.Margin = ScaledThicknessFactory.GetThickness(0);
                    addUpgrade.Padding = ScaledThicknessFactory.GetThickness(4, 1, 4, 1);
                    addUpgrade.Content = "Add Upgrade";
                    controls.Children.Add(addUpgrade);

                    BuildPilotUpgrade swapPilot;
                    swapPilot = new BuildPilotUpgrade();
                    swapPilot.uniquePilotId = uniquePilot.id;
                    swapPilot.uniqueBuildId = build.uniqueBuildId;
                    swapPilot.FontSize = Opt.ApResMod(16);
                    swapPilot.FontWeight = FontWeights.Bold;
                    swapPilot.Click += new RoutedEventHandler(SwapPilot);
                    swapPilot.Margin = ScaledThicknessFactory.GetThickness(8, 0, 8, 0);
                    swapPilot.Padding = ScaledThicknessFactory.GetThickness(4, 1, 4, 1);
                    swapPilot.Content = "Swap Pilot";
                    controls.Children.Add(swapPilot);

                    Label pilotTotalCostLabel;
                    pilotTotalCostLabel = new Label();
                    pilotTotalCostLabel.Content = uniquePilot.totalCost;
                    pilotTotalCostLabel.Margin = ScaledThicknessFactory.GetThickness(0);
                    pilotTotalCostLabel.Padding = ScaledThicknessFactory.GetThickness(0);
                    pilotTotalCostLabel.FontSize = Opt.ApResMod(20);
                    pilotTotalCostLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    pilotTotalCostLabel.VerticalAlignment = VerticalAlignment.Center;
                    controls.Children.Add(pilotTotalCostLabel);

                    pilotAndControlls.Children.Add(controls);
                    pilotAndUpgradeInfoStackPanel.Children.Add(pilotAndControlls);

                    int currentUpgradeNumber = 0;
                    StackPanel upgradesStackPanel = new StackPanel();
                    upgradesStackPanel.Orientation = Orientation.Vertical;
                    upgradesStackPanel.VerticalAlignment = VerticalAlignment.Top;
                    foreach (KeyValuePair<int, Upgrade> upgrade in uniquePilot.upgrades.OrderBy(upgrade => upgrade.Value.upgradeType.ToString()).ThenByDescending(upgrade => upgrade.Value.cost).ThenByDescending(upgrade => upgrade.Value.name))
                    {
                        CardCanvas upgradeCanvas = upgrade.Value.GetCanvas(upgradeCardWidth, upgradeCardHeight, new Thickness(2, 2, 2, 2), this);
                        upgradeCanvas.AddDeleteButtonEvent(this, build.uniqueBuildId, uniquePilot.id);
                        upgradesStackPanel.Children.Add(upgradeCanvas);

                        if (currentUpgradeNumber % 2 == 1)
                        {
                            pilotAndUpgradeInfoStackPanel.Children.Add(upgradesStackPanel);
                            upgradesStackPanel = new StackPanel();
                        }

                        currentUpgradeNumber++;
                    }
                    if (currentUpgradeNumber % 2 == 1)
                    {
                        pilotAndUpgradeInfoStackPanel.Children.Add(upgradesStackPanel);
                    }
                    pilotAndUpgradeInfoStackPanel.Margin = ScaledThicknessFactory.GetThickness(10, 20, 10, 0);
                    buildWrapPanel.Children.Add(pilotAndUpgradeInfoStackPanel);
                }
                contentWrapPanel.Children.Add(buildWrapPanel);
            }
        }
    }
}
