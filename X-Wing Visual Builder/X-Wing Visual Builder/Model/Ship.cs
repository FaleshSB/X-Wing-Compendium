using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    public class Ship
    {
        public ShipType shipType { get; set; }
        public ShipSize shipSize { get; set; }
        public bool isTie { get; set; }
        public bool isXWing { get; set; }
        public string name { get; set; }
        public int agility { get; set; }

        public Ship(int id, ShipType shipType, string name, ShipSize shipSize, bool isTIE, bool isXWing, int attack, int agility, int hull, int shields, List<Action> actions, Faction faction)
        {
            this.shipType = shipType;
            
        }
    }
}
