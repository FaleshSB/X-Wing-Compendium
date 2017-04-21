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
    public class UpgradeCanvas : Canvas
    {
        private Upgrade upgrade;
        private Image upgradeImage;
        private Image addButton = new Image();
        private Image removeButton = new Image();
        private OutlinedTextBlock numberOwned = new OutlinedTextBlock();
        private double addRemoveButtonSize = 22;
        private int upgradeCardWidth = 166;
        private int upgradeCardHeight = 255;

        public UpgradeCanvas(Upgrade upgrade, Image upgradeImage)
        {
            this.upgrade = upgrade;

            this.upgradeImage = upgradeImage;
            this.upgradeImage.Width = Opt.ApResMod(upgradeCardWidth);
            this.upgradeImage.Height = Opt.ApResMod(upgradeCardHeight);
            this.upgradeImage.MouseEnter += new MouseEventHandler(MouseHover);
            this.upgradeImage.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            Canvas.SetLeft(this.upgradeImage, 0);
            Canvas.SetTop(this.upgradeImage, 0);
            Children.Add(this.upgradeImage);

            OutlinedTextBlock numberOwned = new OutlinedTextBlock();
            numberOwned.Text = upgrade.numberOwned.ToString();
            numberOwned.TextAlignment = TextAlignment.Left;
            numberOwned.Width = Opt.ApResMod(30);
            numberOwned.Height = Opt.ApResMod(30);
            numberOwned.StrokeThickness = 1;
            numberOwned.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            numberOwned.FontWeight = FontWeights.ExtraBold;
            numberOwned.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 207, 76));
            numberOwned.FontSize = Opt.ApResMod(20);
            numberOwned.FontFamily = new System.Windows.Media.FontFamily("Verdana");
            Canvas.SetLeft(numberOwned, Opt.ApResMod(1));
            Canvas.SetBottom(numberOwned, Opt.ApResMod(64));
            Children.Add(numberOwned);

            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
            addButton.Source = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\addbutton.png"));
            addButton.Height = Opt.ApResMod(addRemoveButtonSize);
            addButton.Width = Opt.ApResMod(addRemoveButtonSize);
            addButton.UseLayoutRounding = true;
            addButton.MouseLeftButtonDown += new MouseButtonEventHandler(AddUpgradeClicked);
            addButton.MouseWheel += new MouseWheelEventHandler(ContentScroll);
            addButton.MouseEnter += new MouseEventHandler(MouseHover);
            addButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            addButton.Visibility = Visibility.Hidden;
            Canvas.SetLeft(addButton, 0);
            Canvas.SetTop(addButton, Opt.ApResMod(140));
            Children.Add(addButton);
            
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
            removeButton.Source = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\removebutton.png"));
            removeButton.Height = Opt.ApResMod(addRemoveButtonSize);
            removeButton.Width = Opt.ApResMod(addRemoveButtonSize);
            removeButton.UseLayoutRounding = true;
            removeButton.MouseLeftButtonDown += new MouseButtonEventHandler(RemoveUpgradeClicked);
            removeButton.MouseWheel += new MouseWheelEventHandler(ContentScroll);
            removeButton.MouseEnter += new MouseEventHandler(MouseHover);
            removeButton.MouseLeave += new MouseEventHandler(MouseHoverLeave);
            removeButton.Visibility = Visibility.Hidden;
            Canvas.SetLeft(removeButton, 0);
            Canvas.SetTop(removeButton, Opt.ApResMod(190));
            Children.Add(removeButton);
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

        private void ContentScroll(object sender, MouseWheelEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MouseHoverLeave(object sender, MouseEventArgs e)
        {
            addButton.Visibility = Visibility.Hidden;
            removeButton.Visibility = Visibility.Hidden;

        }
        private void MouseHover(object sender, MouseEventArgs e)
        {
            addButton.Visibility = Visibility.Visible;
            removeButton.Visibility = Visibility.Visible;
        }
    }
}
