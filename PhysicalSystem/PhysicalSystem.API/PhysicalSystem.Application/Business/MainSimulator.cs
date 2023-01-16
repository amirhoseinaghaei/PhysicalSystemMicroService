using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PhysicalSystem.Application.Utils.Kafka;
using PhysicalSystem.Configuration;
using PhysicalSystem.Dto.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhysicalSystem.Application.Business
{
    public class MainSimulator : IHostedService
    {

        private IEnvironmentConfig _environmentConfig;
        IMarkovChannels markovChannels;
        int _dataSize;
        IKafkaSender _kafkaSender;
        System.Timers.Timer timerToStartDataGeneration;


        public MainSimulator(IEnvironmentConfig environmentConfig, IKafkaSender kafkaSender)
        {
            _environmentConfig = environmentConfig;
            markovChannels = new MarkovChannels();
            _dataSize = _environmentConfig.GetPhysicalSystemDataInfo().DataSize; 
            _kafkaSender = kafkaSender;
            timerToStartDataGeneration = new System.Timers.Timer(15000);
            timerToStartDataGeneration.Elapsed += TimerToStartDataGeneration_Elapsed;
            timerToStartDataGeneration.Enabled = true;
        }

        private void TimerToStartDataGeneration_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var DataSize = _dataSize;
            var matrix = markovChannels.ChannelTranstionProbabilityMatrix();
            bool isruning = false;
            var transmissionrates = markovChannels.GetMarkovChnnelsTransmissionRate();
            isruning = true;

                Task.Run(() =>
                {
                    while (isruning)
                    {
                    var AgeOfInformation = 0;

                    if (DataSize > 0)
                    {
                        DataSize = DataSize - 10;
                        AgeOfInformation += 1;
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        // send to kafka
                        PhysicalSystemDataDto physicalSystemDataDto = new PhysicalSystemDataDto()
                        {
                            AgeOFInformation = AgeOfInformation,
                            Data = 100,
                            Id = _environmentConfig.GetPhysicalSystemDataInfo().EdgeServerId
                        };

                        _kafkaSender.SendToKafka(physicalSystemDataDto);
                        AgeOfInformation = 0;
                        DataSize = _dataSize;
                        isruning=false; 
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
