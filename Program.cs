
using System.Dynamic;
using System.Security.Cryptography;

namespace MachineLearning2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Evolution evo = new Evolution(80);
            for (int i = 0; i < 30; i++)
            {
                evo.EvolvePopulation();
            }
            AI ai = new AI(Symbol.O);
            List<float> pop = new List<float>();
            pop.AddRange(evo.FitnessValues);
            pop.Sort();
            pop.Reverse();

            for (int i = 0; i < evo.PopulationSize; i++)
            {
                if (pop[0] == evo.FitnessValues[i])
                {
                    ai = evo.Population[i];
                    break;
                }
            }
            while (true)
            {
                TicTacToe Game = new TicTacToe(Symbol.X, ai);
                Victor vic = Victor.Continues;
                while (vic == Victor.Continues)
                {
                    Console.Write("Hamle giriniz: ");
                    vic = Game.AIvsPlayer(int.Parse(Console.ReadLine()));
                    Game.Print();
                }
                Console.WriteLine("\nKazanan: " + vic.ToString());
            }

            
            //Console.WriteLine("\n\n\n");
        }
    }
}