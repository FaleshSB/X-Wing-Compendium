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
    /// Interaction logic for PilotQuizPage.xaml
    /// </summary>
    public partial class PilotQuizPage : Page
    {
        private Pilots pilots;
        private int pilotCardWidth = 292;
        private int pilotCardHeight = 410;
        private Pilot currentRandomPilot;
        private bool isShowingName = false;

        public PilotQuizPage()
        {
            InitializeComponent();
            pilots = new Pilots();

            currentRandomPilot = pilots.GetRandomPilot();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            DisplayCard();
        }

        private void ShowNameClicked(object sender, RoutedEventArgs e)
        {
            if (isShowingName == true)
            {
                currentRandomPilot = pilots.GetRandomPilot();
            }

            isShowingName = !isShowingName;
            DisplayCard();
        }

        private void DisplayCard()
        {
            canvasArea.Children.Clear();

            PilotCard randomPilotCard = currentRandomPilot.GetPilotCard(pilotCardWidth, pilotCardHeight);
            Canvas.SetLeft(randomPilotCard, 760);
            Canvas.SetTop(randomPilotCard, 360);
            canvasArea.Children.Add(randomPilotCard);

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
                blueRectangle.Height = pilotCardHeight * 0.46;
                blueRectangle.Width = pilotCardWidth;
                blueRectangle.Fill = new SolidColorBrush(Color.FromRgb(50, 50, 50));
                blueRectangle.UseLayoutRounding = true;
                Canvas.SetLeft(blueRectangle, 760);
                Canvas.SetTop(blueRectangle, 360);
                canvasArea.Children.Add(blueRectangle);
            }
            else
            {
                showName.Content = "Next Pilot";
            }
            Canvas.SetLeft(showName, 850);
            Canvas.SetTop(showName, 780);
            canvasArea.Children.Add(showName);
        }
    }
}
