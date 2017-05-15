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
        private ImageButton addButton;
        private ImageButton removeButton;
        private ImageButton deleteButton;
        private ImageButton infoButton;
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
        private double numberOwnedLeft;
        private double numberOwnedTop;
        private double addButtonLeft;
        private double addButtonTop;
        private double removeButtonLeft;
        private double removeButtonTop;
        private double infoButtonLeft;
        private double infoButtonTop;
        private bool haveButtonsBeenInitialized = false;
        private bool hasDeleteButton = false;
        private bool isHidingIdentifiers = false;

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
            Children.Clear();

            Thickness margin = ScaledThicknessFactory.GetThickness(this.margin.Left, this.margin.Top, this.margin.Right, this.margin.Bottom);

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

            cardImage.Width = Opt.ApResMod(width);
            cardImage.Height = Opt.ApResMod(height);
            cardImage.MouseEnter += new MouseEventHandler(MouseHover);
            cardImage.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            RenderOptions.SetBitmapScalingMode(this.cardImage, BitmapScalingMode.HighQuality);
            SetLeft(cardImage, 0);
            SetTop(cardImage, 0);
            Children.Add(cardImage);
            

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
            
            
        }

        public void HideCardIdentifiers()
        {
            if (isUpgrade) { identifierHider.Height = Opt.ApResMod(height) * 0.4142259414; }
            else { identifierHider.Height = Opt.ApResMod(height) * 0.4634146341; }
            identifierHider.Width = Opt.ApResMod(width);
            identifierHider.Fill = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            identifierHider.UseLayoutRounding = true;
            identifierHider.Visibility = Visibility.Visible;
            SetLeft(this.identifierHider, 0);
            SetTop(this.identifierHider, 0);
            Children.Add(this.identifierHider);
            isHidingIdentifiers = true;
        }
        public void ShowCardIdentifiers()
        {
            identifierHider.Visibility = Visibility.Hidden;
            isHidingIdentifiers = false;
        }

        public void HideInfoButton()
        {
            isHidingInfoButton = true;
            if (infoButton == null) return;
            infoButton.Visibility = Visibility.Hidden;
        }

        private void InfoButtonClicked(object sender, MouseButtonEventArgs e)
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
            hasDeleteButton = true;
        }
        private void DeleteButtonClicked(object sender, MouseButtonEventArgs e)
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

        private void RemoveButtonClicked(object sender, MouseButtonEventArgs e)
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
        private void AddButtonClicked(object sender, MouseButtonEventArgs e)
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
            if (isHidingIdentifiers) return;
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
            if (isHidingIdentifiers) return;
            if (haveButtonsBeenInitialized == false)
            {
                if (isUpgrade)
                {
                    addButton = new ImageButton("add", 0.9);
                    removeButton = new ImageButton("remove", 0.9);
                    deleteButton = new ImageButton("close", 0.9);
                    infoButton = new ImageButton("info", 0.9);
                }
                else
                {
                    addButton = new ImageButton("add", 1);
                    removeButton = new ImageButton("remove", 1);
                    deleteButton = new ImageButton("close", 1);
                    infoButton = new ImageButton("info", 1);
                }

                addButton.MouseDown += new MouseButtonEventHandler(AddButtonClicked);
                addButton.MouseEnter += new MouseEventHandler(MouseHover);
                addButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
                if (currentPage != null) { addButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll); }
                addButton.Visibility = Visibility.Hidden;
                SetLeft(addButton, addButtonLeft);
                SetTop(addButton, addButtonTop);
                Children.Add(addButton);

                removeButton.MouseDown += new MouseButtonEventHandler(RemoveButtonClicked);
                removeButton.MouseEnter += new MouseEventHandler(MouseHover);
                removeButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
                if (currentPage != null) { removeButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll); }
                removeButton.Visibility = Visibility.Hidden;
                SetLeft(removeButton, removeButtonLeft);
                SetTop(removeButton, removeButtonTop);
                Children.Add(removeButton);

                infoButton.MouseDown += new MouseButtonEventHandler(InfoButtonClicked);
                infoButton.MouseEnter += new MouseEventHandler(MouseHover);
                infoButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
                if (currentPage != null) { infoButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll); }
                infoButton.Visibility = Visibility.Hidden;
                SetLeft(infoButton, infoButtonLeft);
                SetTop(infoButton, infoButtonTop);
                Children.Add(infoButton);
                
                if(hasDeleteButton)
                {
                    deleteButton.MouseDown += new MouseButtonEventHandler(DeleteButtonClicked);
                    deleteButton.MouseEnter += new MouseEventHandler(MouseHover);
                    deleteButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
                    deleteButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll);
                    deleteButton.Visibility = Visibility.Hidden;
                    SetRight(deleteButton, 0);
                    SetTop(deleteButton, 0);
                    Children.Add(deleteButton);
                }

                haveButtonsBeenInitialized = true;
            }

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
