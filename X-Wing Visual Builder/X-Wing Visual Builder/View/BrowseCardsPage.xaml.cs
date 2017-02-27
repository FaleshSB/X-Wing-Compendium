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
    /// Interaction logic for BrowseCardsPage.xaml
    /// </summary>
    public partial class BrowseCardsPage : DefaultPage
    {
        Dictionary<UpgradeType, List<Upgrade>> upgradesToDisplay = new Dictionary<UpgradeType, List<Upgrade>>();
        Dictionary<ShipType, List<Pilot>> pilotsToDisplay = new Dictionary<ShipType, List<Pilot>>();
        private int upgradeCardWidth = 166;
        private int upgradeCardHeight = 255;
        private int pilotCardWidth = 292;
        private int pilotCardHeight = 410;
        private bool isUpgradeChecked = false;
        private bool isSearchDescriptionChecked = false;
        private string previousUpgradeSearchResultIds = "";
        private string previousPilotSearchResultIds = "";

        public BrowseCardsPage()
        {
            InitializeComponent();
            cardScrollViewer.Height = 990;
            searchTextBox.Focus();
            SetIsUpgradeChecked();
            SetIsSearchDescriptionChecked();
        }

        private void UpdateContents()
        {
            if (searchTextBox.Text.Length > 2)
            {
                pilotsToDisplay.Clear();
                upgradesToDisplay.Clear();
                if (isUpgradeChecked)
                {
                    string currentUpgradeSearchResultIds = "";
                    foreach (KeyValuePair<int, Upgrade> entry in Upgrades.upgrades)
                    {
                        bool hasFoundAllWords = true;
                        string[] searchWords = searchTextBox.Text.Split(' ');
                        foreach(string searchWord in searchWords)
                        {
                            if((isSearchDescriptionChecked == false && entry.Value.name.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) < 0)
                            || (isSearchDescriptionChecked == true && entry.Value.description.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) < 0))
                            {
                                hasFoundAllWords = false;
                                break;
                            }
                        }

                        if (hasFoundAllWords)
                        {
                            if (upgradesToDisplay.ContainsKey(entry.Value.upgradeType) == false)
                            {
                                upgradesToDisplay[entry.Value.upgradeType] = new List<Upgrade>();
                            }
                            upgradesToDisplay[entry.Value.upgradeType].Add(entry.Value);
                            currentUpgradeSearchResultIds += entry.Value.id.ToString();
                        }
                    }

                    if(previousUpgradeSearchResultIds != currentUpgradeSearchResultIds)
                    {
                        previousUpgradeSearchResultIds = currentUpgradeSearchResultIds;
                        DisplayUpgradeCards();
                    }
                }
                else
                {
                    string currentPilotSearchResultIds = "";
                    foreach (KeyValuePair<int, Pilot> entry in Pilots.pilots)
                    {
                        bool hasFoundAllWords = true;
                        string[] searchWords = searchTextBox.Text.Split(' ');
                        foreach (string searchWord in searchWords)
                        {
                            if ((isSearchDescriptionChecked == false && entry.Value.name.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) < 0 
                            && isSearchDescriptionChecked == false && entry.Value.ship.name.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) < 0)
                            || ((isSearchDescriptionChecked == true && entry.Value.description.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) < 0) || entry.Value.hasAbility == false))
                            {
                                hasFoundAllWords = false;
                                break;
                            }
                        }

                        if (hasFoundAllWords)
                        {
                            if (pilotsToDisplay.ContainsKey(entry.Value.ship.shipType) == false)
                            {
                                pilotsToDisplay[entry.Value.ship.shipType] = new List<Pilot>();
                            }
                            pilotsToDisplay[entry.Value.ship.shipType].Add(entry.Value);
                            currentPilotSearchResultIds += entry.Value.id.ToString();
                        }
                    }

                    if (previousPilotSearchResultIds != currentPilotSearchResultIds)
                    {
                        previousPilotSearchResultIds = currentPilotSearchResultIds;
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

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        

        private void DisplayUpgradeCards()
        {
            canvasArea.Children.Clear();
            double currentHeightOffset = -30;
            double currentLeftOffset = 20;
            double spacersGap = 4;
            int currentRowNumber = 0;

            foreach(KeyValuePair<UpgradeType, List<Upgrade>> upgradeList in upgradesToDisplay)
            {
                foreach(Upgrade upgrade in upgradeList.Value)
                {
                    UpgradeCard upgradeCard = upgrade.GetUpgradeCard(Options.ApplyResolutionMultiplier(upgradeCardWidth), Options.ApplyResolutionMultiplier(upgradeCardHeight));
                    Canvas.SetLeft(upgradeCard, currentLeftOffset + spacersGap);
                    Canvas.SetTop(upgradeCard, currentHeightOffset + 40);
                    canvasArea.Children.Add(upgradeCard);
                    currentLeftOffset += spacersGap + Options.ApplyResolutionMultiplier(upgradeCardWidth);
                    currentRowNumber++;

                    if (currentRowNumber > 10)
                    {
                        currentHeightOffset += spacersGap + Options.ApplyResolutionMultiplier(upgradeCardHeight);
                        currentLeftOffset = 20;
                        currentRowNumber = 0;
                    }
                }
            }
            canvasArea.Height = currentHeightOffset + spacersGap + Options.ApplyResolutionMultiplier(upgradeCardHeight) + 80;
        }

        private void DisplayPilotCards()
        {
            canvasArea.Children.Clear();
            double currentHeightOffset = -30;
            double currentLeftOffset = 20;
            double spacersGap = 4;
            int currentRowNumber = 0;
            foreach (KeyValuePair<ShipType, List<Pilot>> pilotList in pilotsToDisplay)
            {
                foreach (Pilot pilot in pilotList.Value)
                {
                    PilotCard pilotCard = pilot.GetPilotCard(Options.ApplyResolutionMultiplier(pilotCardWidth), Options.ApplyResolutionMultiplier(pilotCardHeight));
                    Canvas.SetLeft(pilotCard, currentLeftOffset + spacersGap);
                    Canvas.SetTop(pilotCard, currentHeightOffset + 40);
                    canvasArea.Children.Add(pilotCard);
                    currentLeftOffset += spacersGap + Options.ApplyResolutionMultiplier(pilotCardWidth);
                    currentRowNumber++;

                    if (currentRowNumber > 5)
                    {
                        currentHeightOffset += spacersGap + Options.ApplyResolutionMultiplier(pilotCardHeight);
                        currentLeftOffset = 20;
                        currentRowNumber = 0;
                    }
                }
            }
            canvasArea.Height = currentHeightOffset + spacersGap + Options.ApplyResolutionMultiplier(pilotCardHeight) + 80;
        }
    }
}
