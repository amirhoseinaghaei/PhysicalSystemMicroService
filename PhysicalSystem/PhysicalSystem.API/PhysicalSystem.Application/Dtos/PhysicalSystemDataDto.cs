using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicalSystem.Application.Dtos
{
    [MessagePackObject]

    public class PhysicalSystemDataDto
    {
        [Key(1)]
        public int AgeOFInformation { get; set; }
        [Key(2)]
        public int Data { get; set; }
    }
}
