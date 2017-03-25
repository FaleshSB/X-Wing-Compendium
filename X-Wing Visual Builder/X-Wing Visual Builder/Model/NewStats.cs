using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    class NewStats
    {
        private List<DieFace> dieFaces = new List<DieFace>();
        private RollType rollType;
        private int numberOfDice = 3;
        public bool isFocused = true;

        public NewStats(RollType rollType)
        {
            this.rollType = rollType;

            dieFaces.Add(DieFace.Blank);
            dieFaces.Add(DieFace.Blank);
            dieFaces.Add(DieFace.Focus);
            dieFaces.Add(DieFace.Focus);
            dieFaces.Add(DieFace.Hit);
            dieFaces.Add(DieFace.Hit);
            dieFaces.Add(DieFace.Hit);
            dieFaces.Add(DieFace.Crit);
        }

        public void go()
        {
            AllDiceRollResults allDiceRollResults = GetAllDiePossibilities(new AllDiceRollResults(), 1, numberOfDice, new Dictionary<int, DieFace>());
            List<AllDiceRollResults> allReRolls = new List<AllDiceRollResults>();
            foreach (DiceRollResult diceRollResult in allDiceRollResults.diceRollResults)
            {
                // If some dice failed then reroll them
                if (diceRollResult.numberOfFailedResults > 0)
                {
                    AllDiceRollResults allReRollResults = GetAllDiePossibilities(new AllDiceRollResults(), 1, CalculateNumberOfDiceToReRoll(diceRollResult.failedResults), new Dictionary<int, DieFace>());
                    // Keeps the id of the origional roll, e.g. BlankBlankHit, so that the number of times that result happened can be changed to the reroll array
                    allReRollResults.reRollId = diceRollResult.uniqueId;
                    allReRollResults.reRollOrigionalNumberOfResults = diceRollResult.numberOfResults;
                    foreach (DiceRollResult reRollResult in allReRollResults.diceRollResults)
                    {
                        // If there were some successes in the roll then add them to the rerolled results
                        foreach (KeyValuePair<int, DieFace> successfulResult in diceRollResult.successfulResults)
                        {
                            int key = reRollResult.dieFaces.Count + 1;
                            reRollResult.dieFaces.Add(key, successfulResult.Value);
                            reRollResult.dieFaceResults.Add(key, diceRollResult.dieFaceResults[successfulResult.Key]);
                        }
                    }
                    allReRolls.Add(allReRollResults);
                }
            }
            // Remove the results that have been rerolled
            foreach (AllDiceRollResults reRoll in allReRolls)
            {
                allDiceRollResults.RemoveDiceRollResult(reRoll.reRollId);
            }
            foreach (AllDiceRollResults reRoll in allReRolls)
            {
                double totalNumberOfResults = reRoll.totalNumberOfResults;
                foreach (DiceRollResult reRollResult in reRoll.diceRollResults)
                {
                    reRollResult.numberOfResults = (reRoll.reRollOrigionalNumberOfResults / totalNumberOfResults) * reRollResult.numberOfResults;
                    allDiceRollResults.AddDiceRollResult(reRollResult);
                }
            }
            Dictionary<int, double> finalPercentageChangesOfSuccess = new Dictionary<int, double>();
            foreach (DiceRollResult diceRollResult in allDiceRollResults.diceRollResults)
            {
                if(finalPercentageChangesOfSuccess.ContainsKey(diceRollResult.numberOfSuccessfulResults) == true)
                {
                    finalPercentageChangesOfSuccess[diceRollResult.numberOfSuccessfulResults] += (diceRollResult.numberOfResults / allDiceRollResults.totalNumberOfResults) * 100;
                }
                else
                {
                    finalPercentageChangesOfSuccess[diceRollResult.numberOfSuccessfulResults] = (diceRollResult.numberOfResults / allDiceRollResults.totalNumberOfResults) * 100;
                }
            }
            int i = 0;
        }

        private int CalculateNumberOfDiceToReRoll(Dictionary<int, DieFace> failedResults)
        {
            return 9;
        }

        private void SetWhichDieFacesSucceeded(DiceRollResult diceRollResult)
        {
            if (rollType == RollType.Attack)
            {
                foreach (KeyValuePair<int, DieFace> dieNumberDieFace in diceRollResult.dieFaces)
                {
                    switch (dieNumberDieFace.Value)
                    {
                        case DieFace.Blank:
                            diceRollResult.dieFaceResults[dieNumberDieFace.Key] = DieResult.Failure;
                            break;
                        case DieFace.Focus:
                            if (isFocused == true) { diceRollResult.dieFaceResults[dieNumberDieFace.Key] = DieResult.Success; }
                            else { diceRollResult.dieFaceResults[dieNumberDieFace.Key] = DieResult.Failure; }
                            break;
                        case DieFace.Hit:
                            diceRollResult.dieFaceResults[dieNumberDieFace.Key] = DieResult.Success;
                            break;
                        case DieFace.Crit:
                            diceRollResult.dieFaceResults[dieNumberDieFace.Key] = DieResult.Success;
                            break;
                    }
                }
            }
        }

        private AllDiceRollResults GetAllDiePossibilities(AllDiceRollResults allDiceRollResults, int currentDie, int numberOfDice, Dictionary<int, DieFace> currentRoll)
        {
            foreach (DieFace dieFace in dieFaces)
            {
                currentRoll[currentDie] = dieFace;
                if (currentDie < numberOfDice)
                {
                    GetAllDiePossibilities(allDiceRollResults, currentDie + 1, numberOfDice, currentRoll);
                }
                // Only register the result when all die have a value
                else
                {
                    DiceRollResult diceRollResult = new DiceRollResult();
                    diceRollResult.numberOfResults = 1;
                    foreach (KeyValuePair<int, DieFace> dieNumberDieFace in currentRoll)
                    {
                        diceRollResult.dieFaces[dieNumberDieFace.Key] = dieNumberDieFace.Value;
                    }
                    SetWhichDieFacesSucceeded(diceRollResult);                    
                    allDiceRollResults.AddDiceRollResult(diceRollResult);
                }
            }
            return allDiceRollResults;
        }
    }
}
