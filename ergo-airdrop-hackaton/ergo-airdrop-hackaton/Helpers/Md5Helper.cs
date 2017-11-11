using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ergo_airdrop_hackaton.Helpers
{
    public class Md5Helper
    {

        public int GenerateSeed(string hash)
        {
            MD5 md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(hash));
            var seed = BitConverter.ToInt32(hashed, 0);
            return seed;
        }

    }
}
