using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicalSystem.Dto.Dtos.Common
{
    [MessagePackObject]
    public class BaseDto
    {
        [Key(0)]
        public int Id { get; set; }
    }
}
