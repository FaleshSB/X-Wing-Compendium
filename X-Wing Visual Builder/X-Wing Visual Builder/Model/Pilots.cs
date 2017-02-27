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
    class Pilots
    {
        private Dictionary<int, Pilot> pilots = new Dictionary<int, Pilot>();

        public Pilots()
        {
            StringReader sr = new StringReader(Properties.Resources.PilotDatabase);
            using (TextFieldParser parser = new TextFieldParser(sr))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("£");
                parser.HasFieldsEnclosedInQuotes = false;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    Dictionary<UpgradeType, int> possibleUpgrades = new Dictionary<UpgradeType, int>();
                    if (fields[6].Length > 0) {
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
                    pilots.Add(Int32.Parse(fields[0]), new Pilot(Int32.Parse(fields[0]), (Ship)Int32.Parse(fields[1]), Convert.ToBoolean(Int32.Parse(fields[2])), fields[3],
                               Int32.Parse(fields[4]), fields[5], possibleUpgrades, Int32.Parse(fields[7]), fields[8], (Faction)Int32.Parse(fields[9]), Convert.ToBoolean(Int32.Parse(fields[10]))));                    
                }
            }
        }

        public Pilot GetPilot(int id)
        {
            return pilots[id];
        }

        public Pilot GetRandomPilot()
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

        /*
        public List<Upgrade> GetUpgrades(UpgradeType upgradeType, UpgradeSort upgradeSort, Faction faction, ShipSize shipSize)
        {
            List<Upgrade> upgrades = new List<Upgrade>();

            foreach (KeyValuePair<int, Pilot> entry in this.pilots)
            {
                if (entry.Value.upgradeType == upgradeType && (entry.Value.faction == Faction.All || entry.Value.faction == faction) && (entry.Value.shipSize == ShipSize.All || entry.Value.shipSize == shipSize))
                {
                    upgrades.Add(entry.Value);
                }
            }

            upgrades.Sort(
                delegate (Upgrade upgradeOne, Upgrade upgradeTwo)
                {
                    int compareDate = upgradeTwo.cost.CompareTo(upgradeOne.cost);
                    if (compareDate == 0)
                    {
                        return upgradeOne.name.CompareTo(upgradeTwo.name);
                    }
                    return compareDate;
                });


            return upgrades;
        }*/
    }
}
