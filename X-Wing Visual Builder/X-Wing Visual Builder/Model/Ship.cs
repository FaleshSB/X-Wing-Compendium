using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    public class Ship
    {
        public int id;
        public ShipType shipType;
        public string name;
        public ShipSize shipSize;
        public bool isTIE;
        public bool isXWing;
        public int attack;
        public int agility;
        public int hull;
        public int shields;
        public List<Action> actions = new List<Action>();
        public Faction faction;

        public Ship(int id, ShipType shipType, string name, ShipSize shipSize, bool isTIE, bool isXWing, int attack, int agility, int hull, int shields, List<Action> actions, Faction faction)
        {
            this.id = id;
            this.shipType = shipType;
            this.name = name;
            this.shipSize = shipSize;
            this.isTIE = isTIE;
            this.isXWing = isXWing;
            this.attack = attack;
            this.agility = agility;
            this.hull = hull;
            this.shields = shields;
            this.actions = actions;
            this.faction = faction;
        }
    }
}
