using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Resources;
using System.IO;

namespace X_Wing_Visual_Builder.Model
{
    static class Ships
    {
        public static Dictionary<ShipType, Dictionary<Faction, Ship>> ships = new Dictionary<ShipType, Dictionary<Faction, Ship>>();

        static Ships()
        {
            using (StringReader stringReader = new StringReader(Properties.Resources.shipsDatabase))
            {
                using (TextFieldParser parser = new TextFieldParser(stringReader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("£");
                    parser.HasFieldsEnclosedInQuotes = false;
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        List<Action> actions = new List<Action>();
                        if (fields[10].Length > 0)
                        {
                            string[] actionsSplit = fields[10].Split(',');
                            foreach (string action in actionsSplit)
                            {
                                actions.Add((Action)Int32.Parse(action));
                            }
                        }
                        if (ships.ContainsKey((ShipType)Int32.Parse(fields[1])) == false) { ships[(ShipType)Int32.Parse(fields[1])] = new Dictionary<Faction, Ship>(); }
                        ships[(ShipType)Int32.Parse(fields[1])][(Faction)Int32.Parse(fields[11])] = new Ship(Int32.Parse(fields[0]), (ShipType)Int32.Parse(fields[1]),
                            fields[2], (ShipSize)Int32.Parse(fields[3]), Convert.ToBoolean(Int32.Parse(fields[4])), Convert.ToBoolean(Int32.Parse(fields[5])),
                            Int32.Parse(fields[6]), Int32.Parse(fields[7]), Int32.Parse(fields[8]), Int32.Parse(fields[9]), actions, (Faction)Int32.Parse(fields[11]));
                    }
                }
            }
        }
    }
}
