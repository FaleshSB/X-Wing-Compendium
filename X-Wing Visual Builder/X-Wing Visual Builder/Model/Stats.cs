using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    class Stats
    {
        private RollType rollType;
        private int NumberOfDice = 3;
        public int numberOfDice { get { return NumberOfDice; } set { if (value < 1) { NumberOfDice = 1; currentDiceFaces = new DieFace[1 + 1]; } else if (value > 20) { NumberOfDice = 20; currentDiceFaces = new DieFace[20 + 1]; } else { NumberOfDice = value; currentDiceFaces = new DieFace[value + 1]; } } }
        public bool isFocused { get; set; } = false;
        public bool isTargetLocked { get; set; } = false;
        
        private List<DieFace> dieFaces = new List<DieFace>();
        private int currentDie = 1;
        private DieFace[] currentDiceFaces;
        private Dictionary<string, int> results = new Dictionary<string, int>();
        
        public void SetAttackingOrDefending(RollType rollType)
        {
            this.rollType = rollType;

            if (rollType == RollType.Attack)
            {
                dieFaces.Add(DieFace.Blank);
                dieFaces.Add(DieFace.Blank);
                dieFaces.Add(DieFace.Focus);
                dieFaces.Add(DieFace.Focus);
                dieFaces.Add(DieFace.Hit);
                dieFaces.Add(DieFace.Hit);
                dieFaces.Add(DieFace.Hit);
                dieFaces.Add(DieFace.Crit);
            }
            else
            {
                dieFaces.Add(DieFace.Blank);
                dieFaces.Add(DieFace.Blank);
                dieFaces.Add(DieFace.Blank);
                dieFaces.Add(DieFace.Focus);
                dieFaces.Add(DieFace.Focus);
                dieFaces.Add(DieFace.Evade);
                dieFaces.Add(DieFace.Evade);
                dieFaces.Add(DieFace.Evade);
            }
        }

        public Dictionary<int, double> Calculate(bool isReRoll = false)
        {
            GetAllDiePossibilities(currentDie);
            return CalculateAttackPercentages(isReRoll);
        }

        private Dictionary<int, double> CalculateAttackPercentages(bool isReRoll)
        {
            Dictionary<int, double> finalPercentages = new Dictionary<int, double>();

            foreach (KeyValuePair<string, int> result in results)
            {
                int numberOfResults = result.Value;
                string[] splitResults = result.Key.Split(';');
                int successes = 0;
                int diceToReRoll = 0;
                foreach (string dieFace in splitResults)
                {
                    if (rollType == RollType.Attack)
                    {
                        if (dieFace == DieFace.Hit.ToString() || dieFace == DieFace.Crit.ToString())
                        {
                            successes++;
                        }
                        else if (isFocused && dieFace == DieFace.Focus.ToString())
                        {
                            successes++;
                        }
                        else if (isReRoll == false && isTargetLocked && (dieFace == DieFace.Blank.ToString() || dieFace == DieFace.Focus.ToString()))
                        {
                            diceToReRoll++;
                        }
                    }
                    else
                    {
                        if (dieFace == DieFace.Evade.ToString())
                        {
                            successes++;
                        }
                        else if (isFocused && dieFace == DieFace.Focus.ToString())
                        {
                            successes++;
                        }
                    }
                }
                Dictionary<int, double> reRollResults = new Dictionary<int, double>();
                if (diceToReRoll > 0)
                {
                    Stats reRollStats = new Stats(rollType);
                    reRollStats.numberOfDice = diceToReRoll;
                    reRollStats.isFocused = isFocused;
                    // rerollStats.hasUsedConc etc
                    // trouble with things that change more then 1 result so that needs to be tracked, e.g. origional.change2dicetohit reroll.change1dietohit
                    // if you need a blank on attacking then don't re-roll
                    // if you need a blank in defending and have 2+ blanks then re-roll all
                    reRollResults = reRollStats.Calculate(true);
                }

                Dictionary<int, double> percentagesArray = new Dictionary<int, double>();

                if (reRollResults.Count > 0)
                {
                    foreach (KeyValuePair<int, double> reRollResult in reRollResults)
                    {
                        percentagesArray.Add(reRollResult.Key + successes, numberOfResults * (reRollResult.Value / 100));
                    }
                }
                else
                {
                    percentagesArray.Add(successes, numberOfResults);
                }

                foreach (KeyValuePair<int, double> percentages in percentagesArray)
                {
                    int finalSuccesses = percentages.Key;
                    double finalNumberOfResults = percentages.Value;

                    if (finalPercentages.ContainsKey(finalSuccesses))
                    {
                        finalPercentages[finalSuccesses] += finalNumberOfResults / (Math.Pow(8, NumberOfDice) / 100);
                    }
                    else
                    {
                        finalPercentages.Add(finalSuccesses, finalNumberOfResults / (Math.Pow(8, NumberOfDice) / 100));
                    }
                }
            }
            return finalPercentages;
        }

        private void GetAllDiePossibilities(int currentDie)
        {
            for (int i = 0; i < dieFaces.Count; i++)
            {
                currentDiceFaces[currentDie] = dieFaces[i];
                if (currentDie < numberOfDice)
                {
                    GetAllDiePossibilities(currentDie + 1);
                }
                else
                {
                    string combination = null;
                    List<string> sortList = new List<string>();
                    for (int y = 1; y <= numberOfDice; y++)
                    {
                        sortList.Add(currentDiceFaces[y].ToString());
                    }
                    sortList.Sort(delegate (string stringOne, string stringTwo){return stringOne.CompareTo(stringTwo);});
                    foreach(string dieString in sortList)
                    {
                        combination += ';' + dieString;
                    }
                    combination = combination.TrimStart(';');
                    if (results.ContainsKey(combination))
                    {
                        results[combination]++;
                    }
                    else
                    {
                        results.Add(combination, 1);
                    }
                }
            }
        }
    }
}
