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
    /// Interaction logic for UpgradeQuizPage.xaml
    /// </summary>
    public partial class UpgradeQuizPage : DefaultPage
    {
        private int upgradeCardWidth = 233;
        private int upgradeCardHeight = 359;
        private Upgrade currentRandomUpgrade;
        private bool isShowingName = false;
        private Canvas contentCanvas = new Canvas();
        protected AlignableWrapPanel contentWrapPanel = new AlignableWrapPanel();

        public UpgradeQuizPage()
        {
            contentWrapPanel.Name = "contentWrapPanel";
            contentWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentScrollViewer.Content = contentWrapPanel;

            Pages.pages[PageName.UpgradeQuiz] = this;
            contentCanvas.Name = "contentCanvas";
            contentWrapPanel.Children.Add(contentCanvas);
            InitializeComponent();

            currentRandomUpgrade = Upgrades.GetRandomUpgrade();
        }

        private void ShowNameClicked(object sender, RoutedEventArgs e)
        {
            if (isShowingName == true)
            {
                currentRandomUpgrade = Upgrades.GetRandomUpgrade();
            }

            isShowingName = !isShowingName;
            DisplayCard();
        }

        private void DisplayCard()
        {
            contentCanvas.Children.Clear();

            UpgradeCard randomUpgradeCard = currentRandomUpgrade.GetUpgradeCard(upgradeCardWidth, upgradeCardHeight);
            Canvas.SetLeft(randomUpgradeCard, 800);
            Canvas.SetTop(randomUpgradeCard, 400);
            contentCanvas.Children.Add(randomUpgradeCard);

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
                blueRectangle.Height = upgradeCardHeight * 0.42;
                blueRectangle.Width = upgradeCardWidth;
                blueRectangle.Fill = new SolidColorBrush(Color.FromRgb(50, 50, 50));
                blueRectangle.UseLayoutRounding = true;
                Canvas.SetLeft(blueRectangle, 800);
                Canvas.SetTop(blueRectangle, 400);
                contentCanvas.Children.Add(blueRectangle);
            }
            else
            {
                showName.Content = "Next Card";
            }
            Canvas.SetLeft(showName, 850);
            Canvas.SetTop(showName, 780);
            contentCanvas.Children.Add(showName);
        }
    }
}
