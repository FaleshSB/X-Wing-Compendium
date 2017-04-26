using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace X_Wing_Visual_Builder.Model
{
    public interface ICardInfo
    {
        int id { get; set; }
        int cost { get; set; }
        string name { get; set; }
        string description { get; set; }
        List<string> faq { get; set; }
        Faction faction { get; set; }
        int numberOwned { get; set; }
        Dictionary<ExpansionType, int> inExpansion { get; set; }
        CardCanvas GetCanvas(double width, double height, Thickness margin, DefaultPage currentPage = null);
    }
}
