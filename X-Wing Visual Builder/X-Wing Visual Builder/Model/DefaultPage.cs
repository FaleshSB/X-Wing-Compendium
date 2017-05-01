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
        protected AlignableWrapPanel manuNavigationWrapPanel = new AlignableWrapPanel();
        protected AlignableWrapPanel topToolsWrapPanel = new AlignableWrapPanel();
        protected Canvas bottomBarCanvas = new Canvas();
        protected Grid pageStructureGrid = new Grid();
        protected ScrollViewer contentScrollViewer = new ScrollViewer();

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

            manuNavigationWrapPanel.Name = "manuNavigationCanvas";
            Grid.SetColumn(manuNavigationWrapPanel, 0);
            Grid.SetRow(manuNavigationWrapPanel, 0);
            manuNavigationWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            pageStructureGrid.Children.Add(manuNavigationWrapPanel);

            topToolsWrapPanel.Name = "topToolsCanvas";
            Grid.SetColumn(topToolsWrapPanel, 0);
            Grid.SetRow(topToolsWrapPanel, 1);
            topToolsWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            pageStructureGrid.Children.Add(topToolsWrapPanel);

            contentScrollViewer.Name = "contentScrollViewer";
            contentScrollViewer.MaxHeight = 999;
            contentScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Grid.SetColumn(contentScrollViewer, 0);
            Grid.SetRow(contentScrollViewer, 2);
            pageStructureGrid.Children.Add(contentScrollViewer);
            
            bottomBarCanvas.Name = "bottomBarCanvas";
            Grid.SetColumn(bottomBarCanvas, 0);
            Grid.SetRow(bottomBarCanvas, 3);
            pageStructureGrid.Children.Add(bottomBarCanvas);

            /*
            Button closeWindowButton = new Button();
            closeWindowButton.Name = "closeWindowButton";
            closeWindowButton.Width = 40;
            closeWindowButton.Height = 40;
            closeWindowButton.FontSize = Opt.ApResMod(16);
            closeWindowButton.FontWeight = FontWeights.Bold;
            closeWindowButton.Click += new RoutedEventHandler(ExitButton);
            closeWindowButton.UseLayoutRounding = true;
            closeWindowButton.Content = "X";
            Canvas.SetRight(closeWindowButton, 0);
            Canvas.SetTop(closeWindowButton, 0);
            manuNavigationCanvas.Children.Add(closeWindowButton);
            */

            Content = pageStructureGrid;

            this.Loaded += PageLoaded;
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

        public void ContentScroll(object sender, MouseWheelEventArgs e)
        {
            contentScrollViewer.ScrollToVerticalOffset(contentScrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}