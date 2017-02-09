using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace X_Wing_Visual_Builder.Model
{
    class Upgrades
    {
        private List<Upgrade> upgrades = new List<Upgrade>();

        public Upgrades()
        {
            using (TextFieldParser parser = new TextFieldParser(@"D:\Documents\Game Stuff\X-Wing\upgrade cards.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("|");
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();
                    upgrades.Add(new Upgrade(fields[0], Int32.Parse(fields[1]), fields[2], fields[3], fields[4]));
                    int i = 4;
                }
            }
        }
    }
}
