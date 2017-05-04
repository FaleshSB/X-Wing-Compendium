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
        private int upgradeCardWidth = 220;
        private int upgradeCardHeight = 338;
        private Card currentRandomCard;
        private bool isShowingName = false;
        private AlignableWrapPanel contentWrapPanel = new AlignableWrapPanel();
        private CardCanvas randomCard;
        private bool isShowingManeuverCard = false;
        private Ship currentRandomShip;

        public QuizPage()
        {
            contentWrapPanel.Name = "contentWrapPanel";
            contentWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentScrollViewer.Content = contentWrapPanel;

            Pages.pages[PageName.Quiz] = this;
            InitializeComponent();

            GetNewCard();
        }

        private void GetNewCard()
        {
            int randomNumber = Rng.Next(3);
            if (randomNumber == 0) { currentRandomCard = Pilots.GetRandomPilot(); isShowingManeuverCard = false; }
            else if (randomNumber == 1) { currentRandomShip = Ships.GetRandomShip(); isShowingManeuverCard = true; }
            else { currentRandomCard = Upgrades.GetRandomUpgrade(); isShowingManeuverCard = false; }
        }
        
        private void ShowNameClicked(object sender, RoutedEventArgs e)
        {
            if (isShowingName == true)
            {
                GetNewCard();
            }

            isShowingName = !isShowingName;
            DisplayContent();
        }
        
        protected override void DisplayContent()
        {
            contentWrapPanel.Children.Clear();
            StackPanel contentStackPanel = new StackPanel();
            contentStackPanel.Orientation = Orientation.Vertical;
            contentStackPanel.VerticalAlignment = VerticalAlignment.Center;
            contentWrapPanel.Children.Add(contentStackPanel);
            TextBlock shipName = new TextBlock();

            if (isShowingManeuverCard)
            {
                List<string> shipNames = new List<string>();
                foreach(Ship ship in Ships.maneuverCardIndexedShipList[currentRandomShip.uniqueManeuverId])
                {
                    shipNames.Add(ship.name);
                }

                foreach (string thisShipName in shipNames.Distinct())
                {
                    shipName.Inlines.Add(thisShipName);
                    shipName.Inlines.Add(new LineBreak());
                }
                shipName.HorizontalAlignment = HorizontalAlignment.Center;
                shipName.VerticalAlignment = VerticalAlignment.Bottom;
                shipName.Height = 90;
                shipName.FontSize = 20;
                contentStackPanel.Children.Add(shipName);

                ManeuverCard randomManeuverCard = currentRandomShip.GetManeuverCard(30);
                randomManeuverCard.Margin = new Thickness(0, 0, 0, 20);

                shipName.Margin = new Thickness(0, 300 + (pilotCardHeight - (randomManeuverCard.Height + shipName.Height + 20)), 0, 20);
                contentStackPanel.Children.Add(randomManeuverCard);
            }
            else
            {
                if (currentRandomCard.GetType() == typeof(Pilot))
                {
                    randomCard = currentRandomCard.GetCanvas(pilotCardWidth, pilotCardHeight, new Thickness(0, 0, 0, 20), this);
                    contentStackPanel.Margin = new Thickness(0, 300, 0, 0);
                }
                else
                {
                    randomCard = currentRandomCard.GetCanvas(upgradeCardWidth, upgradeCardHeight, new Thickness(0, 0, 0, 20), this);
                    contentStackPanel.Margin = new Thickness(0, 300 + (pilotCardHeight - upgradeCardHeight), 0, 0);
                }
                contentStackPanel.Children.Add(randomCard);
            }
            

            Button showName = new Button();
            showName.Name = "ShowNameButton";
            showName.Width = 130;
            showName.Height = 40;
            showName.FontSize = Opt.ApResMod(16);
            showName.FontWeight = FontWeights.Bold;
            showName.Click += new RoutedEventHandler(ShowNameClicked);
            showName.UseLayoutRounding = true;

            if (isShowingName == false)
            {
                showName.Content = "Show Name";
                if(isShowingManeuverCard) { shipName.Visibility = Visibility.Hidden; }
                else { randomCard.HideCardIdentifiers(); }            
            }
            else
            {
                showName.Content = "Next Card";
                if (isShowingManeuverCard) { shipName.Visibility = Visibility.Visible; }
                else { randomCard.ShowCardIdentifiers(); }
            }
            contentStackPanel.Children.Add(showName);
        }
    }
}
 