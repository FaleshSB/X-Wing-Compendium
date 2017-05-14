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




            ImageButton addImperialBuild = new ImageButton("add_imperial_squad", 0.5);
            addImperialBuild.MouseDown += new MouseButtonEventHandler(AddImperialBuildClicked);
            addImperialBuild.Margin = ScaledThicknessFactory.GetThickness(2, 5, 2, 2);
            topToolsWrapPanel.Children.Add(addImperialBuild);

            ImageButton addRebelBuild = new ImageButton("add_rebel_squad", 0.5);
            addRebelBuild.MouseDown += new MouseButtonEventHandler(AddRebelBuildClicked);
            addRebelBuild.Margin = ScaledThicknessFactory.GetThickness(2, 5, 2, 2);
            topToolsWrapPanel.Children.Add(addRebelBuild);

            ImageButton addScumBuild = new ImageButton("add_scum_squad", 0.5);
            addScumBuild.MouseDown += new MouseButtonEventHandler(AddScumBuildClicked);
            addScumBuild.Margin = ScaledThicknessFactory.GetThickness(2, 5, 2, 2);
            topToolsWrapPanel.Children.Add(addScumBuild);

            ImageButton browseCards = new ImageButton("browse_cards", 0.5);
            browseCards.MouseDown += new MouseButtonEventHandler(BrowseCardsClicked);
            browseCards.Margin = ScaledThicknessFactory.GetThickness(60, 5, 2, 2);
            topToolsWrapPanel.Children.Add(browseCards);

            ImageButton quiz = new ImageButton("quiz", 0.5);
            quiz.MouseDown += new MouseButtonEventHandler(QuizClicked);
            quiz.Margin = ScaledThicknessFactory.GetThickness(2, 5, 2, 2);
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

        private async void QuizClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(Opt.buttonDelay);
            isButtonBeingPressed = false;
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

        private async void BrowseCardsClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(Opt.buttonDelay);
            isButtonBeingPressed = false;
            NavigationService.Navigate((BrowseCardsPage)Pages.pages[PageName.BrowseCards]);
        }

        private async void AddScumBuildClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(Opt.buttonDelay);
            isButtonBeingPressed = false;

            contentScrollViewer.ScrollToTop();

            Builds.AddBuild(Faction.Scum);
            DisplayContent();
        }
        private async void AddRebelBuildClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(Opt.buttonDelay);
            isButtonBeingPressed = false;

            contentScrollViewer.ScrollToTop();

            Builds.AddBuild(Faction.Rebel);
            DisplayContent();
        }
        private async void AddImperialBuildClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(Opt.buttonDelay);
            isButtonBeingPressed = false;

            contentScrollViewer.ScrollToTop();

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
            await Task.Delay(Opt.buttonDelay);
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
            await Task.Delay(Opt.buttonDelay);
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
            await Task.Delay(Opt.buttonDelay);
            isButtonBeingPressed = false;
            ImageButton imageButton = (ImageButton)sender;
            Builds.DeleteBuild(imageButton.uniqueBuildId);
            DisplayContent();
        }

        private async void AddUpgradeClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(Opt.buttonDelay);
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
            await Task.Delay(Opt.buttonDelay);
            isButtonBeingPressed = false;
            ImageButton imageButton = (ImageButton)sender;
            Builds.MoveBuildUp(imageButton.uniqueBuildId);
            DisplayContent();
        }

        private async void DownClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(Opt.buttonDelay);
            isButtonBeingPressed = false;
            ImageButton imageButton = (ImageButton)sender;
            Builds.MoveBuildDown(imageButton.uniqueBuildId);
            DisplayContent();
        }

        private async void CopyForWebClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(Opt.buttonDelay);
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

                OutlinedTextBlock totalCost = new OutlinedTextBlock();
                totalCost.Text = "TOTAL COST";
                totalCost.StrokeThickness = Opt.ApResMod(0.5);
                totalCost.Stroke = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                totalCost.FontWeight = FontWeights.Bold;
                totalCost.Fill = new SolidColorBrush(Color.FromRgb(80, 80, 80));
                totalCost.FontSize = Opt.ApResMod(15);
                totalCost.FontFamily = new FontFamily("Segoe UI");
                totalCost.Margin = ScaledThicknessFactory.GetThickness(2, 3, 0, 0);
                spacerWrapPanel.Children.Add(totalCost);

                OutlinedTextBlock totalCost2 = new OutlinedTextBlock();
                totalCost2.Text = ": " + build.totalCost.ToString();
                totalCost2.StrokeThickness = Opt.ApResMod(0.5);
                totalCost2.Stroke = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                totalCost2.FontWeight = FontWeights.Bold;
                totalCost2.Fill = new SolidColorBrush(Color.FromRgb(80, 80, 80));
                totalCost2.FontSize = Opt.ApResMod(18);
                totalCost2.FontFamily = new FontFamily("Segoe UI");
                totalCost2.Margin = ScaledThicknessFactory.GetThickness(2, 0, 0, 0);
                spacerWrapPanel.Children.Add(totalCost2);

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

                    OutlinedTextBlock pilotAndUpgradesCost = new OutlinedTextBlock();
                    pilotAndUpgradesCost.Text = "COST";
                    pilotAndUpgradesCost.StrokeThickness = Opt.ApResMod(0.5);
                    pilotAndUpgradesCost.Stroke = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                    pilotAndUpgradesCost.FontWeight = FontWeights.Bold;
                    pilotAndUpgradesCost.Fill = new SolidColorBrush(Color.FromRgb(80, 80, 80));
                    pilotAndUpgradesCost.FontSize = Opt.ApResMod(15);
                    pilotAndUpgradesCost.FontFamily = new FontFamily("Segoe UI");
                    pilotAndUpgradesCost.Margin = ScaledThicknessFactory.GetThickness(2, 13, 0, 0);
                    controls.Children.Add(pilotAndUpgradesCost);

                    OutlinedTextBlock pilotAndUpgradesCost2 = new OutlinedTextBlock();
                    pilotAndUpgradesCost2.Text = ": " + uniquePilot.totalCost.ToString();
                    pilotAndUpgradesCost2.StrokeThickness = Opt.ApResMod(0.5);
                    pilotAndUpgradesCost2.Stroke = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                    pilotAndUpgradesCost2.FontWeight = FontWeights.Bold;
                    pilotAndUpgradesCost2.Fill = new SolidColorBrush(Color.FromRgb(80, 80, 80));
                    pilotAndUpgradesCost2.FontSize = Opt.ApResMod(18);
                    pilotAndUpgradesCost2.FontFamily = new FontFamily("Segoe UI");
                    pilotAndUpgradesCost2.Margin = ScaledThicknessFactory.GetThickness(2, 10, 0, 0);
                    controls.Children.Add(pilotAndUpgradesCost2);

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
