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
using System.Windows.Shapes;
using X_Wing_Visual_Builder.Model;
using System.Diagnostics;
using System.Windows.Navigation;

namespace X_Wing_Visual_Builder.View
{
    /// <summary>
    /// Interaction logic for InfoDialogBox.xaml
    /// </summary>
    public partial class InfoDialogBox : Window
    {
        protected Grid pageStructureGrid = new Grid();

        public InfoDialogBox()
        {
            InitializeComponent();
            this.Height = 600;
            this.Width = 360;
            ImageBrush backgroundBrush = new ImageBrush(new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\main_background.png")));
            backgroundBrush.TileMode = TileMode.Tile;
            backgroundBrush.Stretch = Stretch.None;
            backgroundBrush.ViewportUnits = BrushMappingMode.Absolute;
            backgroundBrush.AlignmentX = AlignmentX.Left;
            backgroundBrush.AlignmentY = AlignmentY.Top;
            backgroundBrush.Viewport = new Rect(0, 0, 512, 699);
            Background = backgroundBrush;

            pageStructureGrid.ColumnDefinitions.Add(new ColumnDefinition());
            pageStructureGrid.ColumnDefinitions.Add(new ColumnDefinition());
            

            pageStructureGrid.RowDefinitions.Add(new RowDefinition());

            this.Content = pageStructureGrid;

            
        }

        public void AddUpgradeCard(UpgradeCard upgradeCard)
        {
            this.Width += upgradeCard.Width + 20;
            upgradeCard.Margin = new Thickness(20, 0, 20, 0);
            Grid.SetColumn(upgradeCard, 0);
            Grid.SetRow(upgradeCard, 0);
            pageStructureGrid.Children.Add(upgradeCard);
            pageStructureGrid.ColumnDefinitions[0].Width = GridLength.Auto;
        }

        public void AddUpgradeInfo(Upgrade upgrade)
        {
            TextBlock upgradeInfo = new TextBlock();
            upgradeInfo.Margin = new Thickness(0, 50, 0, 0);
            upgradeInfo.Width = 300;
            upgradeInfo.TextWrapping = TextWrapping.Wrap;
            foreach (string faq in upgrade.faq)
            {
                upgradeInfo.Inlines.Add(faq + "\r\n\r\n");
            }
            Grid.SetColumn(upgradeInfo, 1);
            Grid.SetRow(upgradeInfo, 0);
            pageStructureGrid.Children.Add(upgradeInfo);

            Hyperlink hyperlink = new Hyperlink(new Run("Buy here"));
            hyperlink.NavigateUri = new Uri("http://stackoverflow.com");
            hyperlink.RequestNavigate += new RequestNavigateEventHandler(ClickedLink);

            upgradeInfo.Inlines.Add(hyperlink);
        }

        private void ClickedLink(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hyperlink = (Hyperlink)sender;
            System.Diagnostics.Process.Start(hyperlink.NavigateUri.ToString());
        }
    }
}
