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
        private int NumberOfDice;
        public int numberOfDice { get { return NumberOfDice; } set { if (value < 1) { NumberOfDice = 1; currentDiceFaces = new DieFace[1 + 1]; } else if (value > 10) { NumberOfDice = 10; currentDiceFaces = new DieFace[10 + 1]; } else { NumberOfDice = value; currentDiceFaces = new DieFace[value + 1]; } } }
        public bool isFocused { get; set; } = false;
        public bool isTargetLocked { get; set; } = false;
        
        private List<DieFace> dieFaces = new List<DieFace>();
        private int currentDie = 1;
        private DieFace[] currentDiceFaces;
        private Dictionary<string, double> allDieRollCombinations = new Dictionary<string, double>();
        private Dictionary<string, Dictionary<string, double>> currentReRollDieRollCombinations = new Dictionary<string, Dictionary<string, double>>();

        public Stats(RollType rollType)
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
            numberOfDice = 3;
        }

        public Dictionary<int, double> Calculate()
        {
            GetAllDiePossibilities(currentDie, numberOfDice);

            AddAllReRollPossibilities();
            return CalculateAttackPercentages();
        }

        private void AddAllReRollPossibilities()
        {
            foreach (KeyValuePair<string, double> dieRollCombination in allDieRollCombinations)
            {
                currentReRollDieRollCombinations[dieRollCombination.Key] = new Dictionary<string, double>();
                double numberOfResults = dieRollCombination.Value;
                string[] splitResults = dieRollCombination.Key.Split(';');
                Dictionary<DieFace, int> origionalResults = new Dictionary<DieFace, int>();
                Dictionary<DieFace, int> resultsToKeep = new Dictionary<DieFace, int>();
                Dictionary<DieFace, int> resultsToReRoll = new Dictionary<DieFace, int>();
                foreach (DieFace dieFace in Enum.GetValues(typeof(DieFace)))
                {
                    origionalResults[dieFace] = 0;
                    resultsToKeep[dieFace] = 0;
                    resultsToReRoll[dieFace] = 0;
                }
                foreach (string dieFace in splitResults)
                {
                    origionalResults[(DieFace)Enum.Parse(typeof(DieFace), dieFace)]++;
                }

                Dictionary<DieFace, int> numberOfResultToReRoll = GetNumberOfResultToReRoll();

                foreach (KeyValuePair<DieFace, int> origionalResult in origionalResults)
                {
                    resultsToKeep[origionalResult.Key] = Math.Max(0, origionalResult.Value - numberOfResultToReRoll[origionalResult.Key]);
                    resultsToReRoll[origionalResult.Key] = origionalResult.Value - resultsToKeep[origionalResult.Key];
                }

                List<string> resultsKept = new List<string>();
                foreach (KeyValuePair<DieFace, int> resultToKeep in resultsToKeep)
                {
                    for (int i = 0; i < resultToKeep.Value; i++)
                    {
                        resultsKept.Add(resultToKeep.Key.ToString());
                    }
                }
                if (numberOfDice - resultsKept.Count < 1)
                {
                    continue;
                }
                GetAllReRollPossibilities(1, numberOfDice - resultsKept.Count, resultsKept, dieRollCombination.Key);
            }

            Dictionary<string, double> numberOfResultsReference = new Dictionary<string, double>(allDieRollCombinations);
            foreach (KeyValuePair<string, Dictionary<string, double>> currentReRollDieRollCombination in currentReRollDieRollCombinations)
            {
                if (currentReRollDieRollCombination.Value.Count > 0)
                {
                    allDieRollCombinations[currentReRollDieRollCombination.Key] = 0;
                }
            }
            foreach (KeyValuePair<string, Dictionary<string, double>> currentReRollDieRollCombination in currentReRollDieRollCombinations)
            {
                if (currentReRollDieRollCombination.Value.Count > 0)
                {
                    double totalCombinations = 0;
                    foreach (KeyValuePair<string, double> individualReRollDieRollCombination in currentReRollDieRollCombination.Value)
                    {
                        totalCombinations += individualReRollDieRollCombination.Value;
                    }
                    foreach (KeyValuePair<string, double> individualReRollDieRollCombination in currentReRollDieRollCombination.Value)
                    {
                        allDieRollCombinations[individualReRollDieRollCombination.Key] += (numberOfResultsReference[individualReRollDieRollCombination.Key] / totalCombinations) * individualReRollDieRollCombination.Value;
                    }
                }
            }
        }

        private void GetAllReRollPossibilities(int currentDie, int numberOfDice, List<string> otherResults, string key)
        {
            for (int dieFace = 0; dieFace < dieFaces.Count; dieFace++)
            {
                currentDiceFaces[currentDie] = dieFaces[dieFace];
                if (currentDie < numberOfDice)
                {
                    GetAllReRollPossibilities(currentDie + 1, numberOfDice, otherResults, key);
                }
                // Only register the result when all die have a value
                else
                {
                    string combination = null;
                    List<string> sortList = new List<string>(otherResults);
                    for (int y = 1; y <= numberOfDice; y++)
                    {
                        sortList.Add(currentDiceFaces[y].ToString());
                    }
                    sortList.Sort(delegate (string stringOne, string stringTwo) { return stringOne.CompareTo(stringTwo); });
                    foreach (string dieString in sortList)
                    {
                        combination += ';' + dieString;
                    }
                    combination = combination.TrimStart(';');

                    if (currentReRollDieRollCombinations[key].ContainsKey(combination))
                    {
                        currentReRollDieRollCombinations[key][combination]++;
                    }
                    else
                    {
                        currentReRollDieRollCombinations[key].Add(combination, 1);
                    }
                }
            }
            int i = 0;
        }
        private void GetAllDiePossibilities(int currentDie, int numberOfDice)
        {
            for (int dieFace = 0; dieFace < dieFaces.Count; dieFace++)
            {
                currentDiceFaces[currentDie] = dieFaces[dieFace];
                if (currentDie < numberOfDice)
                {
                    GetAllDiePossibilities(currentDie + 1, numberOfDice);
                }
                // Only register the result when all die have a value
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
                    if (allDieRollCombinations.ContainsKey(combination))
                    {
                        allDieRollCombinations[combination]++;
                    }
                    else
                    {
                        allDieRollCombinations.Add(combination, 1);
                    }
                }
            }
        }


        private Dictionary<DieFace, int> GetNumberOfResultToReRoll()
        {
            Dictionary<DieFace, int> whatToReRoll = new Dictionary<DieFace, int>();
            foreach (DieFace dieFace in Enum.GetValues(typeof(DieFace)))
            {
                switch (dieFace)
                {
                    case DieFace.Blank:
                        if (isTargetLocked == true)
                        {
                            whatToReRoll[dieFace] = 20;
                        }
                        else
                        {
                            whatToReRoll[dieFace] = 0;
                        }
                        break;
                    case DieFace.Focus:
                        if (isFocused == false && isTargetLocked == true)
                        {
                            whatToReRoll[dieFace] = 20;
                        }
                        else
                        {
                            whatToReRoll[dieFace] = 0;
                        }
                        break;
                    case DieFace.Evade:
                        whatToReRoll[dieFace] = 0;
                        break;
                    case DieFace.Hit:
                        whatToReRoll[dieFace] = 0;
                        break;
                    case DieFace.Crit:
                        whatToReRoll[dieFace] = 0;
                        break;
                    default:
                        break;
                }
            }

            return whatToReRoll;
        }


        private Dictionary<int, double> CalculateAttackPercentages()
        {
            Dictionary<int, double> finalPercentages = new Dictionary<int, double>();
            Dictionary<int, double> numberOfSuccessesArray = new Dictionary<int, double>();
            double totalNumberOfResults = 0;

            foreach (KeyValuePair<string, double> result in allDieRollCombinations)
            {
                totalNumberOfResults += result.Value;
                double numberOfResults = result.Value;
                string[] splitResults = result.Key.Split(';');
                int successes = 0;
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
                if (numberOfSuccessesArray.ContainsKey(successes))
                {
                    numberOfSuccessesArray[successes] += numberOfResults;
                }
                else
                {
                    numberOfSuccessesArray.Add(successes, numberOfResults);
                }
            }

            foreach (KeyValuePair<int, double> successes in numberOfSuccessesArray)
            {
                int finalSuccesses = successes.Key;
                double finalNumberOfResults = successes.Value;

                if (finalPercentages.ContainsKey(finalSuccesses))
                {
                    finalPercentages[finalSuccesses] += finalNumberOfResults / (totalNumberOfResults / 100);
                }
                else
                {
                    finalPercentages.Add(finalSuccesses, finalNumberOfResults / (totalNumberOfResults / 100));
                }
            }

            return finalPercentages;
        }
    }
}
