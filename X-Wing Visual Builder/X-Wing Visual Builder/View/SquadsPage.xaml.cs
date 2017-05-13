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
        private bool isButtonBeingPressed = false;


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

        private async void SwapPilotClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(100);
            isButtonBeingPressed = false;
            ImageButton imageButton = (ImageButton)sender;
            BrowseCardsPage browseCardsPage = (BrowseCardsPage)Pages.pages[PageName.BrowseCards];
            browseCardsPage.SwapPilot(Builds.GetBuild(imageButton.uniqueBuildId), Builds.GetBuild(imageButton.uniqueBuildId).GetPilot(imageButton.uniquePilotId));
            NavigationService.Navigate(browseCardsPage);
        }

        private async void AddPilotClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(100);
            isButtonBeingPressed = false;
            ImageButton imageButton = (ImageButton)sender;
            BrowseCardsPage browseCardsPage = (BrowseCardsPage)Pages.pages[PageName.BrowseCards];
            browseCardsPage.AddPilot(Builds.GetBuild(imageButton.uniqueBuildId));
            NavigationService.Navigate(browseCardsPage);
        }

        private async void DeleteBuildClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(100);
            isButtonBeingPressed = false;
            ImageButton imageButton = (ImageButton)sender;
            Builds.DeleteBuild(imageButton.uniqueBuildId);
            DisplayContent();
        }

        private async void AddUpgradeClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(100);
            isButtonBeingPressed = false;
            ImageButton imageButton = (ImageButton)sender;
            BrowseCardsPage browseCardsPage = (BrowseCardsPage)Pages.pages[PageName.BrowseCards];
            browseCardsPage.AddUpgrade(imageButton.uniquePilotId, Builds.GetBuild(imageButton.uniqueBuildId));
            NavigationService.Navigate(browseCardsPage);
        }
        
        private async void UpClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(100);
            isButtonBeingPressed = false;
            ImageButton imageButton = (ImageButton)sender;
            Builds.MoveBuildUp(imageButton.uniqueBuildId);
            DisplayContent();
        }

        private async void DownClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(100);
            isButtonBeingPressed = false;
            ImageButton imageButton = (ImageButton)sender;
            Builds.MoveBuildDown(imageButton.uniqueBuildId);
            DisplayContent();
        }

        private async void CopyForWebClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(100);
            isButtonBeingPressed = false;
            ImageButton imageButton = (ImageButton)sender;
            Build build = Builds.GetBuild(imageButton.uniqueBuildId);
            string buildText = "";
            foreach (UniquePilot uniquePilot in build.pilots.Values.OrderByDescending(uniquePilot => uniquePilot.pilot.pilotSkill).ThenByDescending(uniquePilot => uniquePilot.pilot.cost).ToList())
            {
                buildText += uniquePilot.pilot.name + ": " + uniquePilot.pilot.cost.ToString() + Environment.NewLine;
                foreach (Upgrade upgrade in uniquePilot.upgrades.Values.OrderBy(upgrade => upgrade.upgradeType.ToString()).ThenByDescending(upgrade => upgrade.cost).ThenByDescending(upgrade => upgrade.name))
                {
                    buildText += "\t" + upgrade.name + ": " + upgrade.cost.ToString() + Environment.NewLine;
                }
                buildText += "\tTotal: " + uniquePilot.totalCost.ToString();
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

                ImageButton addPilotButton = new ImageButton("add_pilot", 0.5);
                addPilotButton.uniqueBuildId = build.uniqueBuildId;
                addPilotButton.MouseDown += new MouseButtonEventHandler(AddPilotClicked);
                addPilotButton.Margin = ScaledThicknessFactory.GetThickness(2,0,2,0);
                spacerWrapPanel.Children.Add(addPilotButton);

                ImageButton upButton = new ImageButton("up", 0.5);
                upButton.uniqueBuildId = build.uniqueBuildId;
                upButton.MouseDown += new MouseButtonEventHandler(UpClicked);
                upButton.Margin = ScaledThicknessFactory.GetThickness(2, 0, 2, 0);
                spacerWrapPanel.Children.Add(upButton);
                
                ImageButton downButton = new ImageButton("down", 0.5);
                downButton.uniqueBuildId = build.uniqueBuildId;
                downButton.MouseDown += new MouseButtonEventHandler(DownClicked);
                downButton.Margin = ScaledThicknessFactory.GetThickness(2, 0, 2, 0);
                spacerWrapPanel.Children.Add(downButton);

                ImageButton copyForWebButton = new ImageButton("copy_for_web", 0.5);
                copyForWebButton.uniqueBuildId = build.uniqueBuildId;
                copyForWebButton.MouseDown += new MouseButtonEventHandler(CopyForWebClicked);
                copyForWebButton.Margin = ScaledThicknessFactory.GetThickness(2, 0, 2, 0);
                spacerWrapPanel.Children.Add(copyForWebButton);

                ImageButton deleteBuildButton = new ImageButton("delete_squad", 0.5);
                deleteBuildButton.uniqueBuildId = build.uniqueBuildId;
                deleteBuildButton.MouseDown += new MouseButtonEventHandler(DeleteBuildClicked);
                deleteBuildButton.Margin = ScaledThicknessFactory.GetThickness(2, 0, 2, 0);
                spacerWrapPanel.Children.Add(deleteBuildButton);

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

                    ImageButton addUpgradeButton = new ImageButton("add_upgrade", 0.5);
                    addUpgradeButton.uniquePilotId = uniquePilot.id;
                    addUpgradeButton.uniqueBuildId = build.uniqueBuildId;
                    addUpgradeButton.MouseDown += new MouseButtonEventHandler(AddUpgradeClicked);
                    addUpgradeButton.Margin = ScaledThicknessFactory.GetThickness(2, 10, 2, 0);
                    controls.Children.Add(addUpgradeButton);

                    ImageButton swapPilotButton = new ImageButton("swap_pilot", 0.5);
                    swapPilotButton.uniquePilotId = uniquePilot.id;
                    swapPilotButton.uniqueBuildId = build.uniqueBuildId;
                    swapPilotButton.MouseDown += new MouseButtonEventHandler(SwapPilotClicked);
                    swapPilotButton.Margin = ScaledThicknessFactory.GetThickness(2, 10, 2, 0);
                    controls.Children.Add(swapPilotButton);

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
