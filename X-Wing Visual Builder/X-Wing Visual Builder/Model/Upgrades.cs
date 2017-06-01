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
            Dictionary<int, int> upgradeKeyOwned = LoadUpgradesOwned();
            StringReader sr = new StringReader(Properties.Resources.UpgradeDatabase);
            using (TextFieldParser parser = new TextFieldParser(sr))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("£");
                parser.HasFieldsEnclosedInQuotes = false;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    List<string> faq = new List<string>();
                    if (fields[5].Length > 0)
                    {
                        string[] possibleFaqSplit = fields[5].Split('|');
                        foreach (string possibleFaq in possibleFaqSplit)
                        {
                            faq.Add(possibleFaq);
                        }
                    }
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
                    List<ShipType> shipsThatCanUse = new List<ShipType>();
                    if (fields[8].Length > 0)
                    {
                        string[] shipsThatCanUseSplit = fields[8].Split(',');
                        foreach (string shipThatCanUse in shipsThatCanUseSplit)
                        {
                            shipsThatCanUse.Add((ShipType)Int32.Parse(shipThatCanUse));
                        }
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
                    List<ExpansionType> inExpansion = new List<ExpansionType>();
                    if (fields[23].Length > 0)
                    {
                        string[] inExpansionSplit = fields[23].Split(',');
                        foreach (string inSingleExpansion in inExpansionSplit)
                        {
                            inExpansion.Add((ExpansionType)Int32.Parse(inSingleExpansion));
                        }
                    }
                    int numberOwned = 0;
                    if(upgradeKeyOwned != null && upgradeKeyOwned.ContainsKey(Int32.Parse(fields[0])))
                    {
                        numberOwned = upgradeKeyOwned[Int32.Parse(fields[0])];
                    }
                    
                    upgrades.Add(Int32.Parse(fields[0]), new Upgrade(Int32.Parse(fields[0]), (UpgradeType)Int32.Parse(fields[1]), Int32.Parse(fields[2]), fields[3], fields[4], faq,
                                             (Faction)Int32.Parse(fields[6]), (ShipSize)Int32.Parse(fields[7]), shipsThatCanUse,
                                             Convert.ToBoolean(Int32.Parse(fields[9])), Convert.ToBoolean(Int32.Parse(fields[10])), Convert.ToBoolean(Int32.Parse(fields[11])),
                                             Int32.Parse(fields[12]), Convert.ToBoolean(Int32.Parse(fields[13])), Convert.ToBoolean(Int32.Parse(fields[14])), Convert.ToBoolean(Int32.Parse(fields[15])), upgradesAdded, upgradesRemoved,
                                             requiresPilotSkill, requiresActions, requiresUpgrades, addsActions, addsPilotSkill, numberOwned, inExpansion, fields[24]));
                }
            }
            SaveNumberOfUpgradesOwned();

            // Remove Huge Ship cards
            
            List<int> upgradesToRemove = new List<int>();
            foreach (KeyValuePair<int, Upgrade> upgrade in upgrades)
            {
                if (upgrade.Value.shipThatCanUse.Count > 1 || upgrade.Value.shipThatCanUse[0] != ShipType.All)
                {
                    foreach (ShipType shipType in upgrade.Value.shipThatCanUse)
                    {
                        if (Ships.ships.ContainsKey(shipType) == false || Ships.ships[shipType].First().Value.shipSize == ShipSize.Huge) { upgradesToRemove.Add(upgrade.Key); }
                    }
                }
                if(upgrade.Value.shipSize == ShipSize.Huge) { upgradesToRemove.Add(upgrade.Key); }
            }
            foreach(int upgradeToRemove in upgradesToRemove)
            {
                upgrades.Remove(upgradeToRemove);
            }
        }

        private static Dictionary<int, int> LoadUpgradesOwned()
        {
            string[] allUpgradesOwned = FileHandler.LoadFile("upgradesowned.txt");
            if (allUpgradesOwned == null) { return null; }

            Dictionary<int, int> upgradeKeyOwned = new Dictionary<int, int>();
            if (allUpgradesOwned != null)
            {
                if (allUpgradesOwned.Count() > 0)
                {
                    foreach (string upgradeOwnedString in allUpgradesOwned)
                    {
                        string[] upgradeOwnedInfo = upgradeOwnedString.Split(',');
                        upgradeKeyOwned[Int32.Parse(upgradeOwnedInfo[0])] = Int32.Parse(upgradeOwnedInfo[2]);
                    }
                }
            }
            return upgradeKeyOwned;
        }

        public static void SaveNumberOfUpgradesOwned()
        {
            string numberOfUpgradesOwned = "";
            foreach (Upgrade upgrade in upgrades.Values.OrderBy(upgrade => upgrade.upgradeType).ThenByDescending(upgrade => upgrade.cost).ThenBy(upgrade => upgrade.name).ToList())
            {
                numberOfUpgradesOwned += upgrade.id.ToString() + "," + upgrade.name + "," + upgrade.numberOwned.ToString() + "," + upgrade.faction.ToString() + "," + upgrade.shipSize.ToString() + "," + upgrade.upgradeType.ToString();
                numberOfUpgradesOwned += System.Environment.NewLine;
            }
            FileHandler.SaveFile("upgradesowned.txt", numberOfUpgradesOwned);
        }

        public static Upgrade GetRandomUpgrade()
        {
            List<int> keyList = new List<int>(upgrades.Keys);
            Upgrade randomUpgrade = upgrades[keyList[Rng.Next(keyList.Count)]];
            while(true)
            {
                if(randomUpgrade.shipSize == ShipSize.Huge || randomUpgrade.upgradeType == UpgradeType.Hardpoint || randomUpgrade.upgradeType == UpgradeType.Team)
                {
                    randomUpgrade = upgrades[keyList[Rng.Next(keyList.Count)]];
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

            foreach(Upgrade upgrade in build.pilots[uniquePilotId].upgrades.Values.ToList())
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

        private static bool IsUpgradeUsable(UniquePilot uniquePilot, Upgrade upgrade, bool isRemovingUpgrades = false)
        {
            bool isUpgradeUsable = true;

            if (uniquePilot.pilotSkill < upgrade.requiresPilotSkill) { isUpgradeUsable = false; }
            if (upgrade.faction != Faction.All && upgrade.faction != uniquePilot.pilot.faction) { isUpgradeUsable = false; }
            if (upgrade.shipSize != ShipSize.All && upgrade.shipSize != uniquePilot.pilot.ship.shipSize) { isUpgradeUsable = false; }

            if(uniquePilot.upgrades.ContainsValue(upgrade) && upgrade.isLimited && isRemovingUpgrades == false) { isUpgradeUsable = false; }

            // Check if the pilot has a slot for the upgrade
            if(uniquePilot.possibleUpgrades.ContainsKey(upgrade.upgradeType))
            {
                if (isRemovingUpgrades)
                {
                    isUpgradeUsable = (uniquePilot.possibleUpgrades[upgrade.upgradeType] >= 0) ? isUpgradeUsable : false;
                }
                else
                {
                    isUpgradeUsable = (upgrade.numberOfUpgradeSlots <= uniquePilot.possibleUpgrades[upgrade.upgradeType]) ? isUpgradeUsable : false;
                }
            }
            else
            {
                isUpgradeUsable = false;
            }

            bool isCorrectShipType = false;
            if ((upgrade.shipThatCanUse.Contains(ShipType.All) || (upgrade.shipThatCanUse.Contains(uniquePilot.pilot.ship.shipType)))
                    && (upgrade.isTieOnly == false || (upgrade.isTieOnly == true && uniquePilot.pilot.ship.isTIE))
                    && (upgrade.isXWingOnly == false || (upgrade.isXWingOnly == true && uniquePilot.pilot.ship.isXWing))
                    ) { isCorrectShipType = true; }
            if (isCorrectShipType == false) { isUpgradeUsable = false; }

            if (upgrade.requiresActions.Count > 0)
            {
                foreach (Action requiredAction in upgrade.requiresActions)
                {
                    if (uniquePilot.usableActions.Exists(action => action == requiredAction) == false)
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
                    if (uniquePilot.upgrades.Values.ToList().Exists(upgradeElement => upgradeElement.id == requiredupgrade) == false)
                    {
                        isUpgradeUsable = false;
                        break;
                    }
                }
            }

            if (UpgradeModifiers.SkipGetUpgrade(uniquePilot, upgrade, isRemovingUpgrades))
            {
                isUpgradeUsable = false;
            }

            return isUpgradeUsable;
        }

        public static List<Upgrade> GetUpgrades(UniquePilot uniquePilot)
        {
            List<Upgrade> upgradesToReturn = new List<Upgrade>();
            foreach (Upgrade upgrade in upgrades.Values.ToList())
            {
                if(IsUpgradeUsable(uniquePilot, upgrade))
                {
                    upgradesToReturn.Add(upgrade);
                }
            }

            return upgradesToReturn;
        }
    }
}