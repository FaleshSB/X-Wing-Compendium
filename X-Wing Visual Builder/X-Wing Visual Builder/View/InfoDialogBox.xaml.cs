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
        private int upgradeCardWidth = 166;
        private int upgradeCardHeight = 255;
        private int pilotCardWidth = 292;
        private int pilotCardHeight = 410;

        public InfoDialogBox()
        {
            string baseLocation = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string filteredLocation = System.IO.Path.GetDirectoryName(baseLocation).Replace("file:\\", "") + "\\Misc\\";
            Uri iconUri = new Uri(filteredLocation + "RebelIcon.ico");
            Icon = BitmapFrame.Create(iconUri);

            InitializeComponent();
            this.Height = 600;
            this.Width = 600;
            ImageBrush backgroundBrush = new ImageBrush(new BitmapImage(new Uri(filteredLocation + "main_background.png")));
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

        public void AddCard(Card cardInfo, bool isUpgrade)
        {
            double cardWidth;
            double cardHeight;
            if (isUpgrade)
            {
                cardWidth = upgradeCardWidth;
                cardHeight = upgradeCardHeight;
            }
            else
            {
                cardWidth = pilotCardWidth;
                cardHeight = pilotCardHeight;
            }
            Width += cardWidth + 40;
            CardCanvas upgradeCanvas = cardInfo.GetCanvas(cardWidth, cardHeight, new Thickness(20, 2, 2, 2));
            upgradeCanvas.HideInfoButton();
            Grid.SetColumn(upgradeCanvas, 0);
            Grid.SetRow(upgradeCanvas, 0);
            pageStructureGrid.Children.Add(upgradeCanvas);
            pageStructureGrid.ColumnDefinitions[0].Width = GridLength.Auto;


            ScrollViewer upgradeInfoScrollViewer = new ScrollViewer();
            Grid.SetColumn(upgradeInfoScrollViewer, 1);
            Grid.SetRow(upgradeInfoScrollViewer, 0);
            pageStructureGrid.Children.Add(upgradeInfoScrollViewer);

            AlignableWrapPanel upgradeDetails = new AlignableWrapPanel();
            upgradeDetails.VerticalAlignment = VerticalAlignment.Center;
            upgradeDetails.Margin = ScaledThicknessFactory.GetThickness(20, 0, 20, 0);
            upgradeInfoScrollViewer.Content = upgradeDetails;


            TextBlock upgradeInfo = new TextBlock();
            upgradeInfo.FontSize = Opt.ApResMod(14);
            upgradeInfo.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            upgradeInfo.Padding = ScaledThicknessFactory.GetThickness(20);
            upgradeInfo.TextWrapping = TextWrapping.Wrap;
            upgradeInfo.VerticalAlignment = VerticalAlignment.Center;
            upgradeInfo.Inlines.Add(new Run() { Text = "Availible In", FontWeight = FontWeights.Bold, FontSize = 18 });
            upgradeInfo.Inlines.Add(new LineBreak());
            foreach (KeyValuePair<ExpansionType, int> expansionType in cardInfo.inExpansion)
            {
                upgradeInfo.Inlines.Add(new LineBreak());
                upgradeInfo.Inlines.Add(Expansions.expansions[expansionType.Key].name + " (" + expansionType.Value.ToString() + ")");
            }
            if(cardInfo.faq.Count > 0)
            {
                upgradeInfo.Inlines.Add(new LineBreak());
                upgradeInfo.Inlines.Add(new LineBreak());
                upgradeInfo.Inlines.Add(new Run() { Text = "FAQ", FontWeight = FontWeights.Bold, FontSize = 18 });
                foreach (string faq in cardInfo.faq)
                {
                    upgradeInfo.Inlines.Add(new LineBreak());
                    upgradeInfo.Inlines.Add(new LineBreak());
                    string[] boldSplit = faq.Split('~');
                    int i = 0;
                    foreach (string boldSplitElement in boldSplit)
                    {
                        if(i % 2 != 0)
                        {
                            upgradeInfo.Inlines.Add(new Run() { Text = boldSplitElement, FontWeight = FontWeights.Bold });
                        }
                        else
                        {
                            string[] headingSplit = boldSplitElement.Split('{');
                            int y = 0;
                            foreach (string headingSplitElement in headingSplit)
                            {
                                if (y % 2 != 0)
                                {
                                    upgradeInfo.Inlines.Add(new Run() { Text = headingSplitElement, FontWeight = FontWeights.Bold, FontSize = 18 });
                                }
                                else
                                {
                                    string[] manouverSplit = headingSplitElement.Split('*');
                                    int z = 0;
                                    foreach (string manouverSplitElement in manouverSplit)
                                    {
                                        if (z % 2 != 0)
                                        {
                                            string manouver = "";
                                            switch (manouverSplitElement.ToLower())
                                            {
                                                case "straight":
                                                    manouver = "8";
                                                    break;
                                                case "left turn":
                                                    manouver = "4";
                                                    break;
                                                case "right turn":
                                                    manouver = "6";
                                                    break;
                                                case "k turn":
                                                    manouver = "2";
                                                    break;
                                                case "hit":
                                                    manouver = "d";
                                                    break;
                                                case "crit":
                                                    manouver = "c";
                                                    break;
                                                case "evade":
                                                    manouver = "e";
                                                    break;
                                                case "focus":
                                                    manouver = "f";
                                                    break;
                                                case "turret":
                                                    manouver = "U";
                                                    break;
                                                case "elite":
                                                    manouver = "E";
                                                    break;
                                            }
                                            upgradeInfo.Inlines.Add(new Run() { Text = manouver, FontFamily = new FontFamily("x-wing-symbols") });
                                        }
                                        else
                                        {
                                            upgradeInfo.Inlines.Add(manouverSplitElement);
                                        }
                                        z++;
                                    }
                                }
                                y++;
                            }
                        }
                        i++;
                    }
                }
            }
            

            /*
             * *Hit* *K Turn* *Left Turn*
             * ~bold~ 
             * {Title}
            

            Hyperlink hyperlink = new Hyperlink(new Run("Buy here"));
            hyperlink.NavigateUri = new Uri("http://stackoverflow.com");
            hyperlink.RequestNavigate += new RequestNavigateEventHandler(ClickedLink);

            upgradeInfo.Inlines.Add(hyperlink);*/
            upgradeDetails.Children.Add(upgradeInfo);
        }

        private void ClickedLink(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hyperlink = (Hyperlink)sender;
            System.Diagnostics.Process.Start(hyperlink.NavigateUri.ToString());
        }
    }
}
