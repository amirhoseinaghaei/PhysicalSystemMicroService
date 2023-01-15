using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhysicalSystem.Application.Business
{
    public class MarkovChannels : IMarkovChannels
    {
        private int _numberofchannels;
        private List<double> randomnumbers;
        Random _random = new Random();
        public MarkovChannels()
        {
            _numberofchannels = 10;
        }

        public double[,] ChannelTranstionProbabilityMatrix()
        {
            double[,] TransitionProbabilityMatrix = new double[_numberofchannels, _numberofchannels];//declaration of 2D array  

            for (int i = 0; i < _numberofchannels; i++)
            {
                for (int j = 0; j < _numberofchannels; j++)
                {
                    randomnumbers = GetrandomVariables();
                    TransitionProbabilityMatrix[i, j] = randomnumbers[j];
                }
            }    
            
           
            return TransitionProbabilityMatrix;

        }
        public List<int> GetMarkovChnnelsTransmissionRate()
        {
            List<int> TransmissionRates = new List<int>();
            for (int j = 0; j < _numberofchannels; j++)
            {
                TransmissionRates.Add(j);
            }
            return TransmissionRates;
        }
        public List<double> GetrandomVariables()
        {
            List<double> randomvariables = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                randomvariables.Add(_random.NextDouble());
            }
            var sum = randomvariables.Sum();

            for (int index = 0; index < randomvariables.Count; index++)
            {
                randomvariables[index] = randomvariables[index] / sum;

            }
            return randomvariables;
        }

    }

    public interface IMarkovChannels
    {
        public List<int> GetMarkovChnnelsTransmissionRate();
        public List<double> GetrandomVariables();
        public double[,] ChannelTranstionProbabilityMatrix();
    }
}
