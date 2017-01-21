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

namespace X_Wing_Visual_Builder.View
{
    /// <summary>
    /// Interaction logic for Build.xaml
    /// </summary>
    public partial class Build : Page
    {
        public Build()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            AllignCards();
        }

        private void AllignCards()
        {
            /*
            //Shape one = new Rectangle() { Fill = Brushes.Blue, Height = 5, Width = 5, RadiusX = 4, RadiusY = 4 };
            //Shape two = new Rectangle() { Fill = Brushes.Blue, Height = 5, Width = 5, RadiusX = 4, RadiusY = 4 };
            //Shape three = new Rectangle() { Fill = Brushes.Blue, Height = 5, Width = 5, RadiusX = 4, RadiusY = 4 };
            List<Shape> pilotCards = new List<Shape>();
            //pilotCards.Add(one);
            //pilotCards.Add(two);
            //pilotCards.Add(three);
            pilotCards.Add(new System.Windows.Shapes.Rectangle() { Fill = System.Windows.Media.Brushes.Blue, Height = 45, Width = 45, RadiusX = 4, RadiusY = 4 });


            //System.Drawing.Image image = System.Drawing.Image.FromFile(@"D:\Documents\Game Stuff\Board Games\X-Wing\Pilots\BlueAce.png");

            for (int i = 0; i < pilotCards.Count; i++)
            {
                double devidedWidth = canvasArea.ActualWidth / pilotCards.Count;
                double devidedHeight = canvasArea.ActualHeight / pilotCards.Count;
                double left = ((devidedWidth * i) + (devidedWidth / 2)) - (pilotCards.ElementAt(i).ActualWidth / 2);
                double height = ((devidedHeight * i) + (devidedHeight / 2)) - (pilotCards.ElementAt(i).ActualHeight / 2);

                Canvas.SetLeft(pilotCards.ElementAt(i), left);
                Canvas.SetTop(pilotCards.ElementAt(i), height);
                canvasArea.Children.Add(pilotCards.ElementAt(i));
            }
            */

            List<Image> pilotCards = new List<Image>();

            BitmapImage webImage = new BitmapImage(new Uri("D:\\Documents\\Game Stuff\\Board Games\\X-Wing\\Pilots\\BlueAce.png"));
            Image pilotCard = new Image();
            pilotCard.Source = webImage;
            pilotCard.Height = 426;
            pilotCard.Width = 300;
            pilotCards.Add(pilotCard);

            webImage = new BitmapImage(new Uri("D:\\Documents\\Game Stuff\\Board Games\\X-Wing\\Pilots\\BlueAce.png"));
            pilotCard = new Image();
            pilotCard.Source = webImage;
            pilotCard.Height = 426;
            pilotCard.Width = 300;
            pilotCards.Add(pilotCard);

            webImage = new BitmapImage(new Uri("D:\\Documents\\Game Stuff\\Board Games\\X-Wing\\Pilots\\BlueAce.png"));
            pilotCard = new Image();
            pilotCard.Source = webImage;
            pilotCard.Height = 426;
            pilotCard.Width = 300;
            pilotCards.Add(pilotCard);

            webImage = new BitmapImage(new Uri("D:\\Documents\\Game Stuff\\Board Games\\X-Wing\\Pilots\\BlueAce.png"));
            pilotCard = new Image();
            pilotCard.Source = webImage;
            pilotCard.Height = 426;
            pilotCard.Width = 300;
            pilotCards.Add(pilotCard);


            int pilotCardWidth = 300;
            int pilotCardHeight = 426;

            double height = 50;
            for (int i = 0; i < pilotCards.Count; i++)
            {
                double devidedWidth = canvasArea.ActualWidth / pilotCards.Count;
                double devidedHeight = canvasArea.ActualHeight / pilotCards.Count;
                double cardWidth = pilotCards.ElementAt(i).ActualWidth;
                double left = ((devidedWidth * i) + (devidedWidth / 2)) - (pilotCardWidth / 2);
                

                Canvas.SetLeft(pilotCards.ElementAt(i), left);
                Canvas.SetTop(pilotCards.ElementAt(i), height);
                canvasArea.Children.Add(pilotCards.ElementAt(i));
            }
        }
    }
}
