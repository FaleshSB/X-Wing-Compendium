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

namespace X_Wing_Visual_Builder.Model
{
    public class DefaultPage : Page
    {
        protected Canvas manuNavigationCanvas = new Canvas();
        protected Canvas topToolsCanvas = new Canvas();
        protected Canvas contentCanvas = new Canvas();
        protected Canvas bottomBarCanvas = new Canvas();
        protected Grid pageStructureGrid = new Grid();

        public DefaultPage()
        {
            ImageBrush backgroundBrush = new ImageBrush(new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\main_background.png")));
            backgroundBrush.TileMode = TileMode.Tile;
            backgroundBrush.Stretch = Stretch.None;
            backgroundBrush.ViewportUnits = BrushMappingMode.Absolute;
            backgroundBrush.AlignmentX = AlignmentX.Left;
            backgroundBrush.AlignmentY = AlignmentY.Top;
            backgroundBrush.Viewport = new Rect(0, 0, 512, 699);
            Background = backgroundBrush;
            
            pageStructureGrid.ColumnDefinitions.Add(new ColumnDefinition());

            RowDefinition pageStructureGridRowOne = new RowDefinition();
            pageStructureGridRowOne.Height = new GridLength(1, GridUnitType.Auto);
            RowDefinition pageStructureGridRowTwo = new RowDefinition();
            pageStructureGridRowTwo.Height = new GridLength(1, GridUnitType.Auto);
            RowDefinition pageStructureGridRowThree = new RowDefinition();
            pageStructureGridRowThree.Height = new GridLength(1, GridUnitType.Auto);
            RowDefinition pageStructureGridRowFour = new RowDefinition();
            pageStructureGridRowFour.Height = new GridLength(1, GridUnitType.Auto);

            pageStructureGrid.RowDefinitions.Add(pageStructureGridRowOne);
            pageStructureGrid.RowDefinitions.Add(pageStructureGridRowTwo);
            pageStructureGrid.RowDefinitions.Add(pageStructureGridRowThree);
            pageStructureGrid.RowDefinitions.Add(pageStructureGridRowFour);

            manuNavigationCanvas.Name = "manuNavigationCanvas";
            Grid.SetColumn(manuNavigationCanvas, 0);
            Grid.SetRow(manuNavigationCanvas, 0);
            pageStructureGrid.Children.Add(manuNavigationCanvas);

            topToolsCanvas.Name = "topToolsCanvas";
            Grid.SetColumn(topToolsCanvas, 0);
            Grid.SetRow(topToolsCanvas, 1);
            pageStructureGrid.Children.Add(topToolsCanvas);

            contentCanvas.Name = "contentCanvas";
            Grid.SetColumn(contentCanvas, 0);
            Grid.SetRow(contentCanvas, 2);
            pageStructureGrid.Children.Add(contentCanvas);

            bottomBarCanvas.Name = "bottomBarCanvas";
            Grid.SetColumn(bottomBarCanvas, 0);
            Grid.SetRow(bottomBarCanvas, 3);
            pageStructureGrid.Children.Add(bottomBarCanvas);

            
            Button browseCards = new Button();
            browseCards.Name = "browseCards";
            browseCards.Width = 40;
            browseCards.Height = 40;
            browseCards.FontSize = 16;
            browseCards.FontWeight = FontWeights.Bold;
            browseCards.Click += new RoutedEventHandler(ExitButton);
            browseCards.UseLayoutRounding = true;
            browseCards.Content = "X";
            Canvas.SetRight(browseCards, 0);
            Canvas.SetTop(browseCards, 0);
            manuNavigationCanvas.Children.Add(browseCards);


            Content = pageStructureGrid;
        }

        protected void ExitButton(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(Window.GetWindow(this));
        }

        protected void PageLoaded(object sender, RoutedEventArgs e)
        {
            DisplayContent();
        }

        protected virtual void DisplayContent()
        {

        }
    }
}