using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ergo_airdrop_hackaton.Controllers
{
    [Route("api/[controller]")]
    public class BlocksController : Controller
    {
        private string _apiUri = "https://api.etherscan.io/api?module=block&action=getblockreward";
        private string _apiKey = "AQDRD1BFYFU4T1BM7KJ26G95HA6ZXC5GFE";
        private readonly RestClient _client;
        private Helpers.Md5Helper _md5Helper;

        public BlocksController()
        {
            _client = new RestClient(_apiUri);
            _md5Helper = new Helpers.Md5Helper();
        }

        [HttpGet]
        [Route("count/{count}/blockno/{blockno}/start/{start}/end/{end}")]
        public List<int> GetWinners(int count, int blockno, int? start, int end)
        {
            start = start == null || start <= 0 ? 1 : start;
            
            var winners = new List<int>();
            var request = new RestRequest(Method.GET);
            request.AddQueryParameter("apikey", _apiKey);
            request.AddQueryParameter("blockno", blockno.ToString());

            var response = _client.Execute<JObject>(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return new List<int>();
            var blockNo = JsonConvert.DeserializeObject<Block>(response.Content);

            var seed = _md5Helper.GenerateSeed(blockNo.Result.BlockMiner);
            
            var rand = new Random(seed);
            for (int i=0; i < count; i++)
                winners.Add(rand.Next(start.Value, end + 1));

            return winners;
        }
        
    }
}
