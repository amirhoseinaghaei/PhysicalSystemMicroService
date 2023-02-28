using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicalSystem.Application.Dtos.Common
{
    [MessagePackObject]
    public class BaseDto
    {
        [Key(0)]
        public int Id { get; set; }
    }
}
