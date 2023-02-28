using MessagePack;
using PhysicalSystem.Dto.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicalSystem.Dto.Dtos
{
    [MessagePackObject]
    public class PhysicalSystemDataDto : BaseDto
    {
        [Key(1)]
        public int AgeOFInformation { get; set; }
        [Key(2)]
        public int Data { get; set; }
    }
}
