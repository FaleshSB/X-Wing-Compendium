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
    static class Upgrades
    {
        public static Dictionary<int, Upgrade> upgrades { get; set; } = new Dictionary<int, Upgrade>();

        static Upgrades()
        {
            StringReader sr = new StringReader(Properties.Resources.UpgradeDatabase);
            using (TextFieldParser parser = new TextFieldParser(sr))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("£");
                parser.HasFieldsEnclosedInQuotes = false;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    upgrades.Add(Int32.Parse(fields[0]), new Upgrade(Int32.Parse(fields[0]), (UpgradeType)Int32.Parse(fields[1]), Int32.Parse(fields[2]), fields[3], fields[4], fields[5],
                                             (Faction)Int32.Parse(fields[6]), (ShipSize)Int32.Parse(fields[7]), (ShipType)Int32.Parse(fields[8]),
                                             Convert.ToBoolean(Int32.Parse(fields[9])), Convert.ToBoolean(Int32.Parse(fields[10])), Convert.ToBoolean(Int32.Parse(fields[11])),
                                             Int32.Parse(fields[12]), Convert.ToBoolean(Int32.Parse(fields[13])), Convert.ToBoolean(Int32.Parse(fields[14])),
                                             Convert.ToBoolean(Int32.Parse(fields[15])), Convert.ToBoolean(Int32.Parse(fields[16])), Convert.ToBoolean(Int32.Parse(fields[17]))));
                }
            }
        }

        public static Upgrade GetRandomUpgrade()
        {
            Random rand = new Random();
            List<int> keyList = new List<int>(upgrades.Keys);
            Upgrade randomUpgrade = upgrades[keyList[rand.Next(keyList.Count)]];
            while(true)
            {
                if(randomUpgrade.shipSize == ShipSize.Huge || randomUpgrade.upgradeType == UpgradeType.Hardpoint || randomUpgrade.upgradeType == UpgradeType.Team)
                {
                    randomUpgrade = upgrades[keyList[rand.Next(keyList.Count)]];
                }
                else
                {
                    break;
                }
            }
            return randomUpgrade;
        }
        
        public static List<Upgrade> GetUpgrades(Dictionary<UpgradeType, int> possibleUpgrades, List<Faction> factions, List<ShipSize> shipSizes, List<Ship> ships)
        {
            List<Upgrade> upgradesToReturn = new List<Upgrade>();


            foreach (KeyValuePair<int, Upgrade> entry in upgrades)
            {
                bool isCorrectType = false;
                bool isCorrectFaction = false;
                bool isCorrectShipSize = false;
                bool isCorrectShipType = false;
                foreach (KeyValuePair<UpgradeType, int> possibleUpgrade in possibleUpgrades)
                {
                    if(entry.Value.upgradeType == UpgradeType.All || (entry.Value.upgradeType == possibleUpgrade.Key && entry.Value.numberOfUpgradeSlots <= possibleUpgrade.Value)) { isCorrectType = true;  break; }
                }
                foreach (Faction faction in factions)
                {
                    if (entry.Value.faction == Faction.All || entry.Value.faction == faction) { isCorrectFaction = true; break; }
                }
                foreach (ShipSize shipSize in shipSizes)
                {
                    if (entry.Value.shipSize == ShipSize.All || entry.Value.shipSize == shipSize) { isCorrectShipSize = true; break; }
                }
                foreach (Ship ship in ships)
                {
                    if ((entry.Value.shipType == ShipType.All || (entry.Value.shipType == ship.shipType))
                        && (entry.Value.isTieOnly == false || (entry.Value.isTieOnly == true && ship.isTie))
                        && (entry.Value.isXWingOnly == false || (entry.Value.isXWingOnly == true && ship.isXWing))
                        ) { isCorrectShipType = true; break; }
                }

                if (isCorrectType && isCorrectFaction && isCorrectShipSize && isCorrectShipType)
                {
                    upgradesToReturn.Add(entry.Value);
                }
            }

            return upgradesToReturn;
        }
    }
}