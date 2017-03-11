using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Wing_Visual_Builder.View;

namespace X_Wing_Visual_Builder.Model
{
    static class Pages
    {
        public static Dictionary<PageName, DefaultPage> pages { get; set; } = new Dictionary<PageName, DefaultPage>();

        static Pages()
        {
            pages[PageName.BrowseCards] = new BrowseCardsPage();
            pages[PageName.UpgradeQuiz] = new UpgradeQuizPage();
            pages[PageName.PilotQuiz] = new PilotQuizPage();
            pages[PageName.SquadsPage] = new SquadsPage();
            pages[PageName.CalculateStats] = new StatsPage();
        }
    }
}
