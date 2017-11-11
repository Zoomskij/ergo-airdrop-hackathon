using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ergo_airdrop_hackaton.Controllers
{
    [Route("api/[controller]")]
    public class BlocksController : Controller
    {
        public readonly int END = 10;
        private string _apiUri = "https://api.etherscan.io/api?module=block&action=getblockreward";
        private string _apiKey = "AQDRD1BFYFU4T1BM7KJ26G95HA6ZXC5GFE";
        private readonly RestClient _client;
        private Helpers.Md5Helper _md5Helper;

        public BlocksController()
        {
            _client = new RestClient(_apiUri);
            _md5Helper = new Helpers.Md5Helper();
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{blockno}")]
        [HttpGet]
        public string Get(string blockno)
        {
            var request = new RestRequest(Method.GET);
            request.AddQueryParameter("apikey", _apiKey);
            request.AddQueryParameter("blockno", blockno);


            var response = _client.Execute<JObject>(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return string.Empty;
            var blockNo = JsonConvert.DeserializeObject<Block>(response.Content);
            return blockNo.Result.BlockMiner;
        }

        [HttpGet]
        [Route("count/{count}/blockno/{blockno}")]
        public List<int> Random(int count, int blockno)
        {
            var winners = new List<int>();
            var request = new RestRequest(Method.GET);
            request.AddQueryParameter("apikey", _apiKey);
            request.AddQueryParameter("blockno", blockno.ToString());

            var response = _client.Execute<JObject>(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return new List<int>();
            var blockNo = JsonConvert.DeserializeObject<Block>(response.Content);

            var seed = _md5Helper.GenerateSeed(blockNo.Result.BlockMiner);
            
            var rand = new Random(seed);
            for (int i=0; i < END; i++)
                winners.Add(rand.Next(1, 1000));

            return winners;
        }
        
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
