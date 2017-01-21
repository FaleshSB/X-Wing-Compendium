using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    class Pilot
    {
        private Faction faction;
        private UpgradeType[] upgrades;
        private Action[] actions;
        private Ship ship;

        private byte cost;
        private string pilotAbility;

        // TODO add where you can buy this card
        // TODO add maneuver card array[5][9] maneuver[4][0] = Maneuver.Green is a green 4 turn left, maneuver[5][8] = Maneuver.Red is a red 5 K turn    
        /*
        private byte pilotSkill;
        private byte primaryWeaponValue;
        private byte agility;
        private byte hull;
        private byte shields;
        */
    }
}
