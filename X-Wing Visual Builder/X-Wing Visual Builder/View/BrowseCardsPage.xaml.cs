﻿using System;
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
    public partial class BrowseCardsPage : DefaultPage, ICardClicked
    {
        private TextBox searchTextBox = new TextBox();
        RadioButton upgradesRadioButton = new RadioButton();
        RadioButton pilotsRadioButton = new RadioButton();
        CheckBox searchDescriptionCheckBox = new CheckBox();
        CheckBox showAddRemoveButtonsCheckBox = new CheckBox();
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

        private bool isSwappingPilot = false;
        private bool isAddingPilot = false;
        private bool isAddingUpgrade = false;
        private int uniquePilotId;
        private Build build;
        private UniquePilot pilotToSwap;
        public void SetBuild(Build build)
        {
            this.build = build;
        }
        
        private Canvas contentCanvas = new Canvas();
        protected AlignableWrapPanel contentWrapPanel = new AlignableWrapPanel();

        public Dictionary<int, CardCanvas> pilotCanvasCache = new Dictionary<int, CardCanvas>();
        public Dictionary<int, CardCanvas> upgradeCanvasCache = new Dictionary<int, CardCanvas>();


        public BrowseCardsPage()
        {
            contentWrapPanel.Name = "contentWrapPanel";
            contentWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentScrollViewer.Content = contentWrapPanel;

            contentCanvas.Name = "contentCanvas";
            contentWrapPanel.Children.Add(contentCanvas);

            StackPanel upgradesOrPilots = new StackPanel();
            upgradesOrPilots.VerticalAlignment = VerticalAlignment.Center;
            upgradesOrPilots.Margin = new Thickness(8, 0, 8, 0);

            upgradesRadioButton.Name = "upgradesRadioButton";
            upgradesRadioButton.Content = "Upgrades";
            upgradesRadioButton.FontSize = 14;
            upgradesRadioButton.Checked += IsUpgrade_Checked;
            upgradesRadioButton.IsChecked = true;
            upgradesOrPilots.Children.Add(upgradesRadioButton);
            
            pilotsRadioButton.Name = "pilotsRadioButton";
            pilotsRadioButton.Content = "Pilots";
            pilotsRadioButton.FontSize = 14;
            pilotsRadioButton.Checked += IsUpgrade_Checked;
            pilotsRadioButton.IsChecked = false;
            upgradesOrPilots.Children.Add(pilotsRadioButton);

            manuNavigationWrapPanel.Children.Add(upgradesOrPilots);

            searchTextBox.Name = "searchTextBox";
            searchTextBox.TextChanged += textBox_TextChanged;
            searchTextBox.FontSize = 14;
            searchTextBox.Width = 170;
            searchTextBox.VerticalAlignment = VerticalAlignment.Center;
            searchTextBox.Margin = new Thickness(8, 0, 8, 0);
            manuNavigationWrapPanel.Children.Add(searchTextBox);

            searchDescriptionCheckBox.Name = "searchDescriptionCheckBox";
            searchDescriptionCheckBox.Content = "Search Description";
            searchDescriptionCheckBox.Checked += SearchDescription_Checked;
            searchDescriptionCheckBox.Unchecked += SearchDescription_Checked;
            searchDescriptionCheckBox.FontSize = 14;
            searchDescriptionCheckBox.IsChecked = false;
            searchDescriptionCheckBox.VerticalAlignment = VerticalAlignment.Center;
            searchDescriptionCheckBox.Margin = new Thickness(8, 0, 8, 0);
            manuNavigationWrapPanel.Children.Add(searchDescriptionCheckBox);
            
            /*
            showAddRemoveButtonsCheckBox.Name = "showAddRemoveButtonsCheckBox";
            showAddRemoveButtonsCheckBox.Content = "Show Add Remove Buttons";
            showAddRemoveButtonsCheckBox.Checked += new RoutedEventHandler(ShowAddRemoveButtonsChanged);
            showAddRemoveButtonsCheckBox.Unchecked += new RoutedEventHandler(ShowAddRemoveButtonsChanged);
            showAddRemoveButtonsCheckBox.IsChecked = false;
            Canvas.SetLeft(showAddRemoveButtonsCheckBox, 700);
            Canvas.SetTop(showAddRemoveButtonsCheckBox, 40);
            manuNavigationCanvas.Children.Add(showAddRemoveButtonsCheckBox);
            */
            manuNavigationWrapPanel.Height = 150;

            contentScrollViewer.Height = 1040 - manuNavigationWrapPanel.Height;


            Button squads = new Button();
            squads.Name = "squads";
            squads.Content = "Squads";
            squads.FontSize = 16;
            squads.FontWeight = FontWeights.Bold;
            squads.Click += new RoutedEventHandler(squadsClicked);
            squads.UseLayoutRounding = true;
            squads.VerticalAlignment = VerticalAlignment.Center;
            squads.Margin = new Thickness(8, 0, 8, 0);
            squads.Padding = new Thickness(5, 2, 5, 2);
            manuNavigationWrapPanel.Children.Add(squads);



            Pages.pages[PageName.BrowseCards] = this;
            InitializeComponent();
            upgrades = Upgrades.upgrades.Values.ToList();
            pilots = Pilots.pilots.Values.ToList();
            searchTextBox.Focus();
            SetIsUpgradeChecked();
            SetIsSearchDescriptionChecked();
        }

        private void squadsClicked(object sender, RoutedEventArgs e)
        {
            // TODO having to clear the below is probably each time you leave this page without adding a card to the squad is prone to bugs
            ClearState();
            NavigationService.Navigate((SquadsPage)Pages.pages[PageName.Squads]);
        }

        private void ClearState()
        {
            upgrades = Upgrades.upgrades.Values.ToList();
            pilots = Pilots.pilots.Values.ToList();
            upgradesToDisplay.Clear();
            pilotsToDisplay.Clear();
            isAddingUpgrade = false;
            isAddingPilot = false;
            isSwappingPilot = false;
            previousUpgradeSearchResultIds = "";
            previousPilotSearchResultIds = "";
            //searchTextBox.Text = "";
        }

        public void SwapPilot(Build build, UniquePilot pilotToSwap)
        {
            ClearState();
            this.pilotToSwap = pilotToSwap;
            pilotsRadioButton.IsChecked = true;
            isSwappingPilot = true;
            this.build = build;
            pilots = Pilots.GetPilots(build.faction, pilotToSwap.pilot.ship);
            pilotsToDisplay = pilots.ToList();
        }
        public void AddPilot(Build build)
        {
            ClearState();
            pilotsRadioButton.IsChecked = true;
            isAddingPilot = true;
            this.build = build;
            pilots = Pilots.GetPilots(build.faction);
            pilotsToDisplay = pilots.ToList();
        }
        public void AddUpgrade(int uniquePilotId, Build build)
        {
            ClearState();
            upgradesRadioButton.IsChecked = true;
            isAddingUpgrade = true;
            this.uniquePilotId = uniquePilotId;
            this.build = build;
            upgrades = Upgrades.GetUpgrades(build.GetPilot(uniquePilotId));
            upgradesToDisplay = upgrades.ToList();
        }

        public void AddUpgradeToCache(Upgrade upgrade)
        {
            if (upgradeCanvasCache.ContainsKey(upgrade.id) == false)
            {
                upgradeCanvasCache[upgrade.id] = upgrade.GetCanvas(upgradeCardWidth, upgradeCardHeight, new Thickness(2, 2, 2, 2), this);
                upgradeCanvasCache[upgrade.id].AddCardClickedEvent(this);
            }
        }
        public void AddPilotToCache(Pilot pilot)
        {
            if(pilotCanvasCache.ContainsKey(pilot.id) == false)
            {
                pilotCanvasCache[pilot.id] = pilot.GetCanvas(pilotCardWidth, pilotCardHeight, new Thickness(2, 2, 2, 2), this);
                pilotCanvasCache[pilot.id].AddCardClickedEvent(this);
            }
        }

        protected override void DisplayContent()
        {
            contentWrapPanel.Children.Clear();

            upgradesToDisplay = upgradesToDisplay.OrderBy(upgrade => upgrade.upgradeType.ToString()).ThenByDescending(upgrade => upgrade.cost).ThenBy(upgrade => upgrade.name).ToList();
            foreach (Upgrade upgrade in upgradesToDisplay)
            {
                if (upgrade.shipSize == ShipSize.Huge || upgrade.upgradeType == UpgradeType.Team || upgrade.upgradeType == UpgradeType.Hardpoint
                   || upgrade.upgradeType == UpgradeType.Cargo) { continue; }
                if (upgradeCanvasCache.ContainsKey(upgrade.id) == false)
                {
                    AddUpgradeToCache(upgrade);
                }
                contentWrapPanel.Children.Add(upgradeCanvasCache[upgrade.id]);
            }
            
            pilotsToDisplay = pilotsToDisplay.ToList().OrderBy(pilot => pilot.faction).ThenBy(pilot => pilot.ship.name).ThenByDescending(pilot => pilot.pilotSkill).ThenByDescending(pilot => pilot.cost).ThenBy(pilot => pilot.name).ToList();
            foreach (Pilot pilot in pilotsToDisplay)
            {
                if (pilot.ship.shipSize == ShipSize.Huge) { continue; }
                if (pilotCanvasCache.ContainsKey(pilot.id) == false)
                {
                    AddPilotToCache(pilot);
                }
                else
                {
                    pilotCanvasCache[pilot.id].Children.OfType<OutlinedTextBlock>().Single().Text = Pilots.pilots[pilot.id].numberOwned.ToString();
                }
                contentWrapPanel.Children.Add(pilotCanvasCache[pilot.id]);
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

            if(filteredSearchText == "" && isAddingUpgrade) { previousUpgradeSearchResultIds = "it was changed, really!"; DisplayContent(); }
            if(filteredSearchText == "" && (isAddingPilot || isSwappingPilot)) { previousPilotSearchResultIds = "it was changed, really!"; DisplayContent(); }
            
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
                            bool hasFoundCost = false;
                            regex = new Regex(@"([0-9]+)\-([0-9]+)$", RegexOptions.IgnoreCase);
                            Match match = regex.Match(searchWord);
                            if (match.Success)
                            {
                                if (upgrade.cost >= Int32.Parse(match.Groups[1].Value) && upgrade.cost <= Int32.Parse(match.Groups[2].Value)) { hasFoundCost = true; }
                            }
                            if (isSearchDescriptionChecked == false && hasFoundWordInName == false && hasFoundWordInType == false && hasFoundCost == false)
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
                        if(searchWords.Count() == 1 && searchWords[0].ToLower() == "all") { hasFoundAllWords = true; }
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
                            bool hasFoundPilotSkill = false;
                            regex = new Regex(@"ps[0-9]+$", RegexOptions.IgnoreCase);
                            Match match = regex.Match(searchWord);
                            if(match.Success)
                            {
                                string ps = Regex.Replace(searchWord, "[^0-9.]", "");
                                if (Int32.Parse(ps) == pilot.pilotSkill) { hasFoundPilotSkill = true; }
                            }
                            bool hasFoundUpgradeType = false;
                            foreach(KeyValuePair<UpgradeType, int> upgradeType in pilot.possibleUpgrades)
                            {
                                if(upgradeType.Key.ToString().IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    hasFoundUpgradeType = true;
                                    break;
                                }
                            }
                            bool hasFoundFaction = (pilot.faction.ToString().ToLower() == searchWord.ToLower()) ? true : false;
                            bool hasFoundShipSize = (pilot.ship.shipSize.ToString().ToLower() == searchWord.ToLower()) ? true : false;
                            
                            if (isSearchDescriptionChecked == false && (hasFoundWordInName || hasFoundWordInShipName
                                || hasFoundPilotSkill || hasFoundUpgradeType || hasFoundFaction || hasFoundShipSize))
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
                        if (searchWords.Count() == 1 && searchWords[0].ToLower() == "all") { hasFoundAllWords = true; }
                        if (hasFoundAllWords)
                        {
                            pilotsToDisplay.Add(pilot);
                            currentPilotSearchResultIds += pilot.id.ToString();
                        }
                    }

                    if (previousPilotSearchResultIds != currentPilotSearchResultIds)
                    {
                        previousPilotSearchResultIds = currentPilotSearchResultIds;
                        DisplayContent();
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
        public void UpgradeClicked(int upgradeId)
        {
            if (isAddingUpgrade == true)
            {
                ClearState();
                build.AddUpgrade(uniquePilotId, upgradeId);
                SquadsPage squadsPage = (SquadsPage)Pages.pages[PageName.Squads];

                UpgradeModifiers.AddUpgrade(build, uniquePilotId, upgradeId);

                NavigationService.Navigate(squadsPage);
            }
        }
        public void PilotClicked(int pilotId)
        {
            if (isAddingPilot == true)
            {
                ClearState();
                build.AddPilot(pilotId);
                SquadsPage squadsPage = (SquadsPage)Pages.pages[PageName.Squads];
                NavigationService.Navigate(squadsPage);
            }
            else if (isSwappingPilot == true)
            {
                ClearState();
                int uniquePilotId = build.AddPilot(pilotId);
                build.GetPilot(uniquePilotId).upgrades = pilotToSwap.upgrades;
                Upgrades.RemoveUnusableUpgrades(build, build.GetPilot(uniquePilotId).id);
                build.RemovePilot(pilotToSwap.id);
                SquadsPage squadsPage = (SquadsPage)Pages.pages[PageName.Squads];
                NavigationService.Navigate(squadsPage);
            }
        }
    }
}
