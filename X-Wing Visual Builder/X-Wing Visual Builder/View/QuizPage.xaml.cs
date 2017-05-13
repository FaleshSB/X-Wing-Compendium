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
        private int upgradeCardWidth = 200;
        private int upgradeCardHeight = 308;
        private Card currentRandomCard;
        private bool isShowingName = false;
        private AlignableWrapPanel contentWrapPanel = new AlignableWrapPanel();
        private CardCanvas randomCard;
        private bool isShowingManeuverCard = false;
        private Ship currentRandomShip;
        private bool isInclusingPilots = true;
        private bool isInclusingUpgrades = true;
        private bool isInclusingManeuvers = true;
        private StackPanel selectCategoryStackPanel;
        private StackPanel contentStackPanel;
        private bool isButtonBeingPressed = false;

        public QuizPage()
        {
            contentWrapPanel.Name = "contentWrapPanel";
            contentWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentScrollViewer.Content = contentWrapPanel;

            Pages.pages[PageName.Quiz] = this;
            InitializeComponent();

            contentStackPanel = new StackPanel();

            selectCategoryStackPanel = new StackPanel();
            selectCategoryStackPanel.Orientation = Orientation.Horizontal;
            selectCategoryStackPanel.Margin = ScaledThicknessFactory.GetThickness(0, 0, 0, 20);

            CheckBox pilotsCheckBox = new CheckBox();
            pilotsCheckBox.Content = "Include Piltos";
            pilotsCheckBox.Checked += PilotsChecked;
            pilotsCheckBox.Unchecked += PilotsUnchecked;
            pilotsCheckBox.FontSize = Opt.ApResMod(14);
            pilotsCheckBox.IsChecked = true;
            pilotsCheckBox.VerticalAlignment = VerticalAlignment.Center;
            pilotsCheckBox.Margin = ScaledThicknessFactory.GetThickness(8, 0, 8, 0);
            selectCategoryStackPanel.Children.Add(pilotsCheckBox);

            CheckBox upgradesCheckBox = new CheckBox();
            upgradesCheckBox.Content = "Include Upgrades";
            upgradesCheckBox.Checked += UpgradesChecked;
            upgradesCheckBox.Unchecked += UpgradesUnchecked;
            upgradesCheckBox.FontSize = Opt.ApResMod(14);
            upgradesCheckBox.IsChecked = true;
            upgradesCheckBox.VerticalAlignment = VerticalAlignment.Center;
            upgradesCheckBox.Margin = ScaledThicknessFactory.GetThickness(8, 0, 8, 0);
            selectCategoryStackPanel.Children.Add(upgradesCheckBox);

            CheckBox maneuversCheckBox = new CheckBox();
            maneuversCheckBox.Content = "Include Maneuvers";
            maneuversCheckBox.Checked += ManeuversChecked;
            maneuversCheckBox.Unchecked += ManeuversUnchecked;
            maneuversCheckBox.FontSize = Opt.ApResMod(14);
            maneuversCheckBox.IsChecked = true;
            maneuversCheckBox.VerticalAlignment = VerticalAlignment.Center;
            maneuversCheckBox.Margin = ScaledThicknessFactory.GetThickness(8, 0, 8, 0);
            selectCategoryStackPanel.Children.Add(maneuversCheckBox);

            ImageButton browseCards = new ImageButton("browse_cards", 0.5);
            browseCards.MouseDown += new MouseButtonEventHandler(BrowseCardsClicked);
            browseCards.Margin = ScaledThicknessFactory.GetThickness(2, 5, 2, 2);
            manuNavigationWrapPanel.Children.Add(browseCards);

            ImageButton manageSquads = new ImageButton("manage_squads", 0.5);
            manageSquads.MouseDown += new MouseButtonEventHandler(ManageSquadsClicked);
            manageSquads.Margin = ScaledThicknessFactory.GetThickness(2, 5, 2, 2);
            manuNavigationWrapPanel.Children.Add(manageSquads);




            GetNewCard();
        }


        private async void BrowseCardsClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(Opt.buttonDelay);
            isButtonBeingPressed = false;
            NavigationService.Navigate((BrowseCardsPage)Pages.pages[PageName.BrowseCards]);
        }

        private async void ManageSquadsClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(Opt.buttonDelay);
            isButtonBeingPressed = false;
            NavigationService.Navigate((SquadsPage)Pages.pages[PageName.Squads]);
        }

        private void GetNewCard()
        {
            int randomNumber = Rng.Next(3);
            if (randomNumber == 0)
            {
                if(!isInclusingPilots && (isInclusingUpgrades || isInclusingManeuvers)) { GetNewCard();return; }
                currentRandomCard = Pilots.GetRandomPilot();
                isShowingManeuverCard = false;
            }
            else if (randomNumber == 1)
            {
                if (!isInclusingManeuvers && (isInclusingPilots || isInclusingUpgrades)) { GetNewCard(); return; }
                currentRandomShip = Ships.GetRandomShip();
                isShowingManeuverCard = true;
            }
            else
            {
                if (!isInclusingUpgrades && (isInclusingPilots || isInclusingManeuvers)) { GetNewCard(); return; }
                currentRandomCard = Upgrades.GetRandomUpgrade();
                isShowingManeuverCard = false;
            }
        }
        
        private async void ShowNameClicked(object sender, MouseButtonEventArgs e)
        {
            if (isButtonBeingPressed) return;
            isButtonBeingPressed = true;
            await Task.Delay(Opt.buttonDelay);
            isButtonBeingPressed = false;
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
            
            contentStackPanel.Children.Clear();
            contentStackPanel.Orientation = Orientation.Vertical;
            contentStackPanel.VerticalAlignment = VerticalAlignment.Center;
            contentWrapPanel.Children.Add(contentStackPanel);
            
            contentStackPanel.Children.Add(selectCategoryStackPanel);

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
                randomManeuverCard.Margin = ScaledThicknessFactory.GetThickness(0, 0, 0, 20);
                selectCategoryStackPanel.Margin = ScaledThicknessFactory.GetThickness(0, 280, 0, 20 + (pilotCardHeight - (randomManeuverCard.Height + shipName.Height + 20)));
                shipName.Margin = ScaledThicknessFactory.GetThickness(0, 0, 0, 20);
                contentStackPanel.Children.Add(randomManeuverCard);
            }
            else
            {
                if (currentRandomCard.GetType() == typeof(Pilot))
                {
                    randomCard = currentRandomCard.GetCanvas(pilotCardWidth, pilotCardHeight, ScaledThicknessFactory.GetThickness(0, 0, 0, 20), this);
                    selectCategoryStackPanel.Margin = ScaledThicknessFactory.GetThickness(0, 280, 0, 20);
                }
                else
                {
                    randomCard = currentRandomCard.GetCanvas(upgradeCardWidth, upgradeCardHeight, ScaledThicknessFactory.GetThickness(0, 0, 0, 20), this);
                    selectCategoryStackPanel.Margin = ScaledThicknessFactory.GetThickness(0, 280, 0, 20 + (pilotCardHeight - upgradeCardHeight));
                }
                contentStackPanel.Children.Add(randomCard);
            }

            ImageButton showName;
            if (isShowingName == false)
            {
                showName = new ImageButton("show_name", 0.5);
                showName.MouseDown += new MouseButtonEventHandler(ShowNameClicked);
                showName.Margin = ScaledThicknessFactory.GetThickness(2, 5, 2, 2);
                
                if (isShowingManeuverCard) { shipName.Visibility = Visibility.Hidden; }
                else { randomCard.HideCardIdentifiers(); }            
            }
            else
            {
                showName = new ImageButton("next", 0.5);
                showName.MouseDown += new MouseButtonEventHandler(ShowNameClicked);
                showName.Margin = ScaledThicknessFactory.GetThickness(2, 5, 2, 2);

                if (isShowingManeuverCard) { shipName.Visibility = Visibility.Visible; }
                else { randomCard.ShowCardIdentifiers(); }
            }
            contentStackPanel.Children.Add(showName);
        }

        private void ManeuversUnchecked(object sender, RoutedEventArgs e)
        {
            isInclusingManeuvers = false;
        }

        private void ManeuversChecked(object sender, RoutedEventArgs e)
        {
            isInclusingManeuvers = true;
        }

        private void UpgradesChecked(object sender, RoutedEventArgs e)
        {
            isInclusingUpgrades = true;
        }

        private void UpgradesUnchecked(object sender, RoutedEventArgs e)
        {
            isInclusingUpgrades = false;
        }

        private void PilotsUnchecked(object sender, RoutedEventArgs e)
        {
            isInclusingPilots = false;
        }

        private void PilotsChecked(object sender, RoutedEventArgs e)
        {
            isInclusingPilots = true;
        }
    }
}
 