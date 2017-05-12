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
using System.Windows.Shapes;
using System.Windows.Media.Effects;

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
        private double width;
        private double height;
        private Thickness margin;
        private string filteredLocation;
        private Rectangle identifierHider = new Rectangle();

        public CardCanvas(Card card, Image cardImage, double width, double height, Thickness margin, bool isUpgrade, DefaultPage currentPage = null)
        {
            string baseLocation = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            filteredLocation = System.IO.Path.GetDirectoryName(baseLocation).Replace("file:\\", "") + "\\Misc\\";
            this.margin = margin;
            this.cardImage = cardImage;
            this.currentPage = currentPage;
            this.height = height;
            this.width = width;
            this.isUpgrade = isUpgrade;
            DropShadowEffect dropShadow = new DropShadowEffect();
            dropShadow.BlurRadius = Opt.ApResMod(2);
            dropShadow.ShadowDepth = Opt.ApResMod(1);

            Effect = dropShadow;

            if (this.isUpgrade)
            {
                upgrade = (Upgrade)card;
            }
            else
            {
                pilot = (Pilot)card;
            }
            
            ConstructCanvas();
        }

        public void ConstructCanvas()
        {
            this.Children.Clear();

            Thickness margin = ScaledThicknessFactory.GetThickness(this.margin.Left, this.margin.Top, this.margin.Right, this.margin.Bottom);

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
            miniButtonSize = Opt.ApResMod(21);
            

            Margin = margin;
            Width = Opt.ApResMod(width);
            Height = Opt.ApResMod(height);

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
            numberOwned.FontSize = Opt.ApResMod(22);
            numberOwned.FontFamily = new FontFamily("Verdana");
            if (currentPage != null) { numberOwned.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll); }
            numberOwned.MouseEnter += new MouseEventHandler(MouseHover);
            numberOwned.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            SetLeft(numberOwned, numberOwnedLeft);
            SetTop(numberOwned, numberOwnedTop);
            Children.Add(numberOwned);

            addButton.Source = new BitmapImage(new Uri(filteredLocation + "addbutton.png"));
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

            removeButton.Source = new BitmapImage(new Uri(filteredLocation + "removebutton.png"));
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

            infoButton.Source = new BitmapImage(new Uri(filteredLocation + "infobutton.png"));
            //infoButton.Source = new BitmapImage(new Uri(@"C:\Users\Falesh\Source\Repos\X-Wing\X-Wing Visual Builder\X-Wing Visual Builder\bin\Debug\Misc\infobutton.png"));
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

            if(isUpgrade) { identifierHider.Height = Opt.ApResMod(height) * 0.4142259414; }
            else { identifierHider.Height = Opt.ApResMod(height) * 0.4634146341; }
            identifierHider.Width = Opt.ApResMod(width);
            identifierHider.Fill = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            identifierHider.UseLayoutRounding = true;
            identifierHider.Visibility = Visibility.Hidden;
            SetLeft(this.identifierHider, 0);
            SetTop(this.identifierHider, 0);
            Children.Add(this.identifierHider);

            /*
            Label cost = new Label();
            if (this.isUpgrade)
            {
                cost.Content = upgrade.cost;
            }
            else
            {
                cost.Content = pilot.cost;
            }
            cost.FontWeight = FontWeights.Bold;
            cost.Foreground = new SolidColorBrush(Colors.White);
            cost.Background = new SolidColorBrush(Colors.Black);
            cost.FontSize = 17;
            SetRight(cost,50);
            SetBottom(cost, 0);
            Children.Add(cost);
            */
        }

        public void HideCardIdentifiers()
        {
            identifierHider.Visibility = Visibility.Visible;
        }
        public void ShowCardIdentifiers()
        {
            identifierHider.Visibility = Visibility.Hidden;
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

            deleteButton.Source = new BitmapImage(new Uri(filteredLocation + "deletebutton.png"));
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
