using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MachineLearning2
{
    internal class AI
    {
        public NeuronNetwork NeuralNetwork;
        public Symbol symbol;
        public float Fitness;
        Random rand;

        public AI(Symbol symbol)
        {
            this.symbol = symbol;
            NeuralNetwork = new NeuronNetwork(30, 2, 10, 9);
        }

        public AI(NeuronNetwork network, Symbol symbol, bool mutate)
        {
            rand = new Random();
            NeuralNetwork = network;
            this.symbol = symbol;
            if (mutate && rand.Next() % 250 == 0) NeuralNetwork.Mutate();
        }

        public AI(AI ai, bool mutate)
        {
            rand = new Random();
            NeuralNetwork = new NeuronNetwork(ai.NeuralNetwork);
            symbol = ai.symbol;
            if (mutate && rand.Next() % 250 == 0) NeuralNetwork.Mutate();
        }
        
        public int GetMove(Symbol[] symbols)
        {
            List<float> input = new List<float>();
            for (int i = 0; i < 9; i++)
            {
                input.Add((float)symbols[i] * 10f);
            }
            input.Add((float)symbol * 10f);
            List<float> result = new List<float>();
            result.AddRange(NeuralNetwork.Activate(input.ToArray()));
            List<float> scores = new List<float>();
            scores.AddRange(result);
            scores.Sort();
            scores.Reverse();
            
            for (int k = 0; k < 9; k++)
            {
                int j = result.IndexOf(scores[k]);
                if (symbols[j] == Symbol.E)
                {
                    return j;
                }
            }
            
            return -1;
        }
    }
}
