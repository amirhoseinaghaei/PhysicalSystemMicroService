using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PhysicalSystem.Application.Dtos;
using PhysicalSystem.Application.Utils.Kafka;
using PhysicalSystem.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace PhysicalSystem.Application.Business
{
    public class MainSimulator : IHostedService
    {

        System.Timers.Timer _timer;  
        private IEnvironmentConfig _environmentConfig;
        IMarkovChannels markovChannels;
        int _dataSize;
        IKafkaSender _kafkaSender;
        Random _random;

        public MainSimulator(IEnvironmentConfig environmentConfig, IKafkaSender kafkaSender)
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = 30000;
            _timer.Elapsed += new ElapsedEventHandler(StartDataGeneration);
            _timer.Enabled = true;
            _environmentConfig = environmentConfig;
            markovChannels = new MarkovChannels();
            _dataSize = _environmentConfig.GetPhysicalSystemDataInfo().DataSize; 
            _kafkaSender = kafkaSender;
            _random = new Random();
        }
      
        public void StartDataGeneration(object sender, System.EventArgs e)
        {
            var DataSize = _dataSize;
            var matrix = markovChannels.ChannelTranstionProbabilityMatrix();
            bool isruning = false;
            var RatesList = markovChannels.CreateDataRateMatrix();
            var InitialStateIndex = _random.Next(0, RatesList.Count);
            var InitialStateValue = RatesList[InitialStateIndex];
            var AoI = 0;
            Task.Run(() =>
            {
                isruning = true;
                while (isruning)
                {
                    var test = _random.NextDouble();

                    if (DataSize > 0)
                    {
                        DataSize = DataSize - (int)InitialStateValue;
                        AoI = AoI  + 1;
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        //send to kafka
                        PhysicalSystemDataDto dataDto = new PhysicalSystemDataDto()
                        {
                            AgeOFInformation = 5,
                            Data = 100
                        };
                        _kafkaSender.SendToKafka(dataDto);
                        isruning = false;


                    }
                    if (test < matrix[InitialStateIndex, 0])
                    {
                        InitialStateValue = RatesList[0];
                        InitialStateIndex = 0;
                    }
                    else if (test < matrix[InitialStateIndex, 1])
                    {
                        InitialStateValue = RatesList[1];
                        InitialStateIndex = 1;
                    }
                }
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }

       
        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
