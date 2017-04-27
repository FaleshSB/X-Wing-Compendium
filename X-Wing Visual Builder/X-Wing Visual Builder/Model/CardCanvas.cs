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
using X_Wing_Visual_Builder.View;
using System.Windows.Data;

namespace X_Wing_Visual_Builder.Model
{
    public class CardCanvas : Canvas
    {
        private Upgrade upgrade;
        private Pilot pilot;
        private Image cardImage;
        private Image addButton = new Image();
        private Image removeButton = new Image();
        private Image deleteButton = new Image();
        private Image infoButton = new Image();
        private OutlinedTextBlock numberOwned = new OutlinedTextBlock();
        private double miniButtonSize;
        private double pcntDif;
        private ICardClicked cardClickedPage;
        private IDeleteCard deleteCardPage;
        private ManeuverCard maneuverCard;
        private int uniqueBuildId;
        private int uniquePilotId;
        private DefaultPage currentPage;
        private bool isHidingInfoButton = false;
        private bool isUpgrade;

        public CardCanvas(Card card, Image cardImage, double width, double height, Thickness margin, bool isUpgrade, DefaultPage currentPage = null)
        {
            this.isUpgrade = isUpgrade;
            if(this.isUpgrade)
            {
                upgrade = (Upgrade)card;
            }
            else
            {
                pilot = (Pilot)card;
            }
            Construct(cardImage, width, height, margin, currentPage);
        }

        private void Construct(Image cardImage, double width, double height, Thickness margin, DefaultPage currentPage = null)
        {
            double numberOwnedLeft;
            double numberOwnedTop;
            double addButtonLeft;
            double addButtonTop;
            double removeButtonLeft;
            double removeButtonTop;
            double infoButtonLeft;
            double infoButtonTop;

            if (isUpgrade)
            {
                pcntDif = width / 166;
                numberOwnedLeft = Opt.ApResMod(0 * pcntDif);
                numberOwnedTop = Opt.ApResMod(161 * pcntDif);
                addButtonLeft = Opt.ApResMod(0 * pcntDif);
                addButtonTop = Opt.ApResMod(137 * pcntDif);
                removeButtonLeft = Opt.ApResMod(0 * pcntDif);
                removeButtonTop = Opt.ApResMod(193 * pcntDif);
                infoButtonLeft = Opt.ApResMod(0 * pcntDif);
                infoButtonTop = Opt.ApResMod(0 * pcntDif);
            }
            else
            {
                pcntDif = width / 292;
                numberOwnedLeft = Opt.ApResMod(261 * pcntDif);
                numberOwnedTop = Opt.ApResMod(141 * pcntDif);
                addButtonLeft = Opt.ApResMod(260 * pcntDif);
                addButtonTop = Opt.ApResMod(120 * pcntDif);
                removeButtonLeft = Opt.ApResMod(260 * pcntDif);
                removeButtonTop = Opt.ApResMod(170 * pcntDif);
                infoButtonLeft = Opt.ApResMod(0 * pcntDif);
                infoButtonTop = Opt.ApResMod(0 * pcntDif);
            }
            miniButtonSize = Math.Round(21 * pcntDif);
            

            Margin = margin;
            Width = Opt.ApResMod(width);
            Height = Opt.ApResMod(height);
            
            this.currentPage = currentPage;

            this.cardImage = cardImage;
            this.cardImage.Width = Opt.ApResMod(width);
            this.cardImage.Height = Opt.ApResMod(height);
            this.cardImage.MouseEnter += new MouseEventHandler(MouseHover);
            this.cardImage.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            RenderOptions.SetBitmapScalingMode(this.cardImage, BitmapScalingMode.HighQuality);
            SetLeft(this.cardImage, 0);
            SetTop(this.cardImage, 0);
            Children.Add(this.cardImage);
            

            if (isUpgrade) { numberOwned.Text = upgrade.numberOwned.ToString(); }
            else { numberOwned.Text = pilot.numberOwned.ToString(); }
            numberOwned.TextAlignment = TextAlignment.Left;
            numberOwned.Width = Opt.ApResMod(30 * pcntDif);
            numberOwned.Height = Opt.ApResMod(30 * pcntDif);
            numberOwned.StrokeThickness = 1;
            numberOwned.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            numberOwned.FontWeight = FontWeights.ExtraBold;
            numberOwned.Fill = new SolidColorBrush(Color.FromRgb(255, 207, 76));
            numberOwned.FontSize = Opt.ApResMod(22 * pcntDif);
            numberOwned.FontFamily = new FontFamily("Verdana");
            if (currentPage != null) { numberOwned.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll); }
            numberOwned.MouseEnter += new MouseEventHandler(MouseHover);
            numberOwned.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            SetLeft(numberOwned, numberOwnedLeft);
            SetTop(numberOwned, numberOwnedTop);
            Children.Add(numberOwned);

            addButton.Source = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\addbutton.png"));
            addButton.Height = Opt.ApResMod(miniButtonSize);
            addButton.Width = Opt.ApResMod(miniButtonSize);
            addButton.UseLayoutRounding = true;
            addButton.MouseLeftButtonDown += new MouseButtonEventHandler(AddOwnedClicked);
            if (currentPage != null) { addButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll); }
            addButton.MouseEnter += new MouseEventHandler(MouseHover);
            addButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            addButton.Cursor = Cursors.Hand;
            addButton.Visibility = Visibility.Hidden;
            RenderOptions.SetBitmapScalingMode(addButton, BitmapScalingMode.HighQuality);
            SetLeft(addButton, addButtonLeft);
            SetTop(addButton, addButtonTop);
            Children.Add(addButton);

            removeButton.Source = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\removebutton.png"));
            removeButton.Height = Opt.ApResMod(miniButtonSize);
            removeButton.Width = Opt.ApResMod(miniButtonSize);
            removeButton.UseLayoutRounding = true;
            removeButton.MouseLeftButtonDown += new MouseButtonEventHandler(RemoveOwnedClicked);
            if (currentPage != null) { removeButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll); }
            removeButton.MouseEnter += new MouseEventHandler(MouseHover);
            removeButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            removeButton.Cursor = Cursors.Hand;
            removeButton.Visibility = Visibility.Hidden;
            RenderOptions.SetBitmapScalingMode(removeButton, BitmapScalingMode.HighQuality);
            SetLeft(removeButton, removeButtonLeft);
            SetTop(removeButton, removeButtonTop);
            Children.Add(removeButton);

            infoButton.Source = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\infobutton.png"));
            infoButton.Height = Opt.ApResMod(miniButtonSize);
            infoButton.Width = Opt.ApResMod(miniButtonSize);
            infoButton.UseLayoutRounding = true;
            infoButton.MouseLeftButtonDown += new MouseButtonEventHandler(InfoClicked);
            if (currentPage != null) { infoButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll); }
            infoButton.MouseEnter += new MouseEventHandler(MouseHover);
            infoButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            infoButton.Cursor = Cursors.Hand;
            infoButton.Visibility = Visibility.Hidden;
            RenderOptions.SetBitmapScalingMode(infoButton, BitmapScalingMode.HighQuality);
            SetLeft(infoButton, infoButtonLeft);
            SetTop(infoButton, infoButtonTop);
            Children.Add(infoButton);
        }

        public void HideInfoButton()
        {
            isHidingInfoButton = true;
            infoButton.Visibility = Visibility.Hidden;
        }

        private void InfoClicked(object sender, MouseButtonEventArgs e)
        {
            InfoDialogBox infoDialogBox = new InfoDialogBox();
            infoDialogBox.Owner = Window.GetWindow(currentPage);
            infoDialogBox.ShowInTaskbar = false;
            if(isUpgrade)
            {
                infoDialogBox.AddCard(upgrade, isUpgrade);
            }
            else
            {
                infoDialogBox.AddCard(pilot, isUpgrade);
            }
            infoDialogBox.ShowDialog();
        }
        public void AddDeleteButtonEvent(IDeleteCard deleteUpgradePage, int uniqueBuildId, int uniquePilotId)
        {
            this.uniquePilotId = uniquePilotId;
            this.deleteCardPage = deleteUpgradePage;
            this.uniqueBuildId = uniqueBuildId;

            deleteButton.Source = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\deletebutton.png"));
            deleteButton.Height = Opt.ApResMod(miniButtonSize);
            deleteButton.Width = Opt.ApResMod(miniButtonSize);
            deleteButton.UseLayoutRounding = true;
            deleteButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll);
            deleteButton.MouseLeftButtonDown += new MouseButtonEventHandler(DeleteCardClicked);
            deleteButton.MouseEnter += new MouseEventHandler(MouseHover);
            deleteButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            deleteButton.Cursor = Cursors.Hand;
            deleteButton.Visibility = Visibility.Hidden;
            RenderOptions.SetBitmapScalingMode(deleteButton, BitmapScalingMode.HighQuality);
            SetRight(deleteButton, 0);
            SetTop(deleteButton, 0);
            Children.Add(deleteButton);
        }
        private void DeleteCardClicked(object sender, MouseButtonEventArgs e)
        {
            if (isUpgrade) { deleteCardPage.DeleteUpgradeClicked(uniqueBuildId, uniquePilotId, upgrade.id); }
            else { deleteCardPage.DeletePilotClicked(uniqueBuildId, uniquePilotId); }
        }

        public void AddCardClickedEvent(ICardClicked upgradeClickedPage)
        {
            this.cardClickedPage = upgradeClickedPage;
            cardImage.MouseLeftButtonDown += new MouseButtonEventHandler(CardClicked);
        }
        private void CardClicked(object sender, MouseButtonEventArgs e)
        {
            if (isUpgrade) { cardClickedPage.UpgradeClicked(upgrade.id); }
            else { cardClickedPage.PilotClicked(pilot.id); }
        }

        private void RemoveOwnedClicked(object sender, MouseButtonEventArgs e)
        {
            if (isUpgrade)
            {
                upgrade.numberOwned--;
            }
            else
            {
                pilot.numberOwned--;
            }
        }
        private void AddOwnedClicked(object sender, MouseButtonEventArgs e)
        {
            if (isUpgrade)
            {
                upgrade.numberOwned++;
            }
            else
            {
                pilot.numberOwned++;
            }
        }

        public void UpdateNumberOwned()
        {
            if (isUpgrade)
            {
                numberOwned.Text = upgrade.numberOwned.ToString();
            }
            else
            {
                numberOwned.Text = pilot.numberOwned.ToString();
            }
        }

        private void MouseHoverLeave(object sender, MouseEventArgs e)
        {
            addButton.Visibility = Visibility.Hidden;
            removeButton.Visibility = Visibility.Hidden;
            infoButton.Visibility = Visibility.Hidden;
            deleteButton.Visibility = Visibility.Hidden;

            if (isUpgrade == false)
            {
                Children.Remove(maneuverCard);
                maneuverCard = null;
            }
        }
        private void MouseHover(object sender, MouseEventArgs e)
        {
            addButton.Visibility = Visibility.Visible;
            removeButton.Visibility = Visibility.Visible;
            if (isHidingInfoButton == false) { infoButton.Visibility = Visibility.Visible; }
            deleteButton.Visibility = Visibility.Visible;

            if(isUpgrade == false)
            {
                maneuverCard = pilot.ship.GetManeuverCard(Math.Round(cardImage.Width / 11));
                maneuverCard.MouseEnter += new MouseEventHandler(MouseHover);
                maneuverCard.MouseLeave += new MouseEventHandler(MouseHoverLeave);
                if (currentPage != null) { maneuverCard.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll); }
                Canvas.SetLeft(maneuverCard, (cardImage.Width / 2) - (maneuverCard.Width / 2));
                Canvas.SetTop(maneuverCard, 0);
                Children.Add(maneuverCard);
            }
        }
    }
}
