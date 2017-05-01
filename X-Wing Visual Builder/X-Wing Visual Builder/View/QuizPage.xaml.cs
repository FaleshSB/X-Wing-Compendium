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
        //private int upgradeCardWidth = 166;
        //private int upgradeCardHeight = 255;
        private Pilot currentRandomPilot;
        private bool isShowingName = false;
        private Canvas contentCanvas = new Canvas();
        protected AlignableWrapPanel contentWrapPanel = new AlignableWrapPanel();

        public QuizPage()
        {
            contentWrapPanel.Name = "contentWrapPanel";
            contentWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentScrollViewer.Content = contentWrapPanel;

            Pages.pages[PageName.Quiz] = this;

            contentCanvas.Name = "contentCanvas";
            contentWrapPanel.Children.Add(contentCanvas);
            InitializeComponent();
            contentCanvas.Width = 1800;
            contentCanvas.Height = 1000;
            currentRandomPilot = Pilots.GetRandomPilot();
        }
        
        private void ShowNameClicked(object sender, RoutedEventArgs e)
        {
            if (isShowingName == true)
            {
                currentRandomPilot = Pilots.GetRandomPilot();
            }

            isShowingName = !isShowingName;
            DisplayContent();
        }
        
        protected override void DisplayContent()
        {
            contentCanvas.Children.Clear();
            
            CardCanvas randomPilotCard = currentRandomPilot.GetCanvas(pilotCardWidth, pilotCardHeight, new Thickness(0,0,0,0), this);
            Canvas.SetLeft(randomPilotCard, 760);
            Canvas.SetTop(randomPilotCard, 360);
            contentCanvas.Children.Add(randomPilotCard);

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

                Rectangle blueRectangle = new Rectangle();
                blueRectangle.Height = pilotCardHeight * 0.46;
                blueRectangle.Width = pilotCardWidth;
                blueRectangle.Fill = new SolidColorBrush(Color.FromRgb(50, 50, 50));
                blueRectangle.UseLayoutRounding = true;
                Canvas.SetLeft(blueRectangle, 760);
                Canvas.SetTop(blueRectangle, 360);
                contentCanvas.Children.Add(blueRectangle);
            }
            else
            {
                showName.Content = "Next Pilot";
            }
            Canvas.SetLeft(showName, 850);
            Canvas.SetTop(showName, 780);
            contentCanvas.Children.Add(showName);
        }
    }
}
 