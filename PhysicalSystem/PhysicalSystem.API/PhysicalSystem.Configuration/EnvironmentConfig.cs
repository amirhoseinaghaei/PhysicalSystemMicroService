using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using static PhysicalSystem.Configuration.EnvironmentConfig;

namespace PhysicalSystem.Configuration
{
    public class EnvironmentConfig: IEnvironmentConfig
    {
        private IConfigurationRoot configuration { get; set; }
        PhysicalSystemData? physicalsystemdata;
        KafkaConfig? kafkaConfig;

        public EnvironmentConfig()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            configuration = configurationBuilder.Build();
            physicalsystemdata = configuration.GetSection("PhysicalSystemData").Get<PhysicalSystemData>();
            kafkaConfig = configuration.GetSection("KafkaConfig").GetSection("Producer").Get<KafkaConfig>();

            //  influxconfig = configuration.GetSection("InfluxConfig").Get<UserRequestConfig>();

        }

        public class PhysicalSystemData
        {
            public int DataSize { get; set; }
            public int EdgeServerId { get; set; }
            public string EdgeServerName { get; set; }
            
            public int DTId { get; set; }
            public string Datatype { get; set; }


        }
        public class KafkaConfig
        {
            public string Topic { get; set; }
            public string IP { get; set; }

            public string Port { get; set; }
            public int BrokerAddressTtl { get; set; }
            public int WaitingForAkTimeoutMillisecond { get; set; }


        }
        public PhysicalSystemData GetPhysicalSystemDataInfo()
        {
            return physicalsystemdata;
        }

        public KafkaConfig GetKafkaConfig()
        {
            return kafkaConfig;
        }

    }

    public interface IEnvironmentConfig
    {
        PhysicalSystemData GetPhysicalSystemDataInfo();
        KafkaConfig GetKafkaConfig();
    }
}
