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
    public partial class UpgradeCardsPage : DefaultPage
    {
        private Build build;
        private int upgradeCardWidth = 166;
        private int upgradeCardHeight = 255;

        public UpgradeCardsPage(object build, object upgrades)
        {
            this.build = (Build)build;
            InitializeComponent();
        }

        new private void DisplayContent()
        {
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
            while(true)
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
    }
}