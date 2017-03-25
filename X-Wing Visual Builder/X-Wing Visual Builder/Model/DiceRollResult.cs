using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    class DiceRollResult
    {
        public int numberOfSuccessfulResults
        {
            get
            {
                return successfulResults.Count;
            }
        }
        public Dictionary<int, DieFace> successfulResults
        {
            get
            {
                Dictionary<int, DieFace> successfulResults = new Dictionary<int, DieFace>();
                foreach(KeyValuePair<int, DieResult> dieNumberDieResult in dieFaceResults)
                {
                    if(dieNumberDieResult.Value == DieResult.Success)
                    {
                        successfulResults[dieNumberDieResult.Key] = dieFaces[dieNumberDieResult.Key];
                    }
                }
                return successfulResults;
            }
        }
        public int numberOfFailedResults
        {
            get
            {
                return failedResults.Count;
            }
        }
        public Dictionary<int, DieFace> failedResults
        {
            get
            {
                Dictionary<int, DieFace> failedResults = new Dictionary<int, DieFace>();
                foreach (KeyValuePair<int, DieResult> dieNumberDieResult in dieFaceResults)
                {
                    if (dieNumberDieResult.Value == DieResult.Failure || dieNumberDieResult.Value == DieResult.UsedFail)
                    {
                        failedResults[dieNumberDieResult.Key] = dieFaces[dieNumberDieResult.Key];
                    }
                }
                return failedResults;
            }
        }
        public string uniqueId
        {
            get
            {
                string uniqueId = "";
                List<DieFace> dieFaces = this.dieFaces.Values.ToList();
                dieFaces.Sort();
                foreach (DieFace dieFace in dieFaces)
                {
                    uniqueId += dieFace.ToString();
                }
                return uniqueId;
            }
        }
        public double numberOfResults = 0;
        public Dictionary<int, DieFace> dieFaces = new Dictionary<int, DieFace>();
        public Dictionary<int, DieResult> dieFaceResults = new Dictionary<int, DieResult>();
    }
}
