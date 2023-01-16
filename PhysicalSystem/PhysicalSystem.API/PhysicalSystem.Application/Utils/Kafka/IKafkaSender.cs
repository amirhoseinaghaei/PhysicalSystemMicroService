using PhysicalSystem.Dto.Dtos;
using System;

namespace PhysicalSystem.Application.Utils.Kafka
{
    public interface IKafkaSender
    {
        public Object SendToKafka(PhysicalSystemDataDto physicalSystemDataDto);

    }
}