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
    static class Pilots
    {
        public static Dictionary<int, Pilot> pilots { get; set; } = new Dictionary<int, Pilot>();

        static Pilots()
        {
            using (StringReader stringReader = new StringReader(Properties.Resources.PilotDatabase))
            {
                using (TextFieldParser parser = new TextFieldParser(stringReader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters("£");
                    parser.HasFieldsEnclosedInQuotes = false;
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        Dictionary<UpgradeType, int> possibleUpgrades = new Dictionary<UpgradeType, int>();
                        if (fields[6].Length > 0)
                        {
                            string[] possibleUpgradesSplit = fields[6].Split(',');
                            foreach (string possibleUpgrade in possibleUpgradesSplit)
                            {
                                if (possibleUpgrades.ContainsKey((UpgradeType)Int32.Parse(possibleUpgrade)))
                                {
                                    possibleUpgrades[(UpgradeType)Int32.Parse(possibleUpgrade)]++;
                                }
                                else
                                {
                                    possibleUpgrades[(UpgradeType)Int32.Parse(possibleUpgrade)] = 1;
                                }
                            }
                        }
                        possibleUpgrades.Add(UpgradeType.Title, 1);
                        possibleUpgrades.Add(UpgradeType.Modification, 1);
                        pilots.Add(Int32.Parse(fields[0]), new Pilot(Int32.Parse(fields[0]), (ShipType)Int32.Parse(fields[1]), Convert.ToBoolean(Int32.Parse(fields[2])), fields[3],
                                   Int32.Parse(fields[4]), fields[5], possibleUpgrades, Int32.Parse(fields[7]), fields[8], (Faction)Int32.Parse(fields[9]), Convert.ToBoolean(Int32.Parse(fields[10]))));
                    }
                }
            }
        }

        public static Pilot GetPilotClone(int pilotId)
        {
            return pilots[pilotId].GetPilotClone();
        }

        public static Pilot GetRandomPilot()
        {
            Random rand = new Random();
            List<int> keyList = new List<int>(pilots.Keys);
            Pilot randomPilot = pilots[keyList[rand.Next(keyList.Count)]];
            while (true)
            {
                if (randomPilot.hasAbility == false)
                {
                    randomPilot = pilots[keyList[rand.Next(keyList.Count)]];
                }
                else
                {
                    break;
                }
            }
            return randomPilot;
        }

        public static List<Pilot> GetPilots(Faction faction)
        {
            List<Pilot> pilotsToReturn = new List<Pilot>();


            foreach (KeyValuePair<int, Pilot> entry in pilots)
            {
                if (entry.Value.faction == faction)
                {
                    pilotsToReturn.Add(entry.Value);
                }
            }

            return pilotsToReturn;
        }
    }
}
