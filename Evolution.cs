using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning2
{
    internal class Evolution
    {
        public int PopulationSize;
        public List<AI> Population;
        public int Generation;
        public GameData[,] Result;
        public float[] FitnessValues;
        public TicTacToe Game;
        Random rand;

        public Evolution(int populationSize)
        {
            PopulationSize = populationSize;
            Game = new TicTacToe();
            rand = new Random();
            Generation = 0;
            CreatePopulation();
        }

        public float GetAvaregeFitness()
        {
            float t = 0;

            for (int i = 0; i < Population.Count; i++)
            {
                t += FitnessValues[i];
            }

            float max = -10;
            for (int i = 0; i < Population.Count; i++)
            {
                if (FitnessValues[i] > max)
                {
                    max = FitnessValues[i];
                }
            }

            return (t + max) / (2 * Population.Count);
        }

        public void CreatePopulation()
        {
            Population = new List<AI>();
            for (int i = 0; i < PopulationSize; i++)
            {
                Population.Add(new AI(Symbol.E));
            }
        }

        public void EvolvePopulation()
        {
            Result = new GameData[PopulationSize,PopulationSize];
            for (int i = 0; i < PopulationSize; i++)
            {
                for (int k = 0; k < PopulationSize; k++)
                {
                    List<AI> list = new List<AI>(); // this variable provide determine X and O randomly
                    list.Add(Population[i]);
                    list.Add(Population[k]);
                    if (rand.Next() % 2 == 1) list.Reverse();
                    Result[i, k] = Game.Simulate(list[0], list[1]);
                }
            }
            CalculateFitnessValues();
            EliminateDisadvantaged();
            RecreatePopulation();
            Generation++;
            Console.WriteLine(Generation + ".Nesildeki Ortalama Uyum = " + GetAvaregeFitness());
        }

        public void CalculateFitnessValues()
        {
            // win = 20  lose = 0  scoreless = 5
            FitnessValues = new float[PopulationSize];
            for (int i = 0; i < PopulationSize; i++)
            {
                for (int k = 0; k < PopulationSize; k++)
                {
                    float a = 0;
                    if (Result[i, k].Result == Victor.Scoreless) a = 5; else if (Result[i, k].AI_X.NeuralNetwork == Population[i].NeuralNetwork) a = 20;
                    FitnessValues[i] += a;
                }
                Population[i].Fitness = FitnessValues[i];
            }
        }

        public void EliminateDisadvantaged() // Eliminates Disadvantaged AIs by fitness values
        {
            List<float> fitness = new List<float>();
            fitness.AddRange(FitnessValues);
            fitness.Sort();
            fitness.Reverse();
            
            float average = GetAvaregeFitness();
            for (int k = PopulationSize-1; k >= 0; k--)
            {
                if (FitnessValues[k] <= fitness[6])
                {
                    Population.RemoveAt(k);
                }
            }
        }
       

        public void RecreatePopulation()
        {
            int missingPopulation = PopulationSize - Population.Count;

            for (int i = 0; i < missingPopulation; i++)
            {
                int p0 = rand.Next() % Population.Count;
                int p1 = rand.Next() % Population.Count;
                Population.Add(new AI(NeuronNetwork.CrossNeuronNeworks(Population[p0].NeuralNetwork, Population[p1].NeuralNetwork), Symbol.E, true));
            }
        }

        public void Mutate(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Population[rand.Next() % PopulationSize].NeuralNetwork.Mutate();
            }
        }
    }
}
