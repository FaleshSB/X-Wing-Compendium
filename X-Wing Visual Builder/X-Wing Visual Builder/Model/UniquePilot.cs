using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    public class UniquePilot
    {
        public Pilot pilot;
        public int uniqueId;
        public int totalCost
        {
            get
            {
                int totalCost = 0;
                totalCost += pilot.cost;
                foreach (Upgrade upgrade in upgrades.Values.ToList())
                {
                    totalCost += UpgradeModifiers.UpgradeCost(upgrade, upgrades.Values.ToList());
                }
                return totalCost;
            }
        }
        public int id;
        public int pilotSkill
        {
            get
            {
                int pilotSkill = pilot.pilotSkill;
                foreach (Upgrade upgrade in upgrades.Values.ToList())
                {
                    pilotSkill += upgrade.addsPilotSkill;
                }
                return pilotSkill;
            }
            set
            {
                pilot.pilotSkill = value;
            }
        }
        public List<Action> usableActions
        {
            get
            {
                List<Action> usableActions = new List<Action>(pilot.ship.actions);
                foreach (Upgrade upgrade in upgrades.Values.ToList())
                {
                    usableActions.AddRange(upgrade.addsActions);
                }

                return usableActions;
            }
        }
        public Dictionary<UpgradeType, int> possibleUpgrades
        {
            get
            {
                Dictionary<UpgradeType, int> possibleUpgrades = new Dictionary<UpgradeType, int>(pilot.possibleUpgrades);
                foreach (Upgrade upgrade in upgrades.Values.ToList())
                {
                    if (possibleUpgrades.ContainsKey(upgrade.upgradeType))
                    {
                        possibleUpgrades[upgrade.upgradeType] -= upgrade.numberOfUpgradeSlots;
                    }

                    foreach (KeyValuePair<UpgradeType, int> upgradeAdded in upgrade.upgradesAdded)
                    {
                        if (possibleUpgrades.ContainsKey(upgradeAdded.Key))
                        {
                            possibleUpgrades[upgradeAdded.Key] += upgradeAdded.Value;
                        }
                        else
                        {
                            possibleUpgrades[upgradeAdded.Key] = 0 + upgradeAdded.Value;
                        }
                    }
                    foreach (KeyValuePair<UpgradeType, int> upgradeRemoved in upgrade.upgradesRemoved)
                    {
                        if (possibleUpgrades.ContainsKey(upgradeRemoved.Key))
                        {
                            possibleUpgrades[upgradeRemoved.Key] -= upgradeRemoved.Value;
                        }
                        else
                        {
                            possibleUpgrades[upgradeRemoved.Key] = 0 - upgradeRemoved.Value;
                        }
                    }

                }
                UpgradeModifiers.ChangePossibleUpgrades(this, possibleUpgrades);
                return possibleUpgrades;
            }
        }
        public Dictionary<int, Upgrade> upgrades = new Dictionary<int, Upgrade>();

        public UniquePilot(Pilot pilot, int id)
        {
            this.pilot = pilot;
            this.id = id;
        }
    }
}
