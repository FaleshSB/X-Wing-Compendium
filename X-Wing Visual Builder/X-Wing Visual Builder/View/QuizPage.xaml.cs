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
    /// Interaction logic for UpgradeCards.xaml
    /// </summary>
    public partial class QuizPage : DefaultPage
    {
        private int pilotCardWidth = 292;
        private int pilotCardHeight = 410;
        private int upgradeCardWidth = 166;
        private int upgradeCardHeight = 255;
        private Canvas contentCanvas = new Canvas();
        protected AlignableWrapPanel contentWrapPanel = new AlignableWrapPanel();

        public QuizPage()
        {
            contentCanvas.Name = "contentCanvas";
            contentWrapPanel.Children.Add(contentCanvas);
            InitializeComponent();
        }

        new private void DisplayContent()
        {
            contentWrapPanel.Name = "contentWrapPanel";
            contentWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentScrollViewer.Content = contentWrapPanel;

            contentCanvas.Children.Clear();
            List<List<Upgrade>> upgradesToDisplay = new List<List<Upgrade>>();
            //upgradesToDisplay.Add(Upgrades.GetUpgrades(UpgradeType.Torpedo, UpgradeSort.Cost));
            //upgradesToDisplay.Add(Upgrades.GetUpgrades(UpgradeType.Missile, UpgradeSort.Cost));
            //upgradesToDisplay.Add(Upgrades.GetUpgrades(UpgradeType.Elite, UpgradeSort.Cost, Faction.Rebel, ShipSize.Small));
            //upgradesToDisplay.Add(Upgrades.GetUpgrades(UpgradeType.Astromech, UpgradeSort.Cost, Faction.Rebel, ShipSize.Small));
            double currentHeightOffset = -30;
            double currentLeftOffset = 20;
            double spacersGap = 4;
            int currentTypeId = 0;
            int currentUpgradeId = 0;
            int currentRowNumber = 0;
            while (true)
            {
                if (currentTypeId >= upgradesToDisplay.Count)
                {
                    break;
                }
                else if (currentUpgradeId < upgradesToDisplay.ElementAt(currentTypeId).Count)
                {
                    UpgradeCard upgradeCard = upgradesToDisplay.ElementAt(currentTypeId).ElementAt(currentUpgradeId).GetUpgradeCard(Opt.ApResMod(upgradeCardWidth), Opt.ApResMod(upgradeCardHeight));
                    Canvas.SetLeft(upgradeCard, currentLeftOffset + spacersGap);
                    Canvas.SetTop(upgradeCard, currentHeightOffset + 40);
                    contentCanvas.Children.Add(upgradeCard);
                    currentUpgradeId++;
                    currentLeftOffset += spacersGap + Opt.ApResMod(upgradeCardWidth);
                    currentRowNumber++;
                }
                else
                {
                    currentTypeId++;
                    currentUpgradeId = 0;
                }

                if (currentRowNumber > 10)
                {
                    currentHeightOffset += spacersGap + Opt.ApResMod(upgradeCardHeight);
                    currentLeftOffset = 20;
                    currentRowNumber = 0;
                }
            }
            contentCanvas.Height = currentHeightOffset + spacersGap + Opt.ApResMod(upgradeCardHeight) + 80;
        }

        /*
        private Pilot currentRandomPilot;
        private bool isShowingName = false;
        private Canvas contentCanvas = new Canvas();
        protected AlignableWrapPanel contentWrapPanel = new AlignableWrapPanel();

        public PilotQuizPage()
        {
            contentWrapPanel.Name = "contentWrapPanel";
            contentWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentScrollViewer.Content = contentWrapPanel;

            Pages.pages[PageName.PilotQuiz] = this;

            contentCanvas.Name = "contentCanvas";
            contentWrapPanel.Children.Add(contentCanvas);

            InitializeComponent();

            currentRandomPilot = Pilots.GetRandomPilot();
        }

        private void ShowNameClicked(object sender, RoutedEventArgs e)
        {
            if (isShowingName == true)
            {
                currentRandomPilot = Pilots.GetRandomPilot();
            }

            isShowingName = !isShowingName;
            DisplayContent();
        }

        private void TempButton(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.Content.ToString() == "Browse Cards")
            {
                //NavigationService.GoBack();
                NavigationService.Navigate((BrowseCardsPage)Pages.pages[PageName.BrowseCards]);
            }
        }

        protected override void DisplayContent()
        {
            contentCanvas.Children.Clear();


            Button browseCards = new Button();
            browseCards.Name = "browseCards";
            browseCards.Width = 130;
            browseCards.Height = 40;
            browseCards.FontSize = 16;
            browseCards.FontWeight = FontWeights.Bold;
            browseCards.Click += new RoutedEventHandler(TempButton);
            browseCards.UseLayoutRounding = true;
            browseCards.Content = "Browse Cards";
            Canvas.SetLeft(browseCards, 850);
            Canvas.SetTop(browseCards, 180);
            contentCanvas.Children.Add(browseCards);




            PilotCard randomPilotCard = currentRandomPilot.GetPilotCard(pilotCardWidth, pilotCardHeight);
            Canvas.SetLeft(randomPilotCard, 760);
            Canvas.SetTop(randomPilotCard, 360);
            contentCanvas.Children.Add(randomPilotCard);

            Button showName = new Button();
            showName.Name = "ShowNameButton";
            showName.Width = 130;
            showName.Height = 40;
            showName.FontSize = 16;
            showName.FontWeight = FontWeights.Bold;
            showName.Click += new RoutedEventHandler(ShowNameClicked);
            showName.UseLayoutRounding = true;

            if (isShowingName == false)
            {
                showName.Content = "Show Name";

                Rectangle blueRectangle = new Rectangle();
                blueRectangle.Height = pilotCardHeight * 0.46;
                blueRectangle.Width = pilotCardWidth;
                blueRectangle.Fill = new SolidColorBrush(Color.FromRgb(50, 50, 50));
                blueRectangle.UseLayoutRounding = true;
                Canvas.SetLeft(blueRectangle, 760);
                Canvas.SetTop(blueRectangle, 360);
                contentCanvas.Children.Add(blueRectangle);
            }
            else
            {
                showName.Content = "Next Pilot";
            }
            Canvas.SetLeft(showName, 850);
            Canvas.SetTop(showName, 780);
            contentCanvas.Children.Add(showName);
        }*/
    }
}
 