using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ergo_airdrop_hackaton.Models
{
    public class Uncle
    {
        [JsonProperty("blockreward")]
        public string Blockreward { get; set; }

        [JsonProperty("miner")]
        public string Miner { get; set; }

        [JsonProperty("unclePosition")]
        public string UnclePosition { get; set; }
    }
}
