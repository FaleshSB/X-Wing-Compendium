using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace X_Wing_Visual_Builder.Model
{
    public class Card
    {
        public int id { get; set; }
        public int cost { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<string> faq { get; set; } = new List<string>();
        public Faction faction { get; set; }
        public List<CardCanvas> cardCanvasList = new List<CardCanvas>();
        private BitmapImage resizedCardImage = null;
        protected bool isUpgrade;
        protected string imageFilePath;
        public Dictionary<ExpansionType, int> inExpansion { get; set; } = new Dictionary<ExpansionType, int>();
        public bool hasCardImage = false;

        public CardCanvas GetCanvas(double width, double height, Thickness margin, DefaultPage currentPage = null)
        {
            if (resizedCardImage == null)
            {
                resizedCardImage = new BitmapImage();
                resizedCardImage.BeginInit();
                resizedCardImage.CacheOption = BitmapCacheOption.OnLoad;
                resizedCardImage.StreamSource = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
                resizedCardImage.EndInit();
                resizedCardImage.Freeze();
                hasCardImage = true;
            }

            System.Windows.Controls.Image cardImage = new System.Windows.Controls.Image();
            cardImage.Source = resizedCardImage;
            CardCanvas cardCanvas = new CardCanvas(this, cardImage, width, height, margin, isUpgrade, currentPage);
            cardCanvasList.Add(cardCanvas);
            return cardCanvas;
        }

        public void CacheCardImage()
        {
            resizedCardImage = new BitmapImage();
            resizedCardImage.BeginInit();
            resizedCardImage.CacheOption = BitmapCacheOption.OnLoad;
            resizedCardImage.StreamSource = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            resizedCardImage.EndInit();
            resizedCardImage.Freeze(); 
            hasCardImage = true;
        }
    }
}
