using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning2
{
    internal class NeuronLayer
    {
        public int NeuronCount;
        public int LayerNumber;
        public float[] ThresoldValues;
        public float[,] NextConnectionWeights;
        public bool IsInputLayer;
        Random rand;

        public int NextNeuronCount
        {
            get
            {
                return NextConnectionWeights.GetLength(1);
            }
        }

        public void SetThresoldValue(int i, float value)
        {
            if (value < 0f) value = 0f;
            ThresoldValues[i] = value;
        }

        public void SetNextConnectionWeight(int i, int k, float value)
        {
            if (value < 0f) value = 0f;
            NextConnectionWeights[i, k] = value;
        }

        public NeuronLayer(int neuronCount, int nextNeuronCount, bool isInputLayer, int layerNumber) 
        {  
            NeuronCount = neuronCount;
            ThresoldValues = new float[neuronCount];
            NextConnectionWeights = new float[neuronCount, nextNeuronCount];
            IsInputLayer = isInputLayer;
            LayerNumber = layerNumber;
            rand = new Random();
            RandomInit();
        }

        public NeuronLayer(int neuronCount, float[] thresoldValues, float[,] nextConnectionWeights, bool isInputLayer)
        {
            NeuronCount = neuronCount;
            ThresoldValues = thresoldValues;
            NextConnectionWeights = nextConnectionWeights;
            IsInputLayer = isInputLayer;
            rand = new Random();
        }

        public NeuronLayer(NeuronLayer neuronLayer)
        {
            NeuronCount = neuronLayer.NeuronCount;
            ThresoldValues = (float[])neuronLayer.ThresoldValues.Clone();
            NextConnectionWeights = (float[,])neuronLayer.NextConnectionWeights.Clone();
            IsInputLayer = neuronLayer.IsInputLayer;
            rand = new Random();
        }

        public float[] Activate(float[] input)
        {
            int outputLenght = NextConnectionWeights.GetLength(1);
            float[] output = new float[outputLenght];

            for (int i = 0; i < NeuronCount; i++)
            {
                if (ThresoldValues[i] <= input[i] || IsInputLayer)
                {
                    for (int k = 0; k < outputLenght; k++)
                    {
                        output[k] += input[i] * NextConnectionWeights[i,k];
                    }
                }
            }

            return output;
        }
        
        public void RandomInit()
        {
            if (!IsInputLayer) 
            {
                for (int i = 0; i < NeuronCount; i++)
                {
                    SetThresoldValue(i, rand.NextSingle() * 400f * (LayerNumber/2f));
                }
            }

            for (int i = 0; i < NeuronCount; i++)
            {
                for (int k = 0; k < NextNeuronCount; k++)
                {
                    SetNextConnectionWeight(i,k, rand.NextSingle() * 4f);
                }
            }
        }

        public void Mutate()
        {
            if (!IsInputLayer)
            {
                int i = rand.Next() % NeuronCount;
                SetThresoldValue(i, ThresoldValues[i] + rand.NextSingle() * 150f - 75f);
            }

            int k = rand.Next() % NeuronCount;
            int j = rand.Next() % NextNeuronCount;
            SetNextConnectionWeight(k, j, NextConnectionWeights[k,j] + (rand.NextSingle() * 2f));
        }

        public static NeuronLayer CrossNeuronLayers(NeuronLayer parent0, NeuronLayer parent1)
        {
            float[] thresolds = new float[parent0.NeuronCount];
            float[,] connectionWeights = new float[parent0.NeuronCount, parent0.NextNeuronCount];
            Random rand = new Random();

            for (int i = 0; i < parent0.NeuronCount; i++)
            {
                if(rand.Next() % 2 == 0)
                {
                    thresolds[i] = parent0.ThresoldValues[i];
                }
                else
                {
                    thresolds[i] = parent1.ThresoldValues[i];
                }
                for (int k = 0; k < parent0.NextNeuronCount; k++)
                {
                    if (rand.Next() % 2 == 0)
                    {
                        connectionWeights[i,k] = parent0.NextConnectionWeights[i,k];
                    }
                    else
                    {
                        connectionWeights[i, k] = parent1.NextConnectionWeights[i, k];
                    }
                }
            }

            NeuronLayer child = new NeuronLayer(parent0.NeuronCount, thresolds, connectionWeights, parent0.IsInputLayer);

            return child;
        }

        public void Print()
        {
            Console.Write("\nNeuron Count: " + NeuronCount + "\n");
            Console.Write("Thresold Values: ");
            for (int i = 0; i < NeuronCount; i++)
            {
                Console.Write(Math.Round(ThresoldValues[i], 2) + "  ");
            }
            Console.Write("\nConnection Weights:");
            for (int k = 0; k < NeuronCount; k++)
            {
                Console.Write(" \n" + k + ".Neuron: ");
                for (int j = 0; j < NextNeuronCount; j++)
                {
                    Console.Write(Math.Round(NextConnectionWeights[k, j], 2) + "  ");
                }
            }
            Console.Write("\n\n");
        }
    }
}