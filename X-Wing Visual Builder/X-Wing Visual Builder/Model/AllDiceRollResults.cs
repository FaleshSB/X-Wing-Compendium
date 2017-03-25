using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    class AllDiceRollResults
    {
        public List<DiceRollResult> diceRollResults { get; private set; } = new List<DiceRollResult>();
        public string reRollId;
        public double reRollOrigionalNumberOfResults;
        public double totalNumberOfResults
        {
            get
            {
                double totalNumberOfResults = 0;
                foreach (DiceRollResult diceRollResult in diceRollResults)
                {
                    totalNumberOfResults += diceRollResult.numberOfResults;
                }

                return totalNumberOfResults;
            }
        }

        public void AddDiceRollResult(DiceRollResult newDiceRollResult)
        {
            bool resultAlreadyExists = false;
            foreach(DiceRollResult diceRollResult in diceRollResults)
            {
                if(diceRollResult.uniqueId == newDiceRollResult.uniqueId)
                {
                    resultAlreadyExists = true;
                    diceRollResult.numberOfResults += newDiceRollResult.numberOfResults;
                    break;
                }
            }
            if(resultAlreadyExists == false) { diceRollResults.Add(newDiceRollResult); }
        }
        public void RemoveDiceRollResult(string uniqueId)
        {
            diceRollResults.RemoveAll(diceRollResult => diceRollResult.uniqueId == uniqueId);
        }
    }
}
