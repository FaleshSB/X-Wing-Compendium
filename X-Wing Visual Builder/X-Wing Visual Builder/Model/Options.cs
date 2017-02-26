using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace X_Wing_Visual_Builder.Model
{
    public static class Options
    {
        private static double resolutionMultiplier { get; set; }

        static Options()
        {
            resolutionMultiplier = SystemParameters.PrimaryScreenWidth / 1920;
        }

        public static double ApplyResolutionMultiplier(double number)
        {
            return Math.Round(number * resolutionMultiplier);
        }
    }
}
