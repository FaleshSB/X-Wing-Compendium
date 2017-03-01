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
        private string yourCollectionButtonName = "your_collection";
        private string calculateStatsButtonName = "calculate_stats";
        private string upgradeQuizPageButtonName = "upgrade_quiz";
        private string pilotQuizPageButtonName = "pilot_quiz";
        private string maneuverButtonName = "maneuver_quiz";

        public StartPage()
        {
            InitializeComponent();
            buttons.Add(manageSquadsButtonName, CreateButton(manageSquadsButtonName));
            buttons.Add(yourCollectionButtonName, CreateButton(yourCollectionButtonName));
            buttons.Add(calculateStatsButtonName, CreateButton(calculateStatsButtonName));
            buttons.Add(upgradeQuizPageButtonName, CreateButton(upgradeQuizPageButtonName));
            buttons.Add(pilotQuizPageButtonName, CreateButton(pilotQuizPageButtonName));
            buttons.Add(maneuverButtonName, CreateButton(maneuverButtonName));
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

            if(button.Name == upgradeQuizPageButtonName)
            {
                NavigationService.Navigate((UpgradeQuizPage)Pages.pages[PageName.UpgradeQuiz]);
            }
            else if(button.Name == pilotQuizPageButtonName)
            {
                NavigationService.Navigate((PilotQuizPage)Pages.pages[PageName.PilotQuiz]);
            }
            else if (button.Name == manageSquadsButtonName)
            {
                NavigationService.Navigate((SquadsPage)Pages.pages[PageName.SquadsPage]);
            }
            else if (button.Name == yourCollectionButtonName)
            {
                NavigationService.Navigate((BrowseCardsPage)Pages.pages[PageName.BrowseCards]);
            }
        }

        protected override void DisplayContent()
        {
            contentCanvas.Children.Clear();

            Canvas.SetLeft(buttons[manageSquadsButtonName], 800);
            Canvas.SetTop(buttons[manageSquadsButtonName], 400);
            contentCanvas.Children.Add(buttons[manageSquadsButtonName]);

            Canvas.SetLeft(buttons[yourCollectionButtonName], 800);
            Canvas.SetTop(buttons[yourCollectionButtonName], 450);
            contentCanvas.Children.Add(buttons[yourCollectionButtonName]);

            Canvas.SetLeft(buttons[calculateStatsButtonName], 800);
            Canvas.SetTop(buttons[calculateStatsButtonName], 500);
            contentCanvas.Children.Add(buttons[calculateStatsButtonName]);

            Canvas.SetLeft(buttons[upgradeQuizPageButtonName], 800);
            Canvas.SetTop(buttons[upgradeQuizPageButtonName], 550);
            contentCanvas.Children.Add(buttons[upgradeQuizPageButtonName]);

            Canvas.SetLeft(buttons[pilotQuizPageButtonName], 800);
            Canvas.SetTop(buttons[pilotQuizPageButtonName], 600);
            contentCanvas.Children.Add(buttons[pilotQuizPageButtonName]);

            Canvas.SetLeft(buttons[maneuverButtonName], 800);
            Canvas.SetTop(buttons[maneuverButtonName], 650);
            contentCanvas.Children.Add(buttons[maneuverButtonName]);
        }
    }
}
