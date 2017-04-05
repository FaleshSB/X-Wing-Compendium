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
                    if (fields[16].Length > 0)
                    {
                        string[] possibleUpgradesSplit = fields[16].Split(',');
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
                    if (fields[17].Length > 0)
                    {
                        string[] possibleUpgradesSplit = fields[17].Split(',');
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
                    int requiresPilotSkill = 0;
                    if (fields[18].Length > 0)
                    {
                        requiresPilotSkill = Int32.Parse(fields[18]);
                    }
                    List<Action> requiresActions = new List<Action>();
                    if (fields[19].Length > 0)
                    {
                        string[] requiresActionsSplit = fields[19].Split(',');
                        foreach (string requiresAction in requiresActionsSplit)
                        {
                            requiresActions.Add((Action)Int32.Parse(requiresAction));
                        }
                    }
                    List<int> requiresUpgrades = new List<int>();
                    if (fields[20].Length > 0)
                    {
                        string[] requiresUpgradesSplit = fields[20].Split(',');
                        foreach (string requiresUpgrade in requiresUpgradesSplit)
                        {
                            requiresUpgrades.Add(Int32.Parse(requiresUpgrade));
                        }
                    }
                    List<Action> addsActions = new List<Action>();
                    if (fields[21].Length > 0)
                    {
                        string[] addsActionsSplit = fields[21].Split(',');
                        foreach (string addsAction in addsActionsSplit)
                        {
                            addsActions.Add((Action)Int32.Parse(addsAction));
                        }
                    }
                    int addsPilotSkill = 0;
                    if (fields[22].Length > 0)
                    {
                        addsPilotSkill = Int32.Parse(fields[22]);
                    }
                    upgrades.Add(Int32.Parse(fields[0]), new Upgrade(Int32.Parse(fields[0]), (UpgradeType)Int32.Parse(fields[1]), Int32.Parse(fields[2]), fields[3], fields[4], fields[5],
                                             (Faction)Int32.Parse(fields[6]), (ShipSize)Int32.Parse(fields[7]), (ShipType)Int32.Parse(fields[8]),
                                             Convert.ToBoolean(Int32.Parse(fields[9])), Convert.ToBoolean(Int32.Parse(fields[10])), Convert.ToBoolean(Int32.Parse(fields[11])),
                                             Int32.Parse(fields[12]), Convert.ToBoolean(Int32.Parse(fields[13])), Convert.ToBoolean(Int32.Parse(fields[14])), Convert.ToBoolean(Int32.Parse(fields[15])), upgradesAdded, upgradesRemoved,
                                             requiresPilotSkill, requiresActions, requiresUpgrades, addsActions, addsPilotSkill));
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
        
        public static void RemoveUnusableUpgrades(Build build, int uniquePilotId)
        {
            bool hasAnUpgradeBeenRemoved = false;

            foreach(Upgrade upgrade in build.pilots[uniquePilotId].upgrades)
            {
                if (IsUpgradeUsable(build.pilots[uniquePilotId], upgrade, true) == false)
                {
                    build.RemoveUpgrade(uniquePilotId, upgrade.id);
                    hasAnUpgradeBeenRemoved = true;
                    break;
                }
            }

            if (hasAnUpgradeBeenRemoved == true)
            {
                RemoveUnusableUpgrades(build, uniquePilotId);
            }
        }

        private static bool IsUpgradeUsable(Pilot pilot, Upgrade upgrade, bool isRemovingUpgrades = false)
        {
            bool isUpgradeUsable = true;

            if (pilot.pilotSkill < upgrade.requiresPilotSkill) { isUpgradeUsable = false; }
            if (upgrade.faction != Faction.All && upgrade.faction != pilot.faction) { isUpgradeUsable = false; }
            if (upgrade.shipSize != ShipSize.All && upgrade.shipSize != pilot.ship.shipSize) { isUpgradeUsable = false; }

            if(pilot.possibleUpgrades.ContainsKey(upgrade.upgradeType))
            {
                if (isRemovingUpgrades)
                {
                    isUpgradeUsable = (pilot.possibleUpgrades[upgrade.upgradeType] >= 0) ? isUpgradeUsable : false;
                }
                else
                {
                    isUpgradeUsable = (upgrade.numberOfUpgradeSlots <= pilot.possibleUpgrades[upgrade.upgradeType]) ? isUpgradeUsable : false;
                }
            }
            else
            {
                isUpgradeUsable = false;
            }

            bool isCorrectShipType = false;
            if ((upgrade.shipType == ShipType.All || (upgrade.shipType == pilot.ship.shipType))
                    && (upgrade.isTieOnly == false || (upgrade.isTieOnly == true && pilot.ship.isTIE))
                    && (upgrade.isXWingOnly == false || (upgrade.isXWingOnly == true && pilot.ship.isXWing))
                    ) { isCorrectShipType = true; }
            if (isCorrectShipType == false) { isUpgradeUsable = false; }

            if (upgrade.requiresActions.Count > 0)
            {
                foreach (Action requiredAction in upgrade.requiresActions)
                {
                    if (pilot.usableActions.Exists(action => action == requiredAction) == false)
                    {
                        isUpgradeUsable = false;
                        break;
                    }
                }
            }

            if (upgrade.requiresUpgrades.Count > 0)
            {
                foreach (int requiredupgrade in upgrade.requiresUpgrades)
                {
                    if (pilot.upgrades.Exists(upgradeElement => upgradeElement.id == requiredupgrade) == false)
                    {
                        isUpgradeUsable = false;
                        break;
                    }
                }
            }

            if (UpgradeModifiers.SkipGetUpgrade(pilot, upgrade, isRemovingUpgrades) == true)
            {
                isUpgradeUsable = false;
            }

            return isUpgradeUsable;
        }

        public static List<Upgrade> GetUpgrades(Pilot pilot)
        {
            List<Upgrade> upgradesToReturn = new List<Upgrade>();
            foreach (Upgrade upgrade in upgrades.Values.ToList())
            {
                if(IsUpgradeUsable(pilot, upgrade) == true)
                {
                    upgradesToReturn.Add(upgrade);
                }
            }

            return upgradesToReturn;
        }
    }
}