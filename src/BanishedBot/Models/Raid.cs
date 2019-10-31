using BanishedBot.Enums;
using System;
using System.Collections.Generic;

namespace BanishedBot.Models
{
    public class Raid
    {
        public DateTime DateTime { get; set; }
        public Instance Name { get; set; }
        public List<Raider> Raiders { get; set; } = new List<Raider>();
    }
}
