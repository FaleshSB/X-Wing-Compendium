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
        public static Dictionary<int, Upgrade> upgrades = new Dictionary<int, Upgrade>();
        private static Dictionary<UpgradeType, int> possibleUpgrades;
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
                    Dictionary<UpgradeType, int> upgradesAdded = new Dictionary<UpgradeType, int>();
                    if (fields[18].Length > 0)
                    {
                        string[] possibleUpgradesSplit = fields[18].Split(',');
                        foreach (string possibleUpgrade in possibleUpgradesSplit)
                        {
                            if (upgradesAdded.ContainsKey((UpgradeType)Int32.Parse(possibleUpgrade)))
                            {
                                upgradesAdded[(UpgradeType)Int32.Parse(possibleUpgrade)]++;
                            }
                            else
                            {
                                upgradesAdded[(UpgradeType)Int32.Parse(possibleUpgrade)] = 1;
                            }
                        }
                    }
                    Dictionary<UpgradeType, int> upgradesRemoved = new Dictionary<UpgradeType, int>();
                    if (fields[19].Length > 0)
                    {
                        string[] possibleUpgradesSplit = fields[19].Split(',');
                        foreach (string possibleUpgrade in possibleUpgradesSplit)
                        {
                            if (upgradesRemoved.ContainsKey((UpgradeType)Int32.Parse(possibleUpgrade)))
                            {
                                upgradesRemoved[(UpgradeType)Int32.Parse(possibleUpgrade)]++;
                            }
                            else
                            {
                                upgradesRemoved[(UpgradeType)Int32.Parse(possibleUpgrade)] = 1;
                            }
                        }
                    }
                    upgrades.Add(Int32.Parse(fields[0]), new Upgrade(Int32.Parse(fields[0]), (UpgradeType)Int32.Parse(fields[1]), Int32.Parse(fields[2]), fields[3], fields[4], fields[5],
                                             (Faction)Int32.Parse(fields[6]), (ShipSize)Int32.Parse(fields[7]), (ShipType)Int32.Parse(fields[8]),
                                             Convert.ToBoolean(Int32.Parse(fields[9])), Convert.ToBoolean(Int32.Parse(fields[10])), Convert.ToBoolean(Int32.Parse(fields[11])),
                                             Int32.Parse(fields[12]), Convert.ToBoolean(Int32.Parse(fields[13])),
                                             Convert.ToBoolean(Int32.Parse(fields[14])), Convert.ToBoolean(Int32.Parse(fields[15])), Convert.ToBoolean(Int32.Parse(fields[16])), upgradesAdded, upgradesRemoved));
                }
            }
            // Remove Huge Ship cards
            List<int> upgradesToRemove = new List<int>();
            foreach (KeyValuePair<int, Upgrade> upgrade in upgrades)
            {
                if(upgrade.Value.shipSize == ShipSize.Huge) { upgradesToRemove.Add(upgrade.Key); }
            }
            foreach(int upgradeToRemove in upgradesToRemove)
            {
                upgrades.Remove(upgradeToRemove);
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
        
        public static void AddOrRemoveFromPossibleUpgrades(Dictionary<UpgradeType, int> possibleUpgrades, UpgradeType key, int value)
        {
            if(possibleUpgrades.ContainsKey(key))
            {
                possibleUpgrades[key] += value;
            }
            else
            {
                possibleUpgrades[key] = 0 + value;
            }
        }

        public static Pilot RemoveUnusableUpgrades(Pilot pilot)
        {
            bool hasAnUpgradeBeenRemoved = false;
            Dictionary<UpgradeType, int> possibleUpgrades = GetPossibleUpgrades(pilot);
            foreach (KeyValuePair<UpgradeType, int> possibleUpgrade in possibleUpgrades)
            {
                if(possibleUpgrade.Value < 0)
                {
                    foreach(Upgrade upgrade in pilot.upgrades)
                    {
                        if(upgrade.upgradeType == possibleUpgrade.Key)
                        {
                            pilot.upgrades.Remove(Upgrades.upgrades[upgrade.id]);
                            hasAnUpgradeBeenRemoved = true;
                            break;
                        }
                    }
                }
            }
            //add GetUpgrades search to see if upgrades are present in case of lowering ps/other upgrade requirements
            if (hasAnUpgradeBeenRemoved == true)
            {
                return RemoveUnusableUpgrades(pilot);
            }
            else
            {
                return pilot;
            }
        }

        private static Dictionary<UpgradeType, int> GetPossibleUpgrades(Pilot pilot)
        {
            List<Faction> factions = new List<Faction>();
            factions.Add(pilot.faction);
            List<ShipSize> shipSizes = new List<ShipSize>();
            shipSizes.Add(pilot.ship.shipSize);
            List<Ship> ships = new List<Ship>();
            ships.Add(pilot.ship);

            possibleUpgrades = new Dictionary<UpgradeType, int>(pilot.possibleUpgrades);
            foreach (Upgrade upgrade in pilot.upgrades)
            {
                AddOrRemoveFromPossibleUpgrades(possibleUpgrades, upgrade.upgradeType, 0 - upgrade.numberOfUpgradeSlots);
                foreach (KeyValuePair<UpgradeType, int> upgradeAdded in upgrade.upgradesAdded)
                {
                    AddOrRemoveFromPossibleUpgrades(possibleUpgrades, upgradeAdded.Key, upgradeAdded.Value);
                }
                foreach (KeyValuePair<UpgradeType, int> upgradeRemoved in upgrade.upgradesRemoved)
                {
                    AddOrRemoveFromPossibleUpgrades(possibleUpgrades, upgradeRemoved.Key, upgradeRemoved.Value);
                }
            }

            UpgradeModifiers.ChangePossibleUpgrades(pilot, possibleUpgrades);
            
            return possibleUpgrades;
        }
        public static List<Upgrade> GetUpgrades(Pilot pilot, bool isTestingToRemoveUpgrades = false)
        {
            List<Faction> factions = new List<Faction>();
            factions.Add(pilot.faction);
            List<ShipSize> shipSizes = new List<ShipSize>();
            shipSizes.Add(pilot.ship.shipSize);
            List<Ship> ships = new List<Ship>();
            ships.Add(pilot.ship);

            Dictionary<UpgradeType, int> possibleUpgrades = GetPossibleUpgrades(pilot);
            List<Upgrade> upgradesToReturn = new List<Upgrade>();
            foreach (KeyValuePair<int, Upgrade> entry in upgrades)
            {
                bool isCorrectType = false;
                bool isCorrectFaction = false;
                bool isCorrectShipSize = false;
                bool isCorrectShipType = false;
                if (isTestingToRemoveUpgrades == false)
                {
                    foreach (KeyValuePair<UpgradeType, int> possibleUpgrade in possibleUpgrades)
                    {
                        if (entry.Value.upgradeType == UpgradeType.All || (entry.Value.upgradeType == possibleUpgrade.Key && entry.Value.numberOfUpgradeSlots <= possibleUpgrade.Value)) { isCorrectType = true; break; }
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
                            && (entry.Value.isTieOnly == false || (entry.Value.isTieOnly == true && ship.isTIE))
                            && (entry.Value.isXWingOnly == false || (entry.Value.isXWingOnly == true && ship.isXWing))
                            ) { isCorrectShipType = true; break; }
                    }
                }
                else
                {
                    isCorrectType = true;
                    isCorrectFaction = true;
                    isCorrectShipSize = true;
                    isCorrectShipType = true;
                }
                if (isCorrectType && isCorrectFaction && isCorrectShipSize && isCorrectShipType)
                {
                    if(UpgradeModifiers.SkipGetUpgrade(pilot, entry.Value) == true)
                    {
                        continue;
                    }
                    upgradesToReturn.Add(entry.Value);
                }
            }


            return upgradesToReturn;
        }
        /*
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
        */
    }
}