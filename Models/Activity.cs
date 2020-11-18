using System;
using System.Collections.Generic;

namespace ToDo_Backend.Models
{
    public partial class Activity
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public DateTime When { get; set; }
    }
}
