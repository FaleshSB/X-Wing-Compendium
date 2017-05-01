using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace X_Wing_Visual_Builder.Model
{
    public static class ScaledThicknessFactory
    {
        public static Thickness GetThickness(double all)
        {
            return new Thickness(Opt.ApResMod(all));
        }

        public static Thickness GetThickness(double left, double top, double right, double bottom)
        {
            return new Thickness(Opt.ApResMod(left), Opt.ApResMod(top), Opt.ApResMod(right), Opt.ApResMod(bottom));
        }
    }
}
