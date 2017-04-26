using System;
using System.Collections.Generic;
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
        protected List<CardCanvas> cardCanvasList = new List<CardCanvas>();
        private BitmapImage resizedCardImage = null;
        protected bool isUpgrade;
        protected Uri cardImageUri;
        public Dictionary<ExpansionType, int> inExpansion { get; set; } = new Dictionary<ExpansionType, int>();

        public CardCanvas GetCanvas(double width, double height, Thickness margin, DefaultPage currentPage = null)
        {
            if (resizedCardImage == null)
            {
                resizedCardImage = new BitmapImage();
                resizedCardImage.BeginInit();
                resizedCardImage.CacheOption = BitmapCacheOption.OnLoad;
                resizedCardImage.UriSource = cardImageUri;
                resizedCardImage.EndInit();
            }

            System.Windows.Controls.Image upgradeImage = new System.Windows.Controls.Image();
            upgradeImage.Source = resizedCardImage;
            CardCanvas cardCanvas = new CardCanvas(this, upgradeImage, width, height, margin, isUpgrade, currentPage);
            cardCanvasList.Add(cardCanvas);
            return cardCanvas;
        }
    }
}
