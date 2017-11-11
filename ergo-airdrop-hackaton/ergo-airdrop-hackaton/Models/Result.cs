using ergo_airdrop_hackaton.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ergo_airdrop_hackaton.Controllers
{
    public class Result
    {
        [JsonProperty("blockMiner")]
        public string BlockMiner { get; set; }

        [JsonProperty("blockNumber")]
        public string BlockNumber { get; set; }

        [JsonProperty("blockReward")]
        public string BlockReward { get; set; }

        [JsonProperty("timeStamp")]
        public string TimeStamp { get; set; }

        [JsonProperty("uncleInclusionReward")]
        public string UncleInclusionReward { get; set; }

        [JsonProperty("uncles")]
        public Uncle[] Uncles { get; set; }
    }
}
