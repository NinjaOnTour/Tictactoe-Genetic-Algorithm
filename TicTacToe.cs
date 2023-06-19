using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning2
{
    public enum Symbol
    {
        E = 10, // Empty
        X = 80,
        O = 200
    }

    public enum Victor
    {
        Continues = 10,
        X = 80,
        O = 200,
        Scoreless = 300
    }

    internal class TicTacToe
    {
        public Symbol[] Symbols;
        public Symbol Player;
        public int CurrentRound;
        public AI ai;

        private static readonly int[,] CheckPositions =
        {
            { 0, 1, 2 }, 
            { 3, 4, 5 },
            { 6, 7, 8 },
            { 0, 4, 8 },
            { 2, 4, 6 },
            { 0, 3, 6 },
            { 1, 4, 7 },
            { 2, 5, 8 },
        };
        
        public TicTacToe()
        {
            ai = null;
            ResetGame();
        }

        public TicTacToe(Symbol player, AI ai)
        {
            Player = player;
            this.ai = ai;

            ResetGame();

            if (player == Symbol.O)
            {
                ai.symbol = Symbol.X;
                int a = ai.GetMove(Symbols);
                Symbols[a] = Symbol.X;
            }
            else
            {
                ai.symbol = Symbol.O;
            }
        }

        public Victor Check()
        {
            Victor vic = Victor.Continues;
            int x, y, z;
            for (int i = 0; i < CheckPositions.GetLength(0); i++) // Checks whether there is a victor
            {
                x = CheckPositions[i, 0];
                y = CheckPositions[i, 1];
                z = CheckPositions[i, 2];
                if (Symbols[x] != Symbol.E && Symbols[x] == Symbols[y] && Symbols[y] == Symbols[z])
                {
                    vic = (Victor)Symbols[x];
                }
            }

            if(vic == Victor.Continues) // if vic equals to Victor.Continues there is not a victor
            {
                for (int i = 0; i < 9; i++)
                {
                    if (Symbols[i] == Symbol.E) // if there is an empty slot game continues
                    {
                        break;
                    }
                    else if(i == 8) // if there is not an empty slot and there is not a victor game is scoreless
                    {
                        vic = Victor.Scoreless;
                    }
                }
            }

            return vic;
        }

        public void Print()
        {
            Console.Write("\n");
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Console.Write(Symbols[y*3 + x].ToString() + " ");
                }
                Console.Write("\n");
            }
        }

        public GameData Simulate(AI ai_X, AI ai_O)
        {
            CurrentRound = 0;
            Victor vic = Victor.Continues;
            int a = -1;
            int i = 0;
            int c = 0;
            while(vic == Victor.Continues) 
            {
                c++;
                if (i == 0) // X
                {
                    a = ai_X.GetMove(Symbols);                
                    Symbols[a] = Symbol.X;
                    i = 1;
                }
                else // O
                {
                    a = ai_O.GetMove(Symbols);
                    Symbols[a] = Symbol.O;
                    i = 0;
                }
                vic = Check();
                CurrentRound++;
            }

            GameData data = new GameData(ai_X, ai_O, vic, CurrentRound);
            ResetGame();

            return data;
        }
        
        public void ResetGame()
        {
            CurrentRound = 0;
            Symbols = new Symbol[9];
            for (int i = 0; i < 9; i++)
            {
                Symbols[i] = Symbol.E;
            }
        }
        

        public Victor AIvsPlayer(int move)
        {
            if (Symbols[move] == Symbol.E) 
            {
                Symbols[move] = Player;

                Victor vic = Check();
                if (vic != Victor.Continues) return vic;

                int a = ai.GetMove(Symbols);
                if(a != -1) Symbols[a] = ai.symbol;
            }
            return Check();
        }
    }
}
