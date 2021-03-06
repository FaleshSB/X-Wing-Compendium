﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Drawing;


namespace X_Wing_Visual_Builder.Model
{
    public class Upgrade : Card
    {
        public UpgradeType upgradeType;
        public ShipSize shipSize;
        public List<ShipType> shipThatCanUse;
        public bool isWeapon;
        public bool isUnique;
        public bool isLimited;
        public int numberOfUpgradeSlots;
        public bool isDualCard;
        public bool isTieOnly;
        public bool isXWingOnly;
        public Dictionary<UpgradeType, int> upgradesAdded;
        public Dictionary<UpgradeType, int> upgradesRemoved;
        public List<Faction> factions { get; set; }
        public int requiresPilotSkill;
        public List<Action> requiresActions;
        public List<int> requiresUpgrades;
        public List<Action> addsActions;
        public int addsPilotSkill;
        private int _numberOwned;
        public int numberOwned
        {
            get { return _numberOwned; }
            set { _numberOwned = (value < 0) ? 0 : value; Upgrades.SaveNumberOfUpgradesOwned(); foreach (CardCanvas cardCanvas in cardCanvasList) { cardCanvas.UpdateNumberOwned(); } }
        }
        public string canonicalName;


        public Upgrade(int id, UpgradeType upgradeType, int cost, string name, string description, List<string> faq, List<Faction> factions, ShipSize shipSize,
                       List<ShipType> shipThatCanUse, bool isWeapon, bool isUnique, bool isLimited, int numberOfUpgradeSlots, bool isDualCard, bool isTieOnly, bool isXWingOnly,
                       Dictionary<UpgradeType, int> upgradesAdded, Dictionary<UpgradeType, int> upgradesRemoved, int requiresPilotSkill, List<Action> requiresActions,
                       List<int> requiresUpgrades, List<Action> addsActions, int addsPilotSkill, int numberOwned, List<ExpansionType> inExpansion, string canonicalName)
        {
            this.isUpgrade = true;
            this.imageFilePath = @"Upgrade Cards\" + id.ToString();
            this.id = id;
            this.upgradeType = upgradeType;
            this.cost = cost;
            this.name = name;
            this.description = description;
            this.faq = faq;
            this.factions = factions;
            this.shipSize = shipSize;
            this.shipThatCanUse = shipThatCanUse;
            this.isWeapon = isWeapon;
            this.isUnique = isUnique;
            this.isLimited = isLimited;
            this.numberOfUpgradeSlots = numberOfUpgradeSlots;
            this.isDualCard = isDualCard;
            this.isTieOnly = isTieOnly;
            this.isXWingOnly = isXWingOnly;
            this.upgradesAdded = upgradesAdded;
            this.upgradesRemoved = upgradesRemoved;
            this.requiresPilotSkill = requiresPilotSkill;
            this.requiresActions = requiresActions;
            this.requiresUpgrades = requiresUpgrades;
            this.addsActions = addsActions;
            this.addsPilotSkill = addsPilotSkill;
            this._numberOwned = numberOwned;
            this.canonicalName = canonicalName;

            foreach (ExpansionType expansionType in inExpansion)
            {
                if(this.inExpansion.ContainsKey(expansionType) == false)
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
