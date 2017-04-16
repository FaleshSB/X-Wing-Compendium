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
using System.Threading;

namespace X_Wing_Visual_Builder.View
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : DefaultPage
    {
        private System.Drawing.Size buttonSize = new System.Drawing.Size(168, 36);

        private Dictionary<string, Image> buttons = new Dictionary<string, Image>();
        private string manageSquadsButtonName = "manage_squads";
        private string browseCardsButtonName = "browse_cards";
        private string calculateStatsButtonName = "calculate_stats";
        private string quizButtonName = "quiz";
        private Canvas contentCanvas = new Canvas();

        private AlignableWrapPanel contentWrapPanel = new AlignableWrapPanel();

        public StartPage()
        {
            contentWrapPanel.Name = "contentWrapPanel";
            contentWrapPanel.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentScrollViewer.Content = contentWrapPanel;

            contentCanvas.Name = "contentCanvas";
            contentCanvas.Height = 900;
            contentCanvas.Background = new SolidColorBrush(Colors.Black);
            contentWrapPanel.Children.Add(contentCanvas);
            InitializeComponent();
            buttons.Add(manageSquadsButtonName, CreateButton(manageSquadsButtonName));
            buttons.Add(browseCardsButtonName, CreateButton(browseCardsButtonName));
            buttons.Add(calculateStatsButtonName, CreateButton(calculateStatsButtonName));
            buttons.Add(quizButtonName, CreateButton(quizButtonName));
            contentCanvas.Width = buttons[quizButtonName].Width;
        }

        

        private Image CreateButton(string buttonName)
        {
            Image button = new Image();
            button.Name = buttonName;
            button.Source = ImageResizer.ResizeImage(System.Drawing.Image.FromFile(@"D:\Documents\Game Stuff\X-Wing\buttons\" + buttonName + ".png"), buttonSize);
            button.Width = buttonSize.Width;
            button.Height = buttonSize.Height;
            button.UseLayoutRounding = true;
            button.MouseEnter += new MouseEventHandler(ButtonHover);
            button.MouseLeave += new MouseEventHandler(ButtonStopHover);
            button.MouseDown += new MouseButtonEventHandler(ButtonClicked);
            RenderOptions.SetBitmapScalingMode(button, BitmapScalingMode.HighQuality);

            return button;
        }

        private void ButtonHover(object sender, MouseEventArgs e)
        {
            Image button = (Image)sender;
            buttons[button.Name].Source = ImageResizer.ResizeImage(System.Drawing.Image.FromFile(@"D:\Documents\Game Stuff\X-Wing\buttons\" + button.Name + "_hover.png"), buttonSize);
        }
        private void ButtonStopHover(object sender, MouseEventArgs e)
        {
            Image button = (Image)sender;
            buttons[button.Name].Source = ImageResizer.ResizeImage(System.Drawing.Image.FromFile(@"D:\Documents\Game Stuff\X-Wing\buttons\" + button.Name + ".png"), buttonSize);
        }
        private async void ButtonClicked(object sender, MouseButtonEventArgs e)
        {
            Image button = (Image)sender;
            buttons[button.Name].Source = ImageResizer.ResizeImage(System.Drawing.Image.FromFile(@"D:\Documents\Game Stuff\X-Wing\buttons\" + button.Name + "_pressed.png"), buttonSize);
            await Task.Delay(120);

            if(button.Name == quizButtonName)
            {
                NavigationService.Navigate((QuizPage)Pages.pages[PageName.Quiz]);
            }
            else if (button.Name == manageSquadsButtonName)
            {
                NavigationService.Navigate((SquadsPage)Pages.pages[PageName.Squads]);
            }
            else if (button.Name == browseCardsButtonName)
            {
                NavigationService.Navigate((BrowseCardsPage)Pages.pages[PageName.BrowseCards]);
            }
            else if (button.Name == calculateStatsButtonName)
            {
                NavigationService.Navigate((StatsPage)Pages.pages[PageName.CalculateStats]);
            }
        }

        protected override void DisplayContent()
        {
            contentCanvas.Children.Clear();
            
            Canvas.SetLeft(buttons[manageSquadsButtonName], 0);
            Canvas.SetTop(buttons[manageSquadsButtonName], 400);
            contentCanvas.Children.Add(buttons[manageSquadsButtonName]);

            Canvas.SetLeft(buttons[browseCardsButtonName], 0);
            Canvas.SetTop(buttons[browseCardsButtonName], 450);
            contentCanvas.Children.Add(buttons[browseCardsButtonName]);

            Canvas.SetLeft(buttons[calculateStatsButtonName], 0);
            Canvas.SetTop(buttons[calculateStatsButtonName], 500);
            contentCanvas.Children.Add(buttons[calculateStatsButtonName]);

            Canvas.SetLeft(buttons[quizButtonName], 0);
            Canvas.SetTop(buttons[quizButtonName], 550);
            contentCanvas.Children.Add(buttons[quizButtonName]);
        }
    }
}
