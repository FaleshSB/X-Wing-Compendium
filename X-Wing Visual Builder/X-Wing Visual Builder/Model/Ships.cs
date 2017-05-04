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
    static class Ships
    {
        public static Dictionary<ShipType, Dictionary<Faction, Ship>> ships = new Dictionary<ShipType, Dictionary<Faction, Ship>>();
        public static Dictionary<string, List<Ship>> maneuverCardIndexedShipList = new Dictionary<string, List<Ship>>();

        static Ships()
        {
            using (TextFieldParser parser = new TextFieldParser(new StringReader(Properties.Resources.ShipsDatabase)))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("£");
                parser.HasFieldsEnclosedInQuotes = false;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    List<Action> actions = new List<Action>();
                    if (fields[10].Length > 0)
                    {
                        string[] actionsSplit = fields[10].Split(',');
                        foreach (string action in actionsSplit)
                        {
                            actions.Add((Action)Int32.Parse(action));
                        }
                    }

                    string[] maneuverDistanceSplit = fields[12].Split('|');
                    string uniqueManeuverId = "";
                    Dictionary<int, List<int>> maneuvers = new Dictionary<int, List<int>>();
                    foreach (string maneuverDistance in maneuverDistanceSplit)
                    {
                        string[] maneuverSplit = maneuverDistance.Split(',');
                        int maneuverDistanceKey = Int32.Parse(maneuverSplit[0]);
                        bool isDistanceKey = true;
                        foreach (string maneuver in maneuverSplit)
                        {
                            if(isDistanceKey) { isDistanceKey = false; maneuvers[maneuverDistanceKey] = new List<int>(); continue; }

                            maneuvers[maneuverDistanceKey].Add(Int32.Parse(maneuver));
                            uniqueManeuverId += maneuver;
                        }
                    }

                    if (ships.ContainsKey((ShipType)Int32.Parse(fields[1])) == false) { ships[(ShipType)Int32.Parse(fields[1])] = new Dictionary<Faction, Ship>(); }

                    Ship currentShip = new Ship(Int32.Parse(fields[0]), (ShipType)Int32.Parse(fields[1]), fields[2], (ShipSize)Int32.Parse(fields[3]), 
                                                Convert.ToBoolean(Int32.Parse(fields[4])), Convert.ToBoolean(Int32.Parse(fields[5])), Int32.Parse(fields[6]),
                                                Int32.Parse(fields[7]), Int32.Parse(fields[8]), Int32.Parse(fields[9]), actions, (Faction)Int32.Parse(fields[11]), maneuvers, uniqueManeuverId);

                    ships[(ShipType)Int32.Parse(fields[1])][(Faction)Int32.Parse(fields[11])] = currentShip;

                    if(maneuverCardIndexedShipList.ContainsKey(uniqueManeuverId) == false) { maneuverCardIndexedShipList[uniqueManeuverId] = new List<Ship>(); }

                    maneuverCardIndexedShipList[uniqueManeuverId].Add(currentShip);
                }
            }
        }

        public static Ship GetRandomShip()
        {
            Array values = Enum.GetValues(typeof(ShipType));
            ShipType randomShipType = (ShipType)values.GetValue(Rng.Next(values.Length));
            while(ships.ContainsKey(randomShipType) == false)
            {
                randomShipType = (ShipType)values.GetValue(Rng.Next(values.Length));
            }
            return ships[randomShipType].First().Value;
        }
    }
}
