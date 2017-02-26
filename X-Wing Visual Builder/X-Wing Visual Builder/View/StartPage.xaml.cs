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
    public partial class StartPage : X_Wing_Visual_Builder.Model.DefaultPage
    {
        private System.Drawing.Size buttonSize = new System.Drawing.Size(168, 36);

        private Dictionary<string, Image> buttons = new Dictionary<string, Image>();
        private string manageSquadsButtonName = "manage_squads";
        private string yourCollectionButtonName = "your_collection";
        private string calculateStatsButtonName = "calculate_stats";
        private string upgradeQuizButtonName = "upgrade_quiz";
        private string pilotQuizButtonName = "pilot_quiz";
        private string maneuverButtonName = "maneuver_quiz";

        public StartPage()
        {
            InitializeComponent();

            buttons.Add(manageSquadsButtonName, CreateButton(manageSquadsButtonName));
            buttons.Add(yourCollectionButtonName, CreateButton(yourCollectionButtonName));
            buttons.Add(calculateStatsButtonName, CreateButton(calculateStatsButtonName));
            buttons.Add(upgradeQuizButtonName, CreateButton(upgradeQuizButtonName));
            buttons.Add(pilotQuizButtonName, CreateButton(pilotQuizButtonName));
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

            if(button.Name == upgradeQuizButtonName)
            {
                NavigationService.Navigate(new UpgradeQuiz());
            }
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            DisplayContent();
        }

        private void DisplayContent()
        {
            canvasArea.Children.Clear();

            Canvas.SetLeft(buttons[manageSquadsButtonName], 800);
            Canvas.SetTop(buttons[manageSquadsButtonName], 400);
            canvasArea.Children.Add(buttons[manageSquadsButtonName]);

            Canvas.SetLeft(buttons[yourCollectionButtonName], 800);
            Canvas.SetTop(buttons[yourCollectionButtonName], 450);
            canvasArea.Children.Add(buttons[yourCollectionButtonName]);

            Canvas.SetLeft(buttons[calculateStatsButtonName], 800);
            Canvas.SetTop(buttons[calculateStatsButtonName], 500);
            canvasArea.Children.Add(buttons[calculateStatsButtonName]);

            Canvas.SetLeft(buttons[upgradeQuizButtonName], 800);
            Canvas.SetTop(buttons[upgradeQuizButtonName], 550);
            canvasArea.Children.Add(buttons[upgradeQuizButtonName]);

            Canvas.SetLeft(buttons[pilotQuizButtonName], 800);
            Canvas.SetTop(buttons[pilotQuizButtonName], 600);
            canvasArea.Children.Add(buttons[pilotQuizButtonName]);

            Canvas.SetLeft(buttons[maneuverButtonName], 800);
            Canvas.SetTop(buttons[maneuverButtonName], 650);
            canvasArea.Children.Add(buttons[maneuverButtonName]);
        }
    }
}
