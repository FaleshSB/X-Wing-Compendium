using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for BrowseCardsPage.xaml
    /// </summary>
    public partial class BrowseCardsPage : DefaultPage
    {
        private TextBox searchTextBox = new TextBox();
        RadioButton upgradesRadioButton = new RadioButton();
        RadioButton pilotsRadioButton = new RadioButton();
        CheckBox searchDescriptionCheckBox = new CheckBox();
        Button exitButton = new Button();

        private List<Upgrade> upgrades = new List<Upgrade>();
        private List<Upgrade> upgradesToDisplay = new List<Upgrade>();
        private List<Pilot> pilots = new List<Pilot>();
        private List<Pilot> pilotsToDisplay = new List<Pilot>();
        private int upgradeCardWidth = 166;
        private int upgradeCardHeight = 255;
        private int pilotCardWidth = 292;
        private int pilotCardHeight = 410;
        private bool isUpgradeChecked = false;
        private bool isSearchDescriptionChecked = false;
        private string previousUpgradeSearchResultIds = "";
        private string previousPilotSearchResultIds = "";

        private bool isAddingPilot = false;
        private bool isAddingUpgrade = false;
        private int pilotKey;
        private Build build;
        public void SetBuild(Build build)
        {
            this.build = build;
        }
        
        private Canvas contentCanvas = new Canvas();
        protected AlignableWrapPanel contentWrapPanel = new AlignableWrapPanel();

        public BrowseCardsPage()
        {
            contentWrapPanel.Name = "contentWrapPanel";
            contentWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentScrollViewer.Content = contentWrapPanel;

            contentCanvas.Name = "contentCanvas";
            contentWrapPanel.Children.Add(contentCanvas);

            /*<TextBox Grid.Row="0" Grid.Column="0" x:Name="searchTextBox" HorizontalAlignment="Center" Height="23" Text="" VerticalAlignment="Top" Margin="0,40,0,0" Width="120" TextChanged="textBox_TextChanged"/>
        <RadioButton Content="Upgrades" x:Name="upgradesRadioButton" Grid.Row="0" Grid.Column="0" Checked="IsUpgrade_Checked" HorizontalAlignment="Left" Margin="10,10,0,0" VerticalAlignment="Top" IsChecked="True"/>
        <RadioButton Content="Pilots" x:Name="pilotsRadioButton" Grid.Row="0" Grid.Column="0" Checked="IsUpgrade_Checked" HorizontalAlignment="Left" Margin="10,30,0,0" VerticalAlignment="Top"/>
        <CheckBox Content="Search Description" x:Name="searchDescriptionCheckBox" Grid.Row="0" Grid.Column="0" Checked="SearchDescription_Checked" Unchecked="SearchDescription_Checked" HorizontalAlignment="Left" Margin="10,50,0,0" VerticalAlignment="Top"/>
        <Button x:Name="button" Content="Pilot Quiz" HorizontalAlignment="Left" Height="26" Margin="382,30,0,0" VerticalAlignment="Top" Width="99" Click="TempButton"/>
        <Label x:Name="cardId" Content="" HorizontalAlignment="Center" VerticalAlignment="Top" FontSize="18" FontWeight="Bold" Margin="0,60,0,0"/>
        <Button x:Name="button1" Content="Exit" HorizontalAlignment="right" Height="22"  VerticalAlignment="Top" Width="27" Click="ExitButton"/>*/
            searchTextBox.Name = "searchTextBox";
            searchTextBox.TextChanged += textBox_TextChanged;
            searchTextBox.Width = 120;
            searchTextBox.Height = 23;
            Canvas.SetLeft(searchTextBox, 200);
            Canvas.SetTop(searchTextBox, 20);
            manuNavigationCanvas.Children.Add(searchTextBox);
            
            upgradesRadioButton.Name = "upgradesRadioButton";
            upgradesRadioButton.Content = "Upgrades";
            upgradesRadioButton.Checked += IsUpgrade_Checked;
            upgradesRadioButton.IsChecked = true;
            Canvas.SetLeft(upgradesRadioButton, 300);
            Canvas.SetTop(upgradesRadioButton, 20);
            manuNavigationCanvas.Children.Add(upgradesRadioButton);
            
            pilotsRadioButton.Name = "pilotsRadioButton";
            pilotsRadioButton.Content = "Pilots";
            pilotsRadioButton.Checked += IsUpgrade_Checked;
            pilotsRadioButton.IsChecked = false;
            Canvas.SetLeft(pilotsRadioButton, 300);
            Canvas.SetTop(pilotsRadioButton, 40);
            manuNavigationCanvas.Children.Add(pilotsRadioButton);

            searchDescriptionCheckBox.Name = "searchDescriptionCheckBox";
            searchDescriptionCheckBox.Content = "Search Description";
            searchDescriptionCheckBox.Checked += SearchDescription_Checked;
            searchDescriptionCheckBox.Unchecked += SearchDescription_Checked;
            searchDescriptionCheckBox.IsChecked = false;
            Canvas.SetLeft(searchDescriptionCheckBox, 600);
            Canvas.SetTop(searchDescriptionCheckBox, 40);
            manuNavigationCanvas.Children.Add(searchDescriptionCheckBox);

            manuNavigationCanvas.Height = 150;


            Pages.pages[PageName.BrowseCards] = this;
            InitializeComponent();
            upgrades = Upgrades.upgrades.Values.ToList();
            //cardScrollViewer.Height = 990;
            searchTextBox.Focus();
            SetIsUpgradeChecked();
            SetIsSearchDescriptionChecked();
        }


        internal void AddPilot(Build build)
        {
            pilotsRadioButton.IsChecked = true;
            isAddingPilot = true;
            this.build = build;
            pilots = Pilots.GetPilots(build.faction);
            pilots = pilots.OrderBy(pilot => pilot.faction).ThenBy(pilot => pilot.ship.shipType).ThenByDescending(pilot => pilot.pilotSkill).ThenByDescending(pilot => pilot.cost).ToList();
            pilotsToDisplay = pilots.ToList();
        }
        public void AddAvailibleUpdates(int pilotKey, Build build)
        {
            upgradesRadioButton.IsChecked = true;
            isAddingUpgrade = true;
            this.pilotKey = pilotKey;
            this.build = build;
            Pilot pilot = build.GetPilot(pilotKey);
            List<Faction> factions = new List<Faction>();
            factions.Add(pilot.faction);
            List<ShipSize> shipSizes = new List<ShipSize>();
            shipSizes.Add(pilot.ship.shipSize);
            List<Ship> ships = new List<Ship>();
            ships.Add(pilot.ship);
            upgrades = Upgrades.GetUpgrades(build.GetPossibleUpgrades(pilot.uniquePilotId), factions, shipSizes, ships);
            upgrades = upgrades.OrderBy(upgrade => upgrade.upgradeType).ThenByDescending(upgrade => upgrade.cost).ToList();
            upgradesToDisplay = upgrades.ToList();
        }

        protected override void DisplayContent()
        {
            //DisplayUpgradeCards();
            //DisplayPilotCards();
            contentWrapPanel.Children.Clear();
            
            foreach (Upgrade upgrade in upgradesToDisplay)
            {
                UpgradeCard upgradeCard = upgrade.GetUpgradeCard(Opt.ApResMod(upgradeCardWidth), Opt.ApResMod(upgradeCardHeight));
                upgradeCard.MouseDown += CardClicked;
                upgradeCard.Margin = new Thickness(2, 2, 2, 2);
                contentWrapPanel.Children.Add(upgradeCard);
            }


            foreach (Pilot pilot in pilotsToDisplay)
            {
                PilotCard pilotCard = pilot.GetPilotCard(Opt.ApResMod(pilotCardWidth), Opt.ApResMod(pilotCardHeight));
                pilotCard.MouseDown += CardClicked;
                pilotCard.Margin = new Thickness(2, 2, 2, 2);
                contentWrapPanel.Children.Add(pilotCard);
            }
        }

        private void UpdateContents()
        {
            Regex regex = new Regex("\\s+");
            string filteredSearchText = regex.Replace(searchTextBox.Text, " ");
            regex = new Regex("^\\s+");
            filteredSearchText = regex.Replace(filteredSearchText, "");
            regex = new Regex("\\s+$");
            filteredSearchText = regex.Replace(filteredSearchText, "");
            string[] searchWords = filteredSearchText.Split(' ');
            searchWords = searchWords.Where(s => s != "").ToArray();
            searchWords = searchWords.Where(s => s != "  ").ToArray();
            searchWords = searchWords.Where(s => s.Count() > 2).ToArray();

            if(filteredSearchText == "" && isAddingUpgrade) { upgradesToDisplay = upgrades.ToList(); DisplayUpgradeCards(); previousUpgradeSearchResultIds = "it was changed, really!"; }
            if(filteredSearchText == "" && isAddingPilot) { pilotsToDisplay = pilots.ToList(); DisplayPilotCards(); previousPilotSearchResultIds = "it was changed, really!"; }

            if (searchWords.Count() > 0)
            {
                pilotsToDisplay.Clear();
                upgradesToDisplay.Clear();
                if (isUpgradeChecked)
                {
                    string currentUpgradeSearchResultIds = "";
                    foreach (Upgrade upgrade in upgrades)
                    {
                        bool hasFoundAllWords = true;
                        foreach(string searchWord in searchWords)
                        {
                            bool hasFoundWordInName = upgrade.name.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            bool hasFoundWordInDescription = upgrade.description.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            bool hasFoundWordInType = upgrade.upgradeType.ToString().IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            if (isSearchDescriptionChecked == false && hasFoundWordInName == false && hasFoundWordInType == false)
                            {
                                hasFoundAllWords = false;
                                break;
                            }
                            if (isSearchDescriptionChecked == true && hasFoundWordInDescription == false)
                            {
                                hasFoundAllWords = false;
                                break;
                            }
                        }
                        /*bool hasFoundAllWords = false;
                        foreach (string searchWord in searchWords)
                        {
                            bool hasFoundWordInName = upgrade.name.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            bool hasFoundWordInDescription = upgrade.description.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            bool hasFoundWordInType = upgrade.upgradeType.ToString().IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            if (isSearchDescriptionChecked == false && (hasFoundWordInName == true || hasFoundWordInType == true))
                            {
                                hasFoundAllWords = true;
                                break;
                            }
                            if (isSearchDescriptionChecked == true && hasFoundWordInDescription == true)
                            {
                                hasFoundAllWords = true;
                                break;
                            }
                        }*/

            if (hasFoundAllWords)
                        {
                            upgradesToDisplay.Add(upgrade);
                            currentUpgradeSearchResultIds += upgrade.id.ToString();
                        }
                    }

                    if(previousUpgradeSearchResultIds != currentUpgradeSearchResultIds)
                    {
                        previousUpgradeSearchResultIds = currentUpgradeSearchResultIds;
                        upgradesToDisplay = upgradesToDisplay.OrderBy(upgrade => upgrade.upgradeType).ThenByDescending(upgrade => upgrade.cost).ToList();
                        //DisplayUpgradeCards();
                        DisplayContent();
                    }
                }
                else
                {
                    string currentPilotSearchResultIds = "";
                    foreach (Pilot pilot in pilots)
                    {
                        /*bool hasFoundAllWords = true;
                        foreach (string searchWord in searchWords)
                        {
                            bool hasFoundWordInName = entry.Value.name.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            bool hasFoundWordInShipName = entry.Value.ship.name.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            bool hasFoundWordInDescription = entry.Value.description.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            if (isSearchDescriptionChecked == false && hasFoundWordInName == false && hasFoundWordInShipName == false)
                            {
                                hasFoundAllWords = false;
                                break;
                            }
                            if (isSearchDescriptionChecked == true && (entry.Value.hasAbility == false || hasFoundWordInDescription == false))
                            {
                                hasFoundAllWords = false;
                                break;
                            }
                        }*/
                        bool hasFoundAllWords = false;
                        foreach (string searchWord in searchWords)
                        {
                            bool hasFoundWordInName = pilot.name.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            bool hasFoundWordInShipName = pilot.ship.name.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            bool hasFoundWordInDescription = pilot.description.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            if (isSearchDescriptionChecked == false && (hasFoundWordInName == true || hasFoundWordInShipName == true))
                            {
                                hasFoundAllWords = true;
                                break;
                            }
                            if (isSearchDescriptionChecked == true && (pilot.hasAbility == true && hasFoundWordInDescription == true))
                            {
                                hasFoundAllWords = true;
                                break;
                            }
                        }

                        if (hasFoundAllWords)
                        {
                            pilotsToDisplay.Add(pilot);
                            currentPilotSearchResultIds += pilot.id.ToString();
                        }
                    }

                    if (previousPilotSearchResultIds != currentPilotSearchResultIds)
                    {
                        previousPilotSearchResultIds = currentPilotSearchResultIds;
                        pilotsToDisplay = pilotsToDisplay.OrderBy(pilot => pilot.faction).ThenBy(pilot => pilot.ship.shipType).ThenByDescending(pilot => pilot.cost).ToList();
                        DisplayPilotCards();
                    }
                }
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateContents();
        }

        private void SetIsUpgradeChecked()
        {
            bool origionalValue = isUpgradeChecked;
            if (upgradesRadioButton.IsChecked.HasValue)
            {
                isUpgradeChecked = (bool)upgradesRadioButton.IsChecked;
            }
            else if (pilotsRadioButton.IsChecked.HasValue)
            {
                isUpgradeChecked = (bool)pilotsRadioButton.IsChecked;
            }
            if(origionalValue != isUpgradeChecked)
            {
                UpdateContents();
            }
        }
        private void SetIsSearchDescriptionChecked()
        {
            bool origionalValue = isSearchDescriptionChecked;
            if (searchDescriptionCheckBox.IsChecked.HasValue)
            {
                isSearchDescriptionChecked = (bool)searchDescriptionCheckBox.IsChecked;
            }
            if (origionalValue != isSearchDescriptionChecked)
            {
                UpdateContents();
            }
        }
        private void IsUpgrade_Checked(object sender, RoutedEventArgs e)
        {
            SetIsUpgradeChecked();
        }
        private void SearchDescription_Checked(object sender, RoutedEventArgs e)
        {
            SetIsSearchDescriptionChecked();
        }        

        private void DisplayUpgradeCards()
        {
            contentCanvas.Children.Clear();

            double currentHeightOffset = -30;
            double currentLeftOffset = 20;
            double spacersGap = 4;
            int currentRowNumber = 0;
            foreach(Upgrade upgrade in upgradesToDisplay)
            {
                UpgradeCard upgradeCard = upgrade.GetUpgradeCard(Opt.ApResMod(upgradeCardWidth), Opt.ApResMod(upgradeCardHeight));
                upgradeCard.MouseDown += CardClicked;
                Canvas.SetLeft(upgradeCard, currentLeftOffset + spacersGap);
                Canvas.SetTop(upgradeCard, currentHeightOffset + 40);
                contentCanvas.Children.Add(upgradeCard);
                currentLeftOffset += spacersGap + Opt.ApResMod(upgradeCardWidth);
                currentRowNumber++;

                if (currentRowNumber > 10)
                {
                    currentHeightOffset += spacersGap + Opt.ApResMod(upgradeCardHeight);
                    currentLeftOffset = 20;
                    currentRowNumber = 0;
                }
            }
            contentCanvas.Height = currentHeightOffset + spacersGap + Opt.ApResMod(upgradeCardHeight) + 80;
        }

        private void CardClicked(object sender, MouseButtonEventArgs e)
        {
            Card card = (Card)sender;
            //cardId.Content = card.id.ToString();
            if(isAddingUpgrade == true)
            {
                upgradesToDisplay.Clear();
                pilotsToDisplay.Clear();
                isAddingUpgrade = false;
                isAddingPilot = false;
                build.AddUpgrade(pilotKey, Upgrades.upgrades[card.id]);
                SquadsPage squadsPage = (SquadsPage)Pages.pages[PageName.SquadsPage];
                NavigationService.Navigate(squadsPage);
            }
            else if(isAddingPilot == true)
            {
                upgradesToDisplay.Clear();
                pilotsToDisplay.Clear();
                isAddingUpgrade = false;
                isAddingPilot = false;
                build.AddPilot(Pilots.GetPilotClone(card.id));
                SquadsPage squadsPage = (SquadsPage)Pages.pages[PageName.SquadsPage];
                NavigationService.Navigate(squadsPage);
            }
        }

        private void TempButton(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.Content.ToString() == "Pilot Quiz")
            {
                NavigationService.Navigate((PilotQuizPage)Pages.pages[PageName.PilotQuiz]);
            }
        }

        private void DisplayPilotCards()
        {
            contentCanvas.Children.Clear();

            double currentHeightOffset = -30;
            double currentLeftOffset = 20;
            double spacersGap = 4;
            int currentRowNumber = 0;
            foreach (Pilot pilot in pilotsToDisplay)
            {
                PilotCard pilotCard = pilot.GetPilotCard(Opt.ApResMod(pilotCardWidth), Opt.ApResMod(pilotCardHeight));
                pilotCard.MouseDown += CardClicked;
                Canvas.SetLeft(pilotCard, currentLeftOffset + spacersGap);
                Canvas.SetTop(pilotCard, currentHeightOffset + 40);
                contentCanvas.Children.Add(pilotCard);
                currentLeftOffset += spacersGap + Opt.ApResMod(pilotCardWidth);
                currentRowNumber++;

                if (currentRowNumber > 5)
                {
                    currentHeightOffset += spacersGap + Opt.ApResMod(pilotCardHeight);
                    currentLeftOffset = 20;
                    currentRowNumber = 0;
                }
            }
            contentCanvas.Height = currentHeightOffset + spacersGap + Opt.ApResMod(pilotCardHeight) + 80;
        }
    }
}
