using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace X_Wing_Visual_Builder.Model
{
    static class Opt
    {
        private static double resolutionMultiplier;
        public static double zoom = 1;

        static Opt()
        {
            resolutionMultiplier = SystemParameters.PrimaryScreenWidth / 1920;
        }

        public static double ApResMod(double number)
        {
            return Math.Round((number * resolutionMultiplier) * zoom);
        }
    }
}
