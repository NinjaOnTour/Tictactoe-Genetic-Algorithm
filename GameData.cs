using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning2
{
    internal class GameData
    {
        public AI AI_X;
        public AI AI_O;
        public Victor Result;
        public int RoundCount;

        public GameData(AI aI_X, AI aI_O, Victor result, int roundCount)
        {
            AI_X = aI_X;
            AI_O = aI_O;
            Result = result;
            RoundCount = roundCount;
        }
    }
}
