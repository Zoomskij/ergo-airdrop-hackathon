using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
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
        [Route("count/{count}/blockno/{blockno}/start/{start}/end/{end}/coins/{coins}")]
        public List<int> GetWinners(int count, int blockno, int? start, int end, int coins)
        {
            start = start == null || start <= 0 ? 1 : start;
            var diff = end - (start - 1);
            count = count > diff.Value ? diff.Value : count;

            var winners = new List<int>();
            var request = new RestRequest(Method.GET);
            request.AddQueryParameter("apikey", _apiKey);
            request.AddQueryParameter("blockno", blockno.ToString());

            var response = _client.Execute<JObject>(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return new List<int>();
            var blockNo = JsonConvert.DeserializeObject<Block>(response.Content);

            var seed =  _md5Helper.GenerateSeed(blockNo.Result.BlockMiner);
            
            var rand = new Random(seed);
            for (int i = 0; i < count; i++)
            {
                var range = Enumerable.Range(start.Value, end).Where(x => !winners.Contains(x));
               
                int index = rand.Next(start.Value, end - winners.Count);
                winners.Add(range.ElementAt(index-1));
            }

            return winners;
        }

        public int? GetSeed(string blockno)
        {
            var request = new RestRequest(Method.GET);
            request.AddQueryParameter("apikey", _apiKey);
            request.AddQueryParameter("blockno", blockno);

            var response = _client.Execute<JObject>(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return null;
            var blockNo = JsonConvert.DeserializeObject<Block>(response.Content);

            var seed = _md5Helper.GenerateSeed(blockNo.Result.BlockMiner);
            return seed;
        }

        [HttpPost]
        [Route("winners/{winners}/coins/{coins}/blockno/{blockno}")]
        public Dictionary<string, int> Winners([FromBody] Dictionary<string, int> wallets, int winners, int coins, int blockno)
        {
            var countWinners = new List<int>();
            Dictionary<string, int> selectedWinners = new Dictionary<string, int>();
            

            var wins = new List<string>();

            var seed = GetSeed(blockno.ToString()).Value;
            var rand = new Random(seed);

            for (int i = 0; i < winners; i++)
            {
                var range = Enumerable.Range(1, wallets.Count()).Where(x => !countWinners.Contains(x));

                int index = rand.Next(1, wallets.Count() - countWinners.Count);
                countWinners.Add(range.ElementAt(index - 1));
                selectedWinners.Add(wallets.ElementAt(countWinners.Last()-1).Key, wallets.ElementAt(countWinners.Last()-1).Value);
            }

            var allCoins = selectedWinners.Sum(x => x.Value);

            foreach (var winner in selectedWinners)
                wins.Add(winner.Key);
            coins -= winners;

            for (int i=0; i<coins; i++)
            {
                var getRand = rand.Next(1, allCoins + 1);
                int currentRange = 0;
                foreach (var wallet in selectedWinners)
                {
                    if (getRand <= (currentRange + wallet.Value))
                    {
                        wins.Add(wallet.Key);
                        break;
                    }
                    currentRange += wallet.Value;
                }
            }
            wallets = new Dictionary<string, int>();
            var groupedWinners = wins.GroupBy(i => i);            
            foreach (var grp in groupedWinners)
            {
                wallets.Add(grp.Key, Convert.ToInt32(grp.Count()));
            }


            return wallets;
        }
        
    }
}
