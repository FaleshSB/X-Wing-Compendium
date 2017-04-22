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

namespace X_Wing_Visual_Builder.Model
{
    public class UpgradeCanvas : Canvas
    {
        private Upgrade upgrade;
        private Image upgradeImage;
        private Image addButton = new Image();
        private Image removeButton = new Image();
        private Image deleteButton = new Image();
        private Image infoButton = new Image();
        private OutlinedTextBlock numberOwned = new OutlinedTextBlock();
        private double miniButtonSize;
        private double pcntDif;
        private IUpgradeClicked upgradeClickedPage;
        private IDeleteUpgrade deleteUpgradePage;
        private int uniqueBuildId;
        private DefaultPage currentPage;
        private bool isHidingInfoButton = false;

        public UpgradeCanvas(Upgrade upgrade, Image upgradeImage, double width, double height, Thickness margin, DefaultPage currentPage = null)
        {
            pcntDif = width / 166;
            miniButtonSize = Math.Round(21 * pcntDif);

            Margin = margin;
            Width = Opt.ApResMod(width);
            Height = Opt.ApResMod(height);

            this.upgrade = upgrade;
            this.currentPage = currentPage;

            this.upgradeImage = upgradeImage;
            this.upgradeImage.Width = Opt.ApResMod(width);
            this.upgradeImage.Height = Opt.ApResMod(height);
            this.upgradeImage.MouseEnter += new MouseEventHandler(MouseHover);
            this.upgradeImage.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            RenderOptions.SetBitmapScalingMode(this.upgradeImage, BitmapScalingMode.HighQuality);
            SetLeft(this.upgradeImage, 0);
            SetTop(this.upgradeImage, 0);
            Children.Add(this.upgradeImage);
            
            numberOwned.Text = upgrade.numberOwned.ToString();
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
            SetLeft(numberOwned, Opt.ApResMod(0 * pcntDif));
            SetBottom(numberOwned, Opt.ApResMod(64 * pcntDif));
            Children.Add(numberOwned);

            addButton.Source = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\addbutton.png"));
            addButton.Height = Opt.ApResMod(miniButtonSize);
            addButton.Width = Opt.ApResMod(miniButtonSize);
            addButton.UseLayoutRounding = true;
            addButton.MouseLeftButtonDown += new MouseButtonEventHandler(AddUpgradeClicked);
            if (currentPage != null) { addButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll); }
            addButton.MouseEnter += new MouseEventHandler(MouseHover);
            addButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            addButton.Cursor = Cursors.Hand;
            addButton.Visibility = Visibility.Hidden;
            RenderOptions.SetBitmapScalingMode(addButton, BitmapScalingMode.HighQuality);
            SetLeft(addButton, 0);
            SetTop(addButton, Opt.ApResMod(140 * pcntDif));
            Children.Add(addButton);
            
            removeButton.Source = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\removebutton.png"));
            removeButton.Height = Opt.ApResMod(miniButtonSize);
            removeButton.Width = Opt.ApResMod(miniButtonSize);
            removeButton.UseLayoutRounding = true;
            removeButton.MouseLeftButtonDown += new MouseButtonEventHandler(RemoveUpgradeClicked);
            if (currentPage != null) { removeButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll); }
            removeButton.MouseEnter += new MouseEventHandler(MouseHover);
            removeButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            removeButton.Cursor = Cursors.Hand;
            removeButton.Visibility = Visibility.Hidden;
            RenderOptions.SetBitmapScalingMode(removeButton, BitmapScalingMode.HighQuality);
            SetLeft(removeButton, 0);
            SetTop(removeButton, Opt.ApResMod(190 * pcntDif));
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
            SetLeft(infoButton, 0);
            SetTop(infoButton, 0);
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
            infoDialogBox.AddUpgrade(upgrade);
            infoDialogBox.ShowDialog();
        }

        public void AddDeleteButtonEvent(IDeleteUpgrade deleteUpgradePage, int uniqueBuildId)
        {
            this.deleteUpgradePage = deleteUpgradePage;
            this.uniqueBuildId = uniqueBuildId;

            deleteButton.Source = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\deletebutton.png"));
            deleteButton.Height = Opt.ApResMod(miniButtonSize * pcntDif);
            deleteButton.Width = Opt.ApResMod(miniButtonSize * pcntDif);
            deleteButton.UseLayoutRounding = true;
            deleteButton.MouseWheel += new MouseWheelEventHandler(currentPage.ContentScroll);
            deleteButton.MouseLeftButtonDown += new MouseButtonEventHandler(DeleteUpgradeClicked);
            deleteButton.Cursor = Cursors.Hand;
            deleteButton.Visibility = Visibility.Hidden;
            RenderOptions.SetBitmapScalingMode(deleteButton, BitmapScalingMode.HighQuality);
            SetRight(deleteButton, 0);
            SetTop(deleteButton, 0);
            Children.Add(deleteButton);
        }
        private void DeleteUpgradeClicked(object sender, MouseButtonEventArgs e)
        {
            deleteUpgradePage.DeleteUpgradeClicked(uniqueBuildId, upgrade.uniqueUpgradeId);
        }

        public void AddCardClickedEvent(IUpgradeClicked upgradeClickedPage)
        {
            this.upgradeClickedPage = upgradeClickedPage;
            upgradeImage.MouseLeftButtonDown += new MouseButtonEventHandler(CardClicked);
        }
        private void CardClicked(object sender, MouseButtonEventArgs e)
        {
            upgradeClickedPage.UpgradeClicked(upgrade.id);
        }

        private void RemoveUpgradeClicked(object sender, MouseButtonEventArgs e)
        {
            upgrade.numberOwned--;
            numberOwned.Text = upgrade.numberOwned.ToString();
        }
        private void AddUpgradeClicked(object sender, MouseButtonEventArgs e)
        {
            upgrade.numberOwned++;
            numberOwned.Text = upgrade.numberOwned.ToString();
        }

        private void MouseHoverLeave(object sender, MouseEventArgs e)
        {
            addButton.Visibility = Visibility.Hidden;
            removeButton.Visibility = Visibility.Hidden;
            infoButton.Visibility = Visibility.Hidden;
            deleteButton.Visibility = Visibility.Hidden;
        }
        private void MouseHover(object sender, MouseEventArgs e)
        {
            addButton.Visibility = Visibility.Visible; 
            removeButton.Visibility = Visibility.Visible;
            if (isHidingInfoButton == false) { infoButton.Visibility = Visibility.Visible; }
            deleteButton.Visibility = Visibility.Visible;
        }
    }
}
