using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace X_Wing_Visual_Builder.Model
{
    public class PilotCanvas : Canvas
    {
        private Pilot pilot;
        private Image pilotImage;
        private Image addButton = new Image();
        private Image removeButton = new Image();
        private Image deleteButton = new Image();
        private OutlinedTextBlock numberOwned = new OutlinedTextBlock();
        private double miniButtonSize;
        private double pcntDif;
        private int uniqueBuildId;
        private DefaultPage currentPage;
        private IPilotClicked pilotClickedPage;
        private IDeletePilot deletePilotPage;
        private ManeuverCard maneuverCard;

        public PilotCanvas(Pilot pilot, DefaultPage currentPage, Image pilotImage, double width, double height, Thickness margin)
        {
            pcntDif = width / 292;
            miniButtonSize = Math.Round(21 * pcntDif);

            Margin = margin;
            Width = Opt.ApResMod(width);
            Height = Opt.ApResMod(height);

            this.pilot = pilot;
            this.currentPage = currentPage;

            this.pilotImage = pilotImage;
            this.pilotImage.Width = Opt.ApResMod(width);
            this.pilotImage.Height = Opt.ApResMod(height);
            this.pilotImage.MouseEnter += new MouseEventHandler(MouseHover);
            this.pilotImage.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            RenderOptions.SetBitmapScalingMode(this.pilotImage, BitmapScalingMode.HighQuality);
            SetLeft(this.pilotImage, 0);
            SetTop(this.pilotImage, 0);
            Children.Add(this.pilotImage);

            numberOwned.Text = pilot.numberOwned.ToString();
            numberOwned.TextAlignment = TextAlignment.Left;
            numberOwned.Width = Opt.ApResMod(30 * pcntDif);
            numberOwned.Height = Opt.ApResMod(30 * pcntDif);
            numberOwned.StrokeThickness = 1;
            numberOwned.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            numberOwned.FontWeight = FontWeights.ExtraBold;
            numberOwned.Fill = new SolidColorBrush(Color.FromRgb(255, 207, 76));
            numberOwned.FontSize = Opt.ApResMod(24 * pcntDif);
            numberOwned.FontFamily = new FontFamily("Verdana");
            numberOwned.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll);
            numberOwned.MouseEnter += new MouseEventHandler(MouseHover);
            numberOwned.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            SetLeft(numberOwned, Opt.ApResMod(261 * pcntDif));
            SetTop(numberOwned, Opt.ApResMod(141 * pcntDif));
            Children.Add(numberOwned);

            addButton.Source = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\addbutton.png"));
            addButton.Height = Opt.ApResMod(miniButtonSize);
            addButton.Width = Opt.ApResMod(miniButtonSize);
            addButton.UseLayoutRounding = true;
            addButton.MouseLeftButtonDown += new MouseButtonEventHandler(AddPilotClicked);
            addButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll);
            addButton.MouseEnter += new MouseEventHandler(MouseHover);
            addButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            addButton.Cursor = Cursors.Hand;
            addButton.Visibility = Visibility.Hidden;
            RenderOptions.SetBitmapScalingMode(addButton, BitmapScalingMode.HighQuality);
            SetLeft(addButton, Opt.ApResMod(260 * pcntDif));
            SetTop(addButton, Opt.ApResMod(120 * pcntDif));
            Children.Add(addButton);

            removeButton.Source = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\removebutton.png"));
            removeButton.Height = Opt.ApResMod(miniButtonSize);
            removeButton.Width = Opt.ApResMod(miniButtonSize);
            removeButton.UseLayoutRounding = true;
            removeButton.MouseLeftButtonDown += new MouseButtonEventHandler(RemovePilotClicked);
            removeButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll);
            removeButton.MouseEnter += new MouseEventHandler(MouseHover);
            removeButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            removeButton.Cursor = Cursors.Hand;
            removeButton.Visibility = Visibility.Hidden;
            RenderOptions.SetBitmapScalingMode(removeButton, BitmapScalingMode.HighQuality);
            SetLeft(removeButton, Opt.ApResMod(260 * pcntDif));
            SetTop(removeButton, Opt.ApResMod(170 * pcntDif));
            Children.Add(removeButton);

            maneuverCard = pilot.ship.GetManeuverCard(Math.Round(Opt.ApResMod(width) / 11));
            maneuverCard.MouseEnter += new MouseEventHandler(MouseHover);
            maneuverCard.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            maneuverCard.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll);
            maneuverCard.Visibility = Visibility.Hidden;
            Canvas.SetLeft(maneuverCard, (Opt.ApResMod(width) / 2) - (maneuverCard.Width / 2));
            Canvas.SetTop(maneuverCard, 0);
            Children.Add(maneuverCard);
        }

        public void AddDeleteButtonEvent(IDeletePilot deletePilotPage, int uniqueBuildId)
        {
            this.deletePilotPage = deletePilotPage;
            this.uniqueBuildId = uniqueBuildId;

            deleteButton.Source = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\deletebutton.png"));
            deleteButton.Height = Opt.ApResMod(miniButtonSize * pcntDif);
            deleteButton.Width = Opt.ApResMod(miniButtonSize * pcntDif);
            deleteButton.UseLayoutRounding = true;
            deleteButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll);
            deleteButton.MouseLeftButtonDown += new MouseButtonEventHandler(DeletePilotClicked);
            deleteButton.Cursor = Cursors.Hand;
            RenderOptions.SetBitmapScalingMode(deleteButton, BitmapScalingMode.HighQuality);
            SetRight(deleteButton, 0);
            SetTop(deleteButton, 0);
            Children.Add(deleteButton);
        }
        private void DeletePilotClicked(object sender, MouseButtonEventArgs e)
        {
            deletePilotPage.DeletePilotClicked(uniqueBuildId, pilot.uniquePilotId);
        }

        public void AddCardClickedEvent(IPilotClicked pilotClickedPage)
        {
            this.pilotClickedPage = pilotClickedPage;
            pilotImage.MouseLeftButtonDown += new MouseButtonEventHandler(CardClicked);
        }
        private void CardClicked(object sender, MouseButtonEventArgs e)
        {
            pilotClickedPage.PilotClicked(pilot.id);
        }

        private void RemovePilotClicked(object sender, MouseButtonEventArgs e)
        {
            pilot.numberOwned--;
            numberOwned.Text = pilot.numberOwned.ToString();
        }
        private void AddPilotClicked(object sender, MouseButtonEventArgs e)
        {
            pilot.numberOwned++;
            numberOwned.Text = pilot.numberOwned.ToString();
        }

        private void MouseHoverLeave(object sender, MouseEventArgs e)
        {
            addButton.Visibility = Visibility.Hidden;
            removeButton.Visibility = Visibility.Hidden;
            maneuverCard.Visibility = Visibility.Hidden;

        }
        private void MouseHover(object sender, MouseEventArgs e)
        {
            addButton.Visibility = Visibility.Visible;
            removeButton.Visibility = Visibility.Visible;
            maneuverCard.Visibility = Visibility.Visible;
        }
    }
}
