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

        private bool isSwappingPilot = false;
        private bool isAddingPilot = false;
        private bool isAddingUpgrade = false;
        private int uniquePilotId;
        private Build build;
        private Pilot pilotToSwap;
        public void SetBuild(Build build)
        {
            this.build = build;
        }
        
        private Canvas contentCanvas = new Canvas();
        protected AlignableWrapPanel contentWrapPanel = new AlignableWrapPanel();
        
        private Dictionary<int, Canvas> pilotCanvasCache = new Dictionary<int, Canvas>();
        private Dictionary<int, Canvas> upgradeCanvasCache = new Dictionary<int, Canvas>();

        private bool isShowingNumberOwned = true;

        public BrowseCardsPage()
        {
            contentWrapPanel.Name = "contentWrapPanel";
            contentWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentScrollViewer.Content = contentWrapPanel;

            contentCanvas.Name = "contentCanvas";
            contentWrapPanel.Children.Add(contentCanvas);

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

            contentScrollViewer.Height = 1040 - manuNavigationCanvas.Height;


            Button squads = new Button();
            squads.Name = "squads";
            squads.Content = "Squads";
            squads.Width = 130;
            squads.Height = 40;
            squads.FontSize = 16;
            squads.FontWeight = FontWeights.Bold;
            squads.Click += new RoutedEventHandler(squadsClicked);
            squads.UseLayoutRounding = true;
            Canvas.SetLeft(squads, 10);
            Canvas.SetTop(squads, 10);
            manuNavigationCanvas.Children.Add(squads);



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
            NavigationService.Navigate((SquadsPage)Pages.pages[PageName.SquadsPage]);
        }

        private void ClearState()
        {
            upgradesToDisplay.Clear();
            pilotsToDisplay.Clear();
            isAddingUpgrade = false;
            isAddingPilot = false;
            isSwappingPilot = false;
            previousUpgradeSearchResultIds = "";
            previousPilotSearchResultIds = "";
            searchTextBox.Text = "";
        }

        public void SwapPilot(Build build, Pilot pilotToSwap)
        {
            ClearState();
            this.pilotToSwap = pilotToSwap;
            pilotsRadioButton.IsChecked = true;
            isSwappingPilot = true;
            this.build = build;
            pilots = Pilots.GetPilots(build.faction, pilotToSwap.ship);
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

        protected override void DisplayContent()
        {
            contentWrapPanel.Children.Clear();

            upgradesToDisplay = upgradesToDisplay.OrderBy(upgrade => upgrade.upgradeType).ThenByDescending(upgrade => upgrade.cost).ThenBy(upgrade => upgrade.name).ToList();
            foreach (Upgrade upgrade in upgradesToDisplay)
            {
                if(upgradeCanvasCache.ContainsKey(upgrade.id) == false)
                {
                    upgradeCanvasCache[upgrade.id] = new Canvas();
                    upgradeCanvasCache[upgrade.id].Width = Opt.ApResMod(upgradeCardWidth);
                    upgradeCanvasCache[upgrade.id].Height = Opt.ApResMod(upgradeCardHeight);
                    upgradeCanvasCache[upgrade.id].Margin = new Thickness(2, 2, 2, 2);

                    UpgradeCard upgradeCard = upgrade.GetUpgradeCard(Opt.ApResMod(upgradeCardWidth), Opt.ApResMod(upgradeCardHeight));
                    upgradeCard.MouseLeftButtonDown += new MouseButtonEventHandler(CardClicked);

                    Canvas.SetLeft(upgradeCard, 0);
                    Canvas.SetTop(upgradeCard, 0);
                    upgradeCanvasCache[upgrade.id].Children.Add(upgradeCard);

                    InfoButton infoButton = new InfoButton();
                    infoButton.upgradeId = upgrade.id;
                    infoButton.Cursor = Cursors.Hand;
                    infoButton.MouseLeftButtonDown += new MouseButtonEventHandler(UpgradeInfoClicked);
                    //infoButton.MouseEnter += new MouseEventHandler(UpgradeInfoMouseHover);
                    //infoButton.MouseLeave += new MouseEventHandler(PilotMouseHoverLeave);
                    infoButton.MouseWheel += new MouseWheelEventHandler(ContentScroll);

                    Canvas.SetLeft(infoButton, 0);
                    Canvas.SetTop(infoButton, 0);
                    upgradeCanvasCache[upgrade.id].Children.Add(infoButton);
                }
                
                contentWrapPanel.Children.Add(upgradeCanvasCache[upgrade.id]);
            }

            pilotsToDisplay = pilotsToDisplay.ToList().OrderBy(pilot => pilot.faction).ThenBy(pilot => pilot.ship.name).ThenByDescending(pilot => pilot.pilotSkill).ThenByDescending(pilot => pilot.cost).ThenBy(pilot => pilot.name).ToList();
            foreach (Pilot pilot in pilotsToDisplay)
            {
                if (pilotCanvasCache.ContainsKey(pilot.id) == false)
                {
                    pilotCanvasCache[pilot.id] = new Canvas();
                    pilotCanvasCache[pilot.id].Width = Opt.ApResMod(pilotCardWidth);
                    pilotCanvasCache[pilot.id].Height = Opt.ApResMod(pilotCardHeight);
                    pilotCanvasCache[pilot.id].Margin = new Thickness(2, 2, 2, 2);

                    PilotCard pilotCard = pilot.GetPilotCard(Opt.ApResMod(pilotCardWidth), Opt.ApResMod(pilotCardHeight));
                    pilotCard.MouseLeftButtonDown += new MouseButtonEventHandler(CardClicked);
                    Canvas.SetLeft(pilotCard, 0);
                    Canvas.SetTop(pilotCard, 0);
                    pilotCanvasCache[pilot.id].Children.Add(pilotCard);

                    InfoButton infoButton = new InfoButton();
                    infoButton.uniquePilotId = pilot.id;
                    infoButton.MouseEnter += new MouseEventHandler(PilotMouseHover);
                    infoButton.MouseLeave += new MouseEventHandler(PilotMouseHoverLeave);
                    infoButton.MouseWheel += new MouseWheelEventHandler(ContentScroll);
                    Canvas.SetLeft(infoButton, 0);
                    Canvas.SetTop(infoButton, 0);
                    pilotCanvasCache[pilot.id].Children.Add(infoButton);

                    if (isShowingNumberOwned)
                    {
                        AddButton addPilot = new AddButton();
                        addPilot.pilotId = pilot.id;
                        addPilot.MouseLeftButtonDown += new MouseButtonEventHandler(AddPilotClicked);
                        addPilot.MouseWheel += new MouseWheelEventHandler(ContentScroll);
                        Canvas.SetLeft(addPilot, 80);
                        Canvas.SetTop(addPilot, 0);
                        pilotCanvasCache[pilot.id].Children.Add(addPilot);

                        RemoveButton removePilot = new RemoveButton();
                        removePilot.pilotId = pilot.id;
                        removePilot.MouseLeftButtonDown += new MouseButtonEventHandler(RemovePilotClicked);
                        removePilot.MouseWheel += new MouseWheelEventHandler(ContentScroll);
                        Canvas.SetRight(removePilot, 80);
                        Canvas.SetTop(removePilot, 0);
                        pilotCanvasCache[pilot.id].Children.Add(removePilot);

                        OutlinedTextBlock numberOwned = new OutlinedTextBlock();
                        numberOwned.Text = pilot.numberOwned.ToString();
                        numberOwned.TextAlignment = TextAlignment.Center;
                        numberOwned.Width = 50;
                        numberOwned.Height = 50;
                        numberOwned.StrokeThickness = 2;
                        numberOwned.Stroke = new SolidColorBrush(Color.FromRgb(50, 50, 50));
                        numberOwned.FontWeight = FontWeights.ExtraBold;
                        numberOwned.Fill = new SolidColorBrush(Color.FromRgb(250, 250, 250));
                        numberOwned.FontSize = 30;
                        numberOwned.FontFamily = new FontFamily("Verdana");
                        Canvas.SetLeft(numberOwned, 110);
                        Canvas.SetTop(numberOwned, 0);
                        pilotCanvasCache[pilot.id].Children.Add(numberOwned);                        
                    }
                }
                contentWrapPanel.Children.Add(pilotCanvasCache[pilot.id]);
            }
        }

        private void AddPilotClicked(object sender, MouseButtonEventArgs e)
        {
            AddButton addPilot = (AddButton)sender;
            Pilots.pilots[addPilot.pilotId].numberOwned++;
            pilotCanvasCache[addPilot.pilotId].Children.OfType<OutlinedTextBlock>().Single().Text = Pilots.pilots[addPilot.pilotId].numberOwned.ToString();
        }
        private void RemovePilotClicked(object sender, MouseButtonEventArgs e)
        {
            RemoveButton removePilot = (RemoveButton)sender;
            Pilots.pilots[removePilot.pilotId].numberOwned--;
            pilotCanvasCache[removePilot.pilotId].Children.OfType<OutlinedTextBlock>().Single().Text = Pilots.pilots[removePilot.pilotId].numberOwned.ToString();
        }

        private void UpdatePilot(int pilotId, bool displayManeuverCard)
        {
            int numberOfChildrenToKeep = 2;
            numberOfChildrenToKeep += (isShowingNumberOwned) ? 3 : 0;
            if (displayManeuverCard)
            {
                ManeuverCard maneuverCard = Pilots.pilots[pilotId].ship.GetManeuverCard(Math.Round(Opt.ApResMod(pilotCardWidth) / 11));
                maneuverCard.uniquePilotId = Pilots.pilots[pilotId].id;
                maneuverCard.MouseWheel += new MouseWheelEventHandler(ContentScroll);
                Canvas.SetLeft(maneuverCard, (Opt.ApResMod(pilotCardWidth) / 2) - (maneuverCard.Width / 2));
                Canvas.SetTop(maneuverCard, 0);
                pilotCanvasCache[pilotId].Children.Add(maneuverCard);
            }
            else
            {
                if (pilotCanvasCache[pilotId].Children.Count > numberOfChildrenToKeep)
                {
                    pilotCanvasCache[pilotId].Children.RemoveRange(numberOfChildrenToKeep, pilotCanvasCache[pilotId].Children.Count - numberOfChildrenToKeep);
                }
            }
        }
        private void UpdateUpgradeCard(int upgradeId, bool displayInfo)
        {
            if (displayInfo)
            {
                InfoDialogBox infoDialogBox = new InfoDialogBox();
                infoDialogBox.AddUpgradeCard(Upgrades.upgrades[upgradeId].GetUpgradeCard(Opt.ApResMod(upgradeCardWidth), Opt.ApResMod(upgradeCardHeight)));
                infoDialogBox.AddUpgradeInfo(Upgrades.upgrades[upgradeId]);
                infoDialogBox.ShowDialog();
            }
            else
            {
                if (upgradeCanvasCache[upgradeId].Children.Count > 2)
                {
                    upgradeCanvasCache[upgradeId].Children.RemoveRange(2, upgradeCanvasCache[upgradeId].Children.Count - 2);
                }
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

        private void CardClicked(object sender, MouseButtonEventArgs e)
        {
            Card card = (Card)sender;
            //cardId.Content = card.id.ToString();
            if(isAddingUpgrade == true)
            {
                upgradesToDisplay.Clear();
                pilotsToDisplay.Clear();
                previousUpgradeSearchResultIds = "";
                previousPilotSearchResultIds = "";
                searchTextBox.Text = "";
                isSwappingPilot = false;
                isAddingUpgrade = false;
                isAddingPilot = false;
                build.AddUpgrade(uniquePilotId, Upgrades.upgrades[card.id]);
                SquadsPage squadsPage = (SquadsPage)Pages.pages[PageName.SquadsPage];

                UpgradeModifiers.AddUpgrade(build, uniquePilotId, card.id);

                NavigationService.Navigate(squadsPage);
            }
            else if(isAddingPilot == true)
            {
                upgradesToDisplay.Clear();
                pilotsToDisplay.Clear();
                previousUpgradeSearchResultIds = "";
                previousPilotSearchResultIds = "";
                searchTextBox.Text = "";
                isSwappingPilot = false;
                isAddingUpgrade = false;
                isAddingPilot = false;
                build.AddPilot(Pilots.GetPilotClone(card.id));
                SquadsPage squadsPage = (SquadsPage)Pages.pages[PageName.SquadsPage];
                NavigationService.Navigate(squadsPage);
            }
            else if (isSwappingPilot == true)
            {
                upgradesToDisplay.Clear();
                pilotsToDisplay.Clear();
                previousUpgradeSearchResultIds = "";
                previousPilotSearchResultIds = "";
                searchTextBox.Text = "";
                isSwappingPilot = false;
                isAddingUpgrade = false;
                isAddingPilot = false;
                Pilot newPilot = Pilots.GetPilotClone(card.id);
                newPilot.upgrades = pilotToSwap.upgrades;
                build.AddPilot(newPilot);
                Upgrades.RemoveUnusableUpgrades(build, newPilot.uniquePilotId);
                build.RemovePilot(pilotToSwap.uniquePilotId);
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


        private void UpgradeInfoClicked(object sender, MouseButtonEventArgs e)
        {
            InfoButton hoveredUpgrade = (InfoButton)sender;
            UpdateUpgradeCard(hoveredUpgrade.upgradeId, true);
        }
        private void UpgradeInfoMouseHover(object sender, MouseEventArgs e)
        {
            InfoButton hoveredUpgrade = (InfoButton)sender;
            UpdateUpgradeCard(hoveredUpgrade.upgradeId, true);
        }

        private void PilotMouseHover(object sender, MouseEventArgs e)
        {
            InfoButton hoveredPilot = (InfoButton)sender;
            UpdatePilot(hoveredPilot.uniquePilotId, true);
        }
        private void PilotMouseHoverLeave(object sender, MouseEventArgs e)
        {
            InfoButton hoveredPilot = (InfoButton)sender;
            UpdatePilot(hoveredPilot.uniquePilotId, false);
        }
        /*
        private void ManeuverMouseHover(object sender, MouseEventArgs e)
        {
            ManeuverCard hoveredManeuver = (ManeuverCard)sender;
            UpdatePilot(hoveredManeuver.uniquePilotId, true);
        }
        private void ManeuverMouseHoverLeave(object sender, MouseEventArgs e)
        {
            ManeuverCard hoveredManeuver = (ManeuverCard)sender;
            UpdatePilot(hoveredManeuver.uniquePilotId, false);
        }
        */
        private void ContentScroll(object sender, MouseWheelEventArgs e)
        {
            contentScrollViewer.ScrollToVerticalOffset(contentScrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
