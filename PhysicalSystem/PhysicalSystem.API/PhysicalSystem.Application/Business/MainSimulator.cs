using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PhysicalSystem.Application.Utils.Kafka;
using PhysicalSystem.Configuration;
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

        public MainSimulator(IEnvironmentConfig environmentConfig, IKafkaSender kafkaSender)
        {
            _environmentConfig = environmentConfig;
            markovChannels = new MarkovChannels();
            _dataSize = _environmentConfig.GetPhysicalSystemDataInfo().DataSize; 
            _kafkaSender = kafkaSender; 
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var DataSize = _dataSize; 
            var matrix = markovChannels.ChannelTranstionProbabilityMatrix();
            bool isruning = false;
            var transmissionrates = markovChannels.GetMarkovChnnelsTransmissionRate();
                Task.Run(() =>
                {
                    isruning = true;
                    while (isruning)
                    {
                      if (DataSize > 0)
                        {
                            DataSize = DataSize - 8; 
                        }
                      else
                        {
                            // send to kafka
                            DataSize = _dataSize;
                        }
                    }
                });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
