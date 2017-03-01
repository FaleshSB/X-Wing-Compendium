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

        private List<Upgrade> upgrades = new List<Upgrade>();
        private List<Upgrade> upgradesToDisplay = new List<Upgrade>();
        private List<Pilot> pilotsToDisplay = new List<Pilot>();
        private int upgradeCardWidth = 166;
        private int upgradeCardHeight = 255;
        private int pilotCardWidth = 292;
        private int pilotCardHeight = 410;
        private bool isUpgradeChecked = false;
        private bool isSearchDescriptionChecked = false;
        private string previousUpgradeSearchResultIds = "";
        private string previousPilotSearchResultIds = "";

        public bool isAddingUpgrade { get; set; } = false;
        public int pilotKey { get; set; }
        private Build build { get; set; }
        public void SetBuild(Build build)
        {
            this.build = build;
        }

        public BrowseCardsPage()
        {
            Pages.pages[PageName.BrowseCards] = this;
            InitializeComponent();
            upgrades = Upgrades.upgrades.Values.ToList();
            cardScrollViewer.Height = 990;
            searchTextBox.Focus();
            SetIsUpgradeChecked();
            SetIsSearchDescriptionChecked();
        }

        protected override void DisplayContent()
        {
            if(isAddingUpgrade == true)
            {
                Pilot pilot = build.GetPilot(pilotKey);
                List<Faction> factions = new List<Faction>();
                factions.Add(pilot.faction);
                List<ShipSize> shipSizes = new List<ShipSize>();
                shipSizes.Add(pilot.ship.shipSize);
                List<ShipType> shipTypes = new List<ShipType>();
                shipTypes.Add(pilot.ship.shipType);
                upgrades = Upgrades.GetUpgrades(pilot.possibleUpgrades, factions, shipSizes, shipTypes);
                upgrades = upgrades.OrderBy(upgrade => upgrade.upgradeType).ThenByDescending(upgrade => upgrade.cost).ToList();
                upgradesToDisplay = upgrades;
            }
            DisplayUpgradeCards();
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

            if (searchWords.Count() > 0)
            {
                pilotsToDisplay.Clear();
                upgradesToDisplay.Clear();
                if (isUpgradeChecked)
                {
                    string currentUpgradeSearchResultIds = "";
                    foreach (Upgrade upgrade in upgrades)
                    {
                        /*bool hasFoundAllWords = true;
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
                        }*/
                        bool hasFoundAllWords = false;
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
                        }

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
                        DisplayUpgradeCards();
                    }
                }
                else
                {
                    string currentPilotSearchResultIds = "";
                    foreach (KeyValuePair<int, Pilot> entry in Pilots.pilots)
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
                            bool hasFoundWordInName = entry.Value.name.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            bool hasFoundWordInShipName = entry.Value.ship.name.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            bool hasFoundWordInDescription = entry.Value.description.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0;
                            if (isSearchDescriptionChecked == false && (hasFoundWordInName == true || hasFoundWordInShipName == true))
                            {
                                hasFoundAllWords = true;
                                break;
                            }
                            if (isSearchDescriptionChecked == true && (entry.Value.hasAbility == true && hasFoundWordInDescription == true))
                            {
                                hasFoundAllWords = true;
                                break;
                            }
                        }

                        if (hasFoundAllWords)
                        {
                            pilotsToDisplay.Add(entry.Value);
                            currentPilotSearchResultIds += entry.Value.id.ToString();
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
            cardId.Content = card.id.ToString();
            if(isAddingUpgrade == true)
            {
                build.AddUpgrade(pilotKey, Upgrades.upgrades[card.id]);
                SquadsPage squadsPage = (SquadsPage)Pages.pages[PageName.SquadsPage];
                squadsPage.SetBuild(build);
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
