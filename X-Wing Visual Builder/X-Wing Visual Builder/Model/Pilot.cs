﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace X_Wing_Visual_Builder.Model
{
    public class Pilot : Card
    {
        public Ship ship;
        public bool isUnique;
        public int pilotSkill;
        public Dictionary<UpgradeType, int> possibleUpgrades = new Dictionary<UpgradeType, int>();
        public bool hasAbility;
        private int _numberOwned;
        public int numberOwned
        {
            get { return _numberOwned; }
            set { _numberOwned = (value < 0) ? 0 : value; Pilots.SaveNumberOfPilotsOwned(); foreach (CardCanvas cardCanvas in cardCanvasList) { cardCanvas.UpdateNumberOwned(); } }
        }

        public Pilot(int id, ShipType shipType, bool isUnique, string name, int pilotSkill, string description, Dictionary<UpgradeType, int> possibleUpgrades, int cost,
                     List<string> faq, Faction faction, bool hasAbility, int numberOwned, List<ExpansionType> inExpansion)
        {
            this.isUpgrade = false;
            this.cardImageUri = new Uri(@"D:\Documents\Game Stuff\X-Wing\Pilot Cards\" + id.ToString() + ".png");
            this.id = id;
            this.isUnique = isUnique;
            this.name = name;
            this.pilotSkill = pilotSkill;
            this.description = description;
            this.possibleUpgrades = possibleUpgrades;
            this.cost = cost;
            this.faq = faq;
            this.faction = faction;
            this.hasAbility = hasAbility;
            this._numberOwned = numberOwned;
            this.ship = Ships.ships[shipType][faction];

            foreach (ExpansionType expansionType in inExpansion)
            {
                if (this.inExpansion.ContainsKey(expansionType) == false)
                {
                    this.inExpansion[expansionType] = 1;
                }
                else
                {
                    this.inExpansion[expansionType]++;
                }
            }
        }
    }
}
