using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning2
{
    internal class NeuronNetwork
    {
        public NeuronLayer[] NeuronLayers;
        public int NeuronLayerLenght;
        public int NeuronLayerCount; // input and hide neuron layer count
        public int InputCount;
        public int OutputCount;
        Random rand;

        public NeuronNetwork(int neuronLayerLenght, int hideNeuronLayerCount, int inputCount, int outputCount)
        {
            NeuronLayerLenght = neuronLayerLenght;
            NeuronLayerCount = hideNeuronLayerCount + 1;
            InputCount = inputCount;
            OutputCount = outputCount;
            NeuronLayers = new NeuronLayer[NeuronLayerCount];
            rand = new Random();
            RandomInit();
        }

        public NeuronNetwork(int neuronLayerLenght, int hideNeuronLayerCount, int inputCount, int outputCount, NeuronLayer[] neuronLayers)
        {
            NeuronLayerLenght = neuronLayerLenght;
            NeuronLayerCount = hideNeuronLayerCount + 1;
            InputCount = inputCount;
            OutputCount = outputCount;
            NeuronLayers = neuronLayers;
            rand = new Random();
        }

        public NeuronNetwork(NeuronNetwork neuronNetwork)
        {
            NeuronLayers = new NeuronLayer[neuronNetwork.NeuronLayers.Length];
            for (int i = 0; i < NeuronLayers.Length; i++)
            {
                NeuronLayers[i] = new NeuronLayer(neuronNetwork.NeuronLayers[i]);
            }
            NeuronLayerLenght = neuronNetwork.NeuronLayerLenght;
            NeuronLayerCount = neuronNetwork.NeuronLayerCount;
            InputCount = neuronNetwork.InputCount;
            OutputCount = neuronNetwork.OutputCount;
            rand = new Random();
        }

        public static NeuronNetwork CrossNeuronNeworks(NeuronNetwork parent0, NeuronNetwork parent1)
        {
            NeuronLayer[] layers = new NeuronLayer[parent0.NeuronLayerCount];

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = NeuronLayer.CrossNeuronLayers(parent0.NeuronLayers[i], parent1.NeuronLayers[i]);
            }

            NeuronNetwork child = new NeuronNetwork(parent0.NeuronLayerLenght, parent0.NeuronLayerCount-1, parent0.InputCount, parent0.OutputCount, layers);
            return child;
        }

        public float[] Activate(float[] input)
        {
            
            for (int i = 0; i < NeuronLayerCount; i++) 
            {
                input = NeuronLayers[i].Activate(input);
            }

            return input;
        }

        public void Print()
        {
            Console.Write("Neuron Layer Lenght: " + NeuronLayerLenght);
            Console.Write("\nNeuron Layer Count: " + NeuronLayerCount);
            Console.Write("\nInput Count: " + InputCount);
            Console.Write("\nOutput Count: " + OutputCount);
            Console.WriteLine("\nNeuron Layers: ");
            for (int i = 0; i < NeuronLayerCount; i++)
            {
                Console.Write("\n" + i + ".Neuron Layer:");
                NeuronLayers[i].Print();
            }
            Console.Write("\n\n\n\n");
        }

        public void RandomInit()
        {
            int neuronCount = InputCount;
            int outputCount = NeuronLayerLenght;

            for (int i = 0; i < NeuronLayerCount; i++)
            {
                if(i == NeuronLayerCount-1) outputCount = OutputCount;
                NeuronLayers[i] = new NeuronLayer(neuronCount, outputCount, i == 0, i);
                neuronCount = NeuronLayerLenght;
            }

            //NeuronLayers[0].ThresoldValues = new float[InputCount]; // First neuron layer is input layer. Input neurons have one job: get input and transfer to hide neurons. For this reason input neurons don't need thresold values
        }

        public void Mutate()
        {
            int i = rand.Next() % NeuronLayerCount;
            NeuronLayers[i].Mutate();
        }
    }
}