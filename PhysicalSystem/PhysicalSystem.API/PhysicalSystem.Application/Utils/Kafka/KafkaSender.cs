using Confluent.Kafka;
using MessagePack;
using PhysicalSystem.Application.Dtos;
using PhysicalSystem.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalSystem.Application.Utils.Kafka
{
    public class KafkaSender : IKafkaSender
    {


        private readonly string _topic;

        private readonly ProducerConfig _producerConfig;

        private IEnvironmentConfig _environmentConfig;
        public KafkaSender(IEnvironmentConfig environmentConfig)
        {
            _environmentConfig = environmentConfig;
            _producerConfig = new ProducerConfig 
            { BootstrapServers = $"{_environmentConfig.GetKafkaConfig().IP}:{_environmentConfig.GetKafkaConfig().Port}" };
            _topic = _environmentConfig.GetKafkaConfig().Topic;
        }

        public Object SendToKafka(PhysicalSystemDataDto physicalSystemDataDto)
        {

            Task.Run(() => {

                var body = MessagePackSerializer.Serialize<PhysicalSystemDataDto>(physicalSystemDataDto);
                var producer = new ProducerBuilder<Null, byte[]>(_producerConfig).Build();
                try
                {
                    return producer.ProduceAsync(_topic, new Message<Null, byte[]> { Value = body })
                        .GetAwaiter()
                        .GetResult();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Oops, something went wrong: {e}");
                }

                return null;

            });

            return null;

        }

    }
}
