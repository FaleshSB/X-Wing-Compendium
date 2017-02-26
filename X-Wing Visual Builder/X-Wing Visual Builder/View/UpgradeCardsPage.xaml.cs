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
    public partial class UpgradeCardsPage : Page
    {
        private Build build;
        private Upgrades upgrades;
        private int upgradeCardWidth = 166;
        private int upgradeCardHeight = 255;

        public UpgradeCardsPage(object build, object upgrades)
        {
            this.upgrades = (Upgrades)upgrades;
            this.build = (Build)build;
            InitializeComponent();
        }

        private void DisplayCards()
        {
            canvasArea.Children.Clear();
            List<List<Upgrade>> upgradesToDisplay = new List<List<Upgrade>>();
            //upgradesToDisplay.Add(upgrades.GetUpgrades(UpgradeType.Torpedo, UpgradeSort.Cost));
            //upgradesToDisplay.Add(upgrades.GetUpgrades(UpgradeType.Missile, UpgradeSort.Cost));
            upgradesToDisplay.Add(upgrades.GetUpgrades(UpgradeType.Elite, UpgradeSort.Cost, Faction.Rebel, ShipSize.Small));
            upgradesToDisplay.Add(upgrades.GetUpgrades(UpgradeType.Astromech, UpgradeSort.Cost, Faction.Rebel, ShipSize.Small));
            double currentHeightOffset = -30;
            double currentLeftOffset = 20;
            double spacersGap = 4;
            int currentTypeId = 0;
            int currentUpgradeId = 0;
            int currentRowNumber = 0;
            while(true)
            {
                if (currentTypeId >= upgradesToDisplay.Count)
                {
                    break;
                }
                else if (currentUpgradeId < upgradesToDisplay.ElementAt(currentTypeId).Count)
                {
                    UpgradeCard upgradeCard = upgradesToDisplay.ElementAt(currentTypeId).ElementAt(currentUpgradeId).GetUpgradeCard(Options.ApplyResolutionMultiplier(upgradeCardWidth), Options.ApplyResolutionMultiplier(upgradeCardHeight));
                    Canvas.SetLeft(upgradeCard, currentLeftOffset + spacersGap);
                    Canvas.SetTop(upgradeCard, currentHeightOffset + 40);
                    canvasArea.Children.Add(upgradeCard);
                    currentUpgradeId++;
                    currentLeftOffset += spacersGap + Options.ApplyResolutionMultiplier(upgradeCardWidth);
                    currentRowNumber++;
                }
                else
                {
                    currentTypeId++;
                    currentUpgradeId = 0;
                }
                
                if (currentRowNumber > 10)
                {
                    currentHeightOffset += spacersGap + Options.ApplyResolutionMultiplier(upgradeCardHeight);
                    currentLeftOffset = 20;
                    currentRowNumber = 0;
                }
            }
            canvasArea.Height = currentHeightOffset + spacersGap + Options.ApplyResolutionMultiplier(upgradeCardHeight) + 80;
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            DisplayCards();
        }
    }
}