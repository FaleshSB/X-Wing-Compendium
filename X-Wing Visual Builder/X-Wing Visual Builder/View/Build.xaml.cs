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

namespace X_Wing_Visual_Builder.View
{
    /// <summary>
    /// Interaction logic for Build.xaml
    /// </summary>
    public partial class Build : Page
    {
        public Build()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Shape Rendershape = null;
            Rendershape = new Rectangle() { Fill = Brushes.Blue, Height = 45, Width = 45, RadiusX = 12, RadiusY = 12 };

            Canvas.SetLeft(Rendershape, canvasArea.ActualHeight / 3);
            Canvas.SetTop(Rendershape, canvasArea.ActualWidth / 3);

            canvasArea.Children.Add(Rendershape);
        }
    }
}
